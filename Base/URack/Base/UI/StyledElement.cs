using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class StyledElement : VisualElement
    {
        public Action OnAttach;
        public Action OnDetach;

        public StyledElement() : base()
        {
            OnAttach = () => LoadStyleSheets(this, this.GetType());
            OnDetach = () => ClearStyleSheets(this);
        }

        /// <summary>
        /// Load StyleSheet of name 'ThisElementType.uss' inside 'Resources'.
        /// <para>If the Element Type inherits from a base class, load it's base
        /// class StyleSheet (and it's n parents' StyleSheets).</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elementType"></param>
        static public void LoadStyleSheets(StyledElement element, Type elementType)
        {
            if (elementType.BaseType != null && elementType.BaseType != typeof(StyledElement) && elementType.BaseType != typeof(Module))
                LoadStyleSheets(element, elementType.BaseType);

            element.AddToClassList(elementType.Name);

            var styleSheet = Resources.Load<StyleSheet>(elementType.Name + "Style");
            if (styleSheet != null) element.styleSheets.Add(styleSheet);
        }

        static public void ClearStyleSheets(StyledElement element)
        {
            if (element.styleSheets != null) element.styleSheets.Clear();
            element.ClearClassList();
        }
    }
}