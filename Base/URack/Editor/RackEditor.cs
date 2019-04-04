using System;
using Eidetic.URack.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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
                OpenRack(selectedAsset);
                return true;
            }
            return false;
        }

        [MenuItem("Window/URack")]
        public static void OpenRack(Rack rackAsset)
        {
            UI.RackContainer.Instantiate(rackAsset);
            GetWindow();
        }

        static RackEditor GetWindow()
        {
            var window = GetWindow<RackEditor>(true, "URack");
            window.rootVisualElement.Clear();
            window.rootVisualElement.Add(UI.RackContainer.Instance);
            return window;
        }

        public void OnEnable()
        {
            GetWindow();
            UI.RackContainer.Attach();
        }

        public void OnDisable()
        {
            UI.RackContainer.Detach();
        }
    }
}