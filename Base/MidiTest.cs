using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eidetic.Buddah.Midi;

public class MidiTest : MonoBehaviour
{
    async void Start()
    {
        Eidetic.Buddah.Logger.LogDelegate = Debug.LogFormat;

        await MidiRouter.Forward("Engine MIDI", "OP-1 Midi Device");
        await MidiRouter.Forward("Engine MIDI", "Moog Minitaur");
    }

    private void Update()
    {
    }

    void OnDestroy()
    {
        MidiManager.Close();
    }
}
