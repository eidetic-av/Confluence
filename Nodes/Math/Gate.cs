using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Gate")]
    public class Gate : RuntimeNode
    {
        [Input] public float Input;
        [Input] public bool GateInput;

        [Input] public float Default = 0f;
        [Output] public float Output => GateInput ? Input : Default;
    }
}