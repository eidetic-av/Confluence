using System.Linq;
using UnityEngine;

namespace Eidetic.Confluence
{
    public class RuntimeNodeUpdater : MonoBehaviour
    {
        public static RuntimeNodeUpdater Instance { get; private set; }
        public static RuntimeNodeUpdater Instantiate()
        {
            MainThreadDispatcher.Instantiate();
            if (Instance != null) return Instance;
            else return Instance = new GameObject("RuntimeNodeUpdater").AddComponent<RuntimeNodeUpdater>();
        }

        public void Awake()
        {
            RuntimeNode.ActiveNodes.Clear();

            var activeGraphs = Resources.LoadAll<RuntimeGraph>("").Where(g => g.LoadOnPlay);

            activeGraphs.SelectMany(g => g.nodes)
                .Cast<RuntimeNode>().ToList()
                .ForEach(n => n.OnEnable());

            Threads.RunAtStart(() => RuntimeNode.ActiveNodes.ForEach(n => n.Awake()));
            Threads.RunAtStart(() => RuntimeNode.ActiveNodes.ForEach(n => n.Start()));
        }

        public void Update()
        {
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.ValueUpdate());
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.EarlyUpdate());
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.Update());

            // Todo: definitely move this elsewhere
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Input.GetKeyDown(KeyCode.F4)) Application.Quit();
            }
        }
        public void LateUpdate() => RuntimeNode.ActiveNodes.ForEachOnMain(n => n.LateUpdate());

        public void OnDestroy() => RuntimeNode.ActiveNodes.ForEach(n => n.OnDestroy());
    }
}