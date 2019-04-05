using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Eidetic.URack
{
    [Serializable]
    public class Port
    {
        public enum IO { Input, Output, Undefined }

        public int ConnectionCount { get { return connections.Count; } }
        /// <summary> Return the first non-null connection </summary>
        public Port Connection
        {
            get
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    if (connections[i] != null) return connections[i].Port;
                }
                return null;
            }
        }

        public IO direction { get { return Direction; } }
        public Module.ConnectionType connectionType { get { return ConnectionType; } }

        /// <summary> Is this port connected to anytihng? </summary>
        public bool IsConnected { get { return connections.Count != 0; } }
        public bool IsInput { get { return direction == IO.Input; } }
        public bool IsOutput { get { return direction == IO.Output; } }

        public string MemberName { get { return memberName; } }
        public MemberTypes MemberType { get { return memberType; } }
        public Module Module { get { return module; } }
        public Type ModuleType { get { return module.GetType(); } }
        public bool IsDynamic { get { return Dynamic; } }
        public bool IsStatic { get { return !Dynamic; } }
        public Type ValueType
        {
            get
            {
                if (valueType == null && !string.IsNullOrEmpty(typeQualifiedName)) valueType = Type.GetType(typeQualifiedName, false);
                return valueType;
            }
            set
            {
                valueType = value;
                if (value != null) typeQualifiedName = value.AssemblyQualifiedName;
            }
        }
        private Type valueType;

        [SerializeField] private string memberName;
        [SerializeField] private MemberTypes memberType;
        [SerializeField] private Module module;
        [SerializeField] private string typeQualifiedName;
        [SerializeField] private List<PortConnection> connections = new List<PortConnection>();
        [SerializeField] private IO Direction;
        [SerializeField] private Module.ConnectionType ConnectionType;
        [SerializeField] private bool Dynamic;

        /// <summary> Construct a static targetless moduleport. Used as a template. </summary>
        public Port(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                var fieldInfo = memberInfo as FieldInfo;
                memberName = fieldInfo.Name;
                memberType = MemberTypes.Field;
                ValueType = fieldInfo.FieldType;
                Dynamic = false;
                var attributes = fieldInfo.GetCustomAttributes(false);
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] is Module.InputAttribute)
                    {
                        Direction = IO.Input;
                        ConnectionType = (attributes[i] as Module.InputAttribute).connectionType;
                    }
                    else if (attributes[i] is Module.OutputAttribute)
                    {
                        Direction = IO.Output;
                        ConnectionType = (attributes[i] as Module.OutputAttribute).connectionType;
                    }
                }
            } else if (memberInfo.MemberType == MemberTypes.Property)
            {
                var propertyInfo = memberInfo as PropertyInfo;
                memberName = propertyInfo.Name;
                memberType = MemberTypes.Property;
                ValueType = propertyInfo.PropertyType;
                Dynamic = false;
                var attributes = propertyInfo.GetCustomAttributes(false);
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] is Module.InputAttribute)
                    {
                        Direction = IO.Input;
                        ConnectionType = (attributes[i] as Module.InputAttribute).connectionType;
                    }
                    else if (attributes[i] is Module.OutputAttribute)
                    {
                        Direction = IO.Output;
                        ConnectionType = (attributes[i] as Module.OutputAttribute).connectionType;
                    }
                }
            }
        }

        /// <summary> Copy a modulePort but assign it to another module. </summary>
        public Port(Port modulePort, Module module)
        {
            memberName = modulePort.memberName;
            memberType = modulePort.memberType;
            ValueType = modulePort.valueType;
            Direction = modulePort.direction;
            Dynamic = modulePort.Dynamic;
            ConnectionType = modulePort.ConnectionType;
            this.module = module;
        }

        /// <summary> Construct a dynamic port. Dynamic ports are not forgotten on reimport, and is ideal for runtime-created ports. </summary>
        public Port(string memberName, Type type, IO direction, Module.ConnectionType connectionType, Module module)
        {
            this.memberName = memberName;
            // MemberType = memberType;
            this.ValueType = type;
            Direction = direction;
            this.module = module;
            Dynamic = true;
            ConnectionType = connectionType;
        }

        /// <summary> Checks all connections for invalid references, and removes them. </summary>
        public void VerifyConnections()
        {
            for (int i = connections.Count - 1; i >= 0; i--)
            {
                if (connections[i].module != null &&
                    !string.IsNullOrEmpty(connections[i].MemberName) &&
                    connections[i].module.GetPort(connections[i].MemberName) != null)
                    continue;
                connections.RemoveAt(i);
            }
        }

        /// <summary> Return the output value of this module through its parent modules GetValue override method. </summary>
        /// <returns> <see cref="Module.GetValue(Port)"/> </returns>
        public object GetOutputValue()
        {
            if (direction == IO.Input) return null;
            return Module.GetValue(this);
        }

        /// <summary> Return the output value of the first connected port. Returns null if none found or invalid.</summary>
        /// <returns> <see cref="Port.GetOutputValue"/> </returns>
        public object GetInputValue()
        {
            Port connectedPort = Connection;
            if (connectedPort == null) return null;
            return connectedPort.GetOutputValue();
        }

        /// <summary> Return the output values of all connected ports. </summary>
        /// <returns> <see cref="Port.GetOutputValue"/> </returns>
        public object[] GetInputValues()
        {
            object[] objs = new object[ConnectionCount];
            for (int i = 0; i < ConnectionCount; i++)
            {
                Port connectedPort = connections[i].Port;
                if (connectedPort == null)
                { // if we happen to find a null port, remove it and look again
                    connections.RemoveAt(i);
                    i--;
                    continue;
                }
                objs[i] = connectedPort.GetOutputValue();
            }
            return objs;
        }

        /// <summary> Return the output value of the first connected port. Returns null if none found or invalid. </summary>
        /// <returns> <see cref="Port.GetOutputValue"/> </returns>
        public T GetInputValue<T>()
        {
            object obj = GetInputValue();
            return obj is T ? (T)obj : default(T);
        }

        /// <summary> Return the output values of all connected ports. </summary>
        /// <returns> <see cref="Port.GetOutputValue"/> </returns>
        public T[] GetInputValues<T>()
        {
            object[] objs = GetInputValues();
            T[] ts = new T[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] is T) ts[i] = (T)objs[i];
            }
            return ts;
        }

        /// <summary> Return true if port is connected and has a valid input. </summary>
        /// <returns> <see cref="Port.GetOutputValue"/> </returns>
        public bool TryGetInputValue<T>(out T value)
        {
            object obj = GetInputValue();
            if (obj is T)
            {
                value = (T)obj;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        /// <summary> Return the sum of all inputs. </summary>
        /// <returns> <see cref="Port.GetOutputValue"/> </returns>
        public float GetInputSum(float fallback)
        {
            object[] objs = GetInputValues();
            if (objs.Length == 0) return fallback;
            float result = 0;
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] is float) result += (float)objs[i];
            }
            return result;
        }

        /// <summary> Return the sum of all inputs. </summary>
        /// <returns> <see cref="Port.GetOutputValue"/> </returns>
        public int GetInputSum(int fallback)
        {
            object[] objs = GetInputValues();
            if (objs.Length == 0) return fallback;
            int result = 0;
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] is int) result += (int)objs[i];
            }
            return result;
        }

        /// <summary> Connect this <see cref="Port"/> to another </summary>
        /// <param name="port">The <see cref="Port"/> to connect to</param>
        public void Connect(Port port)
        {
            if (connections == null) connections = new List<PortConnection>();
            if (port == null) { Debug.LogWarning("Cannot connect to null port"); return; }
            if (port == this) { Debug.LogWarning("Cannot connect port to self."); return; }
            if (IsConnectedTo(port)) { Debug.LogWarning("Port already connected. "); return; }
            if (direction == port.direction) { Debug.LogWarning("Cannot connect two " + (direction == IO.Input ? "input" : "output") + " connections"); return; }
            if (port.connectionType == Module.ConnectionType.Override && port.ConnectionCount != 0) { port.ClearConnections(); }
            if (connectionType == Module.ConnectionType.Override && ConnectionCount != 0) { ClearConnections(); }
            connections.Add(new PortConnection(port));
            if (port.connections == null) port.connections = new List<PortConnection>();
            if (!port.IsConnectedTo(this)) port.connections.Add(new PortConnection(this));
            Module.OnCreateConnection(this, port);
            port.Module.OnCreateConnection(this, port);
        }

        public List<Port> GetConnections()
        {
            List<Port> result = new List<Port>();
            for (int i = 0; i < connections.Count; i++)
            {
                Port port = GetConnection(i);
                if (port != null) result.Add(port);
            }
            return result;
        }

        public Port GetConnection(int i)
        {
            //If the connection is broken for some reason, remove it.
            if (connections[i].module == null || string.IsNullOrEmpty(connections[i].MemberName))
            {
                connections.RemoveAt(i);
                return null;
            }
            Port port = connections[i].module.GetPort(connections[i].MemberName);
            if (port == null)
            {
                connections.RemoveAt(i);
                return null;
            }
            return port;
        }

        /// <summary> Get index of the connection connecting this and specified ports </summary>
        public int GetConnectionIndex(Port port)
        {
            for (int i = 0; i < ConnectionCount; i++)
            {
                if (connections[i].Port == port) return i;
            }
            return -1;
        }

        public bool IsConnectedTo(Port port)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].Port == port) return true;
            }
            return false;
        }

        /// <summary> Disconnect this port from another port </summary>
        public void Disconnect(Port port)
        {
            // Remove this ports connection to the other
            for (int i = connections.Count - 1; i >= 0; i--)
            {
                if (connections[i].Port == port)
                {
                    connections.RemoveAt(i);
                }
            }
            if (port != null)
            {
                // Remove the other ports connection to this port
                for (int i = 0; i < port.connections.Count; i++)
                {
                    if (port.connections[i].Port == this)
                    {
                        port.connections.RemoveAt(i);
                    }
                }
            }
            // Trigger OnRemoveConnection
            Module.OnRemoveConnection(this);
            if (port != null) port.Module.OnRemoveConnection(port);
        }

        /// <summary> Disconnect this port from another port </summary>
        public void Disconnect(int i)
        {
            // Remove the other ports connection to this port
            Port otherPort = connections[i].Port;
            if (otherPort != null)
            {
                for (int k = 0; k < otherPort.connections.Count; k++)
                {
                    if (otherPort.connections[k].Port == this)
                    {
                        otherPort.connections.RemoveAt(i);
                    }
                }
            }
            // Remove this ports connection to the other
            connections.RemoveAt(i);

            // Trigger OnRemoveConnection
            Module.OnRemoveConnection(this);
            if (otherPort != null) otherPort.Module.OnRemoveConnection(otherPort);
        }

        public void ClearConnections()
        {
            while (connections.Count > 0)
            {
                Disconnect(connections[0].Port);
            }
        }

        /// <summary> Get reroute points for a given connection. This is used for organization </summary>
        public List<Vector2> GetReroutePoints(int index)
        {
            return connections[index].reroutePoints;
        }

        /// <summary> Swap connections with another module </summary>
        public void SwapConnections(Port targetPort)
        {
            int aConnectionCount = connections.Count;
            int bConnectionCount = targetPort.connections.Count;

            List<Port> portConnections = new List<Port>();
            List<Port> targetPortConnections = new List<Port>();

            // Cache port connections
            for (int i = 0; i < aConnectionCount; i++)
                portConnections.Add(connections[i].Port);

            // Cache target port connections
            for (int i = 0; i < bConnectionCount; i++)
                targetPortConnections.Add(targetPort.connections[i].Port);

            ClearConnections();
            targetPort.ClearConnections();

            // Add port connections to targetPort
            for (int i = 0; i < portConnections.Count; i++)
                targetPort.Connect(portConnections[i]);

            // Add target port connections to this one
            for (int i = 0; i < targetPortConnections.Count; i++)
                Connect(targetPortConnections[i]);

        }

        /// <summary> Copy all connections pointing to a module and add them to this one </summary>
        public void AddConnections(Port targetPort)
        {
            int connectionCount = targetPort.ConnectionCount;
            for (int i = 0; i < connectionCount; i++)
            {
                PortConnection connection = targetPort.connections[i];
                Port otherPort = connection.Port;
                Connect(otherPort);
            }
        }

        /// <summary> Move all connections pointing to this module, to another module </summary>
        public void MoveConnections(Port targetPort)
        {
            int connectionCount = connections.Count;

            // Add connections to target port
            for (int i = 0; i < connectionCount; i++)
            {
                PortConnection connection = targetPort.connections[i];
                Port otherPort = connection.Port;
                Connect(otherPort);
            }
            ClearConnections();
        }

        /// <summary> Swap connected modules from the old list with modules from the new list </summary>
        public void Redirect(List<Module> oldModules, List<Module> newModules)
        {
            foreach (PortConnection connection in connections)
            {
                int index = oldModules.IndexOf(connection.module);
                if (index >= 0) connection.module = newModules[index];
            }
        }

        [Serializable]
        private class PortConnection
        {
            [SerializeField] public string MemberName;
            [SerializeField] public MemberTypes MemberType;
            [SerializeField] public Module module;
            public Port Port { get { return port != null ? port : port = GetPort(); } }

            [NonSerialized] private Port port;
            /// <summary> Extra connection path points for organization </summary>
            [SerializeField] public List<Vector2> reroutePoints = new List<Vector2>();

            public PortConnection(Port port)
            {
                this.port = port;
                module = port.Module;
                MemberName = port.memberName;
                MemberType = port.memberType;
            }

            /// <summary> Returns the port that this <see cref="PortConnection"/> points to </summary>
            private Port GetPort()
            {
                if (module == null || string.IsNullOrEmpty(MemberName)) return null;
                return module.GetPort(MemberName);
            }
        }
    }
}