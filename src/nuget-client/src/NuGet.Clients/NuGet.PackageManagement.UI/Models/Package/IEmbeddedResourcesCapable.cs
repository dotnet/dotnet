// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal interface IEmbeddedResourcesCapable
    {
        Uri? ReadmeUri { get; }

        ValueTask<Stream?> GetIconAsync(CancellationToken cancellationToken);

        ValueTask<Stream?> GetLicenseAsync(CancellationToken cancellationToken);

        ValueTask<Stream?> GetReadmeAsync(CancellationToken cancellationToken);
    }
}
