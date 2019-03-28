using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eidetic.Confluence;
using Eidetic.Utility;
using MidiJack;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence.Midi
{
    [CreateNodeMenu("MIDI/NoteOn"), NodeTint(Colors.ExternalInputTint)]
    public class NoteOn : RuntimeNode
    {
        [SerializeField] public MidiChannel Channel;
        [SerializeField] public int MinNoteNumber = 0;
        [SerializeField] public int MaxNoteNumber = 127;

        bool noteOnTrigger = false;
        [Output] public bool NoteOnTrigger
        {
            get
            {
                if (noteOnTrigger)
                {
                    noteOnTrigger = false;
                    return true;
                }
                return false;
            }
        }

        bool noteOffTrigger = false;
        [Output] public bool NoteOffTrigger
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
        }

        [Output] public int NoteNumber = 0;
        [Output] public float NotePosition => NoteNumber.Map(MinNoteNumber, MaxNoteNumber, 0f, 1f);
        [Output] public float Velocity = 0f;
        [Output] public bool NoteIsOn => Velocity != 0;

        new public void OnEnable()
        {
            base.OnEnable();

            Threads.RunAtStart(() =>
            {
                MidiDriver.Instance.noteOnDelegate += OnNoteOnReceived;
                MidiDriver.Instance.noteOffDelegate += OnNoteOffReceived;
            });
        }

        void OnDestroy()
        {
            if (!Application.isPlaying) return;
            MidiDriver.Instance.noteOnDelegate -= OnNoteOnReceived;
            MidiDriver.Instance.noteOffDelegate -= OnNoteOffReceived;
        }

        void OnNoteOnReceived(MidiChannel channel, int note, float velocity)
        {
            if (channel == Channel && note >= MinNoteNumber && note <= MaxNoteNumber)
            {
                if (velocity != 0)
                {
                    NoteNumber = note;
                    Velocity = velocity;
                    noteOnTrigger = true;
                }
                else OnNoteOffReceived(channel, note);
            }
        }

        void OnNoteOffReceived(MidiChannel channel, int note)
        {
            if (channel == Channel && note >= MinNoteNumber && note <= MaxNoteNumber)
            {
                NoteNumber = note;
                Velocity = 0f;
                noteOffTrigger = true;
            }
        }
    }
}