using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Eidetic.Utility;

[CreateNodeMenu("Math/Map")]
public class Map : RuntimeNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public float Input;
    public Vector2 InputMinMax = new Vector2(0, 1);
    public Vector2 OutputMinMax = new Vector2(0, 1);
    [Output(ShowBackingValue.Always, ConnectionType.Multiple)] public float Output;

    NodePort InputConnection;

    protected override void Init()
    {
        base.Init();
        Input = InputMinMax.x;
        Output = OutputMinMax.x;
    }

    public override void Update()
    {
        Input = GetInputPort("Input").GetInputValue<float>();        
        Output = Input.Map(InputMinMax, OutputMinMax);
    }

    public float GetValue()
    {
        return Output;
    }

    public override object GetValue(NodePort port)
    {
        return GetValue();
    }
}