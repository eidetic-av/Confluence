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

        // Todo: is there a way to make this backing value private?
        // Need to get the value in NodeEditorGUILayout
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

        [Output] public int TestOut = 30;

    }
}
