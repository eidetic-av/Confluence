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

        public override object GetValue(NodePort port)
        {
            if (port.MemberName == "BeatsPerMinute")
                return BeatsPerMinute;
            else if (port.MemberName == "SecondsPerBeat")
                return SecondsPerBeat;
            else
                return null;
        }
    }
}