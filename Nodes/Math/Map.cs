using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Utility;

[CreateNodeMenu("Math/Map")]
public class Map : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public float Input;
    public Vector2 InputMinMax = new Vector2(0, 1);
    public Vector2 OutputMinMax = new Vector2(0, 1);
    [Output(ShowBackingValue.Always, ConnectionType.Multiple)] public float Output;

    public float GetValue()
    {
        return (float) GetValue(null);
    }

    public override object GetValue(NodePort port)
    {
        return GetInputValue<float>("Input").Map(InputMinMax, OutputMinMax);
    }
}