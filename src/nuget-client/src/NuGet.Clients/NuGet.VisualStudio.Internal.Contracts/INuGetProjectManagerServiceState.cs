// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface INuGetProjectManagerServiceState : IDisposable
    {
        AsyncSemaphore AsyncSemaphore { get; }
        PackageIdentity? PackageIdentity { get; set; }
        Dictionary<string, ResolvedAction> ResolvedActions { get; }
        SourceCacheContext? SourceCacheContext { get; set; }

        void Reset();
    }
}
