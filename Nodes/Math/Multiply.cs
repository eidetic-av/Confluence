namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Multiply")]
    public class Multiply : RuntimeNode
    {
        [Input] public float InputA;
        [Input] public float InputB;
        [Output] public float Output => InputA * InputB;
    }
}