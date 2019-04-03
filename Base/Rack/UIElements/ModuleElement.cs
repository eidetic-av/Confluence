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
        public int ModuleDropIndex { get; private set; } = -1;

        RackRow ParentRow;

        VisualElement InsertBlank;

        ModuleHeader Header;

        public ModuleElement() : base()
        {
            InsertBlank = new VisualElement();
            InsertBlank.AddToClassList("InsertBlank");

            RackContainer.Instance.OnMouseDrag += DragModule;
            RackContainer.Instance.OnMouseUp += DropModule;

            Header = new ModuleHeader(this);
            Add(Header);
        }

        class ModuleHeader : RackElement
        {
            public bool DragActive;
            ModuleElement Module;
            public ModuleHeader(ModuleElement parentModule) : base()
            {
                Module = parentModule;
                var header = new TextElement();
                header.text = Module.GetType().Name;
                Add(header);

                OnMouseDown += e => DragActive = true;
            }
        }

        void DragModule(MouseMoveEvent mouseMoveEvent)
        {
            if (!Header.DragActive || !this.MouseDragging) return;

            if (MovingModule == false)
            {
                CurrentMovingModule = this;
                ParentRow = this.parent as RackRow;
                StartDragMousePosition = mouseMoveEvent.localMousePosition;
                CurrentDragMousePosition = StartDragMousePosition;
                StartDragElementPosition = new Vector2(layout.x, layout.y);
                StartDragModuleIndex = ParentRow.IndexOf(this);

                this.style.position = Position.Absolute;
                BringToFront();

                InsertBlank.style.width = this.layout.width;
                InsertBlank.style.height = this.layout.height;

                ParentRow.Insert(StartDragModuleIndex, InsertBlank);

                AddToClassList("Moving");
                MovingModule = true;
            }

            CurrentDragMousePosition = mouseMoveEvent.localMousePosition;

            this.style.left = StartDragElementPosition.x + (CurrentDragMousePosition.x - StartDragMousePosition.x);
            this.style.top = StartDragElementPosition.y + (CurrentDragMousePosition.y - StartDragMousePosition.y);

            // see if we are overlapping the edges of other modules to
            // add the insert blank in between and update the drop index
            foreach (var module in ParentRow.Children())
            {
                if (module == InsertBlank) continue;
                var leftCatchZone = new Rect(module.layout.x - 50, module.layout.y, 100, 400);
                if (leftCatchZone.Contains(CurrentDragMousePosition))
                {
                    ParentRow.Remove(InsertBlank);
                    ParentRow.Insert(ParentRow.IndexOf(module), InsertBlank);
                    ModuleDropIndex = ParentRow.IndexOf(InsertBlank);
                    break;
                }
                var rightCatchZone = new Rect(module.layout.xMax - 50, module.layout.y, 100, 400);
                if (rightCatchZone.Contains(CurrentDragMousePosition))
                {
                    ParentRow.Remove(InsertBlank);
                    ParentRow.Insert(ParentRow.IndexOf(module) + 1, InsertBlank);
                    ModuleDropIndex = ParentRow.IndexOf(InsertBlank);
                    break;
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

            var rowIndex = ModuleDropIndex != -1 ? ModuleDropIndex : StartDragModuleIndex;

            ParentRow.Insert(rowIndex, this);

            ParentRow.Remove(InsertBlank);

            CurrentMovingModule = null;
            MovingModule = false;
            StartDragMousePosition = Vector2.zero;
            CurrentDragMousePosition = Vector2.zero;
            StartDragModuleIndex = -1;
            ModuleDropIndex = -1;
            Header.DragActive = false;
            RemoveFromClassList("Moving");
        }
    }
}