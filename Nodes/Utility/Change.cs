using UnityEngine;
using XNode;
using Eidetic.Confluence;


[CreateNodeMenu("Utility/Change")]
public class Change : RuntimeNode
{
    [Input] public float Input;

    [SerializeField] public bool output;
    [Output]
    public bool Output
    {
        get
        {
            if (output)
            {
                output = false;
                return true;
            }
            else return false;
        }
        set => output = value;
    }

    float lastInput;

    internal override void EarlyUpdate()
    {
        base.EarlyUpdate();
        if (Input != lastInput) Output = true;
    }

    internal override void LateUpdate()
    {
        base.LateUpdate();
        lastInput = Input;
    }
}