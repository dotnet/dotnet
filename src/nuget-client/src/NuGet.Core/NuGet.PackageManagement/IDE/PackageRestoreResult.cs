// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement
{
    public class PackageRestoreResult
    {
        public PackageRestoreResult(bool restored, IEnumerable<PackageIdentity> restoredPackages)
        {
            Restored = restored;
            RestoredPackages = restoredPackages;
        }

        public bool Restored { get; }
        public IEnumerable<PackageIdentity> RestoredPackages { get; }
    }
}
