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
        static URackContainer instance;
        public static URackContainer Instance
        {
            get
            {
                if (instance == null) new URackContainer();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public static List<URackElement> AttachedElements { get; private set; } = new List<URackElement>();

        public URackContainer() : base()
        {
            Instance = this;
            var firstRow = new RackRow();
            firstRow.Add(new ModuleElement());
            var wide = new ModuleElement();
            wide.AddToClassList("WideTest");
            firstRow.Add(wide);
            firstRow.Add(new ModuleElement());
            firstRow.Add(new ModuleElement());

            var mid = new ModuleElement();
            mid.AddToClassList("MidTest");
            firstRow.Add(mid);

            Add(firstRow);

            var secondRow = new RackRow();
            var wide2 = new ModuleElement();
            wide2.AddToClassList("WideTest");
            secondRow.Add(wide2);
            secondRow.Add(new ModuleElement());
            secondRow.Add(new ModuleElement());
            var wide3 = new ModuleElement();
            wide3.AddToClassList("WideTest");
            secondRow.Add(wide3);
            var wide4 = new ModuleElement();
            wide4.AddToClassList("WideTest");
            secondRow.Add(wide4);

            Add(secondRow);

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
        {
        }
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