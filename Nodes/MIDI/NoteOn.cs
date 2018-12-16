using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using MidiJack;
using Eidetic.Confluence;

[CreateNodeMenu("MIDI/NoteOn"),
    NodeTint(Colors.ExternalInputTint)]
public class NoteOn : RuntimeNode
{
    public MidiChannel Channel;
    public int NoteNumber;

    [Output] public bool Trigger = false;

    protected override void Init()
    {
        base.Init();
        MidiMaster.noteOnDelegate += (MidiChannel channel, int noteNumber, float velocity) =>
        {
            if (channel == Channel && noteNumber == NoteNumber)
            {
                Trigger = true;
            }
        };
    }

    void OnDestroy()
    {
    }

    void OnValidate()
    {
    }
    public override void ValueUpdate()
    {
    }

    public override void LateUpdate() 
    {
        Trigger = false;
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