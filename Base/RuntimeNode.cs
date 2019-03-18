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
    [InitializeOnLoad]
    public abstract class RuntimeNode : Node
    {
        public static List<RuntimeNode> ActiveNodes { get; protected set; }
        static RuntimeNode()
        {
            EditorApplication.playModeStateChanged += (stateChange) =>
            {
                if (stateChange == PlayModeStateChange.EnteredPlayMode) OnPlay();
                else if (stateChange == PlayModeStateChange.ExitingPlayMode) OnExit();
            };

            ActiveNodes = new List<RuntimeNode>();
        }
        private static void OnPlay() => RuntimeNodeUpdater.Instantiate();
        private static void OnExit() => ActiveNodes.ForEachOnMain(n => n.Exit());

        public Dictionary<string, Func<object>> Getters { get; private set; }
        public Dictionary<string, Func<object, object>> Setters { get; private set; }
        public Dictionary<string, FieldInfo> BackingFields { get; private set; }

        // OnEnable runs when the node is created
        // and also when play mode is activated
        new void OnEnable()
        {
            base.OnEnable();
            if (GetType() == typeof(NodeSelector)) return;

            if (!ActiveNodes.Contains(this))
            {
                ActiveNodes.Add(this);
                // cache the getters & setters
                Getters = new Dictionary<string, Func<object>>();
                Setters = new Dictionary<string, Func<object, object>>();
                // and store the information about backing fields so that we can
                // serialize the node objects properly (see RuntimeNodeEditor)
                BackingFields = new Dictionary<string, FieldInfo>();
                foreach (var member in GetType().GetMembers().WithAttribute<NodePortAttribute>())
                {
                    if (member.MemberType == MemberTypes.Field)
                    {
                        var field = member as FieldInfo;
                        Getters.Add(field.Name, () => field.GetValue(this));
                        Setters.Add(field.Name, (value) =>
                        {
                            field.SetValue(this, value);
                            return value;
                        });
                    }
                    else
                    {
                        var property = member as PropertyInfo;
                        Getters.Add(property.Name, () => property.GetValue(this));
                        if (property.GetSetMethod() == null) continue;
                        Setters.Add(property.Name, (value) =>
                        {
                            property.SetValue(this, value);
                            return property.GetValue(this);
                        });
                        // the backing field must have the same name as the
                        // property in camelCase
                        var backingField = GetType().GetField(property.Name.ToCamelCase(), BindingFlags.NonPublic | BindingFlags.Instance);
                        if (backingField == null) continue;
                        BackingFields.Add(property.Name, backingField);
                    }
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

        public void RunSetters() => Setters.ForEachOnMain((key, setter) => setter.Invoke(BackingFields[key].GetValue(this)));

        public override object GetValue(NodePort port) => Getters[port.MemberName]();
        internal void ValueUpdate()
        {
            // If an input port is connected, update it's value with
            // the output value of the node it's connected to.
            foreach (var port in Ports.Where(port => port.IsInput && port.IsConnected))
                Setters[port.MemberName](port.Connection.Node.GetValue(port.Connection));
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