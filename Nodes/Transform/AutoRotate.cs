using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Eidetic.Confluence;

[CreateNodeMenu("Transform/AutoRotate"),
    NodeTint(Colors.ControllerTint)]
public class AutoRotate : Node
{

    public GameObject Target;

    [Input(ShowBackingValue.Always)] public Vector3 Speed = new Vector3(20f, 30f, 50f);

    // Update is called once per frame
    void Update()
    {
        Target.transform.Rotate(
            Time.deltaTime * Speed.x,
			Time.deltaTime * Speed.y,
			Time.deltaTime * Speed.z);
    }

    public void SetSpeed(float x, float y, float z)
    {
        Speed = new Vector3(x, y, z);
    }

    public void SetSpeedX(float x)
    {
        Speed.x = x;
    }

    public void SetSpeedY(float y)
    {
        Speed.y = y;
    }

    public void SetSpeedZ(float z)
    {
        Speed.z = z;
    }
}
