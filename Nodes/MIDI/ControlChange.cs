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

[CreateNodeMenu("MIDI/ControlChange"),
    NodeTint(Colors.ExternalInputTint)]
public class ControlChange : RuntimeNode
{
    public String DeviceName;
    public InputDevice Device { get; private set; }
    // public Midi.Channel Channel;
    public int Number;
    public bool BiDirectional;

    // float ccValue;
    // [Output] public int Int => Mathf.RoundToInt(ccValue * 127);
    // [Output] public float Float => BiDirectional ? ccValue - 0.5f : ccValue;
    
    int ccValue;
    [Output] public int Int => BiDirectional ? ccValue - 64 : ccValue;
    [Output] public float Float => Int / 127f;

    internal override void Start()
    {
        base.Start();
        if (Application.isPlaying)
        {
            // MidiJack.MidiDriver.Instance.knobDelegate += (MidiJack.MidiChannel channel, int knobNumber, float knobValue) =>
            // {
            //         if (knobNumber == Number) ccValue = knobValue;
            // };
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

                Device.ControlChange += (ControlChangeMessage m) =>
                {
                    if (m.Control.Number() == Number) ccValue = m.Value;
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