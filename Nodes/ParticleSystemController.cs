using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("ParticleSystemController")]
    public class ParticleSystemController : RuntimeNode
    {
        [SerializeField] string Target;

        ParticleSystem ParticleSystem;

        internal override void Awake()
        {
            if (GameObject.Find(Target) != null)
                ParticleSystem = GameObject.Find(Target).GetComponent<ParticleSystem>();
        }

        [SerializeField] float lifetime = 1;
        [Input]
        public float Lifetime
        {
            set
            {
                if (ParticleSystem == null) return;
                var mainModule = ParticleSystem.main;
                mainModule.startLifetimeMultiplier = value;
            }
        }

        [SerializeField] float speed = 1;
        [Input]
        public float Speed
        {
            set
            {
                if (ParticleSystem == null) return;
                var mainModule = ParticleSystem.main;
                mainModule.simulationSpeed = value;
            }
        }
        
        [SerializeField] float emissionRate = 1;
        [Input]
        public float EmissionRate
        {
            set
            {
                if (ParticleSystem == null) return;
                var emissionModule = ParticleSystem.emission;
                emissionModule.rateOverTimeMultiplier = value;
            }
        }

        [SerializeField] float gravity = 1;
        [Input]
        public float Gravity
        {
            set
            {
                if (ParticleSystem == null) return;
                var mainModule = ParticleSystem.main;
                mainModule.gravityModifier = value;
            }
        }

        [SerializeField] float startSize = 1;
        [Input]
        public float StartSize
        {
            set
            {
                if (ParticleSystem == null) return;
                if (startSize > 0)
                {
                    var mainModule = ParticleSystem.main;
                    mainModule.startSizeMultiplier = value;
                }
            }
        }

        [SerializeField] float maxParticles = 1000;
        [Input]
        public float MaxParticles
        {
            set
            {
                if (ParticleSystem == null) return;
                var mainModule = ParticleSystem.main;
                mainModule.maxParticles = Mathf.RoundToInt(value);
            }
        }


        [SerializeField] float noiseIntensity = 0;
        [Input]
        public float NoiseIntensity
        {
            set
            {
                if (ParticleSystem == null) return;
                var noiseModule = ParticleSystem.noise;
                if (value == 0)
                    noiseModule.enabled = false;
                else
                {
                    noiseModule.strength = new ParticleSystem.MinMaxCurve(value);
                    noiseModule.enabled = true;
                }
            }
        }


        [SerializeField] float noiseFrequency = 1;
        [Input]
        public float NoiseFrequency
        {
            set
            {
                if (ParticleSystem == null) return;
                var noiseModule = ParticleSystem.noise;
                noiseModule.frequency = value;
            }
        }


        [SerializeField] float noiseScrollSpeed = 0;
        [Input]
        public float NoiseScrollSpeed
        {
            set
            {
                if (ParticleSystem == null) return;
                var noiseModule = ParticleSystem.noise;
                noiseModule.scrollSpeed = value;
            }
        }


        [SerializeField] float orbitalVelocityX = 0;
        [Input]
        public float OrbitalVelocityX
        {
            set
            {
                if (ParticleSystem == null) return;
                var velocityModule = ParticleSystem.velocityOverLifetime;
                velocityModule.enabled = true;
                velocityModule.orbitalX = value;
            }
        }


        [SerializeField] float orbitalVelocityY = 0;
        [Input]
        public float OrbitalVelocityY
        {
            set
            {
                if (ParticleSystem == null) return;
                var velocityModule = ParticleSystem.velocityOverLifetime;
                velocityModule.enabled = true;
                velocityModule.orbitalY = value;
            }
        }


        [SerializeField] float radialVelocity = 0;
        [Input]
        public float RadialVelocity
        {
            set
            {
                if (ParticleSystem == null) return;
                var velocityModule = ParticleSystem.velocityOverLifetime;
                velocityModule.enabled = true;
                velocityModule.radial = value;
            }
        }


        [SerializeField] float externalForces = 0;
        [Input]
        public float ExternalForces
        {
            set
            {
                if (ParticleSystem == null) return;
                var externalForcesModule = ParticleSystem.externalForces;
                externalForcesModule.enabled = true;
                externalForcesModule.multiplier = value;
            }
        }

        [SerializeField] float billboardStretch = 0;
        [Input]
        public float BillboardStretch
        {
            set
            {
                if (ParticleSystem == null) return;
                var rendererModule = ParticleSystem.GetComponent<ParticleSystemRenderer>();
                rendererModule.lengthScale = value;
                rendererModule.cameraVelocityScale = value;
                rendererModule.velocityScale = value;
            }
        }

        [SerializeField] float tintHue = -1;
        [Input]
        public float TintHue
        {
            set
            {
                if (ParticleSystem == null) return;
                if (value < 0) return;
                var colorBySpeedModule = ParticleSystem.colorBySpeed;
                Color startingColor = colorBySpeedModule.color.gradient.colorKeys[0].color;

                float h, s, v;
                Color.RGBToHSV(startingColor, out h, out s, out v);

                h = value;

                colorBySpeedModule.range = new Vector2(1, 1);

                var gradient = new Gradient();
                gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.HSVToRGB(h, s, v), 0) };

                var color = colorBySpeedModule.color;
                color.gradient = gradient;

                colorBySpeedModule.color = color;
            }
        }

        [SerializeField] float tintSaturation = -1;
        [Input]
        public float TintSaturation
        {
            set
            {
                if (ParticleSystem == null) return;
                if (value < 0) return;
                var colorBySpeedModule = ParticleSystem.colorBySpeed;
                Color startingColor = colorBySpeedModule.color.gradient.colorKeys[0].color;

                float h, s, v;
                Color.RGBToHSV(startingColor, out h, out s, out v);

                s = value;

                colorBySpeedModule.range = new Vector2(1, 1);

                var gradient = new Gradient();
                gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.HSVToRGB(h, s, v), 0) };

                var color = colorBySpeedModule.color;
                color.gradient = gradient;

                colorBySpeedModule.color = color;
            }
        }

        [SerializeField] float tintValue = -1;
        [Input]
        public float TintValue
        {
            set
            {
                if (ParticleSystem == null) return;
                if (value < 0) return;
                var colorBySpeedModule = ParticleSystem.colorBySpeed;
                Color startingColor = colorBySpeedModule.color.gradient.colorKeys[0].color;

                float h, s, v;
                Color.RGBToHSV(startingColor, out h, out s, out v);

                v = value;

                colorBySpeedModule.range = new Vector2(1, 1);

                var gradient = new Gradient();
                gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.HSVToRGB(h, s, v), 0) };

                var color = colorBySpeedModule.color;
                color.gradient = gradient;

                colorBySpeedModule.color = color;
            }
        }


        [SerializeField] bool enableTrail = false;
        [Input]
        public bool EnableTrail
        {
            set
            {
                if (ParticleSystem == null) return;
                var trailsModule = ParticleSystem.trails;
                trailsModule.enabled = value;
            }
        }
    }
}