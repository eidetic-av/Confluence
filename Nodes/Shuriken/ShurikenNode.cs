using UnityEngine;
using Eidetic.Unity.Utility;
using XNode;
using System.Linq;

using static Eidetic.Confluence.Shuriken.SystemManager;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public abstract class ShurikenNode : RuntimeNode
    {
        public ParticleSystem System { get; protected set; }

        internal override void Start()
        {
            if (GetType() == typeof(Emission)) return;

            // Todo: Instantiating this system with any connected node this has an instantiated system
            // Probably just use the system manager instead?
            System = Ports.Where(port => port.IsConnected)
                .Select(port => port.Node)
                .OfType<ShurikenNode>()?
                .FirstOrDefault(node => node.System != null)?
                .System;
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            var toNode = to.Node as ShurikenNode;
            if (toNode == this && System == null)
            {
                var fromNode = from.Node as ShurikenNode;
                if (fromNode.GetType() == typeof(Emission))
                    System = fromNode.System;
            }
            base.OnCreateConnection(from, to);
        }
        
        public override void OnRemoveConnection(NodePort removedPort) 
        { 
            if (removedPort.Node != this || GetType() == typeof(Emission)) return;
            if (!Ports.Where(port => port.IsConnected && port.Node.GetType() == typeof(Emission)).Any())
                System = null;
            base.OnRemoveConnection(removedPort);
        }
        
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
    }
}