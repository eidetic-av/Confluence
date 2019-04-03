using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.Confluence.Base
{
    public class RackContainer : RackElement
    {
        public RackContainer(VisualElement rootElement) : base(rootElement)
        {
            var firstRow = new RackRow(this);
            firstRow.Add(new ModuleElement(firstRow));
            var wide = new ModuleElement(firstRow);
            wide.AddToClassList("WideTest");
            firstRow.Add(wide);
            firstRow.Add(new ModuleElement(firstRow));
            firstRow.Add(new ModuleElement(firstRow));

            var mid = new ModuleElement(firstRow);
            mid.AddToClassList("MidTest");
            firstRow.Add(mid);

            Add(firstRow);

            var secondRow = new RackRow(this);
            var wide2 = new ModuleElement(secondRow);
            wide2.AddToClassList("WideTest");
            secondRow.Add(wide2);
            secondRow.Add(new ModuleElement(secondRow));
            secondRow.Add(new ModuleElement(secondRow));
            var wide3 = new ModuleElement(secondRow);
            wide3.AddToClassList("WideTest");
            secondRow.Add(wide3);
            var wide4 = new ModuleElement(secondRow);
            wide4.AddToClassList("WideTest");
            secondRow.Add(wide4);

            Add(secondRow);
        }
    }
}