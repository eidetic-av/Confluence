namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/GreaterThan")]
    public class GreaterThan : RuntimeNode
    {
        [Input] public float Input;
        [Input] public float Threshold = 1f;
        [Output] public bool Output => Input > Threshold ? true : false;
    }
}