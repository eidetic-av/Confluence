using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(RuntimeGraphHolder))]
    public class RuntimeGraphHolderEditor : NodeEditor
    {
        // bool Initialised;
        // RuntimeGraphHolder Target;

        // public override void OnHeaderGUI()
        // {
        //     if (!Initialised)
        //     {
        //         Target = target as RuntimeGraphHolder;
        //         Initialised = true;
        //     }
        //     base.OnHeaderGUI();
        // }

        // public override void OnBodyGUI()
        // {
        //     Target.InletPorts.ForEach(inlet =>
        //         NodeEditorGUILayout.PortField(new GUIContent("Inlet"), inlet));
        //     Target.OutletPorts.ForEach(outlet =>
        //         NodeEditorGUILayout.PortField(new GUIContent("Outlet"), outlet));
        // }
    }
}