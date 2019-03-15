using UnityEngine;
using UnityEngine.EventSystems;
using MidiJack;

public class RuntimeNodeUpdater : MonoBehaviour
{
    public static RuntimeNodeUpdater Instance;

    public void Awake()
    {
        Instance = this;
    }
    public void Update()
    {
        RuntimeNode.InstantiatedNodes.ForEachOnMain(n => n.ValueUpdate());
        RuntimeNode.InstantiatedNodes.ForEachOnMain(n => n.Update());
    }
    public void LateUpdate()
    {
        RuntimeNode.InstantiatedNodes.ForEachOnMain(n => n.LateUpdate());
    }
}