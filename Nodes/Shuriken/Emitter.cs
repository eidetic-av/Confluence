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
        [Input] public float Testfloat = 0f;

        ParticleSystem System;
        public override void Start()
        {
            System = new GameObject("Shuriken Instance")
                .AddComponent<ParticleSystem>();
            SystemManager.InstantiatedSystems.Add(System);
        }
        public override void Exit()
        {
            SystemManager.InstantiatedSystems.Remove(System);
            System.gameObject.Destroy();
        }

        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case "Particles":
                    return null;
                default:
                    return null;
            }
        }

        int maxParticles = 50;
        public int MaxParticles
        {
            get
            {
                return maxParticles;
            }
            set
            {
                var mainModule = System.main;
                mainModule.maxParticles = value;
                maxParticles = value;
            }
        }

    }
}
