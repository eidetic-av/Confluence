using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Eidetic.Confluence
{
    [CustomNodeEditor(typeof(NodeSelector))]
    public class ObjectSelectorEditor : NodeEditor
    {
        public static Vector2 Position;

        public string Input = "";
        public Type SelectedNodeType;
        public RuntimeGraph SelectedRuntimeGraph;

        public Dictionary<string, Type> RuntimeNodeTypes = new Dictionary<string, Type>();
        public Dictionary<string, RuntimeGraph> RuntimeGraphs = new Dictionary<string, RuntimeGraph>();

        public NodeEditorWindow Window;
        public NodeSelector Target;

        bool Initialised;
        int SelectedListIndex = 0;
        int ListDisplayLength = 5;

        public override void OnHeaderGUI()
        {
            if (!Initialised)
            {
                Target = target as NodeSelector;
                Window = NodeEditorWindow.GetWindow(typeof(NodeEditorWindow)) as NodeEditorWindow;
                Window.SelectNode(Target, false);

                // Add runtime nodes to the dictionary for querying
                // We do this so it can be easily appended to the next dictionary's keys
                NodeEditorWindow.nodeTypes.ToList().ForEach(n =>
                    {
                    // eliminate the system nodes
                    if (n.Name != "ObjectSelector" && n.Name != "RuntimeGraphHolder")
                            RuntimeNodeTypes.Add(n.Name, n);
                    }
                );

                // Add other runtime graphs to the dictionary which will also appear in the queries
                // these runtime graphs will be used as 'sub-graph' nodes
                string[] runtimeGraphGuids = AssetDatabase.FindAssets("t:RuntimeGraph", null);
                runtimeGraphGuids.ToList().ForEach(g =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(g);
                    var graph = AssetDatabase.LoadAssetAtPath<RuntimeGraph>(path);
                    var name = path.Split('/').Last().Split('.').First();
                    if (name != Window.graph.name)
                        RuntimeGraphs.Add(name, graph);
                });

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
                    SelectedListIndex = (int)Mathf.Repeat(--SelectedListIndex, ListDisplayLength);
                    e.Use();
                }
                else if (e.keyCode == KeyCode.DownArrow)
                {
                    SelectedListIndex = (int)Mathf.Repeat(++SelectedListIndex, ListDisplayLength);
                    e.Use();
                }
                else
                {
                    // If we press any other key it means we're inputting a letter for the query
                    // So set the SelectedListIndex back to 0 because the list is being "refreshed"
                    SelectedListIndex = 0;
                }
            }

            // Sort the list of node/graph types based on the input
            var nameList = RuntimeNodeTypes.Keys
                // add the graph names
                .Union(RuntimeGraphs.Keys)
                // first order alphabetically
                .OrderBy(name => name)
                // then order by the query
                .OrderBy(name => QueryOrder(Input, name));

            // Draw the list of node/graph types
            for (int i = 0; i < ListDisplayLength; i++)
            {
                var name = nameList.ElementAt(i);
                var isGraph = RuntimeGraphs.ContainsKey(name);

                if (i == SelectedListIndex)
                {
                    // Set the node or graph as selected
                    if (!isGraph)
                    {
                        RuntimeNodeTypes.TryGetValue(name, out SelectedNodeType);
                        SelectedRuntimeGraph = null;
                    }
                    else
                    {
                        RuntimeGraphs.TryGetValue(name, out SelectedRuntimeGraph);
                        SelectedNodeType = null;
                    }
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
            if (SelectedNodeType != null)
            {
                // if it's a node create it normally
                var node = Window.CreateNode(SelectedNodeType, Position);
                Window.SelectNode(node, false);
            }
            // else if (SelectedRuntimeGraph != null)
            // {
            //     // if it's a graph, create the holder node
            //     var node = Window.CreateGraphHolderNode(SelectedRuntimeGraph, Position);
            //     Window.SelectNode(node, false);
            // }
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
}