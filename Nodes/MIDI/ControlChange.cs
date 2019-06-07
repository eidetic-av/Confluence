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
    public Channel Channel;
    public int Number;
    
    [Output] public int Int;
    [Output] public float Float => Int / 127f;

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
                    UnityEngine.Debug.LogFormat("Opening MIDI Device: {0}", Device.Name);
                    Device.Open();
                    Device.StartReceiving(null);
                    UnityEngine.Debug.LogFormat("Successfully opened MIDI Device: {0}", Device.Name);
                }

                Device.ControlChange += (ControlChangeMessage m) =>
                {
                    UnityEngine.Debug.Log(m.Control.Number());
                    if (m.Channel == Channel && m.Control.Number() == Number)
                        Int = m.Value;
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