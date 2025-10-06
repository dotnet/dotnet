// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    public interface IPackageRule
    {
        IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder);
        string MessageFormat { get; }
    }
}
