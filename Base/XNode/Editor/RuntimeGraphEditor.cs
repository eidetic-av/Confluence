using System;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeGraphEditor(typeof(RuntimeGraph))]
    public class RuntimeGraphEditor : NodeGraphEditor
    {
        RuntimeGraph Target;
        public override void OnGUI()
        {
            if (Target == null)
                Target = target as RuntimeGraph;
        }
    }
}