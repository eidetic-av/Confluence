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
    public Midi.Channel Channel;
    public int Number;
    public bool BiDirectional;
    [Output(ShowBackingValue.Always)] public int Int;
    [Output(ShowBackingValue.Always)] public float Float;

    protected override void Init()
    {
        Debug.Log("init");
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
            Device.Open();
            Device.StartReceiving(null);
            Device.ControlChange += UpdateValues;
            Debug.LogFormat("Opened MIDI Device: {0}", Device.Name);
        }
    }

    void UpdateValues(ControlChangeMessage m)
    {
        if (m.Channel == Channel)
        {
            if (m.Control.Number() == Number)
            {
                Int = m.Value;
                Float = (m.Value / 127f);
                if (BiDirectional)
                {
                    Int -= 64;
                    Float -= .5f;
                }
            }
        }
    }
}