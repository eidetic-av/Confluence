using UnityEngine;

public class AutoRotator : MonoBehaviour
{

    public Vector3 Speed = new Vector3(20f, 30f, 50f);
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * Speed.x,
			Time.deltaTime * Speed.y,
			Time.deltaTime * Speed.z);
    }
}
