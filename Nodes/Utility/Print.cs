using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


[CreateNodeMenu("Utility/Print")]
public class Print : RuntimeNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public float Input;

    public float GetValue()
    {
        return GetInputValue<float>("Input");
    }

    public override void Update()
    {
        Debug.Log(GetValue());
    }
}