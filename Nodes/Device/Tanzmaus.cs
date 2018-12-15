
using System.Collections.Generic;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Device/Tanzmaus"),
        NodeTint(Colors.DeviceTint)]
    public class Tanzmaus : Device
    {
        DevicePanel PanelLayout = new DevicePanel
        {
            Groups = new List<DevicePanel.PanelGroup>()
            {
                new DevicePanel.PanelGroup("BD", 2, 3, 0, 0),
                new DevicePanel.PanelGroup("SD", 1, 3, 1, 0),
                new DevicePanel.PanelGroup("CP", 1, 2, 2, 0),
                new DevicePanel.PanelGroup("TT", 2, 2, 3, 0),
                new DevicePanel.PanelGroup("SP1", 2, 1, 2, 1),
                new DevicePanel.PanelGroup("SP2", 2, 1, 2, 1)
            }
        };
        public override DevicePanel GetDevicePanelLayout()
        {
            return PanelLayout;
        }

        // BD
        [ControlChangeOutput(2, "BD", 0, 0, "Attack")] public float BassDrumAttack;
        [ControlChangeOutput(5, "BD", 0, 1, "Decay")] public float BassDrumDecay;
        [ControlChangeOutput(65, "BD", 0, 2, "Pitch")] public float BassDrumPitch;
        [ControlChangeOutput(5, "BD", 1, 0, "Noise Decay")] public float BassDrumNoiseDecay;
        [ControlChangeOutput(4, "BD", 1, 1, "Noise")] public float BassDrumNoiseLevel;
        [ControlChangeOutput(3, "BD", 1, 2, "Tune")] public float BassDrumTune;

        // SD
        [ControlChangeOutput(67, "SD", 0, 0, "Noise Decay")] public float SnareNoiseDecay;
        [ControlChangeOutput(13, "SD", 0, 1, "Noise")] public float SnareNoiseLevel;
        [ControlChangeOutput(11, "SD", 0, 2, "Tune")] public float SnareTune;

        // Clap
        [ControlChangeOutput(18, "CP", 0, 1, "Filter")] public float ClapFilter;
        [ControlChangeOutput(75, "CP", 0, 2, "Decay")] public float ClapDecay;

        // TT
        [ControlChangeOutput(79, "TT", 0, 0, "Attack")] public float TomAttack;
        [ControlChangeOutput(82, "TT", 0, 1, "Pitch")] public float TomPitch;
        [ControlChangeOutput(20, "TT", 1, 0, "Decay")] public float TomDecay;
        [ControlChangeOutput(19, "TT", 1, 1, "Tune")] public float TomTune;

        // SP1
        [ControlChangeOutput(84, "SP1", 0, 0, "Tune")] public float Sample1Tune;
        [ControlChangeOutput(85, "SP1", 1, 0, "Decay")] public float Sample1Decay;

        // SP2
        [ControlChangeOutput(89, "SP2", 0, 0, "Tune")] public float Sample2Tune;
        [ControlChangeOutput(90, "SP2", 1, 0, "Decay")] public float Sample2Decay;
    }
}