using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Eidetic.Confluence.Shuriken
{
    /// <summary>
    /// Shuriken system-wide management.
    /// </summary>
    public static class SystemManager
    {
        public static Dictionary<ShurikenNode, ParticleSystem> Systems = new Dictionary<ShurikenNode, ParticleSystem>();
        public static Dictionary<ParticleSystem, List<ShurikenNode>> SystemConnections = new Dictionary<ParticleSystem, List<ShurikenNode>>();
        public static List<ParticleSystem> AllSystems => Systems.Select(kvp => kvp.Value).ToList();
    }
}