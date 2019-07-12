using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Loop")]
    public class Loop : RuntimeNode
    {
        [Input] public float Input;
        [Input] public float Minimum = 0f;
        [Input] public float Maximum = 1f;
        [Output] public float Output => UnityEngine.Mathf.Repeat(Input - Minimum, Maximum - Minimum) + Minimum;
    }
}