// All node logic for the ObjectSelector is based in the Editor script
public class Outlet : RuntimeNode 
{
    public RuntimeGraphHolder Holder;
    [Input] public object Value;
    [Output] public object HolderOutput;
}