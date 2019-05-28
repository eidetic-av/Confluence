using XNode;
using UnityEngine;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("ParticleSystemController")]
    public class ParticleSystemController : RuntimeNode
    {
        [SerializeField] string Target;

        ParticleSystem ParticleSystem;

        internal override void Awake()
        {
            ParticleSystem = GameObject.Find(Target).GetComponent<ParticleSystem>();
        }

        [SerializeField] float speed = 1;
        [Input]
        public float Speed
        {
            set
            {
                var mainModule = ParticleSystem.main;
                mainModule.simulationSpeed = value;
            }
        }

        [SerializeField] float maxParticles = 1000;
        [Input]
        public float MaxParticles
        {
            set
            {
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
                var noiseModule = ParticleSystem.noise;
                noiseModule.frequency = value;
            }
        }


        [SerializeField] float orbitalVelocityX = 0;
        [Input]
        public float OrbitalVelocityX
        {
            set
            {
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
                var velocityModule = ParticleSystem.velocityOverLifetime;
                velocityModule.enabled = true;
                velocityModule.radial = value;
            }
        }

        [SerializeField] float billboardStretch = 0;
        [Input]
        public float BillboardStretch
        {
            set
            {
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
    }
}