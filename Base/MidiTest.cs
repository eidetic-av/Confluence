using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eidetic.Buddah.Midi;

public class MidiTest : MonoBehaviour
{
    async void Start()
    {
        Eidetic.Buddah.Logger.LogDelegate = Debug.LogFormat;
    }

    private void Update()
    {
    }

    void OnDestroy()
    {
        MidiManager.Close();
    }
}
