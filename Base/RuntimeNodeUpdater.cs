using UnityEngine;
using UnityEngine.EventSystems;
using MidiJack;

namespace Eidetic.Confluence
{
    public class RuntimeNodeUpdater : MonoBehaviour
    {
        public static RuntimeNodeUpdater Instance;

        public void Awake()
        {
            Instance = this;
        }
        public void Update()
        {
            // Todo:
            // Investigate the sync nature of this queing...
            // Make sure it all occurs in this order
            RuntimeNode.InstantiatedNodes.ForEachOnMain(n => n.ValueUpdate());
            RuntimeNode.InstantiatedNodes.ForEachOnMain(n => n.EarlyUpdate());
            RuntimeNode.InstantiatedNodes.ForEachOnMain(n => n.Update());
        }
        public void LateUpdate()
        {
            RuntimeNode.InstantiatedNodes.ForEachOnMain(n => n.LateUpdate());
        }
    }
}