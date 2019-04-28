using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Add")]
    public class Add : RuntimeNode
    {
        [Input] public float InputA;
        [Input] public float InputB;
        [Output] public float Output => InputA + InputB;
    }
}