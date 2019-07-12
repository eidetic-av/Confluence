using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/LessThan")]
    public class LessThan : RuntimeNode
    {
        [Input] public float Input;
        [Input] public float Threshold = 1f;
        [Output] public bool Output => Input < Threshold ? true : false;
    }
}