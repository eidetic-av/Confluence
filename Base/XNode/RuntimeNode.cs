using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eidetic.Unity.Utility;
using Eidetic.Utility;
using UnityEditor;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public abstract class RuntimeNode : Node
    {
        public static List<RuntimeNode> ActiveNodes { get; protected set; }
        static RuntimeNode()
        {
            ActiveNodes = new List<RuntimeNode>();
#if UNITY_EDITOR

            EditorApplication.playModeStateChanged += (stateChange) =>
            {
                if (stateChange == PlayModeStateChange.EnteredPlayMode) OnPlay();
                else if (stateChange == PlayModeStateChange.ExitingPlayMode) OnExit();
            };
#else
            foreach (var node in Resources.Load<RuntimeGraph>("TrickFilm").nodes)
            {
                ActiveNodes.Add(node as RuntimeNode);
            }
            OnPlay();
#endif
        }
#if UNITY_STANDALONE
        [RuntimeInitializeOnLoadMethod]
#endif
        private static void OnPlay() => RuntimeNodeUpdater.Instantiate();
        private static void OnExit() => ActiveNodes.ForEachOnMain(n => n.Exit());

        public Dictionary<string, Func<object>> Getters { get; private set; }
        public Dictionary<string, Action<object>> Setters { get; private set; }
        public Dictionary<string, FieldInfo> BackingFields { get; private set; }

        new public void OnEnable()
        {
            base.OnEnable();
            if (GetType() == typeof(NodeSelector)) return;
            if (ActiveNodes.Contains(this)) return;

            ActiveNodes.Add(this);

            Getters = new Dictionary<string, Func<object>>();
            Setters = new Dictionary<string, Action<object>>();
            BackingFields = new Dictionary<string, FieldInfo>();

            foreach (var member in GetType().GetMembers().WithAttribute<NodePortAttribute>())
            {
                if (member is FieldInfo field)
                {
                    Getters.Add(field.Name, () => field.GetValue(this));
                    Setters.Add(field.Name, (value) => field.SetValue(this, value));
                }
                else if (member is PropertyInfo property)
                {
                    Getters.Add(property.Name, () => property.GetValue(this));
                    if (property.GetSetMethod() != null)
                        Setters.Add(property.Name, (value) => property.SetValue(this, value));
                    BackingFields.Add(property.Name, GetType().GetBackingField(property), false);
                }
            }
        }
        void OnDestroy()
        {
            ActiveNodes.Remove(this);
            Destroy();
            if (Application.isPlaying) Exit();
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            RunSetters();
        }

        public override void OnRemoveConnection(NodePort removedPort)
        {
            base.OnRemoveConnection(removedPort);
            RunSetters();
        }

        public void RunSetters()
        {
            BackingFields.ForEachOnMain((key, backingField) =>
            {
                if (!Setters.ContainsKey(key)) return;
                Setters[key].Invoke(backingField.GetValue(this));
            });
        }

        public override object GetValue(NodePort port) => Getters[port.MemberName]();
        internal void ValueUpdate()
        {
            foreach (var port in Ports.Where(port => port.IsInput && port.IsConnected))
                Setters[port.MemberName].Invoke(port.Connection.Node.GetValue(port.Connection));
        }
        internal virtual void Awake() { }
        internal virtual void Start()
        {
            // Run the set methods for all properties on start because the
            // backing values may have changed outside of play mode, and
            // validation, side-effects etc. may still need to be invoked.
            RunSetters();
        }
        internal virtual void EarlyUpdate() { }
        internal virtual void Update() { }
        internal virtual void LateUpdate() { }
        internal virtual void Destroy() { }
        internal virtual void Exit() { }
    }
}