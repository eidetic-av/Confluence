using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class StyledElement : URackElement
    {
        /// <summary>
        /// Load StyleSheet of name 'ThisElementType.uss' inside 'Resources'.
        /// <para>If the Element Type inherits from a base class, load it's base
        /// class StyleSheet (and it's n parents' StyleSheets).</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elementType"></param>
        static public void LoadStyleSheets(StyledElement element, Type elementType)
        {
            if (elementType.BaseType != null && elementType.BaseType != typeof(URackElement))
                LoadStyleSheets(element, elementType.BaseType);
            element.AddToClassList(elementType.Name);
            var styleSheet = Resources.Load<StyleSheet>(elementType.Name);
            if (styleSheet != null) element.styleSheets.Add(styleSheet);
        }

        static public void ClearStyleSheets(StyledElement element)
        {
            if (element.styleSheets != null) element.styleSheets.Clear();
            element.ClearClassList();   
        }

        override public void OnAttach() => LoadStyleSheets(this, this.GetType());
        override public void OnDetach() => ClearStyleSheets(this);
    }
}