using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class RuntimeGraph : NodeGraph
{
    public static bool NodeUpdaterInstantiated = false;

    void Awake()
    {
        InstantiateUpdater();
    }
    void OnEnable()
    {
        InstantiateUpdater();
    }

    void InstantiateUpdater()
    {
        // if (!NodeUpdaterInstantiated)
        // {
        //     GameObject heirarchyObject = new GameObject();
        //     heirarchyObject.name = "RuntimeGraph Node Updater";
        //     heirarchyObject.AddComponent(typeof(RuntimeNodeUpdater));
        //     if (GameObject.Find(heirarchyObject.name) == null)
        //         Instantiate(heirarchyObject);
        //     NodeUpdaterInstantiated = true;
        // }
    }
}