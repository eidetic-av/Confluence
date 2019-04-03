using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.Confluence.Base
{
    [CustomEditor(typeof(Rack))]
    public class RackEditor : EditorWindow
    {
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var selectedAsset = Selection.activeObject as Rack;
            if (selectedAsset != null)
            {
                ShowWindow(selectedAsset);
                return true;
            }
            return false;
        }

        static Rack ActiveRack;

        [MenuItem("Window/Confluence Graph Editor")]
        public static void ShowWindow(Rack rackAsset)
        {
            ActiveRack = rackAsset;
            var window = GetWindow<RackEditor>();
        }

        public void OnEnable()
        {
            var styleSheet = Resources.Load<StyleSheet>("Rack");
            if (styleSheet != null) rootVisualElement.styleSheets.Add(styleSheet);
            
            var container = new RackContainer(rootVisualElement);
            rootVisualElement.Add(container);
        }

        public void OnDisable()
        {

        }
    }
}