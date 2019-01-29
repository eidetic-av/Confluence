using UnityEngine;
using UnityEngine.EventSystems;
using MidiJack;

// [ExecuteInEditMode]
public class RuntimeNodeUpdater : MonoBehaviour, IPointerDownHandler{
    public static RuntimeNodeUpdater Instance;

    public void Awake() 
    {
        Instance = this;
    }
    public void Update() 
    {
        MidiDriver.Instance.Update();
        RuntimeNode.InstantiatedNodes.ForEach(n => n.ValueUpdate());
        RuntimeNode.InstantiatedNodes.ForEach(n => n.Update());
    }
    public void LateUpdate() 
    {
        RuntimeNode.InstantiatedNodes.ForEach(n => n.LateUpdate());
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("pionter");
    }
}