using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Eidetic.Confluence
{
    public class PointCloudController : RuntimeNode
    {
        [SerializeField] public int TargetSystem = 0;

        [SerializeField] bool emit;
        [Input]
        public bool Emit
        {
            set
            {
                if (Receiver == null) return;
                var system = Receiver.ParticleSystems[TargetSystem];
                if (system == null) return;
                if (value) system.Emit = true;
            }
        }

        [SerializeField] bool clearOnEmit;
        [Input]
        public bool ClearOnEmit
        {
            set
            {
                clearOnEmit = value;
                if (Receiver == null) return;
                var system = Receiver.ParticleSystems[TargetSystem];
                if (system == null) return;
                system.ClearOnEmit = clearOnEmit;

            }
        }

        [SerializeField] int emissionSkip = 1;
        [Input]
        public int EmissionSkip
        {
            set
            {
                emissionSkip = value;
                if (Receiver == null) return;
                var system = Receiver.ParticleSystems[TargetSystem];
                if (system == null) return;
                system.ManualEmissionSkip = emissionSkip;

            }
        }

        [SerializeField] bool constantEmission;
        [Input]
        public bool ConstantEmission
        {
            set
            {
                constantEmission = value;
                if (Receiver == null) return;
                var system = Receiver.ParticleSystems[TargetSystem];
                if (system == null) return;
                system.ConstantEmission = constantEmission;

            }
        }

        [SerializeField] int constantEmissionRounds = 1;
        [Input]
        public int ConstantEmissionRounds
        {
            set
            {
                constantEmissionRounds = value;
                if (Receiver == null) return;
                var system = Receiver.ParticleSystems[TargetSystem];
                if (system == null) return;
                system.EmissionRounds = constantEmissionRounds;
                    
            }
        }

        [SerializeField] float constantEmissionInterval = 0.05f;
        [Input]
        public float ConstantEmissionInterval
        {
            set
            {
                constantEmissionInterval = value;
                if (Receiver == null) return;
                var system = Receiver.ParticleSystems[TargetSystem];
                if (system == null) return;
                system.EmissionInterval = constantEmissionInterval;

            }
        }

        PointCloudReceiver Receiver;
        internal override void Start()
        {
            base.Start();
            Receiver = GameObject.Find("LiveScan").GetComponent<PointCloudReceiver>();
        }
    }
}
