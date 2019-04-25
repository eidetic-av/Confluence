//
// MidiJack - MIDI Input Plugin for Unity
//
// Copyright (C) 2013-2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Eidetic.Unity.Utility;
using UnityEngine;

namespace MidiJack
{
    public class MidiDriver : MonoBehaviour
    {
        #region Internal Data

        class ChannelState
        {
            // Note state array
            // X<0    : Released on this frame
            // X=0    : Off
            // 0<X<=1 : On (X represents velocity)
            // 1<X<=2 : Triggered on this frame
            //          (X-1 represents velocity)
            public float[] _noteArray;

            // Knob number to knob value mapping
            public Dictionary<int, float> _knobMap;

            public ChannelState()
            {
                _noteArray = new float[128];
                _knobMap = new Dictionary<int, float>();
            }
        }

        // Channel state array
        ChannelState[] _channelArray;

        // Last update frame number
        int _lastFrame;

        // Timecode message history
        Queue<float> _midiClockHistory;

        public Queue<float> MidiClockHistory
        {
            get { return _midiClockHistory; }
        }
        System.Diagnostics.Stopwatch MidiClockTimer;
        public static float BeatsPerMinute;
        public static float SecondsPerBeat;

        #endregion

        #region Event Delegates

        public delegate void NoteOnDelegate(MidiChannel channel, int note, float velocity);
        public delegate void NoteOffDelegate(MidiChannel channel, int note);
        public delegate void KnobDelegate(MidiChannel channel, int knobNumber, float knobValue);

        public NoteOnDelegate noteOnDelegate { get; set; }
        public NoteOffDelegate noteOffDelegate { get; set; }
        public KnobDelegate knobDelegate { get; set; }

        #endregion

        #region Editor Support

#if UNITY_EDITOR

        // Update timer
        const float _updateInterval = 1.0f / 120;
        float _lastUpdateTime;

        bool ShouldUpdate
        {
            get
            {

                if (Application.isPlaying && _lastFrame != Time.frameCount)
                {
                    _lastFrame = Time.frameCount;
                    return true;
                }
                else
                {
                    var current = Time.realtimeSinceStartup;
                    if (current - _lastUpdateTime > _updateInterval || current < _lastUpdateTime)
                    {
                        _lastUpdateTime = current;
                        return true;
                    }
                    return false;
                }
            }
        }

        // Total message count
        public int TotalMessageCount { get; private set; }

        // Message history
        Queue<MidiMessage> _messageHistory;

        public Queue<MidiMessage> History
        {
            get { return _messageHistory; }
        }

#endif

        #endregion

        #region Public Methods

        MidiDriver()
        {
            _channelArray = new ChannelState[17];
            for (var i = 0; i < 17; i++)
                _channelArray[i] = new ChannelState();

            _midiClockHistory = new Queue<float>();
            MidiClockTimer = new System.Diagnostics.Stopwatch();
            MidiClockTimer.Start();

#if UNITY_EDITOR
            _messageHistory = new Queue<MidiMessage>();
#endif
        }

        #endregion

        #region Private Methods

        void Update()
        {
#if UNITY_EDITOR
            if (ShouldUpdate)
#endif
                DequeueMessages();
        }

        public void DequeueMessages()
        {
            // Update the note state array.
            foreach (var cs in _channelArray)
            {
                for (var i = 0; i < 128; i++)
                {
                    var x = cs._noteArray[i];
                    if (x > 1)
                        cs._noteArray[i] = x - 1; // Key down -> Hold.
                    else if (x < 0)
                        cs._noteArray[i] = 0; // Key up -> Off.
                }
            }

            // Process the message queue.
            while (true)
            {
                // Pop from the queue.
                var data = DequeueIncomingData();
                if (data == 0) break;

                // Parse the message.
                var message = new MidiMessage(data);

                // Split the first byte.
                var statusCode = (StatusCode)(message.status >> 4);
                var channelNumber = message.status & 0xf;

                if (statusCode == StatusCode.NoteOn)
                {
                    var velocity = 1.0f / 127 * message.data2 + 1;
                    _channelArray[channelNumber]._noteArray[message.data1] = velocity;
                    _channelArray[(int)MidiChannel.All]._noteArray[message.data1] = velocity;
                    if (noteOnDelegate != null)
                        noteOnDelegate((MidiChannel)channelNumber, message.data1, velocity - 1);
                }

                else if (statusCode == StatusCode.NoteOff || (statusCode == StatusCode.NoteOn && message.data2 == 0))
                {
                    _channelArray[channelNumber]._noteArray[message.data1] = -1;
                    _channelArray[(int)MidiChannel.All]._noteArray[message.data1] = -1;
                    if (noteOffDelegate != null)
                        noteOffDelegate((MidiChannel)channelNumber, message.data1);
                }

                else if (statusCode == StatusCode.ControlChange)
                {
                    // Normalize the value.
                    var level = 1.0f / 127 * message.data2;
                    // Update the channel if it already exists, or add a new channel.
                    _channelArray[channelNumber]._knobMap[message.data1] = level;
                    // Do again for All-ch.
                    _channelArray[(int)MidiChannel.All]._knobMap[message.data1] = level;
                    if (knobDelegate != null)
                        knobDelegate((MidiChannel)channelNumber, message.data1, level);
                }

                else if (statusCode == StatusCode.Clock)
                {
                    // Add the current time to the clock history queue
                    _midiClockHistory.Enqueue(Time.time);
                    // Keep 96 messages in the queue (a bar worth)
                    while (_midiClockHistory.Count > 96)
                    {
                        _midiClockHistory.Dequeue();
                        // Calculate the tempo by averaging the difference between each time recorded
                        var times = _midiClockHistory.ToArray();
                        var sum = 0f;
                        for (int i = 1; i < times.Length; i++)
                        {
                            sum += times[i] - times[i - 1];
                        }
                        SecondsPerBeat = sum / 96;
                        BeatsPerMinute = 60 / (SecondsPerBeat * 24);
                    }
                }

#if UNITY_EDITOR
                // If it's not a clock, record the message.
                if (statusCode != StatusCode.Clock)
                {
                    TotalMessageCount++;
                    _messageHistory.Enqueue(message);
                }
#endif
            }

#if UNITY_EDITOR
            // Truncate the history.
            while (_messageHistory.Count > 8)
                _messageHistory.Dequeue();
#endif
        }

        #endregion

        #region Native Plugin Interface

        [DllImport("MidiJackPlugin", EntryPoint = "MidiJackDequeueIncomingData")]
        public static extern ulong DequeueIncomingData();

        #endregion

        #region Singleton Class Instance

        static MidiDriver instance;

        public static MidiDriver Instance
        {
            get
            {
                if (instance == null && Application.isPlaying)
                {
                    instance = new GameObject("Midi Updater")
                        .WithHideFlags(HideFlags.NotEditable)
                        .InDontDestroyMode()
                        .AddComponent<MidiDriver>();
                }
                return instance;
            }
        }

        public enum StatusCode
        {
            NoteOn = 9, NoteOff = 8, ControlChange = 0xb, Clock = 0xf
        }

        #endregion
    }
}