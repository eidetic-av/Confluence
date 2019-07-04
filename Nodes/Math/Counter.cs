using UnityEngine;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Counter")]
    public class Counter : RuntimeNode
    {
        [Input] public bool Increment = false;

        [Input] public int Max = 0;

        [Output] public int Position = 0;

        float updateInterval = 0.075f;
        float lastUpdateTime = 0f;

        internal override void Awake()
        {
            Increment = false;
            Position = 0;
            lastUpdateTime = Time.time;
        }

        internal override void Update()
        {
            if (Increment)
            {
                if (Time.time > lastUpdateTime + updateInterval)
                {
                    Position++;
                    Increment = false;
                    lastUpdateTime = Time.time;
                }
            }
            if (Position > Max) Position = 0;
        }
    }
}