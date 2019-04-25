using Midi;
using Eidetic;
using Eidetic.Confluence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNode;

[CreateNodeMenu("MIDI/SingleNoteOn"),
    NodeTint(Colors.ExternalInputTint)]
public class SingleNoteOn : RuntimeNode
{
    public String DeviceName;
    public InputDevice Device { get; private set; }
    public Midi.Channel Channel;
    public int NoteNumber;

    bool noteOn = false;
    [Output]
    public bool NoteOn
    {
        get => noteOn;
        set
        {
            if (value)
            {
                noteOn = true;
                Trigger = true;
            }
            else
            {
                noteOn = false;
                NoteOffTrigger = true;
                Velocity = 0;
            }
        }
    }
    [Output] public int NoteOnInt => NoteOn ? 1 : 0;
    bool trigger = false;
    [Output]
    public bool Trigger
    {
        get
        {
            if (trigger)
            {
                trigger = false;
                return true;
            }
            return false;
        }
        set
        {
            trigger = value;
        }
    }
    [Output] public int TriggerInt => Trigger ? 1 : 0;

    [Output] public int Velocity;

    bool noteOffTrigger = false;
    [Output]
    public bool NoteOffTrigger
    {
        get
        {
            if (noteOffTrigger)
            {
                noteOffTrigger = false;
                return true;
            }
            return false;
        }
        set
        {
            noteOffTrigger = value;
        }
    }
    [Output] public int NoteOffTriggerInt => NoteOffTrigger ? 1 : 0;

    internal override void Start()
    {
        base.Start();
        if (Application.isPlaying)
        {
            foreach (InputDevice inputDevice in InputDevice.InstalledDevices)
            {
                if (inputDevice.Name.ToLower().Equals(DeviceName.ToLower()))
                {
                    Device = inputDevice;
                    break;
                }
            }
            if (Device != null)
            {
                if (!InputDevice.OpenedDevices.Contains(Device))
                {
                    Debug.LogFormat("Opening MIDI Device: {0}", Device.Name);
                    Device.Open();
                    Device.StartReceiving(null);
                    Debug.LogFormat("Successfully opened MIDI Device: {0}", Device.Name);
                }

                Device.NoteOn += (NoteOnMessage m) =>
                {
                    if (m.Channel == Channel && m.Pitch.NoteNumber() == NoteNumber) {
                        NoteOn = true;
                        Velocity = m.Velocity;
                    }
                };

                Device.NoteOff += (NoteOffMessage m) =>
                {
                    if (m.Channel == Channel && m.Pitch.NoteNumber() == NoteNumber) {
                        NoteOn = false;
                        Velocity = 0;
                    }
                };
            }
        }
    }

    internal override void Exit()
    {
        if (Device != null && Device.IsOpen)
        {
            Device.StopReceiving();
            Device.Close();
        }
    }
}