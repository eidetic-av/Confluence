using MidiJack;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using XNode;
using Eidetic.Buddah.Midi;

namespace Eidetic.Confluence.Midi
{
    public class MidiClock : RuntimeNode
    {
        [Output] public float BeatsPerMinute => MidiManager.ActiveInputDevices["Engine MIDI"].BPM;
    }
}