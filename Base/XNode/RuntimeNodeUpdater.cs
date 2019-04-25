using MidiJack;
using UnityEngine;
using UnityEngine.EventSystems;
using Eidetic.Unity.Utility;

namespace Eidetic.Confluence
{
    public class RuntimeNodeUpdater : MonoBehaviour
    {
        public static RuntimeNodeUpdater Instance { get; private set; }
        public static RuntimeNodeUpdater Instantiate()
        {
            MainThreadDispatcher.Instantiate();
            if (Instance != null) return Instance;
            else return Instance = new GameObject("RuntimeNodeUpdater")
              .WithHideFlags(HideFlags.NotEditable)
              .InDontDestroyMode()
              .AddComponent<RuntimeNodeUpdater>();
        }

        RuntimeNodeUpdater() {
        }

        public void Awake() => RuntimeNode.ActiveNodes.ForEachOnMain(n => n.Awake());
        public void Start() => RuntimeNode.ActiveNodes.ForEachOnMain(n => n.Start());
        public void Update()
        {
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.ValueUpdate());
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.EarlyUpdate());
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.Update());
        }
        public void LateUpdate() => RuntimeNode.ActiveNodes.ForEachOnMain(n => n.LateUpdate());
    }
}