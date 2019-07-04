using System;
using XNode;

namespace Eidetic.Confluence
{
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
            public String Group;
            public int GroupColumn;
            public int GroupRow;
            public String DisplayName;
            public ControlChangeOutputAttribute(int number) :
                base(Node.ShowBackingValue.Never, Node.ConnectionType.Multiple)
            {
                Number = number;
                Group = null;
            }
            public ControlChangeOutputAttribute(int number, String group, int groupColumn, int groupRow, String displayName = null) :
                base(Node.ShowBackingValue.Never, Node.ConnectionType.Multiple)
            {
                Number = number;
                Group = group;
                GroupColumn = groupColumn;
                GroupRow = groupRow;
                DisplayName = displayName;
            }
        }
    }
}