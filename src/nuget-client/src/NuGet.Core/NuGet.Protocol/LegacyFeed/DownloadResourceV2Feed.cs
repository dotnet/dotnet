// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Events;

namespace NuGet.Protocol
{
    public class DownloadResourceV2Feed : DownloadResource
    {
        private readonly V2FeedParser _feedParser;
        private readonly string _source;

        [Obsolete("Use constructor with source parameter")]
        public DownloadResourceV2Feed(V2FeedParser feedParser)
            : this(feedParser, source: null)
        {
        }

        public DownloadResourceV2Feed(V2FeedParser feedParser, string source)
        {
            if (feedParser == null)
            {
                throw new ArgumentNullException(nameof(feedParser));
            }

            _feedParser = feedParser;
            _source = source;
        }

        public override async Task<DownloadResourceResult> GetDownloadResourceResultAsync(
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

            if (downloadContext == null)
            {
                throw new ArgumentNullException(nameof(downloadContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                token.ThrowIfCancellationRequested();

                var sourcePackage = identity as SourcePackageDependencyInfo;
                bool isFromUri = sourcePackage?.PackageHash != null
                                && sourcePackage?.DownloadUri != null;

                try
                {
                    if (isFromUri)
                    {
                        // If this is a SourcePackageDependencyInfo object with everything populated
                        // and it is from an online source, use the machine cache and download it using the
                        // given url.
                        return await _feedParser.DownloadFromUrl(
                            sourcePackage,
                            sourcePackage.DownloadUri,
                            downloadContext,
                            globalPackagesFolder,
                            logger,
                            token);
                    }
                    else
                    {
                        using (var sourceCacheContext = new SourceCacheContext())
                        {
                            // Look up the package from the id and version and download it.
                            return await _feedParser.DownloadFromIdentity(
                            identity,
                            downloadContext,
                            globalPackagesFolder,
                            sourceCacheContext,
                            logger,
                            token);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    return new DownloadResourceResult(DownloadResourceResultStatus.Cancelled);
                }
                catch (Exception ex) when (!(ex is FatalProtocolException))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Strings.Log_ErrorDownloading, identity, _feedParser.Source);
                    throw new FatalProtocolException(message, ex);
                }
            }
            finally
            {
                ProtocolDiagnostics.RaiseEvent(new ProtocolDiagnosticResourceEvent(
                    _source,
                    resourceType: nameof(DownloadResource),
                    type: nameof(DownloadResourceV2Feed),
                    method: nameof(GetDownloadResourceResultAsync),
                    stopwatch.Elapsed));
            }
        }
    }
}
