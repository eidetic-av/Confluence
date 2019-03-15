using System;
using System.Collections;
using System.Collections.Generic;
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
    public virtual void Start() { }
    public virtual void Exit() { }
    public virtual void ValueUpdate() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
}