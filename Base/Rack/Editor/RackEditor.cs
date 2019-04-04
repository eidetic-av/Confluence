using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Eidetic.URack.UI;

namespace Eidetic.URack.Editor
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
        }

        public void OnEnable()
        {
            var window = GetWindow<RackEditor>();
            if (!window.rootVisualElement.Contains(UI.URackContainer.Instance))
                window.rootVisualElement.Add(UI.URackContainer.Instance);  

            UI.URackContainer.Attach();
        }

        public void OnDisable()
        {
            UI.URackContainer.Detach();
        }
    }
}