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
            if (Input == -1) return;

            if (Input != LastInput)
                if (PointCloudReceiver.Instance != null)
                    if (PointCloudReceiver.Instance.ParticleSystems[Input] != null)
                        PointCloudReceiver.Instance.ParticleSystems[Input].Emit = true;

            LastInput = Input;
        }
    }
}