# Confluence
Node editor for live creation of audio-visual scenes in Unity.

This is very early development and will probably change a lot.


You can create a RuntimeGraph in the Project window with Right-click > Create > RuntimeGraph.

Double-click anywhere in the graph and type the name of your node to add it to the graph.


To create your own node, extend RuntimeNode.
A RuntimeNode can override Start() and Update() to influence game logic.

See the xNode documentation below to learn how to create Input and Output ports.


Uses [xNode from Siccity](https://github.com/Siccity/xNode) as a base.
Copyright (c) 2017 Thor Brigsted

As well as [MidiJack by Keijiro](https://github.com/keijiro/MidiJack) for Midi (although probably not for much longer).
Copyright (C) 2013-2015 Keijiro Takahashi

## License

Copyright (C) 2018 Matthew Hughes

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
