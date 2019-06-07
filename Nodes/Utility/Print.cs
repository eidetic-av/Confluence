using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Utility/Print")]
    public class Print : RuntimeNode
    {
        [Input] public float Input { get; set; }

        internal override void LateUpdate()
        {
            UnityEngine.Debug.Log(Input);
        }
    }
}