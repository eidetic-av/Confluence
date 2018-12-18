using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;


[CustomNodeEditor(typeof(ObjectSelector))]
public class ObjectSelectorEditor : NodeEditor
{
    public static Vector2 Position;
    public static string Input = "";
    public static Type SelectedType;

    public NodeEditorWindow Window;
    public ObjectSelector Target;

    bool Initialised;
    int SelectedListIndex = 0;
    int ListDisplayLength = 5;

    public override void OnHeaderGUI()
    {
        if (!Initialised)
        {
            Target = target as ObjectSelector;
            Window = NodeEditorWindow.GetWindow(typeof(NodeEditorWindow)) as NodeEditorWindow;
            Initialised = true;
        }
        // set the header field to editing
        int controlID = EditorGUIUtility.GetControlID(FocusType.Keyboard) + 1;
        EditorGUIUtility.keyboardControl = controlID;
        EditorGUIUtility.editingTextField = true;

        Input = EditorGUILayout.TextField(Input, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    }

    public override void OnBodyGUI()
    {
        var e = Event.current;
        // On 'Enter' pressed, swap this out for the matched node in the list
        if (e.type == EventType.KeyUp)
        {
            if (e.keyCode == KeyCode.Return)
            {
                FinaliseNodeType();
                e.Use();
                return;
            }
            else if (e.keyCode == KeyCode.UpArrow)
            {
                SelectedListIndex = (int) Mathf.Repeat(--SelectedListIndex, ListDisplayLength);
                e.Use();
            }
            else if (e.keyCode == KeyCode.DownArrow)
            {
                SelectedListIndex = (int) Mathf.Repeat(++SelectedListIndex, ListDisplayLength);
                e.Use();
            } else
            {
                // If we press any other key it means we're inputting a letter for the query
                // So set the SelectedListIndex back to 0 because the list is being "refreshed"
                SelectedListIndex = 0;
            }
        }

        // Sort the list of node types based on the input
        var nodeList = NodeEditorWindow.nodeTypes
            // eliminate the ObjectSelector
            .Where(type => !type.Name.Equals("ObjectSelector"))
            // order by the QueryOrder method
            .OrderBy(type => QueryOrder(Input, type.Name));

        // Draw the list of node types
        for (int i = 0; i < ListDisplayLength; i++)
        {
            var type = nodeList.ElementAt(i);
            var name = type.Name;

            if (i == SelectedListIndex)
            {
                SelectedType = type;
                EditorGUILayout.LabelField(name, NodeEditorResources.styles.selectorListHighlighted);
            }
            else
                EditorGUILayout.LabelField(name, NodeEditorResources.styles.selectorList);

            if (i == ListDisplayLength - 1) break;
        }
    }

    public void FinaliseNodeType()
    {
        EditorGUIUtility.editingTextField = false;
        GUIUtility.keyboardControl = 0;
        Initialised = false;
        Window.SelectNode(Target, false);
        Window.RemoveSelectedNodes();
        var node = Window.CreateNode(SelectedType, Position);
        Window.SelectNode(node, false);
    }

    int QueryOrder(string inputQuery, string typeName)
    {
        // lowercase inputs makes it easier
        inputQuery = inputQuery.ToLower();
        typeName = typeName.ToLower();

        // Exact matches go to the very top
        if (typeName == inputQuery)
            return -1;
        // Partial matches next
        if (typeName.Contains(inputQuery))
            return 0;
        // Everything else last
        return 1;
    }
}