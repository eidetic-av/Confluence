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

    int intValue;
    [Output] public int Int => BiDirectional ? intValue - 64 : intValue;
    [Output] public float Float => Int / 127f;

    internal override void Awake()
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
            if (!Device.IsOpen)
            {
                Debug.LogFormat("Opening MIDI Device: {0}", Device.Name);
                Device.Open();
                Device.StartReceiving(null);
                Debug.LogFormat("Successfully opened MIDI Device: {0}", Device.Name);
            }

            Device.ControlChange += (ControlChangeMessage m) =>
            {
                if (m.Control.Number() == Number) intValue = m.Value;
            };
        }
    }

    internal override void Exit()
    {
        if (Device != null && Device.IsOpen)
            Device.Close();
    }
}