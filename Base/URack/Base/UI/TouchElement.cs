using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Eidetic.Utility;

namespace Eidetic.URack.UI
{
    public class TouchElement : StyledElement
    {
        public Action<MouseDownEvent> OnTouch;
        public Action<MouseUpEvent> OnRelease;

        public bool TouchActive { get; private set; } = false;

        public TouchElement() : base()
        {
            OnTouch = BaseTouchCallback;
            RegisterCallback<MouseDownEvent>(e => OnTouch.Invoke(e));

            // Release occurs on the whole rack
            OnRelease = e => { };
            if (this is RackElement)
                RegisterCallback<MouseUpEvent>(e => OnRelease.Invoke(e));
        }

        void BaseTouchCallback(MouseDownEvent mouseDownEvent)
        {
            TouchActive = true;
            AddToClassList("Touch");
            RackElement.Instance.OnRelease += BaseReleaseCallback;
        }
        void BaseReleaseCallback(MouseUpEvent mouseUpEvent)
        {
            TouchActive = false;
            RemoveFromClassList("Touch");
            RackElement.Instance.OnRelease -= BaseReleaseCallback;
        }
    }
}