using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNode;
using MidiJack;
using Eidetic.Confluence;

[CreateNodeMenu("MIDI/NoteOn"),
    NodeTint(Colors.ExternalInputTint)]
public class NoteOn : Node
{
    static readonly float TriggerHoldTime = .003f;

    public MidiChannel Channel;
    public int NoteNumber;
    [Output(ShowBackingValue.Never)] public bool Trigger;
    float LastTriggeredTime;

    class NodePortTriggerField
    {
        // TODO This can't be static, we need to make it per NoteOn node
        public static List<NodePortTriggerField> TriggerFields = new List<NodePortTriggerField>();
        readonly MethodInfo Field;
        readonly ITriggeredNode Node;
        readonly Action<bool> Setter;
        NodePortTriggerField(MethodInfo field, ITriggeredNode node)
        {
            Field = field;
            Node = node;
            Setter = (value) => Field.Invoke(Node, new object[] { true });
        }
        /// <summary>
        /// Create a "Trigger" with a Setter that has a reference kept in the list TriggerFields.
        /// 
        /// By storing these set methods' reference in the list, we can easily set the value whenever we want,
        /// and pass setter methods around like any other variable.
        /// </summary>
        /// <returns>NodePortTriggerField</returns>
        public static NodePortTriggerField Create(MethodInfo field, ITriggeredNode node)
        {
            var newTriggerField = new NodePortTriggerField(field, node);
            TriggerFields.Add(newTriggerField);
            return newTriggerField;
        }

        public void Set(bool value)
        {
            Setter.Invoke(value);
        }
    }

    public interface ITriggeredNode
    {
        bool Trigger { set; }
    }

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        if (to.nodeType.GetInterfaces().Contains(typeof(ITriggeredNode)))
        {
            var field = typeof(ITriggeredNode).GetProperty("Trigger").GetSetMethod();
            NodePortTriggerField.Create(field, to.node as ITriggeredNode);

            // Make sure trigger is part of the delegate
            MidiMaster.noteOnDelegate -= TriggerConnectedNodes;
            MidiMaster.noteOnDelegate += TriggerConnectedNodes;
        }
    }

    void TriggerConnectedNodes(MidiChannel channel, int noteNumber, float velocity)
    {
        if (channel == Channel && noteNumber == NoteNumber && velocity != 0)
        {
            NodePortTriggerField.TriggerFields.ForEach(tf => tf.Set(true));
        }
    }
}