using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public partial class PortElement : TouchElement
    {
        public class Factory : UxmlFactory<PortElement, Traits>
        {

        }
        public class Traits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(element, bag, context);
                var portElement = element as PortElement;

                // portElement.Port.Direction;
            }

            // No children allowed in this element
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }
        }
    }
}