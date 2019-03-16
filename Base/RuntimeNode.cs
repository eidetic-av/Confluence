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

namespace Eidetic.Confluence
{
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

            InstantiatedNodes.ForEachOnMain(node =>
            {
                node.Start();

                // Run the property setters in the dictionary
                // with the backing field values to trigger any set behaviours
                // that might need to be completed in play mode
                foreach (var entry in node.Setters.ToList())
                    entry.Value(node.Getters[entry.Key]());
            });
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

            // cache the getters & setters
            Getters = new Dictionary<string, Func<object>>();
            Setters = new Dictionary<string, Action<object>>();

            foreach (var member in GetType().GetMembers().WithAttribute<NodePortAttribute>())
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    var field = member as FieldInfo;
                    Getters.Add(field.Name, () => field.GetValue(this));
                    Setters.Add(field.Name, (value) => field.SetValue(this, value));
                }
                else
                {
                    var property = member as PropertyInfo;
                    Getters.Add(property.Name, () => property.GetValue(this));
                    if (property.GetSetMethod() != null)
                        Setters.Add(property.Name, (value) => property.SetValue(this, value));
                }
            }

            if (Application.isPlaying) Start();
        }

        void OnDestroy()
        {
            InstantiatedNodes.Remove(this);
            if (Application.isPlaying) Exit();
        }

        public override object GetValue(NodePort port) => Getters[port.MemberName]();

        // Todo:
        // All input ports are updated every frame
        // Is that necessary if the values don't update?
        internal void ValueUpdate()
        {
            foreach (var port in Ports.Where(port => port.IsInput && port.IsConnected))
            {
                var inputValue = port.Connection.Node.GetValue(port.Connection);
                Setters[port.MemberName](inputValue);
            }
        }

        internal virtual void Start() { }
        internal virtual void Exit() { }
        internal virtual void EarlyUpdate() { }
        internal virtual void Update() { }
        internal virtual void LateUpdate() { }
    }
}