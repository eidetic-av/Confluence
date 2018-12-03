using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TestNode : Node
{
    [Input] public float In;
    [Output] public float Out;

    protected override void Init()
    {
        base.Init();
    }

    public override object GetValue(NodePort port)
    {
        switch (port.fieldName)
        {
            case "Out":
                return In;
			default:
				return 0;
        }
    }
}