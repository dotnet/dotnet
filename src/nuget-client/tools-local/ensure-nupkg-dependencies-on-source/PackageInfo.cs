// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Core;

namespace ensure_nupkg_dependencies_on_source
{
    record PackageInfo(PackageIdentity PackageIdentity, IReadOnlyList<PackageIdentity> Dependencies);
}
