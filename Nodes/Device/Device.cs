using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MidiJack;

namespace Eidetic.Confluence
{
    public abstract partial class Device : RuntimeNode
    {
        public MidiChannel MidiChannel;

        public List<MidiDriver.KnobDelegate> ControlChangeDelegates = new List<MidiDriver.KnobDelegate>();

        public FieldInfo[] Fields;
        public List<FieldInfo> ControlChangeOutputs;

        public virtual DevicePanel GetDevicePanelLayout() { return null; }

        protected override void Init()
        {
            base.Init();
            InitialiseFields();
            InitialiseMidiConnections();
        }

        void InitialiseFields()
        {
            // Get all the fields on the class
            Fields = GetType().GetFields();
            // Get the Control Change output ports
            ControlChangeOutputs = Fields.Where(
                f => f.IsDefined(typeof(ControlChangeOutputAttribute), false)
            ).ToList();
        }

        void InitialiseMidiConnections()
        {
            // For each port created, add a delegate that changes the field when the 
            // corresponding MIDI event occurs
            ControlChangeOutputs.ForEach(portField =>
            {
                var attributes = CustomAttributeData.GetCustomAttributes(portField);
                var ccNumber = (int)attributes.First().ConstructorArguments.First().Value;
                MidiMaster.knobDelegate += CreateControlChangeDelegate(ccNumber, portField);
            });
        }

        MidiDriver.KnobDelegate CreateControlChangeDelegate(int ccNumber, FieldInfo portField)
        {
            MidiDriver.KnobDelegate knobDelegate = (MidiChannel channel, int knobNumber, float knobValue) =>
            {
                if (channel.Equals(this.MidiChannel))
                {
                    if (knobNumber.Equals(ccNumber))
                    {
                        portField.SetValue(this, knobValue);
                        Debug.Log("cc: " + ccNumber + ", " + knobValue);
                    }
                }
            };
            ControlChangeDelegates.Add(knobDelegate);
            return knobDelegate;
        }

        void OnDestroy()
        {
            // Remove the delegates from the MidiMaster
            ControlChangeDelegates.ForEach(del => MidiMaster.knobDelegate -= del);
        }

        public class DevicePanel
        {
            public List<PanelGroup> Groups;
            public PanelGroup GetPanelGroup(string groupName)
            {
                return Groups.FirstOrDefault(g => g.Name.Equals(groupName));
            }
            public class PanelGroup
            {
                public string Name;
                public int Width;
                public int Height;
                public int VerticalPosition;
                public int HorizontalPosition;
                public PanelGroup(string name, int width, int height, int horizontalPosition, int verticalPosition)
                {
                    Name = name;
                    Width = width;
                    Height = height;
                    HorizontalPosition = horizontalPosition;
                    VerticalPosition = verticalPosition;
                }
            }
        }
    }
}
