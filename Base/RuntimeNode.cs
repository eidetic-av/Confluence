using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;
using XNode;
using Eidetic.Unity.Utility;
using Eidetic.Utility;

[InitializeOnLoad]
public abstract class RuntimeNode : Node
{
    public static GameObject Updater { get; protected set; }
    public static List<RuntimeNode> InstantiatedNodes = new List<RuntimeNode>();

    public Dictionary<string, Func<object>> Getters { get; private set; }
    public Dictionary<string, Action<object>> Setters { get; private set; }

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

        InstantiatedNodes.ForEachOnMain(node => node.Start());
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

        // run and cache the getters & setters
        Getters = new Dictionary<string, Func<object>>();
        Setters = new Dictionary<string, Action<object>>();

        var nodeType = this.GetType();

        var attributedFields = nodeType.GetFields()
            .Where(field => field.GetCustomAttributes().OfType<NodePortAttribute>() != null);

        foreach (var field in attributedFields)
        {
            // assume the field is a backing field for a PascalCased property
            var propertyName = field.Name.ToPascal();
            var property = nodeType.GetProperty(propertyName);
            if (property != null)
            {
                Getters.Add(propertyName, () => property.GetValue(this));
                Setters.Add(propertyName, (value) => property.SetValue(this, value));
            }
            // but if there's no corresponding property treat it like a standard field
            else
            {
                var standardField = nodeType.GetField(field.Name);
                Getters.Add(field.Name, () => standardField.GetValue(this));
                Setters.Add(field.Name, (value) => standardField.SetValue(this, value));
            }
        }

        if (Application.isPlaying) Start();
    }

    void OnDestroy()
    {
        InstantiatedNodes.Remove(this);
        if (Application.isPlaying) Exit();
    }

    // Todo:
    // All input ports are updated every frame
    // Is that necessary if the values don't update?
    public void ValueUpdate()
    {
        foreach (var port in Ports.Where(port => port.IsInput && port.IsConnected))
        {
            var inputValue = port.Connection.Node.GetValue(port.Connection);
            Setters[port.MemberName](inputValue);
        }
    }

    public override object GetValue(NodePort port) => Getters[port.MemberName]();

    public virtual void Start() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
}