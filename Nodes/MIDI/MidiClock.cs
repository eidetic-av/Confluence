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
        [SerializeField] string DeviceName = "";
        [Output] public float BeatsPerMinute => DeviceName != "" ? MidiManager.ActiveInputDevices[DeviceName].BPM : 120f;
    }
}