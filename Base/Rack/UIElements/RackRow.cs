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
            Add(new NewModuleBlank(this));
        }

        class NewModuleBlank : RackElement
        {
            RackRow ParentRow;

            public NewModuleBlank(RackRow parentRow) : base(parentRow.Container)
            {
                ParentRow = parentRow;
                AddToClassList("NewModuleBlank");
                style.top = ParentRow.layout.y;
                style.height = 400;
                var text = new TextElement();
                text.text = "Add Module";
                Add(text);

                OnMouseDown += OpenModuleMenu;
            }

            void OpenModuleMenu(MouseDownEvent e)
            {
                
            }
        }
    }
}