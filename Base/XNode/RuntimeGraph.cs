using System;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateAssetMenu]
    public class RuntimeGraph : NodeGraph
    {
        public bool LoadOnPlay = false;
        /// <summary> Add a node to the graph by type </summary>
        public override Node AddNode(Type type)
        {
            return base.AddNode(type);
        }
    }
}