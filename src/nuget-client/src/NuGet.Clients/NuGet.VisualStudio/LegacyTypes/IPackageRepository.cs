using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NuGet
{
    /// <summary>
    /// Legacy IPackageRepository
    /// </summary>
    /// <remarks>Do not use!</remarks>
    public interface IPackageRepository
    {
        /// <summary>
        /// Legacy
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Legacy
        /// </summary>
        PackageSaveModes PackageSaveMode { get; set; }

        /// <summary>
        /// Legacy
        /// </summary>
        bool SupportsPrereleasePackages { get; }

        /// <summary>
        /// Legacy
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This call might be expensive")]
        IQueryable<IPackage> GetPackages();

        /// <summary>
        /// Legacy
        /// </summary>
        void AddPackage(IPackage package);

        /// <summary>
        /// Legacy
        /// </summary>
        void RemovePackage(IPackage package);
    }
}
