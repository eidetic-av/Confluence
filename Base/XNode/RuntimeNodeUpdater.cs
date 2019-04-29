
using MidiJack;
using UnityEngine;
using UnityEngine.EventSystems;
using Eidetic.Unity.Utility;
using UnityEditor;

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
              .AddComponent<RuntimeNodeUpdater>();
        }

        RuntimeNodeUpdater()
        {
        }

        public void Awake()
        {
            var graph = Resources.Load<RuntimeGraph>("TrickFilm");
            foreach (var node in graph.nodes)
                ((RuntimeNode)node).OnEnable();
                
            Threads.RunAtStart(() =>
            {
                RuntimeNode.ActiveNodes.ForEach(n => n.Awake());
            });
        }

        public void Start()
        {
            Threads.RunOnMain(() =>
            {
                RuntimeNode.ActiveNodes.ForEach(n => n.Start());
            });
        }
        public void Update()
        {
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.ValueUpdate());
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.EarlyUpdate());
            RuntimeNode.ActiveNodes.ForEachOnMain(n => n.Update());

            // Todo: definitely move this elsewhere
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Input.GetKeyDown(KeyCode.F4))
                    Application.Quit();
            }
        }
        public void LateUpdate() => RuntimeNode.ActiveNodes.ForEachOnMain(n => n.LateUpdate());

        public void OnDestroy()
        {
            foreach (var node in RuntimeNode.ActiveNodes)
                node.OnDestroy();
        }
    }
}