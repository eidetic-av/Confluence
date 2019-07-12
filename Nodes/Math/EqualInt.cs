namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/EqualInt")]
    public class EqualInt : RuntimeNode
    {
        [Input] public int Input;
        [Input] public int Check = 1;
        [Output] public bool Output => Input == Check ? true : false;
    }
}