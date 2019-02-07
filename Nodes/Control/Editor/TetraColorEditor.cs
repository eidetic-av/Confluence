using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

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
