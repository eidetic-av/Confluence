using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(Device))]
    public class DeviceEditor : NodeEditor
    {
        Padding nodePadding = new Padding()
        {
            left = 16,
            right = 16,
            top = 3,
            bottom = 0
        };
        GUIStyle deviceHeader = new GUIStyle()
        {
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };

        public override void OnHeaderGUI()
        {
            GUILayout.Space(30);
            var deviceNode = target as Device;
            var headerPosition = new Rect(nodePadding.left, nodePadding.top, 110, 30);
            GUI.Label(headerPosition, deviceNode.GetType().Name, deviceHeader);
            var channelSelectorPosition = new Rect(110, nodePadding.top + 8, 94, 30);
            EditorGUI.EnumFlagsField(channelSelectorPosition, deviceNode.MidiChannel);
        }
        public override void OnBodyGUI()
        {
            // base.OnBodyGUI();

            var deviceNode = target as Device;

            // object value = deviceNode.GetValue();
            // if (value != null)
            //     EditorGUILayout.LabelField(value.ToString());
        }

        struct Padding 
        {
            public int left, right, top, bottom;
        }
    }
}