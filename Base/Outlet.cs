namespace Eidetic.Confluence
{
    public class Outlet : RuntimeNode
    {
        public RuntimeGraphHolder Holder;
        [Input] public object Value;
        [Output] public object HolderOutput;
    }
}