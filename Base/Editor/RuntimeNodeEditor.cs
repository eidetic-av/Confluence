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
        public override void OnBodyGUI()
        {
            var node = this.target as RuntimeNode;

            serializedObject.Update();

            string[] excludes = { "m_Script", "graph", "position", "ports" };
            portPositions = new Dictionary<XNode.NodePort, Vector2>();

            // Draw the serializable ports

            var drawnPorts = new List<string>();

            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            EditorGUIUtility.labelWidth = 84;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;

                NodeEditorGUILayout.PropertyField(iterator, true);

                if (serializedObject.hasModifiedProperties && Application.isPlaying)
                {
                    // check if the updated field is a backing value for a property
                    var propertyName = iterator.name.ToPascalCase();
                    FieldInfo backingField;
                    if (node.BackingFields.TryGetValue(propertyName, out backingField))
                    {
                        // if it is, run the setter method
                        node.Setters[propertyName].Invoke(backingField.GetValue(node));
                        // Todo:
                        // and set the iterator to the correct value since the
                        // setter could have performed validation
                        // iterator.SetValue(node.Getters[propertyName]());
                    }
                }

                drawnPorts.Add(iterator.name.ToPascalCase());
            }
            serializedObject.ApplyModifiedProperties();

            // Draw the ports that are properties without backing fields
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