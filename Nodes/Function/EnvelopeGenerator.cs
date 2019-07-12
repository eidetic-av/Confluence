using System.Diagnostics;
using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Function/EnvelopeGenerator"),
        NodeTint(Colors.FunctionTint)
    ]
    public class EnvelopeGenerator : RuntimeNode
    {
        public Stopwatch EnvelopeTimer { get; private set; } = new Stopwatch();

        bool trigger;
        [Input] public bool Trigger
        {
            get => trigger;
            set
            {
                trigger = value;
                if (value) EnvelopeTimer.Restart();
            }
        }

        [Input] public float Length = 1f;
        [Input] public AnimationCurve Shape = AnimationCurve.EaseInOut(1, 1, 0, 0);
        
        [Output] public float Value => Shape.Evaluate(EnvelopePosition);

        [Output] public bool Running => EnvelopeTimer.IsRunning;
        
        [Output] public float EnvelopePosition
        {
            get
            {
                var position = EnvelopeTimer.Elapsed.TotalSeconds / Length;
                if (position > 1)
                {
                    EnvelopeTimer.Stop();
                    position = 1;
                }
                return (float) position;
            }
        }
    }
}