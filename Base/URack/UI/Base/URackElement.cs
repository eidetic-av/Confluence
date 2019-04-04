using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public abstract class URackElement : VisualElement
    {
        public static List<URackElement> Elements {get; private set;} = new List<URackElement>();

        public bool Attached => RackContainer.AttachedElements.Contains(this);

        public URackElement() : base()
        {
            Elements.Add(this);
            RackContainer.AttachElement(this);
        }

        virtual public void OnAttach() { }
        virtual public void OnDetach() { }
    }
}