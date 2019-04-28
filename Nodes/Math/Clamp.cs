using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Clamp")]
    public class Clamp : RuntimeNode
    {
        [Input] public float Input;
        [Input] public float Minimum = 0f;
        [Input] public float Maximum = 1f;
        [Output] public float Output => UnityEngine.Mathf.Clamp(Input, Minimum, Maximum);
    }
}