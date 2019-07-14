using System;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateAssetMenu]
    public class RuntimeGraph : NodeGraph
    {
        [SerializeField] bool active = false;
        public bool Active
        {
            get => active;
            set
            {
                active = value;
                if (active) OnActivate();
                else OnDeactivate();
            }
        }
        public Action OnActivate = () => { };
        public Action OnDeactivate = () => { };

        public void OnEnable()
        {
            if (Active) OnActivate();
        }

        /// <summary> Add a node to the graph by type </summary>
        public override Node AddNode(Type type)
        {
            return base.AddNode(type);
        }
    }
}