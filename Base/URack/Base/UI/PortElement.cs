using System;
using System.Collections.Generic;
using Eidetic.Utility;
using Eidetic.Unity.UI.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class PortElement : BindableElement
    {

        public PortElement() : base() { }

        public class Factory : UxmlFactory<PortElement, Traits>
        {

        }
        public class Traits : BindableElement.UxmlTraits
        {
            UxmlBoolAttributeDescription showLabelAttribute = new UxmlBoolAttributeDescription { name = "showLabel" };
            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(element, bag, context);
                var port = element as PortElement;

                port.Add(new VisualElement().WithName("Port"));

                var showLabel = true;
                showLabelAttribute.TryGetValueFromBag(bag, context, ref showLabel);
                if (showLabel && !port.name.IsNullOrEmpty())
                    port.Add(new TextElement().WithText(port.name).WithName("Label"));
            }

            // No children allowed in this element
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }
        }
    }
}