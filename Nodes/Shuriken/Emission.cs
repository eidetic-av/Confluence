﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using Eidetic.Utility;
using Eidetic.Unity.Utility;
using static UnityEngine.ParticleSystem;

namespace Eidetic.Confluence.Shuriken
{
    public class Emission : RuntimeNode
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

        [SerializeField] public int maxParticles = 50;
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
                if (System != null && System.isPlaying)
                {
                    var mainModule = System.main;
                    mainModule.maxParticles = value;
                }
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
                if (System != null && System.isPlaying)
                {
                    var emissionModule = System.emission;
                    emissionModule.rateOverTime = new MinMaxCurve(emissionRate);
                }
            }
        }

        [Output] public int TestOut = 30;

    }
}
