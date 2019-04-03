using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.Confluence.Base
{
    public abstract class RackElement : VisualElement
    {
        public Action<MouseDownEvent> OnMouseDown;
        public Action<MouseUpEvent> OnMouseUp;
        public Action<MouseMoveEvent> OnMouseDrag;

        public bool MouseDown { get; private set; } = false;
        public bool MouseDragging { get; private set; } = false;

        public RackElement()
        {
            if (styleSheets != null) styleSheets.Clear();
            ClearClassList();
            LoadStyleSheets(this, this.GetType());

            if (this.GetType() != typeof(RackContainer))
                RackContainer.Instance.OnMouseUp += DefaultMouseUp;

            OnMouseDown = e => DefaultMouseDown(e);
            OnMouseUp = e => DefaultMouseUp(e);
            OnMouseDrag = e => DefaultMouseDrag(e);

            RegisterCallback<MouseDownEvent>(e => OnMouseDown.Invoke(e));
            RegisterCallback<MouseUpEvent>(e => OnMouseUp.Invoke(e));
            RegisterCallback<MouseMoveEvent>(e => OnMouseDrag.Invoke(e));
        }

        public static void LoadStyleSheets(RackElement element, Type elementType)
        {
            // Load stylesheets in order of inheritance, recursive up until we
            // get to the RackElement Type
            if (elementType.BaseType != null && elementType.BaseType != typeof(RackElement))
                LoadStyleSheets(element, elementType.BaseType);

            element.AddToClassList(elementType.Name);
            var styleSheet = Resources.Load<StyleSheet>(elementType.Name);
            if (styleSheet != null) element.styleSheets.Add(styleSheet);
        }

        void DefaultMouseDown(MouseDownEvent mouseDownEvent)
        {
            MouseDown = true;
            AddToClassList("MouseDown");
        }
        void DefaultMouseUp(MouseUpEvent mouseUpEvent)
        {
            MouseDown = false;
            MouseDragging = false;
            RemoveFromClassList("MouseDown");
            RemoveFromClassList("Dragging");
        }
        void DefaultMouseDrag(MouseMoveEvent mouseMoveEvent)
        {
            if (MouseDown)
            {
                MouseDragging = true;
                AddToClassList("Dragging");
            }
        }
    }
}