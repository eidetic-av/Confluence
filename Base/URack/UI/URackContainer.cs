using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class URackContainer : DraggableElement
    {
        public static URackContainer Instance {get; private set;}
        public static URackContainer Instantiate(Rack rack)
        {
            Instance = new URackContainer(rack);
            return Instance;
        }

        public static List<URackElement> AttachedElements { get; private set; } = new List<URackElement>();

        public Rack Rack { get; private set; }

        public URackContainer(Rack rack) : base()
        {
            Instance = this;
            Rack = rack;

            var firstRow = new RackRow();
            firstRow.Add(new ModuleElement());

            Add(firstRow);
            
            Add(new NewModuleButton());
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
        public RackRow() : base()
        { }
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