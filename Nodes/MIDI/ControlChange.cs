using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNode;
using MidiJack;
using Eidetic.Confluence;

[CreateNodeMenu("MIDI/ControlChange"),
    NodeTint(Colors.ExternalInputTint)]
public class ControlChange : RuntimeNode
{
    public MidiChannel Channel;
    public int Number;
    public bool BiDirectional;
    [Output(ShowBackingValue.Never)] public int Int;
    [Output(ShowBackingValue.Never)] public float Float;

    public override void Update()
    {
        Debug.Log(MidiMaster.GetKnob(Channel, Number));
    }

    public override object GetValue(NodePort port)
    {
        switch (port.fieldName)
        {
            case "Int":
                {
                    if (BiDirectional)
                        return Mathf.RoundToInt(MidiMaster.GetKnob(Channel, Number) * 127) - 64;
                    else
                        return Mathf.RoundToInt(MidiMaster.GetKnob(Channel, Number) * 127);
                };
            case "Float":
                {
                    if (BiDirectional)
                        return MidiMaster.GetKnob(Channel, Number) - .5f;
                    else
                        return MidiMaster.GetKnob(Channel, Number);
                }
            default: return null;
        }
    }
}