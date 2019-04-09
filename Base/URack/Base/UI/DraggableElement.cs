using System;
using System.Collections.Generic;
using Eidetic.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class DraggableElement : TouchElement
    {
        Action<MouseMoveEvent> OnDrag;
        public bool Dragging { get; protected set; } = false;

        public DraggableElement() : base()
        {
            OnDrag = BaseDragCallback;
            // Drag event occurs on entire Rack
            if (this is RackElement)
                RegisterCallback<MouseMoveEvent>(e => { if (TouchActive) OnDrag.Invoke(e); });
            // The handlers for the instance drag are added in the touch
            // callback and removed in the release callback
            else OnTouch += e =>
            {
                RackElement.Instance.OnDrag += OnDrag;
                RackElement.Instance.OnRelease += DragReleaseCallback;
            };
        }
        public void AddDragAction(DraggableElement element, Action<MouseMoveEvent> action)
        {
            OnDrag += e => { if (element.Dragging) action.Invoke(e); };
        }
        void BaseDragCallback(MouseMoveEvent mouseMoveEvent)
        {
            Dragging = true;
            AddToClassList("Dragging");
        }
        void DragReleaseCallback(MouseUpEvent mouseUpEvent)
        {
            Dragging = false;
            RemoveFromClassList("Dragging");
            RackElement.Instance.OnRelease -= DragReleaseCallback;
            RackElement.Instance.OnDrag -= OnDrag;
        }
    }
}