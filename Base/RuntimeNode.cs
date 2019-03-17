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
        public Dictionary<string, Func<object>> Getters { get; private set; }
        public Dictionary<string, Func<object, object>> Setters { get; private set; }
        public Dictionary<string, FieldInfo> BackingFields { get; private set; }
        static RuntimeNode()
        {
            EditorApplication.playModeStateChanged += (playModeStateChange) =>
            {
                if (playModeStateChange == PlayModeStateChange.EnteredPlayMode) OnPlay();
                else if (playModeStateChange == PlayModeStateChange.ExitingPlayMode) OnExit();
            };

            ActiveNodes = new List<RuntimeNode>();
        }
        public static void OnPlay()
        {
            RuntimeNodeUpdater.Instantiate();
            
            ActiveNodes.ForEachOnMain(node => node.Start());
            // Run the property setters in the dictionary with the backing
            // field value to trigger any set method side effects that might
            // need to be run in play mode
            RunSetters();
        }
        private static void RunSetters()
        {
            ActiveNodes.ForEachOnMain(node =>
            {
                foreach (var entry in node.Setters.ToList())
                    entry.Value(node.Getters[entry.Key]());
            });
        }
        public static void OnExit()
        {
            ActiveNodes.ForEachOnMain(n => n.Exit());
        }
        new void OnEnable()
        {
            base.OnEnable();
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
                    var backingField = GetType().GetField(property.Name.ToCamel());
                    if (backingField == null) continue;
                    BackingFields.Add(property.Name, backingField);
                }
            }
            if (Application.isPlaying)
            {
                Awake();
                Start();
            }
        }
        void OnDestroy()
        {
            ActiveNodes.Remove(this);
            Destroy();
            if (Application.isPlaying) Exit();
        }
        public override object GetValue(NodePort port) => Getters[port.MemberName]();
        internal void ValueUpdate()
        {
            // If an input port is connected, update it's value with
            // the output value of the node it's connected to.
            foreach (var port in Ports.Where(port => port.IsInput))
                if (port.IsConnected)
                    Setters[port.MemberName](port.Connection.Node.GetValue(port.Connection));
        }
        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            var toNode = to.Node as RuntimeNode;
            if (toNode != this) return;
            RunSetters();
            base.OnCreateConnection(from, to);
        }
        internal virtual void Awake() { }
        internal virtual void Start() { }
        internal virtual void EarlyUpdate() { }
        internal virtual void Update() { }
        internal virtual void LateUpdate() { }
        internal virtual void Destroy() { }
        internal virtual void Exit() { }
    }
}