using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Function/EnvelopeGenerator"),
        NodeTint(Colors.FunctionTint)]
    public class EnvelopeGenerator : Node, NoteOn.ITriggeredNode
    {
        public Stopwatch EnvelopeTimer { get; private set; } = new Stopwatch();

        [TriggerInput] public bool _trigger;
        public bool Trigger { set { if (value) EnvelopeTimer.Restart(); } }
        [Input] public float Length = 1f;
        [Input] public AnimationCurve Shape = AnimationCurve.EaseInOut(1, 1, 0, 0);

        [Output] public float _value = 0f;
        float Value { get { return Shape.Evaluate(Position); } }
        [Output] public bool _running;
        bool Running { get { return EnvelopeTimer.IsRunning; } }
        [Output] public float _position;
        float Position
        {
            get
            {
                var position = EnvelopeTimer.Elapsed.TotalSeconds / Length;
                if (position > 1)
                {
                    EnvelopeTimer.Reset();
                    position = 0;
                }
                return (float)position;
            }
        }

        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case "Value":
                    return Value;
                default:
                    return 0f;
            }
        }
    }
}