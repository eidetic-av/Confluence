using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Control/PointCloudEmitter")]
    public class PointCloudEmitter : RuntimeNode
    {
        [Input] public int Input = -1;
        int LastInput;

        internal override void EarlyUpdate()
        {
            // if (Input != LastInput && Input != -1)
            // {
            //     if (LiveScanReceiver.Instance != null)
            //         if (LiveScanReceiver.Instance.ParticleSystems[Input] != null)
            //             LiveScanReceiver.Instance.ParticleSystems[Input].Emit = true;
            // }

            LastInput = Input;
        }
    }
}