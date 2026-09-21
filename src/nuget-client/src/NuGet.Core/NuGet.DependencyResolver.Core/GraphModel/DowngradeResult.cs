namespace NuGet.DependencyResolver
{
    public class DowngradeResult<TItem>
    {
        public required GraphNode<TItem> DowngradedFrom { get; set; }
        public required GraphNode<TItem> DowngradedTo { get; set; }
    }
}
