using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(Device))]
    public class DeviceEditor : NodeEditor
    {
        // Layout sizing
        public Padding HeaderPadding = new Padding()
        {
            left = 16,
            right = 16,
            top = 3,
            bottom = 0
        };
        public Size GridSize = new Size()
        {
            width = 16,
            height = 16
        };
        public GUIStyle DeviceHeader = new GUIStyle()
        {
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        public Device Target;
        public Dictionary<FieldInfo, GroupArguments> FieldGroupArguments = new Dictionary<FieldInfo, GroupArguments>();

        public override void OnHeaderGUI()
        {
            if (Target == null)
                Target = target as Device;
            GUILayout.Space(30);
            var headerPosition = new Rect(HeaderPadding.left, HeaderPadding.top, 110, 30);
            GUI.Label(headerPosition, Target.GetType().Name, DeviceHeader);
            var channelSelectorPosition = new Rect(110, 12, 94, 30);
            EditorGUI.EnumPopup(channelSelectorPosition, Target.MidiChannel);
        }
        public override void OnBodyGUI()
        {
            // base.OnBodyGUI();
            DrawDeviceLayout();

            // object value = deviceNode.GetValue();
            // if (value != null)
            //     EditorGUILayout.LabelField(value.ToString());
        }

        void InitDisplayGroups()
        {
            // Cache the constructor arguments
            foreach (var controlChangeOutput in Target.ControlChangeOutputs)
            {
                var attributesData = controlChangeOutput.GetCustomAttributesData();
                var groupedAttributes = attributesData.Where(a => a.ConstructorArguments.Count > 1);
                if (groupedAttributes.Count() != 0)
                {
                    groupedAttributes.ToList().ForEach(a =>
                    {
                        var constructorArguments = a.ConstructorArguments;
                        FieldGroupArguments.Add(controlChangeOutput, new GroupArguments()
                        {
                            GroupName = (string)constructorArguments.ElementAt(1).Value,
                            GroupRow = (int)constructorArguments.ElementAt(2).Value,
                            GroupColumn = (int)constructorArguments.ElementAt(3).Value,
                            DisplayName = (string)constructorArguments.ElementAt(4).Value
                        });
                    });
                }
            }
        }

        void DrawDeviceLayout()
        {
            if (FieldGroupArguments.Count == 0)
                InitDisplayGroups();

            var panelLayout = Target.GetDevicePanelLayout();

            foreach (var controlChangeOutput in Target.ControlChangeOutputs)
            {
                // Parse cached constructor arguments
                GroupArguments groupArguments;
                if (FieldGroupArguments.TryGetValue(controlChangeOutput, out groupArguments))
                {
                    DrawControlNodeInGroup(controlChangeOutput, groupArguments, panelLayout);
                }
                // But if there are no group arguments, just draw it basically
                else
                {
                    DrawControlNode(controlChangeOutput);
                }
            }
        }

        void DrawControlNodeInGroup(FieldInfo controlChangeOutput, GroupArguments groupArguments, Device.DevicePanel panelLayout)
        {
            var panelGroup = panelLayout.GetPanelGroup(groupArguments.GroupName);
            var groupRow = groupArguments.GroupRow;
            var groupColumn = groupArguments.GroupColumn;
        }

        void DrawControlNode(FieldInfo controlChangeOutput)
        {

        }

        public struct Padding
        {
            public int left, right, top, bottom;
        }
        public struct Size
        {
            public int width, height;
        }
        public struct GroupArguments
        {
            public string GroupName;
            public int GroupRow;
            public int GroupColumn;
            public string DisplayName;
        }
    }
}