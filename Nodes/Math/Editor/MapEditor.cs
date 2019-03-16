using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(Map))]
    public class MapEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            var mapNode = target as Map;

            object value = mapNode.GetValue();
            if (value != null)
                EditorGUILayout.LabelField(value.ToString());
        }
    }
}