using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public abstract class DraggableElement : TouchElement
    {
        public Action<MouseMoveEvent> OnDrag;
        public bool Dragging { get; protected set; } = false;

        public DraggableElement() : base()
        {
            OnDrag = e => BaseDragCallback(this, e);
            OnRelease += e => DragReleaseCallback(this, e);
            if (this.GetType() != typeof(RackContainer))
                RackContainer.Instance.OnRelease += e => DragReleaseCallback(this, e);

            RegisterCallback<MouseMoveEvent>(e => OnDrag.Invoke(e));

        }
        static void BaseDragCallback(DraggableElement element, MouseMoveEvent mouseMoveEvent)
        {
            if (element.TouchActive)
            {
                element.Dragging = true;
                element.AddToClassList("Dragging");
            }
        }

        static void DragReleaseCallback(DraggableElement element, MouseUpEvent mouseUpEvent)
        {
            if (element.Dragging)
            {
                element.RemoveFromClassList("Dragging");
                element.Dragging = false;
            }
        }
    }
}