using System.Collections.Generic;
using System.Linq;
using Eidetic.Unity.Utility;
using Eidetic.Utility;
using UnityEngine;
using XNode;
using static System.Collections.Specialized.NotifyCollectionChangedAction;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public abstract class ShurikenNode : RuntimeNode
    {
        private ParticleSystem particleSystem;
        public ParticleSystem ParticleSystem
        {
            get
            {
                if (!Application.isPlaying) return null;
                if (particleSystem != null) return particleSystem;
                if (GetType() == typeof(Emission))
                    return particleSystem = CreateParticleSystemInstance();
                else return particleSystem = ConnectedShurikenNodes
                    .FirstOrDefault(connectedNode => connectedNode.ParticleSystem != null) ?
                    .ParticleSystem;
            }
        }

        // Dummy input, only used to connect ShurikenNodes together
        [Input] public Particle[] Input = new Particle[0];

        [Output] public Particle[] Particles
        {
            get
            {
                if (ParticleSystem == null) return new Particle[0];
                var particles = new Particle[ParticleSystem.particleCount];
                ParticleSystem.GetParticles(particles, ParticleSystem.particleCount);
                return particles;
            }
        }

        private static ParticleSystem CreateParticleSystemInstance() =>
            Instantiate(Resources.Load<GameObject>("Shuriken Instance"))
            .InGroup("Shuriken")
            .GetComponent<ParticleSystem>();

        public IEnumerable<ShurikenNode> ConnectedShurikenNodes =>
            Ports.Where(port => port.IsConnected)
            .Select(port => port.Connection.Node)
            .OfType<ShurikenNode>();

        public override void OnRemoveConnection(NodePort removedPort)
        {
            base.OnRemoveConnection(removedPort);
            if (removedPort.MemberName == "Input")
            {
                OnParticleSystemDisconnected();
                particleSystem = null;
            }
        }

        internal override void Exit()
        {
            if (ParticleSystem != null) ParticleSystem.gameObject.Destroy();
        }

        internal virtual void OnParticleSystemDisconnected() { }

    }
}