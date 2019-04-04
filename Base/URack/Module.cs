using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eidetic.Unity.Utility;
using Eidetic.Utility;
using UnityEditor;
using UnityEngine;

namespace Eidetic.URack
{
    [InitializeOnLoad]
    public abstract partial class Module : ScriptableObject
    {
        public static List<Module> ActiveModules { get; protected set; }
        static Module()
        {
            ActiveModules = new List<Module>();
            EditorApplication.playModeStateChanged += (stateChange) =>
            {
                if (stateChange == PlayModeStateChange.EnteredPlayMode) OnPlay();
                else if (stateChange == PlayModeStateChange.ExitingPlayMode) OnExit();
            };
        }
        private static void OnPlay() => RackRuntimeUpdater.Instantiate();
        private static void OnExit() => ActiveModules.ForEachOnMain(n => n.Exit());
        
        
         [SerializeField] private PortDictionary ports = new PortDictionary();
        public IEnumerable<Port> Ports { get { foreach (Port port in ports.Values) yield return port; } }
        public IEnumerable<Port> Outputs { get { foreach (Port port in Ports) { if (port.IsOutput) yield return port; } } }
        public IEnumerable<Port> Inputs { get { foreach (Port port in Ports) { if (port.IsInput) yield return port; } } }
        public IEnumerable<Port> InstancePorts { get { foreach (Port port in Ports) { if (port.IsDynamic) yield return port; } } }
        /// <summary> Iterate over all instance outputs on this node. </summary>
        public IEnumerable<Port> InstanceOutputs { get { foreach (Port port in Ports) { if (port.IsDynamic && port.IsOutput) yield return port; } } }
        public IEnumerable<Port> InstanceInputs { get { foreach (Port port in Ports) { if (port.IsDynamic && port.IsInput) yield return port; } } }

        /// <summary> Parent rack <see cref="Rack"/> </summary>
        [SerializeField] public Rack Rack;

        /// <summary> Position of the module on the <see cref="Rack"/> </summary>
        [SerializeField] public Vector2Int Position;

        public Dictionary<string, Func<object>> Getters { get; private set; }
        public Dictionary<string, Action<object>> Setters { get; private set; }
        public Dictionary<string, FieldInfo> BackingFields { get; private set; }

        public void OnEnable()
        {
            UpdateStaticPorts();
            Init();
            if (ActiveModules.Contains(this)) return;

            ActiveModules.Add(this);

            Getters = new Dictionary<string, Func<object>>();
            Setters = new Dictionary<string, Action<object>>();
            BackingFields = new Dictionary<string, FieldInfo>();

            foreach (var member in GetType().GetMembers().WithAttribute<PortAttribute>())
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
            ActiveModules.Remove(this);
            Destroy();
            if (Application.isPlaying) Exit();
        }

        public virtual void OnCreateConnection(Port from, Port to)
        {
            RunSetters();
        }

