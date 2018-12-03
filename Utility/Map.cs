using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Utility;

public class Map : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public float Input;
    public Vector2 InputMinMax;
    public Vector2 OutputMinMax;
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