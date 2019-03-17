using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Eidetic.Unity.Utility;
using UnityEngine;
using XNode;
using static System.Collections.Specialized.NotifyCollectionChangedAction;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public abstract class ShurikenNode : RuntimeNode
    {
        public static ObservableCollection<ShurikenNode> Nodes { get; private set; } = new ObservableCollection<ShurikenNode>();
        public static List<ParticleSystem> Systems { get; private set; } = new List<ParticleSystem>();
        public static Dictionary<ShurikenNode, ParticleSystem> SystemFromNode { get; private set; } = new Dictionary<ShurikenNode, ParticleSystem>();
        public static Dictionary<ParticleSystem, List<ShurikenNode>> NodesFromSystem { get; private set; } = new Dictionary<ParticleSystem, List<ShurikenNode>>();

        public ParticleSystem System { get; private set; }
        public List<ShurikenNode> ConnectedNodes { get; private set; }

        static ShurikenNode()
        {
            Nodes.CollectionChanged += UpdateCollections;
        }
        static void UpdateCollections(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (eventArgs.Action == Add)
            {
                var newNodes = eventArgs.NewItems as List<ShurikenNode>;
                List<Emission> newEmissionNodes = new List<Emission>();
                foreach (var node in newNodes)
                {
                    ParticleSystem system = null;
                    if (node.GetType() == typeof(Emission))
                    {
                        var shurikenInstance = Resources.Load("Shuriken Instance") as GameObject;
                        system = shurikenInstance.GetComponent<ParticleSystem>();
                    }
                    else
                    {

                    }
                }

                // update systems
                // update system from node
                // update nodes from system
                // update connected nodes
            }
            else if (eventArgs.Action == Remove)
            {

            }
        }

        internal override void Start() => Nodes.Add(this);
        internal override void Destroy() => Nodes.Remove(this);

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