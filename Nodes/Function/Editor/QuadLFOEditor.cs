using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(QuadLFO))]
    public class QuadLFOEditor : NodeEditor
    {
        // Styles
        public GUIStyle LabelStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            alignment = TextAnchor.MiddleCenter,
        };
        public GUIStyle ValueStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.grey
            },
            alignment = TextAnchor.MiddleCenter,
        };

        public override int GetWidth()
        {
            return 194;
        }

        bool Initialised;
        QuadLFO Target;

        public override void OnHeaderGUI()
        {
            if (!Initialised)
            {
                Target = target as QuadLFO;
                Initialised = true;
            }
            base.OnHeaderGUI();
        }
        public override void OnBodyGUI()
        {
            // set the height of this device
            GUILayout.Space(300);

            DrawClockInputs();
            DrawOutputPorts();
        }
        void DrawClockInputs()
        {
            // headers
            EditorGUI.LabelField(new Rect(5, 38, 60, 15), "Clock", LabelStyle);
            EditorGUI.LabelField(new Rect(88, 38, 60, 15), "Multipliers", LabelStyle);

            // master clock
            if (!Target.ClockPort.IsConnected)
            {
                Target.Clock = GUI.VerticalSlider(new Rect(29, 81, 15, 80), Target.Clock, QuadLFO.ClockMax, QuadLFO.ClockMin);
                Target.Clock = EditorGUI.FloatField(new Rect(20, 167, 30, 15), Target.Clock);
            }
            else
            {
                // if the port is connected, the values aren't editable via the node
                GUI.VerticalSlider(new Rect(29, 81, 15, 80), Target.Clock, QuadLFO.ClockMin, QuadLFO.ClockMax);
                EditorGUI.LabelField(new Rect(20, 167, 30, 15), Target.Clock + "");
            }

            // multiplier a
            if (!Target.MultiplierAPort.IsConnected)
                Target.MultiplierAPosition = Mathf.RoundToInt(GUI.VerticalSlider(new Rect(70, 81, 15, 80), Target.MultiplierAPosition, QuadLFO.MultiplierRates.Length - 1, 0));
            else GUI.VerticalSlider(new Rect(70, 81, 15, 80), Target.MultiplierAPosition, QuadLFO.MultiplierRates.Length - 1, 0);
            EditorGUI.LabelField(new Rect(61, 167, 30, 15), Target.MultiplierALabel, ValueStyle);

            // multiplier b
            if (!Target.MultiplierBPort.IsConnected)
                Target.MultiplierBPosition = Mathf.RoundToInt(GUI.VerticalSlider(new Rect(111, 81, 15, 80), Target.MultiplierBPosition, QuadLFO.MultiplierRates.Length - 1, 0));
            else GUI.VerticalSlider(new Rect(111, 81, 15, 80), Target.MultiplierBPosition, QuadLFO.MultiplierRates.Length - 1, 0);
            EditorGUI.LabelField(new Rect(102, 167, 30, 15), Target.MultiplierBLabel, ValueStyle);

            // multiplier c
            if (!Target.MultiplierCPort.IsConnected)
                Target.MultiplierCPosition = Mathf.RoundToInt(GUI.VerticalSlider(new Rect(152, 81, 15, 80), Target.MultiplierCPosition, QuadLFO.MultiplierRates.Length - 1, 0));
            else GUI.VerticalSlider(new Rect(152, 81, 15, 80), Target.MultiplierCPosition, QuadLFO.MultiplierRates.Length - 1, 0);
            EditorGUI.LabelField(new Rect(143, 167, 30, 15), Target.MultiplierCLabel, ValueStyle);

            // ports
            NodeEditorGUILayout.PortField(new Vector2(27, 60), Target.ClockPort);
            NodeEditorGUILayout.PortField(new Vector2(68, 60), Target.MultiplierAPort);
            NodeEditorGUILayout.PortField(new Vector2(109, 60), Target.MultiplierBPort);
            NodeEditorGUILayout.PortField(new Vector2(150, 60), Target.MultiplierCPort);
        }
        void DrawOutputPorts()
        {
            // Sine wave outputs
            NodeEditorGUILayout.PortField(new Vector2(27, 200), Target.MasterSinePort);
            EditorGUI.LabelField(new Rect(20, 220, 30, 15), "Sin", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(68, 200), Target.MultiplierASinePort);
            EditorGUI.LabelField(new Rect(61, 220, 30, 15), "Sin", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(109, 200), Target.MultiplierBSinePort);
            EditorGUI.LabelField(new Rect(102, 220, 30, 15), "Sin", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(150, 200), Target.MultiplierCSinePort);   
            EditorGUI.LabelField(new Rect(143, 220, 30, 15), "Sin", ValueStyle);  

            // Triangle wave outputs
            NodeEditorGUILayout.PortField(new Vector2(27, 245), Target.MasterTrianglePort);
            EditorGUI.LabelField(new Rect(20, 265, 30, 15), "Tri", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(68, 245), Target.MultiplierATrianglePort);
            EditorGUI.LabelField(new Rect(61, 265, 30, 15), "Tri", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(109, 245), Target.MultiplierBTrianglePort);
            EditorGUI.LabelField(new Rect(102, 265, 30, 15), "Tri", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(150, 245), Target.MultiplierCTrianglePort);   
            EditorGUI.LabelField(new Rect(143, 265, 30, 15), "Tri", ValueStyle);    

            // Saw wave outputs
            NodeEditorGUILayout.PortField(new Vector2(27, 290), Target.MasterSawtoothPort);
            EditorGUI.LabelField(new Rect(20, 310, 30, 15), "Saw", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(68, 290), Target.MultiplierASawtoothPort);
            EditorGUI.LabelField(new Rect(61, 310, 30, 15), "Saw", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(109, 290), Target.MultiplierBSawtoothPort);
            EditorGUI.LabelField(new Rect(102, 310, 30, 15), "Saw", ValueStyle);
            NodeEditorGUILayout.PortField(new Vector2(150, 290), Target.MultiplierCSawtoothPort);   
            EditorGUI.LabelField(new Rect(143, 310, 30, 15), "Saw", ValueStyle);  
        }
    }
}