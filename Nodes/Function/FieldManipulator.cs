using System;
using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Function/FieldManipulator"),
        NodeTint(Colors.ControllerTint)]
    public class FieldManipulator : RuntimeNode
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

        protected override void Init()
        {
            TargetObjectName = LastTargetObjectName = _targetName;
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
            {
                var component = Target.GetComponent(ComponentTypeName);
                var property = component.GetType().GetProperty(PropertyName);
                var setMethod = property.GetSetMethod();
                Setter = (float value) => setMethod.Invoke(component, new object[] { value });
            }

            LastValidatedTime = Time.time;
        }

        // Update is called once per frame
        internal override void Update()
        {
            if (Target == null) return;

            if (Mathf.Abs(currentInput - Input) > 0.005f)
                currentInput = currentInput + (Input - currentInput) / DampingRate;

            if (Setter != null)
                Setter.Invoke(currentInput);
        }

    }
}