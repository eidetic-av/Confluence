namespace Eidetic.Confluence
{
    [CreateNodeMenu("Math/Increment")]
    public class Increment : RuntimeNode
    {
        public float input;
        [Input] public float Input
        {
            set => Output += value;
        }
        public bool reset;
        [Input] public bool Reset
        {
            set
            {
                if (value) Output = 0;
            }
        }
        [Output] public float Output { get; set; }
    }
}