using XNode;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Map")]
    public class Map : RuntimeNode
    {
        [Input] public float Input;
        
        [Input] public float InputMinimum = 0f;
        [Input] public float InputMaximum = 1f;
        [Input] public float OutputMinimum = 0f;
        [Input] public float OutputMaximum = 1f;

        [Output] public float Output => Input.Map(InputMinimum, InputMaximum, OutputMinimum, OutputMaximum);
        [Output] public int RoundedOutput => Output.RoundToInt();
    }
}