        public virtual void OnRemoveConnection(Port removedPort)
        {
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

        public object GetValue(Port port) => Getters[port.MemberName]();
        internal void ValueUpdate()
        {
            foreach (var port in Ports.Where(port => port.IsInput && port.IsConnected))
                Setters[port.MemberName].Invoke(port.Connection.Module.GetValue(port.Connection));
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

        
        /// <summary> Update static ports to reflect class fields. This happens automatically on enable. </summary>
        public void UpdateStaticPorts()
        {
            ModuleDataCache.UpdatePorts(this, ports);
        }

        /// <summary> Initialize node. Called on creation. </summary>
        protected virtual void Init() { }

        /// <summary> Checks all connections for invalid references, and removes them. </summary>
        public void VerifyConnections()
        {
            foreach (Port port in Ports) port.VerifyConnections();
        }

        public Port AddInstanceInput(Type type, Module.ConnectionType connectionType = Module.ConnectionType.Multiple, string fieldName = null)
        {
            return AddInstancePort(type, Port.IO.Input, connectionType, fieldName);
        }
        public Port AddInstanceOutput(Type type, Module.ConnectionType connectionType = Module.ConnectionType.Multiple, string fieldName = null)
        {
            return AddInstancePort(type, Port.IO.Output, connectionType, fieldName);
        }

        /// <summary> Add a dynamic, serialized port to this Module. </summary>
        /// <seealso cref="AddInstanceInput"/>
        /// <seealso cref="AddInstanceOutput"/>
        private Port AddInstancePort(Type type, Port.IO direction, Module.ConnectionType connectionType = Module.ConnectionType.Multiple, string fieldName = null)
        {
            if (fieldName == null)
            {
                fieldName = "instanceInput_0";
                int i = 0;
                while (HasPort(fieldName)) fieldName = "instanceInput_" + (++i);
            }
            else if (HasPort(fieldName))
            {
                Debug.LogWarning("Port '" + fieldName + "' already exists in " + name, this);
                return ports[fieldName];
            }
            Port port = new Port(fieldName, type, direction, connectionType, this);
            ports.Add(fieldName, port);
            return port;
        }

        /// <summary> Remove an instance port from the Module </summary>
        public void RemoveInstancePort(string fieldName)
        {
            RemoveInstancePort(GetPort(fieldName));
        }

        /// <summary> Remove an instance port from the Module </summary>
        public void RemoveInstancePort(Port port)
        {
            if (port == null) throw new ArgumentNullException("port");
            else if (port.IsStatic) throw new ArgumentException("cannot remove static port");
            port.ClearConnections();
            ports.Remove(port.MemberName);
        }

        /// <summary> Removes all instance ports from the Module </summary>
        [ContextMenu("Clear Instance Ports")]
        public void ClearInstancePorts()
        {
            List<Port> instancePorts = new List<Port>(InstancePorts);
            foreach (Port port in instancePorts)
            {
                RemoveInstancePort(port);
            }
        }

        #region Ports
        /// <summary> Returns output port which matches memberName </summary>
        public Port GetOutputPort(string memberName)
        {
            Port port = GetPort(memberName);
            if (port == null || port.direction != Port.IO.Output) return null;
            else return port;
        }

        /// <summary> Returns input port which matches memberName </summary>
        public Port GetInputPort(string memberName)
        {
            Port port = GetPort(memberName);
            if (port == null || port.direction != Port.IO.Input) return null;
            else return port;
        }

        /// <summary> Returns port which matches memberName </summary>
        public Port GetPort(string memberName)
        {
            Port port;
            if (ports.TryGetValue(memberName, out port)) return port;
            else return null;
        }

        public bool HasPort(string memberName)
        {
            return ports.ContainsKey(memberName);
        }
        #endregion

        #region Inputs/Outputs
        /// <summary> Return input value for a specified port. Returns fallback value if no ports are connected </summary>
        /// <param name="fieldName">Field name of requested input port</param>
        /// <param name="fallback">If no ports are connected, this value will be returned</param>
        public T GetInputValue<T>(string fieldName, T fallback = default(T))
        {
            Port port = GetPort(fieldName);
            if (port != null && port.IsConnected) return port.GetInputValue<T>();
            else return fallback;
        }

        /// <summary> Return all input values for a specified port. Returns fallback value if no ports are connected </summary>
        /// <param name="fieldName">Field name of requested input port</param>
        /// <param name="fallback">If no ports are connected, this value will be returned</param>
        public T[] GetInputValues<T>(string fieldName, params T[] fallback)
        {
            Port port = GetPort(fieldName);
            if (port != null && port.IsConnected) return port.GetInputValues<T>();
            else return fallback;
        }
        #endregion

        /// <summary> Disconnect everything from this Module </summary>
        public void ClearConnections()
        {
            foreach (Port port in Ports) port.ClearConnections();
        }

        public override int GetHashCode()
        {
            return JsonUtility.ToJson(this).GetHashCode();
        }

        /// <summary> Used by <see cref="InputAttribute"/> and <see cref="OutputAttribute"/> to determine when to display the field value associated with a <see cref="Port"/> </summary>
        public enum ShowBackingValue
        {
            /// <summary> Never show the backing value </summary>
            Never,
            /// <summary> Show the backing value only when the port does not have any active connections </summary>
            Unconnected,
            /// <summary> Always show the backing value </summary>
            Always
        }

        public enum ConnectionType
        {
            /// <summary> Allow multiple connections</summary>
            Multiple,
            /// <summary> always override the current connection </summary>
            Override,
        }
        public class PortAttribute : Attribute { }

        /// <summary> Mark a field or property as an input port. You can access this through <see cref="GetInputPort(string)"/> </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
        public class InputAttribute : PortAttribute
        {
            public ShowBackingValue backingValue;
            public ConnectionType connectionType;
            public bool instancePortList;

            /// <summary> Mark a serializable field as an input port. You can access this through <see cref="GetInputPort(string)"/> </summary>
            /// <param name="backingValue">Should we display the backing value for this port as an editor field? </param>
            /// <param name="connectionType">Should we allow multiple connections? </param>
            public InputAttribute(ShowBackingValue backingValue = ShowBackingValue.Unconnected, ConnectionType connectionType = ConnectionType.Multiple, bool instancePortList = false)
            {
                this.backingValue = backingValue;
                this.connectionType = connectionType;
                this.instancePortList = instancePortList;
            }
        }

        /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputPort(string)"/> </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
        public class OutputAttribute : PortAttribute
        {
            public ShowBackingValue backingValue;
            public ConnectionType connectionType;
            public bool instancePortList;

            /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputPort(string)"/> </summary>
            /// <param name="backingValue">Should we display the backing value for this port as an editor field? </param>
            /// <param name="connectionType">Should we allow multiple connections? </param>
            public OutputAttribute(ShowBackingValue backingValue = ShowBackingValue.Never, ConnectionType connectionType = ConnectionType.Multiple, bool instancePortList = false)
            {
                this.backingValue = backingValue;
                this.connectionType = connectionType;
                this.instancePortList = instancePortList;
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class CreateNodeMenuAttribute : Attribute
        {
            public string menuName;
            /// <summary> Manually supply node class with a context menu path </summary>
            /// <param name="menuName"> Path to this node in the context menu. Null or empty hides it. </param>
            public CreateNodeMenuAttribute(string menuName)
            {
                this.menuName = menuName;
            }
        }

        [Serializable]
        private class PortDictionary : Dictionary<string, Port>, ISerializationCallbackReceiver
        {
            [SerializeField] private List<string> keys = new List<string>();
            [SerializeField] private List<Port> values = new List<Port>();

            public void OnBeforeSerialize()
            {
                keys.Clear();
                values.Clear();
                foreach (KeyValuePair<string, Port> pair in this)
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }
            public void OnAfterDeserialize()
            {
                this.Clear();
                if (keys.Count != values.Count)
                    throw new System.Exception("there are " + keys.Count + " keys and " + values.Count + " values after deserialization. Make sure that both key and value types are serializable.");
                for (int i = 0; i < keys.Count; i++)
                    this.Add(keys[i], values[i]);
            }
        }
    }
}