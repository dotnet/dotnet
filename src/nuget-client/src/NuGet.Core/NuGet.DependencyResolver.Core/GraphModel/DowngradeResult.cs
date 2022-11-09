namespace NuGet.DependencyResolver
{
    public class DowngradeResult<TItem>
    {
        public GraphNode<TItem> DowngradedFrom { get; set; }
        public GraphNode<TItem> DowngradedTo { get; set; }
    }
}
