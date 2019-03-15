using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using Eidetic.Unity.Utility;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public class Emitter : RuntimeNode
    {
        ParticleSystem System;
        public override void Start()
        {
            if (System == null)
            {
                System = new GameObject("Shuriken Instance")
                    .AddComponent<ParticleSystem>();
                SystemManager.InstantiatedSystems.Add(System);
            }
        }
        public override void Exit()
        {
            SystemManager.InstantiatedSystems.Remove(System);
            System.gameObject.Destroy();
        }

        public override object GetValue(NodePort port)
        {
            switch (port.MemberName)
            {
                case "TestOut":
                    return TestOut;
                default:
                    return null;
            }
        }

        [SerializeField] int maxParticles = 50;
        [Input]
        public int MaxParticles
        {
            get
            {
                return maxParticles;
            }
            set
            {
                if (value <= 0) value = 0;
                var mainModule = System.main;
                mainModule.maxParticles = value;
                maxParticles = value;
            }
        }

        [Output] public int TestOut = 30;

    }
}
