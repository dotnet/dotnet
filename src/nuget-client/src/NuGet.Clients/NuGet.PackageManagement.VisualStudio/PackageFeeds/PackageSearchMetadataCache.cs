// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public class PackageSearchMetadataCache
    {
        // Cached Package Metadata
        public IReadOnlyCollection<PackageSearchMetadataContextInfo> Packages { get; set; }

        // Remember the IncludePrerelease setting corresponding to the Cached Metadata
        public bool IncludePrerelease { get; set; }
    }
}
