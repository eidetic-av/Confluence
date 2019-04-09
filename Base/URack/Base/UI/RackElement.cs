using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class RackElement : DraggableElement
    {
        public static RackElement Instance { get; private set; }
        public static RackElement Instantiate(Rack rack)
        {
            Instance = new RackElement(rack);
            return Instance;
        }

        public Rack Rack { get; private set; }

        public RackElement(Rack rack) : base()
        {
            Instance = this;
            Rack = rack;

            Instance.Add(ModuleElement.Create(ScriptableObject.CreateInstance<Function.Oscillator4D>()));
            Instance.Add(ModuleElement.Create(ScriptableObject.CreateInstance<Math.Map>()));
            Instance.Add(ModuleElement.Create(ScriptableObject.CreateInstance<Function.Oscillator4D>()));
            Instance.Add(ModuleElement.Create(ScriptableObject.CreateInstance<Math.Map>()));
            
            LoadStyleSheets(this, this.GetType());
        }

        public void Attach()
        {
            OnAttach();
            foreach (var moduleElement in Children())
                AttachModule(moduleElement as ModuleElement);
        }

        public void Detach()
        {
            OnDetach();
            foreach (var moduleElement in Children())
                AttachModule(moduleElement as ModuleElement);
        }

        public void AttachModule(ModuleElement moduleElement)
        {
            if (Instance.Contains(moduleElement))
                DetachModule(moduleElement);
            Add(moduleElement);
            moduleElement.OnAttach();
        }

        public void DetachModule(ModuleElement element)
        {
            if (!Instance.Contains(element)) return;
            Remove(element);
            OnDetach();
        }
    }
}