using UnityEngine;
using Eidetic.Unity.Utility;

using static Eidetic.Confluence.Shuriken.SystemManager;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public abstract class ShurikenNode : RuntimeNode
    {
        public ParticleSystem System { get; protected set; }
        
        [Output] public int ParticleCount => System == null ? 0 : System.particleCount;

        // This is the internal array used within all nodes connected to the system.
        internal Particle[] particles = null;

        // This outputs the corresponding ParticleSystem's Particle array.
        // If the particle array for this frame is unset, get the particles and
        // set the backing field.
        [Output]
        public Particle[] Particles
        {
            get
            {
                if (System == null) return new Particle[0];
                if (particles == null)
                {
                    particles = new Particle[ParticleCount];
                    System.GetParticles(particles, ParticleCount);
                }
                return particles;
            }
        }
        internal override void LateUpdate()
        {

        }
    }
}