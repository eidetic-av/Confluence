using Eidetic.Buddah.Midi;
using UnityEngine;

public class MidiTest : MonoBehaviour
{
    async void Start()
    {
        Eidetic.Buddah.Logger.LogDelegate = UnityEngine.Debug.LogFormat;
    }

    private void Update()
    {
    }

    void OnDestroy()
    {
        MidiManager.Close();
    }
}
