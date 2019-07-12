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

    [Output] public float VelocityFloat => Velocity / 127f;

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

    float time;

    internal override void Start()
    {
        base.Start();
        //if (Application.isPlaying)
        //{
        //    foreach (InputDevice inputDevice in InputDevice.InstalledDevices)
        //    {
        //        if (inputDevice.Name.ToLower().Equals(DeviceName.ToLower()))
        //        {
        //            Device = inputDevice;
        //            break;
        //        }
        //    }
        //    if (Device != null)
        //    {
        //        if (!InputDevice.OpenedDevices.Contains(Device))
        //        {
        //            Debug.LogFormat("Opening MIDI Device: {0}", Device.Name);
        //            Device.Open();
        //            Device.StartReceiving(null);
        //            Debug.LogFormat("Successfully opened MIDI Device: {0}", Device.Name);
        //        }

        //        Device.NoteOn += (NoteOnMessage m) =>
        //        {
        //            if (m.Channel == Channel && m.Pitch.NoteNumber() == NoteNumber)
        //            {
        //                Velocity = m.Velocity;
        //                NoteOn = Velocity > 0;
        //                if (NoteOn)
        //                {
        //                    Threads.RunOnMain(() => Debug.Log(m.Time));
        //                    LastNoteVelocity = Velocity;
        //                    Trigger = true;
        //                }
        //            }
        //        };

        //        Device.NoteOff += (NoteOffMessage m) =>
        //        {
        //            if (m.Channel == Channel && m.Pitch.NoteNumber() == NoteNumber)
        //            {
        //                NoteOn = false;
        //                Velocity = 0;
        //                NoteOffTrigger = true;
        //            }
        //        };
        //    }
        //}

        if (Application.isPlaying)
        {
            //GameObject.Find("ChuckSub").GetComponent<ChuckSubInstance>().RunCode(@"
		       // SinOsc foo => dac;
		       // while( true )
		       // {
			      //  Math.random2f( 300, 1000 ) => foo.freq;
			      //  100::ms => now;
		       // }
	        //");
        }

    }

    int t = 0;
    internal override void Update()
    {
        base.Update();
        if (Trigger)
        {
            Debug.Log(Time.time);
            if (t++ % 2 == 0)
                Camera.main.backgroundColor = Color.white;
            else
                Camera.main.backgroundColor = Color.black;
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