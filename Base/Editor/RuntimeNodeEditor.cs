using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using Eidetic.Utility;
using Eidetic.Unity.Utility;

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

            // Store the property names we've accounted for in this list
            var displayedMembers = new List<string>();

            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            EditorGUIUtility.labelWidth = 84;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;
                NodeEditorGUILayout.PropertyField(iterator, true);

                displayedMembers.Add(iterator.name.ToPascal());

                // Come on Unity, why do you call these things
                // 'SerializedProperties' when they're actually fields...
                // And "PropertyField"...are you serious? ◔_◔

                if (!serializedObject.hasModifiedProperties || !Application.isPlaying) continue;
                // if the "PropertyFields" have been updated at runtime,
                // we need to check if they are backing fields for real
                // properties, and if they are, run the appropriate setter
                var propertyName = iterator.name.ToPascal();
                FieldInfo backingField;
                if (node.BackingFields.TryGetValue(propertyName, out backingField))
                {
                    node.Setters[propertyName](backingField.GetValue(node));
                    // and set the serialized value to the one from the property
                    // Getter because validation may have just been run on the
                    // input

                    // Todo: can't get the value to update this way...
                    // iterator.SetValue();
                }
            }

            serializedObject.ApplyModifiedProperties();

            // Draw the ports that are expressions or properties without
            // backing fields -- these aren't serialized in the object, and thus
            // haven't yet been added to the displayedMembers list

            foreach(var kvp in node.Getters.Where(kvp => !displayedMembers.Contains(kvp.Key)))
            {
                var name = kvp.Key;
                var port = node.GetPort(name);
                NodeEditorGUILayout.DrawOutputPropertyPort(port, name, kvp.Value);
            }
        }

        // if (Event.current.isMouse)
        // {
        //     if (port.MemberType == MemberTypes.Property)
        //         if (port.Node.GetType().IsSubclassOf(typeof(RuntimeNode)))
        //         {
        //             var node = port.Node as RuntimeNode;
        //             object backingFieldValue = node.Getters[port.MemberName]();
        //             node.Setters[port.MemberName](backingFieldValue);
        //         }
        // }
    }
}