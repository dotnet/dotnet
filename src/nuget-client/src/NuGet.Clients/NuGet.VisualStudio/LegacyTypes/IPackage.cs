using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;

namespace NuGet
{
    /// <summary>
    /// Legacy
    /// </summary>
    /// <remarks>Do not use!</remarks>
    public interface IPackage
    {
        /// <summary>
        /// Legacy
        /// </summary>
        bool IsAbsoluteLatestVersion { get; }

        /// <summary>
        /// Legacy
        /// </summary>
        bool IsLatestVersion { get; }

        /// <summary>
        /// Legacy
        /// </summary>
        bool Listed { get; }

        /// <summary>
        /// Legacy
        /// </summary>
        DateTimeOffset? Published { get; }

        /// <summary>
        /// Legacy
        /// </summary>
        IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; }

        /// <summary>
        /// Legacy
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This might be expensive")]
        IEnumerable<IPackageFile> GetFiles();

        /// <summary>
        /// Legacy
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This might be expensive")]
        IEnumerable<FrameworkName> GetSupportedFrameworks();

        /// <summary>
        /// Legacy
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This might be expensive")]
        Stream GetStream();

        /// <summary>
        /// Legacy
        /// </summary>
        void ExtractContents(IFileSystem fileSystem, string extractPath);
    }
}
