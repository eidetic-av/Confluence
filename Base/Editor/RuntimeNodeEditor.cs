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
            EditorGUIUtility.labelWidth = 84;

            var node = target as RuntimeNode;

            portPositions = new Dictionary<XNode.NodePort, Vector2>();

            serializedObject.Update();

            DrawPanel(node);

            if (serializedObject.hasModifiedProperties && Application.isPlaying) node.RunSetters();

            serializedObject.ApplyModifiedProperties();
        }

        public virtual void DrawPanel(RuntimeNode node)
        {
            // Draw the serializable ports

            var drawnPorts = new List<string>();

            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (PortTypeExclusions.Contains(iterator.name)) continue;

                NodeEditorGUILayout.PropertyField(iterator, true);

                drawnPorts.Add(iterator.name.ToPascalCase());
            }

            if (node.Getters == null) return;
            
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