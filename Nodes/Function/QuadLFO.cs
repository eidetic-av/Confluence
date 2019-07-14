using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Function/QuadLFO"),
        NodeTint(Colors.FunctionTint)]
    public class QuadLFO : RuntimeNode
    {
        public static readonly float[] MultiplierRates = new float[] {
            (1/32f),
            (1/16f),
            (1/16f)*(4/3f),
            (1/8f),
            (1/8f)*(4/3f),
            (1/4f),
            (1/4f)*(4/3f),
            (1/2f),
            (2/3f),
            (1f),
            (4/3f),
            (2f),
            (4f),
            (8f),
            (12f),
            (16f),
            (24),
            (32)
        };
        public static readonly string[] MultiplierRateLabels = new string[] {
            "1/32",
            "1/16",
            "1/16T",
            "1/8",
            "1/8T",
            "1/4",
            "1/4T",
            "1/2",
            "2/3",
            "1",
            "4/3",
            "2",
            "4",
            "8",
            "12",
            "16",
            "24",
            "32",
        };
        
        public static int ClockMin { get; } = 40;
        public static int ClockMax { get; } = 170;

        /// <summary>
        /// Master clock rate in beats per minute (BPM).
        /// </summary>
        [Input] public float Clock = 120f;
        public NodePort ClockPort { get; private set; }

        [Input] public int MultiplierAPosition = 3;
        public NodePort MultiplierAPort { get; private set; }
        public float MultiplierA {get {return MultiplierRates[MultiplierAPosition]; } }
        public string MultiplierALabel {get {return MultiplierRateLabels[MultiplierAPosition]; } }
        [Input] public int MultiplierBPosition = 5;
        public NodePort MultiplierBPort { get; private set; }
        public float MultiplierB {get {return MultiplierRates[MultiplierBPosition]; } }
        public string MultiplierBLabel {get {return MultiplierRateLabels[MultiplierBPosition]; } }
        [Input] public int MultiplierCPosition = 7;
        public NodePort MultiplierCPort { get; private set; }
        public float MultiplierC {get {return MultiplierRates[MultiplierCPosition]; } }
        public string MultiplierCLabel {get {return MultiplierRateLabels[MultiplierCPosition]; } }

        // Output ports
        [Output] public float MasterSine;
        public NodePort MasterSinePort;
        [Output] public float MultiplierASine;
        public NodePort MultiplierASinePort;
        [Output] public float MultiplierBSine;
        public NodePort MultiplierBSinePort;
        [Output] public float MultiplierCSine;
        public NodePort MultiplierCSinePort;
        [Output] public float MasterTriangle;
        public NodePort MasterTrianglePort;
        [Output] public float MultiplierATriangle;
        public NodePort MultiplierATrianglePort;
        [Output] public float MultiplierBTriangle;
        public NodePort MultiplierBTrianglePort;
        [Output] public float MultiplierCTriangle;
        public NodePort MultiplierCTrianglePort;
        [Output] public float MasterSawtooth;
        public NodePort MasterSawtoothPort;
        [Output] public float MultiplierASawtooth;
        public NodePort MultiplierASawtoothPort;
        [Output] public float MultiplierBSawtooth;
        public NodePort MultiplierBSawtoothPort;
        [Output] public float MultiplierCSawtooth;
        public NodePort MultiplierCSawtoothPort;

        LFO Oscillator = new LFO();

        protected override void Init()
        {
            base.Init();
            ClockPort = GetInputPort("Clock");
            MultiplierAPort = GetInputPort("MultiplierAPosition");
            MultiplierBPort = GetInputPort("MultiplierBPosition");
            MultiplierCPort = GetInputPort("MultiplierCPosition");
            MasterSinePort = GetOutputPort("MasterSine");
            MultiplierASinePort = GetOutputPort("MultiplierASine");
            MultiplierBSinePort = GetOutputPort("MultiplierBSine");
            MultiplierCSinePort = GetOutputPort("MultiplierCSine");
            MasterTrianglePort = GetOutputPort("MasterTriangle");
            MultiplierATrianglePort = GetOutputPort("MultiplierATriangle");
            MultiplierBTrianglePort = GetOutputPort("MultiplierBTriangle");
            MultiplierCTrianglePort = GetOutputPort("MultiplierCTriangle");
            MasterSawtoothPort = GetOutputPort("MasterSawtooth");
            MultiplierASawtoothPort = GetOutputPort("MultiplierASawtooth");
            MultiplierBSawtoothPort = GetOutputPort("MultiplierBSawtooth");
            MultiplierCSawtoothPort = GetOutputPort("MultiplierCSawtooth");
        }

        internal override void Update()
        {
            base.Update();
            Oscillator.Speed = Clock;
        }

        public override object GetValue(NodePort port)
        {
            switch(port.MemberName)
            {
                case "MasterSine": return Oscillator.Sine;
                case "MultiplierASine": return Oscillator.GetSineMultiple(1 / MultiplierA);
                case "MultiplierBSine": return Oscillator.GetSineMultiple(1 / MultiplierB);
                case "MultiplierCSine": return Oscillator.GetSineMultiple(1 / MultiplierC);
                case "MasterTriangle": return Oscillator.Triangle;
                case "MultiplierATriangle": return Oscillator.GetTriangleMultiple(1 / MultiplierA);
                case "MultiplierBTriangle": return Oscillator.GetTriangleMultiple(1 / MultiplierB);
                case "MultiplierCTriangle": return Oscillator.GetTriangleMultiple(1 / MultiplierC);
                case "MasterSawtooth": return Oscillator.Sawtooth;
                case "MultiplierASawtooth": return Oscillator.GetSawtoothMultiple(1 / MultiplierA);
                case "MultiplierBSawtooth": return Oscillator.GetSawtoothMultiple(1 / MultiplierB);
                case "MultiplierCSawtooth": return Oscillator.GetSawtoothMultiple(1 / MultiplierC);
                default: return 0f;
            }
        }

        public class LFO
        {
            /// <summary>
            /// Speed of the oscillator in BPM.
            /// default = 120
            /// </summary>
            public float Speed { get; set; } = 120f;

            /// <summary>
            /// Length of the oscillator cycle in seconds.
            /// </summary>
            public float Length { get { return (60 / Speed) * 4; } }

            /// <summary>
            /// The current normalised phase of the oscillator.
            /// </summary>
            public float Phase
            {
                get
                {
                    var elapsedTime = Time.time - LastPhaseUpdateTime;
                    var elapsedPhase = elapsedTime / Length;
                    var phase = LastPhase + elapsedPhase;
                    LastPhaseUpdateTime = Time.time;
                    return LastPhase = phase;
                }
            }
            float LastPhaseUpdateTime;
            float LastPhase;

            /// <summary>
            /// Sine wave output value.
            /// </summary>
            public float Sine { get { return Mathf.Sin((Phase) * (Mathf.PI * 2)); } }

            /// <summary>
            /// Returns the sine wave with it's speed multiplied by an input float.
            /// </summary>
            public float GetSineMultiple(float multiplier)
            {
                return Mathf.Sin((Phase * multiplier) * (Mathf.PI * 2));
            }

            /// <summary>
            /// Triangle wave output value.
            /// </summary>
            public float Triangle { get { return (Mathf.PingPong((Phase), .5f) * 4) - 1; } }

            /// <summary>
            /// Returns the triangle wave with it's speed multiplied by an input float.
            /// </summary>
            public float GetTriangleMultiple(float multiplier)
            {
                return (Mathf.PingPong((Phase * multiplier), .5f) * 4) - 1;
            }

            /// <summary>
            /// Sawtooth wave output value.
            /// </summary>
            public float Sawtooth { get { return Ramp * -1; } }
            /// <summary>
            /// Returns the sawtooth wave with it's speed multiplied by an input float.
            /// </summary>
            public float GetSawtoothMultiple(float multiplier)
            {
                return GetRampMultiple(multiplier) * -1;
            }

            /// <summary>
            /// Ramp (inverse-sawtooth) wave output value.
            /// </summary>
            public float Ramp { get { return ((Phase % 1f) * 2) - 1; } }
            /// <summary>
            /// Returns the ramp wave with it's speed multiplied by an input float.
            /// </summary>
            public float GetRampMultiple(float multiplier)
            {
                return (((Phase * multiplier) % 1f) * 2) - 1;
            }
        }
    }
}