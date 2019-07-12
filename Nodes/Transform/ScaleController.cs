using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Transform/ScaleController"),
        NodeTint(Colors.ControllerTint)]
    public class ScaleController : RuntimeNode
    {
        public static readonly float ValidationInterval = .5f;
        public float LastValidatedTime { get; private set; }

        [SerializeField]
        string _targetName;
        string TargetName { set { Target = GameObject.Find(_targetName); } }
        string LastTargetName;

        [Input] public float X;
        [Input] public float Y;
        [Input] public float Z;

        float currentX, currentY, currentZ;

        Vector3 Scale => new Vector3(X, Y, Z);

        [Input] public float DampingRate = 3f;

        public GameObject Target
        { get; private set; }

        internal override void Awake()
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

            if (Mathf.Abs(currentX - X) > 0.005f)
                currentX = currentX + (X - currentX) / DampingRate;
            else currentX = X;
            if (Mathf.Abs(currentY - Y) > 0.005f)
                currentY = currentY + (Y - currentY) / DampingRate;
            else currentY = Y;
            if (Mathf.Abs(currentZ - Z) > 0.005f)
                currentZ = currentZ + (Z - currentZ) / DampingRate;
            else currentZ = Z;

            var newScale = Target.transform.localScale;
            newScale.x = currentX;
            newScale.y = currentY;
            newScale.z = currentZ;

            Target.transform.localScale = newScale;
        }

    }
}