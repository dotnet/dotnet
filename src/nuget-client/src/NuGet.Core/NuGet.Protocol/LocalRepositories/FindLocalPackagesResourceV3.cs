// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Protocol
{
    /// <summary>
    /// Retrieve packages from a local folder or UNC share that uses the V3 folder structure.
    /// </summary>
    public class FindLocalPackagesResourceV3 : FindLocalPackagesResource
    {
        public FindLocalPackagesResourceV3(string root)
        {
            Root = root;
        }

        public override IEnumerable<LocalPackageInfo> FindPackagesById(string id, ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackagesV3(Root, id, logger, token);
        }

        public override LocalPackageInfo GetPackage(Uri path, ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackage(path, logger);
        }

        public override LocalPackageInfo GetPackage(PackageIdentity identity, ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackageV3(Root, identity, logger);
        }

        public override IEnumerable<LocalPackageInfo> GetPackages(ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackagesV3(Root, logger, token);
        }
    }
}
