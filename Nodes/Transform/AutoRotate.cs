using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using Eidetic.Confluence;

[CreateNodeMenu("Transform/AutoRotate"),
    NodeTint(Colors.ControllerTint)]
public class AutoRotate : RuntimeNode
{
    public static readonly float ValidationInterval = .5f;
    public float LastValidatedTime { get; private set; }

    [SerializeField]
    string _targetName;
    string TargetName { set { Target = GameObject.Find(_targetName); } }
    string LastTargetName;

    [Input(ShowBackingValue.Always)]
    public float SpeedX, SpeedY, SpeedZ;


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
                    case "SpeedX":
                        float? speedX;
                        port.TryGetInputValue(out speedX);
                        if (speedX.HasValue)
                            SpeedX = speedX.Value;
                        break;
                    case "SpeedY":
                        float? speedY;
                        port.TryGetInputValue(out speedY);
                        if (speedY.HasValue)
                            SpeedY = speedY.Value;
                        break;
                    case "SpeedZ":
                        float? speedZ;
                        port.TryGetInputValue(out speedZ);
                        if (speedZ.HasValue)
                            SpeedZ = speedZ.Value;
                        break;
                }
            });

        // Rotate the target game object
        Target.transform.Rotate(
            Time.deltaTime * SpeedX,
            Time.deltaTime * SpeedY,
            Time.deltaTime * SpeedZ);
    }

    // Surely this process can be automated?!
    // using runtime reflection
    public override object GetValue(NodePort port)
    {
        switch (port.GetType().Name)
        {
            case "SpeedX":
                return SpeedX;
            case "SpeedY":
                return SpeedY;
            case "SpeedZ":
                return SpeedZ;
            default:
                return 0f;
        }
    }
}
