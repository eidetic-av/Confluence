using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(DisplayFloat))]
public class DisplayFloatEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        base.OnBodyGUI();

        var displayFloatNode = target as DisplayFloat;

        object value = displayFloatNode.GetValue();
        if (value != null)
            EditorGUILayout.LabelField(value.ToString());
    }
}