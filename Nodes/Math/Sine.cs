namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Sine")]
    public class Sine : RuntimeNode
    {
        [Input] public float Input;
        [Output] public float Output => UnityEngine.Mathf.Sin(Input);
    }
}