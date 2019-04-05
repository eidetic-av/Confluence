using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Eidetic.Unity.UI.Utility;

namespace Eidetic.URack.UI
{
    public partial class ModuleElement : DraggableElement
    {
        public static ModuleElement CurrentDraggingModule { get; private set; }

        public bool MovingModule { get; private set; }

        public Vector2 StartDragMousePosition { get; private set; }
        public Vector2 CurrentDragMousePosition { get; private set; }

        public Vector2 StartDragElementPosition { get; private set; }
        public int StartDragModuleIndex { get; private set; }
        public int ModuleDropIndex { get; private set; } = -1;

        public Module Module {get; private set;}

        RackRow ParentRow;

        VisualElement InsertBlank = new Box().WithName("InsertBlank");

        ModuleHeader Header;

        ModuleElement(Module module) : base()
        {
            Module = module;

            Header = new ModuleHeader(this);
            Add(Header);

            RackContainer.Instance.OnDrag += DragModule;
            RackContainer.Instance.OnRelease += DropModule;
        }

        public static ModuleElement Create(Module module)
        {
            var element = new ModuleElement(module);
            
            var moduleTemplate = Resources.Load<VisualTreeAsset>(module.GetType().Name);           
            moduleTemplate.CloneTree(element);
            
            LoadStyleSheets(element, module.GetType());

            return element;
        }

        void DragModule(MouseMoveEvent mouseMoveEvent)
        {
            if (!Header.DragActive || !this.Dragging) return;

            if (MovingModule == false)
            {
                CurrentDraggingModule = this;

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

                MovingModule = true;

                AddToClassList("Drag");
            }

            CurrentDragMousePosition = mouseMoveEvent.localMousePosition;

            this.style.left = StartDragElementPosition.x + (CurrentDragMousePosition.x - StartDragMousePosition.x);
            this.style.top = StartDragElementPosition.y + (CurrentDragMousePosition.y - StartDragMousePosition.y);

            // see if we are overlapping the edges of other modules to
            // add the insert blank in between and update the drop index
            foreach (var module in ParentRow.Children())
            {
                if (module == InsertBlank) continue;
                var leftCatchZone = new Rect(module.layout.x - 50, ParentRow.layout.y, 100, 400);
                if (leftCatchZone.Contains(CurrentDragMousePosition))
                {
                    ParentRow.Remove(InsertBlank);
                    ParentRow.Insert(ParentRow.IndexOf(module), InsertBlank);
                    ModuleDropIndex = ParentRow.IndexOf(InsertBlank);
                    break;
                }
                var rightCatchZone = new Rect(module.layout.xMax - 50, ParentRow.layout.y, 100, 400);
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
            Header.DragActive = false;
            if (!MovingModule) return;

            ParentRow.Remove(this);

            this.style.position = Position.Relative;
            this.style.left = 0;
            this.style.top = 0;

            var rowIndex = ModuleDropIndex != -1 ? ModuleDropIndex : StartDragModuleIndex;

            ParentRow.Insert(rowIndex, this);

            ParentRow.Remove(InsertBlank);

            CurrentDraggingModule = null;
            MovingModule = false;
            StartDragMousePosition = Vector2.zero;
            CurrentDragMousePosition = Vector2.zero;
            StartDragModuleIndex = -1;
            ModuleDropIndex = -1;

            RemoveFromClassList("Drag");
        }

        class ModuleHeader : TouchElement
        {
            public bool DragActive;
            ModuleElement ModuleElement;
            public ModuleHeader(ModuleElement parentModule) : base()
            {
                ModuleElement = parentModule;
                Add(new TextElement().WithText(parentModule.Module.Name));
                OnTouch += e => DragActive = true;
            }
        }
    }
}