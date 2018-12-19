using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(QuadLFO))]
    public class QuadLFOEditor : NodeEditor
    {
        // Layout parameters
        public static int ClockMin = 40;
        public static int ClockMax = 170;

        // Styles
        public GUIStyle LabelStyle = new GUIStyle() {
            normal = new GUIStyleState() 
            {
                textColor = Color.white
            }
        };
        public GUIStyle ValueStyle = new GUIStyle() {
            normal = new GUIStyleState() 
            {
                textColor = Color.grey
            }
        };
        public GUILayoutOption[] ClockSliderLayoutParameters = new GUILayoutOption[]
        {
            GUILayout.Width(500),
            GUILayout.ExpandWidth(true)
        };

        bool Initialised;
        QuadLFO Target;

        public override void OnHeaderGUI()
        {
            if (!Initialised)
            {
                Target = target as QuadLFO;
                Initialised = true;
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Synced Quad LFO", NodeEditorResources.styles.nodeHeader);
            EditorGUILayout.Space();
        }
        public override void OnBodyGUI()
        {
            EditorGUILayout.Space();
            EditorGUI.LabelField(new Rect(45, 37, 60, 30), "Clock", LabelStyle);
            NodeEditorGUILayout.AddPortField(Target.GetPort("Clock"));
            Target.Clock = GUI.HorizontalSlider(new Rect(85, 35, 60, 15), Target.Clock, ClockMin, ClockMax);
            GUI.Label(new Rect(155, 37, 35, 30), Target.Clock + "", ValueStyle);
            
            EditorGUILayout.Space();
            // NodeEditorGUILayout.AddPortField(Target.GetInputPort("Clock"));
        }
    }
}