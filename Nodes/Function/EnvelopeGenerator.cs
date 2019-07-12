using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using XNode;

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

        float value = 0f;
        [Output] float Value => Shape.Evaluate(EnvelopePosition);
        bool running;
        [Output] bool Running => EnvelopeTimer.IsRunning;
        float envelopePosition;
        [Output] float EnvelopePosition
        {
            get
            {
                var position = EnvelopeTimer.Elapsed.TotalSeconds / Length;
                if (position > 1)
                {
                    EnvelopeTimer.Reset();
                    position = 0;
                }
                return (float) position;
            }
        }
    }
}