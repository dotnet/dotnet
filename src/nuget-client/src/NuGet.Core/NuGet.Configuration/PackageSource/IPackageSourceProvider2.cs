// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Configuration
{
    [Obsolete("https://github.com/NuGet/Home/issues/10098")]
    public interface IPackageSourceProvider2 : IPackageSourceProvider
    {
        /// <summary>
        /// Compares the given list of PackageSources with the current PackageSources in the configuration and adds, removes or updates each source as needed.
        /// </summary>
        /// <param name="sources">PackageSources to be saved</param>
        /// <param name="packageSourceUpdateOptions">Settings to use when updating the sources</param>
        void SavePackageSources(IEnumerable<PackageSource> sources, PackageSourceUpdateOptions packageSourceUpdateOptions);
    }
}
