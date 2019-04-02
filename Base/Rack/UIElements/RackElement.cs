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
        static List<RackElement> InstantiatedElements;
        static RackElement()
        {
            InstantiatedElements = new List<RackElement>();
        }

        public Action<MouseDownEvent> OnMouseDown;
        public Action<MouseUpEvent> OnMouseUp;
        public Action<MouseMoveEvent> OnMouseDrag;

        public bool MouseDown {get; private set;}
        public bool MouseDragging {get; private set;}

        void Initialise()
        {
            // Stylesheets
            InstantiatedElements.Add(this);
            if (styleSheets != null) styleSheets.Clear();
            ClearClassList();
            LoadStyleSheets(this, this.GetType());

            // Actions
            OnMouseDown = mouseDownEvent => DefaultMouseDown(mouseDownEvent);
            OnMouseUp = mouseUpEvent => DefaultMouseUp(mouseUpEvent);
            OnMouseDrag = mouseMoveEvent => DefaultMouseDrag(mouseMoveEvent);
            
            RegisterCallback<MouseDownEvent>(e => OnMouseDown.Invoke(e));
            RegisterCallback<MouseUpEvent>(e => OnMouseUp.Invoke(e));
            RegisterCallback<MouseMoveEvent>(e => OnMouseDrag.Invoke(e));
        }

        public RackElement() => Initialise();

        public RackElement(RackContainer parentContainer)
        {
            Initialise();
            parentContainer.OnMouseUp += DefaultMouseUp;
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
            this.AddToClassList("MouseDown");
        }
        void DefaultMouseUp(MouseUpEvent mouseUpEvent)
        {
            MouseDown = false;
            MouseDragging = false;
            this.RemoveFromClassList("MouseDown");
            this.RemoveFromClassList("Dragging");
        }
        void DefaultMouseDrag(MouseMoveEvent mouseMoveEvent)
        {
            if (MouseDown) {
                MouseDragging = true;
                this.AddToClassList("Dragging");
            }
        }
    }
}