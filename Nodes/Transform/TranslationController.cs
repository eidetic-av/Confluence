using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Transform/TranslationController"),
        NodeTint(Colors.ControllerTint)]
    public class TranslationController : RuntimeNode
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

        Vector3 Translation => new Vector3(X, Y, Z);

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

            var newPosition = Target.transform.position;
            newPosition.x = currentX;
            newPosition.y = currentY;
            newPosition.z = currentZ;

            Target.transform.position = newPosition;
        }

    }
}