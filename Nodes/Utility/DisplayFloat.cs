using UnityEngine;
using XNode;


[CreateNodeMenu("Utility/DisplayFloat")]
public class DisplayFloat : Node
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public float Input;

    public float GetValue()
    {
        return GetInputValue<float>("Input");
    }
}