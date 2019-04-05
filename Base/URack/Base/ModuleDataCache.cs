using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Eidetic.URack
{
    /// <summary> Precaches reflection data in editor so we won't have to do it runtime </summary>
    public static class ModuleDataCache
    {
        private static PortDataCache portDataCache;
        private static bool Initialized { get { return portDataCache != null; } }

        /// <summary> Update static ports to reflect class fields. </summary>
        public static void UpdatePorts(Module module, Dictionary<string, Port> ports)
        {
            if (!Initialized) BuildCache();

            Dictionary<string, Port> staticPorts = new Dictionary<string, Port>();
            System.Type moduleType = module.GetType();

            List<Port> typePortCache;
            if (portDataCache.TryGetValue(moduleType, out typePortCache))
            {
                for (int i = 0; i < typePortCache.Count; i++)
                {
                    staticPorts.Add(typePortCache[i].MemberName, portDataCache[moduleType][i]);
                }
            }

            // Cleanup port dict - Remove nonexisting static ports - update static port types
            // Loop through current module ports
            foreach (Port port in ports.Values.ToList())
            {
                // If port still exists, check it it has been changed
                Port staticPort;
                if (staticPorts.TryGetValue(port.MemberName, out staticPort))
                {
                    // If port exists but with wrong settings, remove it. Re-add it later.
                    if (port.connectionType != staticPort.connectionType || port.IsDynamic || port.direction != staticPort.direction) ports.Remove(port.MemberName);
                    else port.ValueType = staticPort.ValueType;
                }
                // If port doesn't exist anymore, remove it
                else if (port.IsStatic) ports.Remove(port.MemberName);
            }
            // Add missing ports
            foreach (Port staticPort in staticPorts.Values)
            {
                if (!ports.ContainsKey(staticPort.MemberName))
                {
                    ports.Add(staticPort.MemberName, new Port(staticPort, module));
                }
            }
        }

        private static void BuildCache()
        {
            portDataCache = new PortDataCache();
            System.Type baseType = typeof(Module);
            List<System.Type> moduleTypes = new List<System.Type>();
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            Assembly selfAssembly = Assembly.GetAssembly(baseType);
            if (selfAssembly.FullName.StartsWith("Assembly-CSharp") && !selfAssembly.FullName.Contains("-firstpass"))
            {
                moduleTypes.AddRange(selfAssembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)));
            }
            for (int i = 0; i < moduleTypes.Count; i++)
            {
                CachePorts(moduleTypes[i]);
            }
        }

        private static void CachePorts(System.Type moduleType)
        {
            // Cache ports attributed to fields
            System.Reflection.MemberInfo[] fieldInfo = moduleType.GetFields();
            for (int i = 0; i < fieldInfo.Length; i++)
            {

                //Get InputAttribute and OutputAttribute
                object[] attributes = fieldInfo[i].GetCustomAttributes(false);
                Module.InputAttribute inputAttribute = attributes.FirstOrDefault(x => x is Module.InputAttribute) as Module.InputAttribute;
                Module.OutputAttribute outputAttribute = attributes.FirstOrDefault(x => x is Module.OutputAttribute) as Module.OutputAttribute;

                if (inputAttribute == null && outputAttribute == null) continue;

                if (inputAttribute != null && outputAttribute != null) Debug.LogError("Field " + fieldInfo[i].Name + " of type " + moduleType.FullName + " cannot be both input and output.");
                else
                {
                    if (!portDataCache.ContainsKey(moduleType)) portDataCache.Add(moduleType, new List<Port>());
                    portDataCache[moduleType].Add(new Port(fieldInfo[i]));
                }
            }
            // Cache ports that are attributed to properties
            System.Reflection.PropertyInfo[] propertyInfo = moduleType.GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {

                //Get InputPropertyAttribute and OutputAttribute
                object[] attributes = propertyInfo[i].GetCustomAttributes(false);
                var inputAttribute = attributes.FirstOrDefault(x => x is Module.InputAttribute) as Module.InputAttribute;
                var outputAttribute = attributes.FirstOrDefault(x => x is Module.OutputAttribute) as Module.OutputAttribute;

                if (inputAttribute == null && outputAttribute == null) continue;
                else
                {
                    if (!portDataCache.ContainsKey(moduleType)) portDataCache.Add(moduleType, new List<Port>());
                    portDataCache[moduleType].Add(new Port(propertyInfo[i]));
                }
            }
        }

        [System.Serializable]
        private class PortDataCache : Dictionary<System.Type, List<Port>>, ISerializationCallbackReceiver
        {
            [SerializeField] private List<System.Type> keys = new List<System.Type>();
            [SerializeField] private List<List<Port>> values = new List<List<Port>>();

            // save the dictionary to lists
            public void OnBeforeSerialize()
            {
                keys.Clear();
                values.Clear();
                foreach (var pair in this)
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }

            // load dictionary from lists
            public void OnAfterDeserialize()
            {
                this.Clear();

                if (keys.Count != values.Count)
                    throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

                for (int i = 0; i < keys.Count; i++)
                    this.Add(keys[i], values[i]);
            }
        }
    }
}