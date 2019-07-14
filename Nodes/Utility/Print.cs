using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Utility/Print")]
    public class Print : RuntimeNode
    {
        [Input] public float Input { get; set; }

        internal override void Update()
        {
            base.Update();
            UnityEngine.Debug.Log(Input);
        }
    }
}