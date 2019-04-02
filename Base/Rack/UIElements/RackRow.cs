using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.Confluence.Base
{
    public class RackRow : RackElement
    {
        public RackContainer Container {get; private set;}

        public RackRow(RackContainer parentContainer) : base(parentContainer)
        {
            Container = parentContainer;
        }
    }
}