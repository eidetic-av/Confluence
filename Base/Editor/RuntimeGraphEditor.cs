using System;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(RuntimeGraph))]
public class RuntimeGraphEditor : NodeGraphEditor
{
    RuntimeGraph Target;
    public override void OnGUI()
    {
        if (Target == null)
            Target = target as RuntimeGraph;
    }

    public Node AddNode(Type type)
    {
        Debug.Log("abababa");
        return null;
    }
}