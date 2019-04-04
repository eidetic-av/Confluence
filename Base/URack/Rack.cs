using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack
{
    [CreateAssetMenu, Serializable]
    public class Rack : ScriptableObject
    {
        /// <summary> All modules in the rack. <para/>
        /// See: <see cref="AddModule{T}"/> </summary>
        [SerializeField] public List<Module> Modules = new List<Module>();

        /// <summary> Add a module to the rack by type </summary>
        public T AddModule<T>() where T : Module
        {
            return AddModule(typeof(T)) as T;
        }

        /// <summary> Add a module to the rack by type </summary>
        public virtual Module AddModule(Type type)
        {
            Module module = ScriptableObject.CreateInstance(type) as Module;
            Modules.Add(module);
            module.Rack = this;
            return module;
        }

        /// <summary> Creates a copy of the original module in the rack</summary>
        public virtual Module CopyModule(Module original)
        {
            Module module = ScriptableObject.Instantiate(original);
            module.ClearConnections();
            Modules.Add(module);
            module.Rack = this;
            return module;
        }

        /// <summary> Safely remove a module and all its connections </summary>
        /// <param name="module"> The module to remove </param>
        public void RemoveModule(Module module)
        {
            module.ClearConnections();
            Modules.Remove(module);
            if (Application.isPlaying) Destroy(module);
        }

        /// <summary> Remove all modules and connections from the rack </summary>
        public void Clear()
        {
            if (Application.isPlaying) foreach (var module in Modules) Destroy(module);
            Modules.Clear();
        }

        /// <summary> Create a new deep copy of this rack </summary>
        public Rack Copy()
        {
            // Instantiate a new rack instance
            var rackCopy = Instantiate(this);
            // Instantiate all modules inside the graph
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] == null) continue;
                var module = Instantiate(Modules[i]);
                module.Rack = rackCopy;
                rackCopy.Modules[i] = module;
            }

            // Redirect all connections
            for (int i = 0; i < rackCopy.Modules.Count; i++)
            {
                if (rackCopy.Modules[i] == null) continue;
                foreach (Port port in rackCopy.Modules[i].Ports)
                {
                    port.Redirect(Modules, rackCopy.Modules);
                }
            }

            return rackCopy;
        }

        private void OnDestroy()
        {
            // Remove all nodes prior to graph destruction
            Clear();
        }
    }
}