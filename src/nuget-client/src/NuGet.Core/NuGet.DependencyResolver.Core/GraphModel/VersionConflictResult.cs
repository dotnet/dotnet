
namespace NuGet.DependencyResolver
{
    public class VersionConflictResult<TItem>
    {
        public required GraphNode<TItem> Selected { get; set; }
        public required GraphNode<TItem> Conflicting { get; set; }
    }
}
