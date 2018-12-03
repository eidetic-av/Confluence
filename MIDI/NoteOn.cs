using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using MidiJack;

[CreateNodeMenu("MIDI/NoteOn"),
    NodeTint(Nodes.Colors.ExternalInputTint)]
public class NoteOn : RuntimeNode
{
    public MidiChannel Channel;
    public int NoteNumber;

    [Output] public bool Trigger = false;

    protected override void Init()
    {
        base.Init();
    }

    void OnDestroy()
    {
    }

    void OnValidate()
    {
    }
    public override void ValueUpdate()
    {
        Trigger = false;
        if (MidiMaster.GetKeyDown(Channel, NoteNumber))
        {
            Trigger = true;
        }
    }

    public override object GetValue(NodePort port)
    {
        switch (port.fieldName)
        {
            case "Trigger":
                return Trigger;
			default:
				return null;
        }
    }
}