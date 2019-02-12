# Confluence
Node editor for live creation of audio-visual scenes in Unity.

Uses [xNode from Siccity](https://github.com/Siccity/xNode) as a base. Thanks a lot Siccity.

You can create a RuntimeGraph in the Project window with Right-click > Create > RuntimeGraph.

Then double-click anywhere in the graph and type the name of your node to add it to the graph.

To create your own node, extend RuntimeNode.
A RuntimeNode can override Start() and Update() to influence game logic.

See the xNode documentation to learn how to create Input and Output ports.

This is very early development and will probably change a lot.
