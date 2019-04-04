using MidiJack;
using UnityEngine;
using UnityEngine.EventSystems;
using Eidetic.Unity.Utility;

namespace Eidetic.URack
{
    public class RackRuntimeUpdater : MonoBehaviour
    {
        public static RackRuntimeUpdater Instance { get; private set; }
        public static RackRuntimeUpdater Instantiate()
        {
            MainThreadDispatcher.Instantiate();
            if (Instance != null) return Instance;
            else return Instance = new GameObject("RuntimeModuleUpdater")
                .WithHideFlags(HideFlags.NotEditable)
                .InDontDestroyMode()
                .AddComponent<RackRuntimeUpdater>();
        }
        public void Awake() => Module.ActiveModules.ForEachOnMain(module => module.Awake());
        public void Start() => Module.ActiveModules.ForEachOnMain(module => module.Start());
        public void Update()
        {
            Module.ActiveModules.ForEachOnMain(module => module.ValueUpdate());
            Module.ActiveModules.ForEachOnMain(module => module.EarlyUpdate());
            Module.ActiveModules.ForEachOnMain(module => module.Update());
        }
        public void LateUpdate() => Module.ActiveModules.ForEachOnMain(module => module.LateUpdate());
    }
}