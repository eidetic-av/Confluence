namespace Eidetic.Confluence
{
    [CreateNodeMenu("Control/PointCloudEmitter")]
    public class PointCloudEmitter : RuntimeNode
    {
        [Input] public int Input = -1;
        int LastInput;

        internal override void EarlyUpdate()
        {
            if (Input != LastInput && Input != -1)
            {
                if (PointCloudReceiver.Instance != null)
                    if (PointCloudReceiver.Instance.ParticleSystems[Input] != null)
                        PointCloudReceiver.Instance.ParticleSystems[Input].Emit = true;
            }

            LastInput = Input;
        }
    }
}