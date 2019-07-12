using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence {
    [CreateNodeMenu("Transform/RotationController"),
        NodeTint(Colors.ControllerTint)]
    public class RotationController : RuntimeNode
    {
        public static readonly float ValidationInterval = .5f;
        public float LastValidatedTime { get; private set; }

        [SerializeField]
        string _targetName;
        string TargetName { set { Target = GameObject.Find(_targetName); } }
        string LastTargetName;

        [Input(ShowBackingValue.Always)]
        public float XRotation, YRotation, ZRotation;

        float currentX, currentY, currentZ;
        
        [Input] public float DampingRate = 3f;

        public GameObject Target
        { get; private set; }

        protected override void Init()
        {
            TargetName = LastTargetName = _targetName;
        }

        void OnValidate()
        {
            // if the game object name has changed, update the target GameObject
            // by setting the GameObjectName string
            if (_targetName != LastTargetName)
                TargetName = LastTargetName = _targetName;

            LastValidatedTime = Time.time;
        }

        // Update is called once per frame
        internal override void Update()
        {
            if (Target == null) return;

            if (Mathf.Abs(currentX - XRotation) > 0.005f)
                currentX = currentX + (XRotation - currentX) / DampingRate;
            else currentX = XRotation;
            if (Mathf.Abs(currentY - YRotation) > 0.005f)
                currentY = currentY + (YRotation - currentY) / DampingRate;
            else currentY = YRotation;
            if (Mathf.Abs(currentZ - ZRotation) > 0.005f)
                currentZ = currentZ + (ZRotation - currentZ) / DampingRate;
            else currentZ = ZRotation;

            Target.transform.rotation = Quaternion.Euler(currentX, currentY, currentZ);
        }
    }
}