using System.Collections;
using System.Collections.Generic;
using Eidetic.Unity.Utility;
using Eidetic.Utility;
using UnityEditor;
using UnityEngine;
using XNode;
using static UnityEngine.ParticleSystem;
namespace Eidetic.Confluence.Shuriken
{
    public class Emission : ShurikenNode
    {
        [SerializeField] int maxParticles = 250;
        [Input] public int MaxParticles
        {
            get => maxParticles;
            set
            {
                if (value <= 0) value = 0;
                maxParticles = value;
                var mainModule = ParticleSystem.main;
                mainModule.maxParticles = value;
            }
        }

        [SerializeField] int emissionRate = 50;
        [Input] public int EmissionRate
        {
            get => emissionRate;
            set
            {
                emissionRate = value.Clamp(0, 1000);
                var emissionModule = ParticleSystem.emission;
                emissionModule.rateOverTime = new MinMaxCurve(emissionRate);
            }
        }

        [SerializeField] int manualEmissionCount = 50;
        [Input] public int ManualEmissionCount
        {
            get => manualEmissionCount;
            set => manualEmissionCount = value.Clamp(0, 1000);
        }

        [SerializeField] bool emit = false;
        [Input] public bool Emit
        {
            set
            {
                if (value)
                {
                    ParticleSystem.Emit(ManualEmissionCount);
                    emit = false;
                }
            }
        }

        [SerializeField] bool clear = false;
        [Input] public bool Clear
        {
            set
            {
                if (value)
                {
                    ParticleSystem.Clear();
                    clear = false;
                }
            }
        }
    }
}