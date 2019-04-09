using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Eidetic.Utility;

namespace Eidetic.URack.UI
{
    public abstract class DraggableElement : TouchElement
    {
        public Action<MouseMoveEvent> OnDrag;
        public bool Dragging { get; protected set; } = false;

        public DraggableElement() : base()
        {
            // Drag occurs on entire Rack
            OnDrag = BaseDragCallback;
            if (this is RackElement)
                RegisterCallback<MouseMoveEvent>(e => OnDrag.Invoke(e));
            else RackElement.Instance.OnDrag += OnDrag;

            OnTouch += e => {
                RackElement.Instance.OnRelease += DragReleaseCallback;
            };
        }
        void BaseDragCallback(MouseMoveEvent mouseMoveEvent)
        {
            if (TouchActive)
            {
                Dragging = true;
                AddToClassList("Dragging");
            }
        }
        void DragReleaseCallback(MouseUpEvent mouseUpEvent)
        {
            Dragging = false;
            RemoveFromClassList("Dragging");
            RackElement.Instance.OnRelease -= DragReleaseCallback;
        }
    }
}