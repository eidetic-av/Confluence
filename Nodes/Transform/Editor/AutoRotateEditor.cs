using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(AutoRotate))]
public class AutoRotateEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        base.OnBodyGUI();

        var mapNode = target as AutoRotate;

        // object value = mapNode.GetValue();
        // if (value != null)
        //     EditorGUILayout.LabelField(value.ToString());
    }
}