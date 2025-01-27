// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Tests
{
    public class TestFindLocalPackagesResource : FindLocalPackagesResource
    {
        public List<TestLocalPackageInfo> Packages { get; }

        public TestFindLocalPackagesResource(IEnumerable<TestLocalPackageInfo> packages)
        {
            Packages = packages.ToList();
        }

        public override IEnumerable<LocalPackageInfo> FindPackagesById(string id, ILogger logger, CancellationToken token)
        {
            return Packages.Where(p => StringComparer.OrdinalIgnoreCase.Equals(id, p.Identity.Id)).ToList();
        }

        public override LocalPackageInfo GetPackage(PackageIdentity identity, ILogger logger, CancellationToken token)
        {
            return Packages.FirstOrDefault(p => p.Identity.Equals(identity));
        }

        public override LocalPackageInfo GetPackage(Uri path, ILogger logger, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<LocalPackageInfo> GetPackages(ILogger logger, CancellationToken token)
        {
            return Packages;
        }
    }
}
