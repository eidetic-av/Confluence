using UnityEngine;
namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Random")]
    public class Random : RuntimeNode
    {
        public bool trigger;
        [Input] public bool Trigger
        {
            set
            {
                if (value) Output = UnityEngine.Random.value;
            }
        }
        [Output] public float Output { get; set; }
    }
}