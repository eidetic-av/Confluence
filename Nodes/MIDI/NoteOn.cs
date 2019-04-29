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

[CreateNodeMenu("MIDI/NoteOn"),
    NodeTint(Colors.ExternalInputTint)]
public class NoteOn : RuntimeNode
{
    public String DeviceName;
    public InputDevice Device { get; private set; }
    public Midi.Channel Channel;

    bool noteOn = false;
    [Output]
    public bool NoteIsOn
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
    [Output] public int NoteOnInt => NoteIsOn ? 1 : 0;
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

    [Output] public float VelocityFloat => Velocity / 127f;

    [Output] public int NoteNumber;

    [Output] public float NoteNumberFloat => Velocity / 127f;

    [Output] public int LastNoteVelocity;

    [Output] public float LastNoteVelocityFloat => LastNoteVelocity / 127f;

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
                    if (m.Channel == Channel)
                    {
                        Threads.RunOnMain(() =>
                        {
                            NoteNumber = m.Pitch.NoteNumber();
                            Velocity = m.Velocity;
                            NoteIsOn = Velocity > 0;
                            if (NoteIsOn)
                                LastNoteVelocity = Velocity;
                            Trigger = true;
                        });
                    }
                };

                Device.NoteOff += (NoteOffMessage m) =>
                {
                    if (m.Channel == Channel)
                    {
                        NoteNumber = m.Pitch.NoteNumber();
                        NoteIsOn = false;
                        Velocity = 0;
                        NoteOffTrigger = true;
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