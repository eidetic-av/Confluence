using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using Eidetic.Confluence;

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
    public override void Update()
    {
        if (Target == null) return;

        // Update values from each input port
        this.Ports.Where(port => port.IsInput).ToList()
            .ForEach((NodePort port) =>
            {
                switch (port.fieldName)
                {
                    case "XRotation":
                        float? xRotation;
                        port.TryGetInputValue(out xRotation);
                        if (xRotation.HasValue)
                            XRotation = xRotation.Value;
                        break;
                    case "YRotation":
                        float? yRotation;
                        port.TryGetInputValue(out yRotation);
                        if (yRotation.HasValue)
                            YRotation = yRotation.Value;
                        break;
                    case "ZRotation":
                        float? zRotation;
                        port.TryGetInputValue(out zRotation);
                        if (zRotation.HasValue)
                            ZRotation = zRotation.Value;
                        break;
                }
            });

        // Rotate the target game object
        Target.transform.SetPositionAndRotation(
            Target.transform.position,
            Quaternion.Euler(
                XRotation, YRotation, ZRotation
            )
        );
    }

    // Surely this process can be automated?!
    // using runtime reflection
    public override object GetValue(NodePort port)
    {
        switch (port.GetType().Name)
        {
            case "XRotation":
                return XRotation;
            case "YRotation":
                return YRotation;
            case "ZRotation":
                return ZRotation;
            default:
                return 0f;
        }
    }
}
