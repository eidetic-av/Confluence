using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Function/InvokeMethod"),
        NodeTint(Colors.ControllerTint)]
    public class InvokeMethod : RuntimeNode
    {

        public static readonly float ValidationInterval = .5f;
        public float LastValidatedTime { get; private set; }

        [SerializeField]
        string _targetName;
        string TargetObjectName { set { Target = GameObject.Find(_targetName); } }
        string LastTargetObjectName;

        [SerializeField]
        string ComponentTypeName;
        string LastComponentTypeName;

        [SerializeField]
        string MethodName;
        string LastMethodName;

        Action<int> Method;

        int lastInput;
        [Input]
        public int Input { get; set; }

        public GameObject Target
        { get; private set; }

        protected override void Init()
        {
            TargetObjectName = LastTargetObjectName = _targetName;
            RefreshTargetValueSetter();
        }

        void OnValidate()
        {
            // if the game object name has changed, update the target GameObject
            // by setting the GameObjectName string
            var updatedTarget = false;
            if (_targetName != LastTargetObjectName)
            {
                TargetObjectName = LastTargetObjectName = _targetName;
                updatedTarget = true;
            }

            if (ComponentTypeName != LastComponentTypeName || MethodName != LastMethodName || updatedTarget)
                RefreshTargetValueSetter();

            LastValidatedTime = Time.time;
        }

        void RefreshTargetValueSetter()
        {
            var component = Target.GetComponent(ComponentTypeName);
            var method = component.GetType().GetMethod(MethodName);
            Method = (int value) => method.Invoke(component, new object[] { value });
        }

        override internal void Update()
        {
            if (lastInput != Input)
            {
                if (Method != null)
                    Method.Invoke(Input);
                lastInput = Input;
            }
        }

    }
}