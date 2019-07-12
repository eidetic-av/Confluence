namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/And")]
    public class And : RuntimeNode
    {
        [Input] public bool InputA;
        [Input] public bool InputB;

        [Output] public bool Output => InputA && InputB;
    }
}