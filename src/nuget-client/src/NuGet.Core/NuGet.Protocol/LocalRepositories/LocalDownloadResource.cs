// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Events;

namespace NuGet.Protocol
{
    public class LocalDownloadResource : DownloadResource
    {
        private readonly FindLocalPackagesResource _localResource;
        private readonly string _source;

        [Obsolete("Use constructor with source parameter")]
        public LocalDownloadResource(FindLocalPackagesResource localResource)
            : this(source: null, localResource)
        {
        }

        public LocalDownloadResource(string source, FindLocalPackagesResource localResource)
        {
            if (localResource == null)
            {
                throw new ArgumentNullException(nameof(localResource));
            }

            _source = source;
            _localResource = localResource;
        }

        public override Task<DownloadResourceResult> GetDownloadResourceResultAsync(
            PackageIdentity identity,
            PackageDownloadContext downloadContext,
            string globalPackagesFolder,
            ILogger logger,
            CancellationToken token)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                // Find the package from the local folder
                LocalPackageInfo packageInfo = null;

                var sourcePackage = identity as SourcePackageDependencyInfo;

                if (sourcePackage?.DownloadUri != null)
                {
                    // Get the package directly if the full path is known
                    packageInfo = _localResource.GetPackage(sourcePackage.DownloadUri, logger, token);
                }
                else
                {
                    // Search for the local package
                    packageInfo = _localResource.GetPackage(identity, logger, token);
                }

                if (packageInfo != null)
                {
                    var stream = File.OpenRead(packageInfo.Path);
                    return Task.FromResult(new DownloadResourceResult(stream, packageInfo.GetReader(), _localResource.Root));
                }
                else
                {
                    return Task.FromResult(new DownloadResourceResult(DownloadResourceResultStatus.NotFound));
                }
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _source,
                    resourceType: nameof(DownloadResource),
                    type: nameof(LocalDownloadResource),
                    method: nameof(GetDownloadResourceResultAsync),
                    stopwatch.Elapsed));
            }
        }
    }
}
