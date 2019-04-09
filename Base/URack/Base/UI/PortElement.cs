using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Eidetic.Unity.UI.Utility;

namespace Eidetic.URack.UI
{
    public partial class PortElement : BindableElement
    {

        public PortElement() : base()
        {
        }
        public class Factory : UxmlFactory<PortElement, Traits>
        {

        }
        public class Traits : BindableElement.UxmlTraits
        {
            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(element, bag, context);
                var portElement = element as PortElement;
            }

            // No children allowed in this element
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }
        }
    }
}