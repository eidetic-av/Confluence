using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/RoundToInt")]
    public class RoundToInt : RuntimeNode
    {
        [Input] public float Input;
        [Output] public int Output => UnityEngine.Mathf.RoundToInt(Input);
    }
}