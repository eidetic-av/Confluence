using UnityEngine;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(TetraColor))]
    public class TetraColorEditor : NodeEditor
    {
        public Color[] Colors = new Color[] {
        Color.red, Color.magenta, Color.cyan, Color.yellow
    };

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
        }
    }
}
