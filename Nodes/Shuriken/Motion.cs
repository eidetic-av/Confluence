using UnityEngine;
using XNode;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public class Motion : ShurikenNode
    {
        [Input] public Particle[] ParticlesInput = new Particle[0];

        [SerializeField] float noiseIntensity = 0f;
        [Input] public float NoiseIntensity
        {
            get
            {
                return noiseIntensity;
            }
            set
            {
                noiseIntensity = value;
                var noiseModule = System.noise;
                noiseModule.strength = noiseIntensity;
            }
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            var noiseModule = System.noise;
            noiseModule.enabled = true;
            base.OnCreateConnection(from, to);
        }

        public override void OnRemoveConnection(NodePort removedPort)
        {
            var noiseModule = System.noise;
            noiseModule.enabled = false;
            base.OnRemoveConnection(removedPort);
        }
    }
}