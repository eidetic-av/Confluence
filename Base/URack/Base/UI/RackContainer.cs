using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class RackContainer : DraggableElement
    {
        public static RackContainer Instance { get; private set; }
        public static RackContainer Instantiate(Rack rack)
        {
            Instance = new RackContainer(rack);
            return Instance;
        }

        public static List<URackElement> AttachedElements { get; private set; } = new List<URackElement>();

        public Rack Rack { get; private set; }

        public RackContainer(Rack rack) : base()
        {
            Instance = this;
            Rack = rack;
            LoadModuleElements();
        }

        public void LoadModuleElements()
        {
            Clear();

            var firstRow = new RackRow();
            Add(firstRow);

            firstRow.Add(ModuleElement.Create(new Math.Map()));
            firstRow.Add(ModuleElement.Create(new Function.QuadLFO()));
        }

        public static void Attach()
        {
            foreach (URackElement element in URackElement.Elements)
                AttachElement(element);
        }

        public static void Detach()
        {
            foreach (URackElement element in URackElement.Elements)
                AttachElement(element);
        }

        public static void AttachElement(URackElement element)
        {
            if (AttachedElements.Contains(element)) return;
            AttachedElements.Add(element);
            element.OnAttach();
        }

        public static void DetachElement(URackElement element)
        {
            if (!AttachedElements.Contains(element)) return;
            AttachedElements.Remove(element);
            element.OnDetach();
        }
    }

    internal class RackRow : StyledElement
    {
        public RackRow() : base() { }
    }

    internal class NewModuleButton : TouchElement
    {

        public NewModuleButton() : base()
        {
            AddToClassList("NewModuleBlank");
            var text = new TextElement();
            text.text = "Add Module";
            Add(text);

            OnTouch += OpenModuleMenu;
        }

        void OpenModuleMenu(MouseDownEvent e)
        {
            Debug.Log("Add");
        }
    }
}