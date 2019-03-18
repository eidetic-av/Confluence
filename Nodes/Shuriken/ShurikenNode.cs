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
        public static List<ShurikenNode> Nodes { get; private set; } = new List<ShurikenNode>();
        public static List<ParticleSystem> Systems { get; private set; } = new List<ParticleSystem>();
        public static Dictionary<ShurikenNode, ParticleSystem> SystemFromNode { get; private set; } = new Dictionary<ShurikenNode, ParticleSystem>();
        public static Dictionary<ParticleSystem, List<ShurikenNode>> NodesFromSystem { get; private set; } = new Dictionary<ParticleSystem, List<ShurikenNode>>();

        public ParticleSystem System { get; private set; }
        public List<ShurikenNode> ConnectedShurikenNodes { get; private set; }

        internal override void Awake()
        {
            base.Awake();
            Nodes.Add(this);
            UpdateCollections();
        }
        internal override void Destroy()
        {
            base.Destroy();
            Nodes.Remove(this);
        }

        private void UpdateCollections()
        {
            ConnectedShurikenNodes = Ports
                .Where(port => port.IsConnected)
                .Select(port => port.Connection.Node)
                .OfType<ShurikenNode>().ToList();

            if (System == null)
            {
                // If it's an Emission node, instantiate a new system.
                if (GetType() == typeof(Emission))
                    System = (Instantiate(Resources.Load("Shuriken Instance")) as GameObject)
                    .GetComponent<ParticleSystem>();
                // Otherwise set the system to the Emission node it's connected to
                else System = ConnectedShurikenNodes
                    .OfType<Emission>()
                    .SingleOrDefault() ?
                    .System;
            }

            if (System != null)
            {
                SystemFromNode[this] = System;

                if (NodesFromSystem.ContainsKey(System))
                    NodesFromSystem[System].Add(this);
                else
                    NodesFromSystem[System] = new List<ShurikenNode>().With(this);
            }
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            UpdateCollections();
        }
        public override void OnRemoveConnection(NodePort removedPort)
        {
            base.OnRemoveConnection(removedPort);
            UpdateCollections();
        }

        // This is the internal array used within all nodes connected to the system.
        private Particle[] particles = null;

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
                    particles = new Particle[System.particleCount];
                    System.GetParticles(particles, System.particleCount);
                }
                return particles;
            }
        }
    }
}