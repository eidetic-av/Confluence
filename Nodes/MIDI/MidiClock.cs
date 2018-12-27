using MidiJack;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using XNode;

namespace Eidetic.Confluence.Midi
{
    public class MidiClock : RuntimeNode
    {
        [Output(ShowBackingValue.Always)]
        public float BeatsPerMinute;
        [Output(ShowBackingValue.Always)]
        public float SecondsPerBeat;
        public override void ValueUpdate()
        {
            BeatsPerMinute = MidiDriver.BeatsPerMinute;
            SecondsPerBeat = MidiDriver.SecondsPerBeat;
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "BeatsPerMinute")
                return BeatsPerMinute;
            else if (port.fieldName == "SecondsPerBeat")
                return SecondsPerBeat;
            else
                return null;
        }
    }
}