using UnityEngine;
using System.Collections.Generic;

namespace Eidetic.Confluence.Shuriken
{
    public static class SystemManager
    {
        public static List<ParticleSystem> InstantiatedSystems = new List<ParticleSystem>();
        public static int Count => InstantiatedSystems.Count;
    }
}