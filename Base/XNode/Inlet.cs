namespace Eidetic.Confluence
{
    public class Inlet : RuntimeNode
    {
        public RuntimeGraphHolder Holder;
        [Input] public float HolderInput;
        [Output] public float Value;

        public override object GetValue(XNode.NodePort port)
        {
            return HolderInput;
        }
    }
}