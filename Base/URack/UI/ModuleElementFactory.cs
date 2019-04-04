using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[assembly: UxmlNamespacePrefix("Eidetic.URack.UI", "URack")]

namespace Eidetic.URack.UI
{
    public partial class ModuleElement : DraggableElement
    {
        public class Factory : UxmlFactory<ModuleElement, Traits>
        {

        }
        public class Traits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement element, IUxmlAttributes attributes, CreationContext context)
            {
                base.Init(element, attributes, context);
            }
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get
                {
                    yield return new UxmlChildElementDescription(typeof(VisualElement));
                }
            }
        }
    }
}