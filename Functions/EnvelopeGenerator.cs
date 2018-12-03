using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Functions/EnvelopeGenerator"),
    NodeTint(Nodes.Colors.FunctionTint)]
public class EnvelopeGenerator : RuntimeNode
{

    [Input] public bool Trigger = false;
    [Input] public float Length = 1f;
    [Input] public AnimationCurve Shape = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Output] public bool Running = false;
    [Output] public float StartTime = 0f;
    [Output] public float Position = 0f;
    [Output] public float Value = 0f;


    protected override void Init()
    {
        base.Init();
    }

    public void TriggerEnvelope()
    {
        StartTime = Time.time;
        Position = 0;
        Running = true;
        Trigger = false;
    }

    public override void Update()
    {
		Trigger = GetInputValue<bool>("Trigger");

        if (Trigger) TriggerEnvelope();
        if (Running)
        {
            Position = (Time.time - StartTime) / Length;
            if (Position > 1)
            {
                Position = 1;
                Running = false;
            }
            Value = Shape.Evaluate(Position);
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