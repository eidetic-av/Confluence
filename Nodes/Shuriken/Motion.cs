using UnityEngine;
using XNode;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public class Motion : ShurikenNode
    {
        [Input]
        public Particle[] Particles
        {
            get
            {
                if (System == null) return new Particle[0];
                return particles;
            }
            set { }
        }

        [SerializeField]
        public float noiseIntensity = 0f;

        [Input]
        public float NoiseIntensity
        {
            get
            {
                return noiseIntensity;
            }
            set
            {
                noiseIntensity = value;
                if (System == null) return;
                var noiseModule = System.noise;
                noiseModule.enabled = noiseIntensity != 0;
                noiseModule.strength = noiseIntensity;
            }
        }

        public override void OnRemoveConnection(NodePort removedPort) 
        { 
            var noiseModule = System.noise;
            noiseModule.enabled = false;
            base.OnRemoveConnection(removedPort);
        }
    }
}