using UnityEngine;
using Eidetic.Unity.Utility;

using static Eidetic.Confluence.Shuriken.SystemManager;

namespace Eidetic.Confluence.Shuriken
{
    public abstract class ShurikenNode : RuntimeNode
    {
        public ParticleSystem System {get; protected set;}
    }
}