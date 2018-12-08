
namespace Eidetic.Confluence
{
    [CreateNodeMenu("Device/Tanzmaus"),
        NodeTint(Colors.DeviceTint)]
    public class Tanzmaus : Device
    {
        // BD
        [ControlChangeOutput(2)] public float BassDrumAttack;
        [ControlChangeOutput(5)] public float BassDrumDecay;
        [ControlChangeOutput(65)] public float BassDrumPitch;
        [ControlChangeOutput(3)] public float BassDrumTune;
        [ControlChangeOutput(4)] public float BassDrumNoiseLevel;
        // [ControlChangeOutput(5)] public float BassDrumNoiseDecay;

        // SD
        [ControlChangeOutput(67)] public float SnareNoiseDecay;
        [ControlChangeOutput(13)] public float SnareNoiseLevel;
        [ControlChangeOutput(11)] public float SnareTune;

        // Clap
        [ControlChangeOutput(18)] public float ClapFilter;
        [ControlChangeOutput(75)] public float ClapDecay;

        // TT
        [ControlChangeOutput(79)] public float TomAttack;
        [ControlChangeOutput(20)] public float TomDecay;
        [ControlChangeOutput(82)] public float TomPitch;
        [ControlChangeOutput(19)] public float TomTune;

        // SP1
        [ControlChangeOutput(84)] public float Sample1Tune;
        [ControlChangeOutput(85)] public float Sample1Decay;

        // SP2
        [ControlChangeOutput(89)] public float Sample2Tune;
        [ControlChangeOutput(90)] public float Sample2Decay;
    }
}