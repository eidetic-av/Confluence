using Eidetic.Buddah.Midi;
using UnityEngine;

namespace Eidetic.Confluence.Midi
{
    public class MidiClock : RuntimeNode
    {
        [SerializeField] string DeviceName = "";
        [Output] public float BeatsPerMinute => DeviceName != "" ? MidiManager.ActiveInputDevices[DeviceName].BPM : 120f;
    }
}