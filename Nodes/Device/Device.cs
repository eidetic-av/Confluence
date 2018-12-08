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

        protected override void Init()
        {
            base.Init();
            InitialiseMidiConnections();
        }

        /// <summary>
        /// Initialise Input and Output NodePorts that update based on midi data.
        /// </summary>
        void InitialiseMidiConnections()
        {
            // Get all the fields on the class
            var fields = GetType().GetFields();
            // Get the Control Change output ports
            var ccOutputPorts = fields.Where(
                f => f.IsDefined(typeof(ControlChangeOutputAttribute), false)
            ).ToList();
            // For each port created, add a delegate that changes the field when the 
            // corresponding MIDI event occurs
            ccOutputPorts.ForEach(portField =>
            {
                var attributes = CustomAttributeData.GetCustomAttributes(portField);
                var ccNumber = (int) attributes.First().ConstructorArguments.First().Value;
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
    }
}
