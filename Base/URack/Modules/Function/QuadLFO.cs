using UnityEngine;
using System.Collections.Generic;
using Eidetic.URack;

namespace Eidetic.URack.Function
{
    public class QuadLFO : Module
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
            (4f)
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
            "4"
        };
        
        public static int ClockMin { get; } = 40;
        public static int ClockMax { get; } = 170;

        /// <summary>
        /// Master clock rate in beats per minute (BPM).
        /// </summary>
        [Input] public float Clock = 120f;
        public Port ClockPort { get; private set; }

        [Input] public int MultiplierAPosition = 3;
        public Port MultiplierAPort { get; private set; }
        public float MultiplierA {get {return MultiplierRates[MultiplierAPosition]; } }
        public string MultiplierALabel {get {return MultiplierRateLabels[MultiplierAPosition]; } }
        [Input] public int MultiplierBPosition = 5;
        public Port MultiplierBPort { get; private set; }
        public float MultiplierB {get {return MultiplierRates[MultiplierBPosition]; } }
        public string MultiplierBLabel {get {return MultiplierRateLabels[MultiplierBPosition]; } }
        [Input] public int MultiplierCPosition = 7;
        public Port MultiplierCPort { get; private set; }
        public float MultiplierC {get {return MultiplierRates[MultiplierCPosition]; } }
        public string MultiplierCLabel {get {return MultiplierRateLabels[MultiplierCPosition]; } }

        // Output ports
        [Output] public float MasterSine;
        public Port MasterSinePort;
        [Output] public float MultiplierASine;
        public Port MultiplierASinePort;
        [Output] public float MultiplierBSine;
        public Port MultiplierBSinePort;
        [Output] public float MultiplierCSine;
        public Port MultiplierCSinePort;
        [Output] public float MasterTriangle;
        public Port MasterTrianglePort;
        [Output] public float MultiplierATriangle;
        public Port MultiplierATrianglePort;
        [Output] public float MultiplierBTriangle;
        public Port MultiplierBTrianglePort;
        [Output] public float MultiplierCTriangle;
        public Port MultiplierCTrianglePort;
        [Output] public float MasterSawtooth;
        public Port MasterSawtoothPort;
        [Output] public float MultiplierASawtooth;
        public Port MultiplierASawtoothPort;
        [Output] public float MultiplierBSawtooth;
        public Port MultiplierBSawtoothPort;
        [Output] public float MultiplierCSawtooth;
        public Port MultiplierCSawtoothPort;

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

        // public override object GetValue(Port port)
        // {
        //     switch(port.MemberName)
        //     {
        //         case "MasterSine": return Oscillator.Sine;
        //         case "MultiplierASine": return Oscillator.GetSineMultiple(1 / MultiplierA);
        //         case "MultiplierBSine": return Oscillator.GetSineMultiple(1 / MultiplierB);
        //         case "MultiplierCSine": return Oscillator.GetSineMultiple(1 / MultiplierC);
        //         case "MasterTriangle": return Oscillator.Triangle;
        //         case "MultiplierATriangle": return Oscillator.GetTriangleMultiple(1 / MultiplierA);
        //         case "MultiplierBTriangle": return Oscillator.GetTriangleMultiple(1 / MultiplierB);
        //         case "MultiplierCTriangle": return Oscillator.GetTriangleMultiple(1 / MultiplierC);
        //         case "MasterSawtooth": return Oscillator.Sawtooth;
        //         case "MultiplierASawtooth": return Oscillator.GetSawtoothMultiple(1 / MultiplierA);
        //         case "MultiplierBSawtooth": return Oscillator.GetSawtoothMultiple(1 / MultiplierB);
        //         case "MultiplierCSawtooth": return Oscillator.GetSawtoothMultiple(1 / MultiplierC);
        //         default: return 0f;
        //     }
        // }

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
                    var phase = (LastPhase + elapsedPhase) % 1;
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