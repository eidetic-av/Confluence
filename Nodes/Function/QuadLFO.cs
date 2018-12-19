using UnityEngine;
using System.Collections.Generic;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Function/EnvelopeGenerator"),
        NodeTint(Colors.FunctionTint)]
    public class QuadLFO : RuntimeNode
    {
        /// <summary>
        /// Master clock rate in beats per minute (BPM).
        /// </summary>
        [Input] public float Clock = 120f;
        float LastClock;

        LFO OscillatorA = new LFO()
        {
            Shape = LFO.WaveShape.Sin,
            SyncRate = 0
        };

        LFO OscillatorB = new LFO()
        {
            Shape = LFO.WaveShape.Sin,
            SyncRate = 2
        };

        LFO OscillatorC = new LFO()
        {
            Shape = LFO.WaveShape.Sin,
            SyncRate = 4
        };

        LFO OscillatorD = new LFO()
        {
            Shape = LFO.WaveShape.Sin,
            SyncRate = 6
        };

        public override void ValueUpdate()
        {
            if (Clock != LastClock)
                SetClockDivisions(Clock);

            OscillatorA.Update();
            OscillatorB.Update();
            OscillatorC.Update();
            OscillatorD.Update();

        }

        void SetClockDivisions(float clock)
        {

            LastClock = clock;
        }

        public class LFO
        {
            /// <summary>
            /// LFO speed in BPM.
            /// </summary>
            public float Speed;
            /// <summary>
            /// Length of LFO in milisecond.
            /// </summary>
            public float Length { get { return (60000 / Speed); } }
            /// <summary>
            /// Rate 'step' to multiply speed by.
            /// </summary>
            public int SyncRate;
            ///<summary>
            /// Shape of the waveform.
            ///</summary>
            public WaveShape Shape;
            ///<summary>
            /// Current output value of the LFO.
            ///</summary>
            public float Value { get; private set; }

            public void Update()
            {

            }

            public static Dictionary<string, float> SyncRates { get; } = new Dictionary<string, float>()
            {
                {"1/16", (1/16f)},
                {"1/16T", (1/16f)*(4/3f)},
                {"1/8", (1/8f)},
                {"1/8T", (1/8f)*(4/3f)},
                {"1/4", (1/4f)},
                {"1/3", (1/3f)},
                {"1/2", (1/2f)},
                {"2/3", (2/3f)},
                {"1", 1f},
                {"3/2 (1.5x)", (3/2f)},
                {"2", 2f},
                {"3", 3f},
                {"4", 4f},
                {"8", 8f},
                {"16", 16f}
            };
            public static List<string> SyncRateLabels { get; } = new List<string>()
            {
                "1/16",
                "1/16T",
                "1/8",
                "1/8T",
                "1/4",
                "1/3",
                "1/2",
                "2/3",
                "1",
                "3/2 (1.5x)",
                "2",
                "3",
                "4",
                "8",
                "16"
            };
            public enum WaveShape
            {
                Sin, Triangle, Saw, Ramp, Square, SampleAndHold
            }
        }
    }
}