using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/RountToInt")]
    public class RountToInt : RuntimeNode
    {
        [Input] public float Input;
        [Output] public int Output => UnityEngine.Mathf.RoundToInt(Input);
    }
}