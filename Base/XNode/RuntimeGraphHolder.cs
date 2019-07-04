namespace Eidetic.Confluence
{
    public class RuntimeGraphHolder : RuntimeNode
    {
        // public RuntimeGraph TargetGraph { get; private set; }

        // public List<NodePort> InletPorts = new List<NodePort>();
        // public List<NodePort> OutletPorts = new List<NodePort>();

        // public void InitialiseTargetGraph(RuntimeGraph graph)
        // {
        //     TargetGraph = graph.Copy() as RuntimeGraph;
        //     TargetGraph.nodes.ForEach(n =>
        //     {
        //         if (n.GetType() == typeof(Inlet))
        //         {
        //             var inlet = (n as Inlet);
        //             inlet.Holder = this;
        //             InletPorts.Add(inlet.GetPort("HolderInput"));
        //         }
        //         else if (n.GetType() == typeof(Outlet))
        //         {
        //             var outlet = (n as Outlet);
        //             outlet.Holder = this;
        //             OutletPorts.Add(outlet.GetPort("HolderOutput"));
        //         }
        //     });
        // }
    }
}