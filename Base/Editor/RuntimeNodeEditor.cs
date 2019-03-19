using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eidetic.Unity.Utility;
using Eidetic.Utility;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(RuntimeNode))]
    public class RuntimeNodeEditor : NodeEditor
    {
        public static readonly string[] PortTypeExclusions = { "m_Script", "graph", "position", "ports" };
        public override void OnBodyGUI()
        {
            var node = target as RuntimeNode;

            portPositions = new Dictionary<XNode.NodePort, Vector2>();

            serializedObject.Update();

            // Draw the serializable ports

            var drawnPorts = new List<string>();

            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            EditorGUIUtility.labelWidth = 84;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (PortTypeExclusions.Contains(iterator.name)) continue;

                NodeEditorGUILayout.PropertyField(iterator, true);

                if (serializedObject.hasModifiedProperties && Application.isPlaying)
                    node.RunSetters();

                drawnPorts.Add(iterator.name.ToPascalCase());
            }
            serializedObject.ApplyModifiedProperties();

            // Draw the output ports that are properties without backing fields
            // e.g. expressions
            foreach (var getterEntry in node.Getters.Where(entry => !drawnPorts.Contains(entry.Key)))
            {
                var name = getterEntry.Key;
                var port = node.GetPort(name);
                NodeEditorGUILayout.DrawPropertyPort(port, name, getterEntry.Value);
            }
        }
    }
}