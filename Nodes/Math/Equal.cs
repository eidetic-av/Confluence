namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Equal")]
    public class Equal : RuntimeNode
    {
        [Input] public float Input;
        [Input] public float Check = 1f;
        [Output] public bool Output => Input == Check ? true : false;
    }
}