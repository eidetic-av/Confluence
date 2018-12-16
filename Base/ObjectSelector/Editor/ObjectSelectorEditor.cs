using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;


[CustomNodeEditor(typeof(ObjectSelector))]
public class ObjectSelectorEditor : NodeEditor
{
    public ObjectSelector Target;
    public string Input { get; private set; }
}