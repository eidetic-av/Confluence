using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.Confluence.Base
{
    public class RackContainer : RackElement
    {
        public static RackContainer Instance { get; private set; }

        public RackContainer() : base()
        {
            Instance = this;

            var firstRow = new RackRow();
            firstRow.Add(new ModuleElement());
            var wide = new ModuleElement();
            wide.AddToClassList("WideTest");
            firstRow.Add(wide);
            firstRow.Add(new ModuleElement());
            firstRow.Add(new ModuleElement());

            var mid = new ModuleElement();
            mid.AddToClassList("MidTest");
            firstRow.Add(mid);

            Add(firstRow);

            var secondRow = new RackRow();
            var wide2 = new ModuleElement();
            wide2.AddToClassList("WideTest");
            secondRow.Add(wide2);
            secondRow.Add(new ModuleElement());
            secondRow.Add(new ModuleElement());
            var wide3 = new ModuleElement();
            wide3.AddToClassList("WideTest");
            secondRow.Add(wide3);
            var wide4 = new ModuleElement();
            wide4.AddToClassList("WideTest");
            secondRow.Add(wide4);

            Add(secondRow);
        }
    }
}