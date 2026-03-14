// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Packaging
{
    public interface IPackageContentReader
    {
        /// <summary>
        /// Returns all framework references found in the nuspec.
        /// </summary>
        IEnumerable<FrameworkSpecificGroup> GetFrameworkItems();

        /// <summary>
        /// Returns all items under the build folder.
        /// </summary>
        IEnumerable<FrameworkSpecificGroup> GetBuildItems();

        /// <summary>
        /// Returns all items under the tools folder.
        /// </summary>
        IEnumerable<FrameworkSpecificGroup> GetToolItems();

        /// <summary>
        /// Returns all items found in the content folder.
        /// </summary>
        /// <remarks>
        /// Some legacy behavior has been dropped here due to the mix of content folders and target framework
        /// folders here.
        /// </remarks>
        IEnumerable<FrameworkSpecificGroup> GetContentItems();

        /// <summary>
        /// Returns all lib items without any filtering. Use GetReferenceItems for the filtered list.
        /// </summary>
        IEnumerable<FrameworkSpecificGroup> GetLibItems();

        /// <summary>
        /// Returns lib items + filtering based on the nuspec and other NuGet rules.
        /// </summary>
        IEnumerable<FrameworkSpecificGroup> GetReferenceItems();

        /// <summary>
        /// Returns package dependencies.
        /// </summary>
        IEnumerable<PackageDependencyGroup> GetPackageDependencies();
    }
}
