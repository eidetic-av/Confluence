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

        [SerializeField] float emissionRate = 50;
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