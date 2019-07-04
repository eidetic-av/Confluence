using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Damp")]
    public class Damp : RuntimeNode
    {
        [Input] public float Input;

        [Input] public float DampingRate = 15f;

        [Output] public float Output;

        internal override void Update()
        {
            if (Mathf.Abs(Output - Input) > 0.005f)
                Output = Output + (Input - Output) / DampingRate;
        }
    }
}
