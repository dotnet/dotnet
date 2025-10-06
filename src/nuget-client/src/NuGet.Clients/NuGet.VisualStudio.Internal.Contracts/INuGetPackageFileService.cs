// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface INuGetPackageFileService : IDisposable
    {
        ValueTask<Stream?> GetPackageIconAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken);
        ValueTask<Stream?> GetEmbeddedLicenseAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken);
        ValueTask<Stream?> GetReadmeAsync(Uri readmeUri, CancellationToken cancellationToken);
    }
}
