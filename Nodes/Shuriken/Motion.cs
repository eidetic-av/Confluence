using UnityEngine;

namespace Eidetic.Confluence.Shuriken
{
    public class Motion : ShurikenNode
    {
        [SerializeField] float noiseIntensity = 0f;
        [Input] public float NoiseIntensity
        {
            get => noiseIntensity;
            set
            {
                noiseIntensity = value;
                if (ParticleSystem == null) return;
                var noiseModule = ParticleSystem.noise;
                noiseModule.strength = noiseIntensity;
                noiseModule.enabled = noiseIntensity != 0;
            }
        }

        [SerializeField] float scrollSpeed = 0f;
        [Input] public float ScrollSpeed
        {
            get => scrollSpeed;
            set
            {
                scrollSpeed = value;
                if (ParticleSystem == null) return;
                var noiseModule = ParticleSystem.noise;
                noiseModule.scrollSpeed = scrollSpeed;
            }
        }

        internal override void OnParticleSystemDisconnected() 
        {
            var noiseModule = ParticleSystem.noise;
            noiseModule.enabled = false;
        }

    }
}