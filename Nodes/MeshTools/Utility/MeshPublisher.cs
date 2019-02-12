using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPublisher : MonoBehaviour
{
    public static Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
    string _id;
    public string Id;
    public bool Publish;
    Mesh Mesh;

    void OnValidate()
    {
        if (!Application.isPlaying) return;
        
        if (Publish)
        {
            if (Mesh)
                Meshes.Remove(_id);
            _id = Id;
            Mesh = GetComponent<MeshFilter>().mesh;
            Meshes.Add(Id, Mesh);
            Publish = false;    
        }
    }
}
