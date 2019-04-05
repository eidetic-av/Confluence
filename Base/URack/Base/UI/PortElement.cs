using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Eidetic.Unity.UI.Utility;

namespace Eidetic.URack.UI
{
    public partial class PortElement : TouchElement
    {
        public PortElement() : base()
        {
            Add(new Box().WithName("Inner"));
        }
    }
}