using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Utility/Print")]
    public class Print : RuntimeNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public float Input;

        public float GetValue()
        {
            return GetInputValue<float>("Input");
        }

        internal override void Update()
        {
            Debug.Log(GetValue());
        }
    }
}