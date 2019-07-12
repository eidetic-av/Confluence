using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Function/PropertyManipulator"),
        NodeTint(Colors.ControllerTint)]
    public class PropertyManipulator : RuntimeNode
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
        string PropertyName;
        string LastPropertyName;

        Action<float> Setter;

        [Input] public float Input;
        float currentInput;

        [Input] public float DampingRate = 3f;

        public GameObject Target
        { get; private set; }

        override protected void Init()
        {
            TargetObjectName = LastTargetObjectName = _targetName;
            Threads.RunAtStart(RefreshTargetValueSetter);
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

            if (ComponentTypeName != LastComponentTypeName || PropertyName != LastPropertyName || updatedTarget)
                RefreshTargetValueSetter();

            LastValidatedTime = Time.time;
        }

        void RefreshTargetValueSetter()
        {
            if (Target == null) return;
            var component = Target.GetComponent(ComponentTypeName);
            if (component == null) return;
            var property = component.GetType().GetProperty(PropertyName);
            if (property == null) return;
            var setMethod = property.GetSetMethod();
            if (setMethod == null) return;
            Setter = (float value) => setMethod.Invoke(component, new object[] { value });
        }

        // Update is called once per frame
        internal override void EarlyUpdate()
        {
            if (Mathf.Abs(currentInput - Input) > 0.005f)
                currentInput = currentInput + (Input - currentInput) / DampingRate;

            if (Setter != null)
                Setter.Invoke(currentInput);
        }

    }
}