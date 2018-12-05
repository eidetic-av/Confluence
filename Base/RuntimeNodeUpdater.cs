using UnityEngine;

public class RuntimeNodeUpdater : MonoBehaviour{
    public static RuntimeNodeUpdater Instance;

    public void Awake() 
    {
        Instance = this;
    }
    public void Update() 
    {
        RuntimeNode.InstantiatedNodes.ForEach(n => n.ValueUpdate());
        RuntimeNode.InstantiatedNodes.ForEach(n => n.Update());
    }
    public void LateUpdate() 
    {
        RuntimeNode.InstantiatedNodes.ForEach(n => n.LateUpdate());
    }
}