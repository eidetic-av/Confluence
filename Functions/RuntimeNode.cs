using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class RuntimeNode : Node
{
    public static List<RuntimeNode> InstantiatedNodes = new List<RuntimeNode>();
    protected override void Init()
    {
        base.Init();
    }

    new void OnEnable()
    {
        base.OnEnable();
        InstantiatedNodes.Add(this);
    }

    void OnDestroy()
    {
        InstantiatedNodes.Remove(this);
    }

    public virtual void ValueUpdate() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
}