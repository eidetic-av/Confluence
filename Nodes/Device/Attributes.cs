using System;
using UnityEngine;
using XNode;
using MidiJack;

namespace Eidetic.Confluence {
    public abstract partial class Device {

        /// <summary>
        /// Mark a serializable field as an control change output port.
        /// Same as an output port, but updates its value based on Midi input.
        /// You can access this through <see cref="GetOutputPort(string)"/>
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
        public class ControlChangeOutputAttribute : Node.OutputAttribute
        {
            public int Number;
            public ControlChangeOutputAttribute(int number) :
                base(Node.ShowBackingValue.Never, Node.ConnectionType.Multiple)
            {
                Number = number;
            }
        }
    }
}