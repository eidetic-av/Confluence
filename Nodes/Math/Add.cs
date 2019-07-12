using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Add")]
    public class Add : RuntimeNode
    {
        [Input] public float InputA = 0f;
        [Input] public float InputB = 0f;
        [Output] public float Output => InputA + InputB;
    }
}