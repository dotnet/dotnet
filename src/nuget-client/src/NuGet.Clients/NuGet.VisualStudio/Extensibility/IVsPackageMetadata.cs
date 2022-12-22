// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains information about an installed package.
    /// </summary>
    [ComImport]
    [Guid("8B3C4B38-632E-436C-8934-4669C6118845")]
    public interface IVsPackageMetadata
    {
        /// <summary>
        /// Id of the package.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Version of the package.
        /// </summary>
        /// <remarks>
        /// Do not use this property because it will require referencing NuGet.Core.dll assembly. Use the VersionString
        /// property instead.
        /// </remarks>
        [Obsolete("Do not use this property because it will require referencing NuGet.Core.dll assembly. Use the VersionString property instead.")]
        NuGet.SemanticVersion Version { get; }

        /// <summary>
        /// Title of the package.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Description of the package.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The authors of the package.
        /// </summary>
        IEnumerable<string> Authors { get; }

        /// <summary>
        /// The location where the package is installed on disk.
        /// </summary>
        string InstallPath { get; }

        // IMPORTANT: This property must come LAST, because it was added in 2.5. Having it declared 
        // LAST will avoid breaking components that compiled against earlier versions which doesn't
        // have this property.
        /// <summary>
        /// The version of the package.
        /// </summary>
        /// <remarks>
        /// Use this property instead of the Version property becase with the type string,
        /// it doesn't require referencing NuGet.Core.dll assembly.
        /// </remarks>
        string VersionString { get; }
    }
}
