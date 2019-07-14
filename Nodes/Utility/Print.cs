using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Utility/Print")]
    public class Print : RuntimeNode
    {
        [Input] public float Input { get; set; }

        internal override void LateUpdate()
        {
            base.LateUpdate();
            UnityEngine.Debug.Log(Input);
        }
    }
}