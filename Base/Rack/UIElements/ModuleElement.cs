using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.Confluence.Base
{
    public class ModuleElement : RackElement
    {
        public static ModuleElement CurrentMovingModule { get; private set; }

        public bool MovingModule { get; private set; }

        public Vector2 StartDragMousePosition { get; private set; }
        public Vector2 CurrentDragMousePosition { get; private set; }

        public Vector2 StartDragElementPosition { get; private set; }
        public int StartDragModuleIndex { get; private set; }
        public int NewModuleIndex { get; private set; } = -1;

        RackRow ParentRow;
        RackContainer Container;

        static VisualElement InsertBlank;

        public ModuleElement(RackRow parentRow) : base(parentRow.Container)
        {
            ParentRow = parentRow;
            Container = ParentRow.Container;
            Container.OnMouseDrag += DragModule;
            Container.OnMouseUp += DropModule;

            InsertBlank = new VisualElement();
            InsertBlank.AddToClassList("InsertBlank");
        }

        void DragModule(MouseMoveEvent mouseMoveEvent)
        {
            if (this.MouseDragging == false) return;

            if (MovingModule == false)
            {
                CurrentMovingModule = this;
                StartDragMousePosition = mouseMoveEvent.localMousePosition;
                CurrentDragMousePosition = StartDragMousePosition;
                StartDragElementPosition = new Vector2(layout.x, layout.y);
                StartDragModuleIndex = ParentRow.IndexOf(this);

                this.style.position = Position.Absolute;
                this.BringToFront();

                InsertBlank.style.width = this.layout.width;
                InsertBlank.style.height = this.layout.height;

                MovingModule = true;
            }

            CurrentDragMousePosition = mouseMoveEvent.localMousePosition;

            this.style.left = StartDragElementPosition.x + (CurrentDragMousePosition.x - StartDragMousePosition.x);
            this.style.top = StartDragElementPosition.y + (CurrentDragMousePosition.y - StartDragMousePosition.y);

            // see if we are overlapping the edges of other modules to initiate
            // add a dummy spacer and initiate a move

            if (ParentRow.Contains(InsertBlank) && InsertBlank.layout.Contains(CurrentDragMousePosition))
            {
                NewModuleIndex = ParentRow.IndexOf(InsertBlank);
            }
            else
            {
                foreach (var module in ParentRow.Children())
                {
                    var leftCatchZone = new Rect(module.layout.x - 50, module.layout.y,
                        100, ParentRow.layout.height);
                    if (leftCatchZone.Contains(CurrentDragMousePosition) && module != InsertBlank)
                    {
                        if (ParentRow.Contains(InsertBlank))
                            ParentRow.Remove(InsertBlank);
                        ParentRow.Insert(ParentRow.IndexOf(module), InsertBlank);
                        break;
                    }
                }
            }
        }

        void DropModule(MouseUpEvent mouseUpEvent)
        {
            if (!MovingModule) return;

            ParentRow.Remove(this);

            this.style.position = Position.Relative;
            this.style.left = 0;
            this.style.top = 0;

            var rowIndex = NewModuleIndex != -1 ? NewModuleIndex : StartDragModuleIndex;

            if (CurrentDragMousePosition.x > ParentRow.ElementAt(ParentRow.childCount - 1).layout.x)
                rowIndex = ParentRow.childCount;

            ParentRow.Insert(rowIndex, this);

            if (ParentRow.Contains(InsertBlank))
                ParentRow.Remove(InsertBlank);

            CurrentMovingModule = null;
            MovingModule = false;
            StartDragMousePosition = Vector2.zero;
            CurrentDragMousePosition = Vector2.zero;
            StartDragModuleIndex = -1;
            NewModuleIndex = -1;
        }
    }
}