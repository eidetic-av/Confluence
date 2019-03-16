using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using Eidetic.Utility;
using Eidetic.Unity.Utility;
using static UnityEngine.ParticleSystem;
using static Eidetic.Confluence.Shuriken.SystemManager;

namespace Eidetic.Confluence.Shuriken
{
    public class Emission : ShurikenNode
    {
        
        internal override void Awake()
        {
            if (System != null) return;
            System = (Instantiate(Resources.Load("Shuriken Instance")) as GameObject)
                .GetComponent<ParticleSystem>();
            Systems.Add(this, System);
        }
        internal override void Exit()
        {
            if (System == null) return;
            System.gameObject.Destroy();
            Systems.Remove(this);
        }

        [SerializeField] public int maxParticles = 250;
        [Input] public int MaxParticles
        {
            get
            {
                return maxParticles;
            }
            set
            {
                if (value <= 0) value = 0;
                maxParticles = value;
                var mainModule = System.main;
                mainModule.maxParticles = value;
            }
        }

        [SerializeField] public float emissionRate = 50;
        [Input] public float EmissionRate
        {
            get
            {
                return emissionRate;
            }
            set
            {
                emissionRate = value.Clamp(0, 100000);
                var emissionModule = System.emission;
                emissionModule.rateOverTime = new MinMaxCurve(emissionRate);
            }
        }

    }
}
