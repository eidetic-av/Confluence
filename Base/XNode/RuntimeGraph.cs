using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateAssetMenu]
    public class RuntimeGraph : NodeGraph
    {
        /// <summary> Add a node to the graph by type </summary>
        public override Node AddNode(Type type)
        {
            return base.AddNode(type);
        }
    }
}