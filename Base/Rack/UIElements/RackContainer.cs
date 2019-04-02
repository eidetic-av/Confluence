using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.Confluence.Base
{
    public class RackContainer : RackElement
    {
        public RackContainer()
        {
            var firstRow = new RackRow(this);
            firstRow.Add(new ModuleElement(firstRow));
            var wide = new ModuleElement(firstRow);
            wide.AddToClassList("WideTest");
            firstRow.Add(wide);
            firstRow.Add(new ModuleElement(firstRow));

            Add(firstRow);
        }
    }
}