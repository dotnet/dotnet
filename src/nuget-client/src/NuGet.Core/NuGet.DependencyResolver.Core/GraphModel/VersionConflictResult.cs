
namespace NuGet.DependencyResolver
{
    public class VersionConflictResult<TItem>
    {
        public GraphNode<TItem> Selected { get; set; }
        public GraphNode<TItem> Conflicting { get; set; }
    }
}
