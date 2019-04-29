using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Counter")]
    public class Counter : RuntimeNode
    {
        [Input] public bool Increment;

        [Input] public int Max = 0;

        [Output] public int Position;

        internal override void EarlyUpdate()
        {
            if (Increment) Position++;
            if (Position > Max) Position = 0;
        }
    }
}