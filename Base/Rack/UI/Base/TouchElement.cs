using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack.UI
{
    public class TouchElement : StyledElement
    {
        public Action<MouseDownEvent> OnTouch;
        public Action<MouseUpEvent> OnRelease;
        public Action<MouseMoveEvent> OnMove;

        public bool TouchActive { get; private set; } = false;
        public bool TouchMoving { get; private set; } = false;

        public TouchElement()
        {

            OnTouch = e => BaseTouchCallback(this, e);
            OnRelease = e => BaseReleaseCallback(this, e);
            OnMove = e => BaseMoveCallback(this, e);

            RegisterCallback<MouseDownEvent>(e => OnTouch.Invoke(e));
            RegisterCallback<MouseUpEvent>(e => OnRelease.Invoke(e));
            RegisterCallback<MouseMoveEvent>(e => OnMove.Invoke(e));
        }

        static void BaseTouchCallback(TouchElement element, MouseDownEvent mouseDownEvent)
        {
            element.TouchActive = true;
            element.AddToClassList("TouchActive");
        }
        static void BaseReleaseCallback(TouchElement element, MouseUpEvent mouseUpEvent)
        {
            element.TouchActive = false;
            element.TouchMoving = false;
            element.RemoveFromClassList("MouseDown");
            element.RemoveFromClassList("Moving");
        }
        static void BaseMoveCallback(TouchElement element, MouseMoveEvent mouseMoveEvent)
        {
            element.TouchMoving = true;
            element.AddToClassList("Moving");
        }
    }
}