using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;
using XNode;
using Eidetic.Unity.Utility;

[InitializeOnLoad]
public abstract class RuntimeNode : Node
{
    public static GameObject Updater { get; protected set; }
    public static List<RuntimeNode> InstantiatedNodes = new List<RuntimeNode>();

    static RuntimeNode() => EditorApplication.playModeStateChanged += (playModeStateChange) =>
    {
        if (playModeStateChange == PlayModeStateChange.EnteredPlayMode) OnPlay();
        else if (playModeStateChange == PlayModeStateChange.ExitingPlayMode) OnExit();
    };

    protected override void Init()
    {
        base.Init();
    }

    public static void OnPlay()
    {
        if (RuntimeNode.Updater == null)
            RuntimeNode.Updater = new GameObject("RuntimeNodeUpdater")
                .WithComponent<RuntimeNodeUpdater>()
                .WithHideFlags(HideFlags.NotEditable)
                .InDontDestroyMode();
        InstantiatedNodes.ForEachOnMain(n => n.Start());
    }

    public static void OnExit()
    {
        RuntimeNode.Updater.Destroy();
        InstantiatedNodes.ForEachOnMain(n => n.Exit());
    }

    new void OnEnable()
    {
        base.OnEnable();
        InstantiatedNodes.Add(this);
        if (Application.isPlaying) Start();
    }

    void OnDestroy()
    {
        InstantiatedNodes.Remove(this);
        if (Application.isPlaying) Exit();
    }

    // Todo:
    // All connected ports are updated every frame
    // Is that necessary if the values don't update?
    public void ValueUpdate()
    {
        // Todo:
        // I guess these property setters should be cached so there is no Runtime reflection?

        // Same with the GetValue() method

        var connectedInputPorts = Ports.Where(port => port.IsInput && port.IsConnected);
        foreach (var port in connectedInputPorts)
        {
            var inputValue = port.Connection.Node.GetValue(port.Connection);
            var member = this.GetType().GetMember(port.MemberName).Single();

            if (port.MemberType == MemberTypes.Property)
                (member as PropertyInfo).SetValue(this, inputValue);
            else
                (member as FieldInfo).SetValue(this, inputValue);
        }
    }

    public override object GetValue(NodePort port)
    {
        var member = this.GetType().GetMember(port.MemberName).Single();

        if (port.MemberType == MemberTypes.Property)
            return (member as PropertyInfo).GetValue(this);
        else
            return (member as FieldInfo).GetValue(this);
    }

    public virtual void Start() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
}