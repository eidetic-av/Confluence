using UnityEngine;
using XNode;
using Eidetic.Confluence;


[CreateNodeMenu("Utility/Trigger")]
public class Trigger : RuntimeNode
{
    [Input] public bool Input;

    [SerializeField] bool output;
    [Output]
    public bool Output
    {
        get
        {
            if (readyToOutput && Input)
                return true;
            else return false;
        }
    }

    bool readyToOutput;
    bool lastInput;

    internal override void EarlyUpdate()
    {
        base.EarlyUpdate();
        if (!readyToOutput)
        {
            if (lastInput && !Input)
                readyToOutput = true;
        } else
        {
            if (lastInput && Input)
                readyToOutput = false;
        }
    }

    internal override void LateUpdate()
    {
        base.LateUpdate();
        lastInput = Input;
    }
}