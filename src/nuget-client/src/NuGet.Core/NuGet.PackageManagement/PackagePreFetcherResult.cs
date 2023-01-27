// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public class PackagePreFetcherResult : IDisposable
    {
        private readonly Task<DownloadResourceResult> _downloadTask;
        private readonly string _nupkgPath;
        private DownloadResourceResult _result;
        private ExceptionDispatchInfo _exception;
        private readonly DateTimeOffset _downloadStartTime;
        private DateTimeOffset _packageFetchTime;
        private DateTimeOffset _taskReturnTime;
        private bool _disposed = false;

        /// <summary>
        /// True if the result came from the packages folder.
        /// </summary>
        /// <remarks>Not thread safe.</remarks>
        public bool InPackagesFolder { get; }

        /// <summary>
        /// Package identity.
        /// </summary>
        public PackageIdentity Package { get; }

        /// <summary>
        /// PackageSource for the download. This is null if the packages folder was used.
        /// </summary>
        public Configuration.PackageSource Source { get; }

        /// <summary>
        /// True if the download is complete.
        /// </summary>
        public bool IsComplete { get; private set; }

        public const string PackagePreFetcherInformation = nameof(PackagePreFetcherInformation);

        /// <summary>
        /// Create a PreFetcher result for a downloaded package.
        /// </summary>
        public PackagePreFetcherResult(
            Task<DownloadResourceResult> downloadTask,
            PackageIdentity package,
            Configuration.PackageSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (downloadTask == null)
            {
                throw new ArgumentNullException(nameof(downloadTask));
            }

            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            _downloadTask = downloadTask;
            Package = package;
            InPackagesFolder = false;
            Source = source;
            _downloadStartTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// Create a PreFetcher result for a package in the packages folder.
        /// </summary>
        public PackagePreFetcherResult(
            string nupkgPath,
            PackageIdentity package)
        {
            if (nupkgPath == null)
            {
                throw new ArgumentNullException(nameof(nupkgPath));
            }

            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            InPackagesFolder = true;
            IsComplete = true;
            Package = package;
            _nupkgPath = nupkgPath;
        }

        /// <summary>
        /// A safe wait for the download task. Exceptions are caught and stored.
        /// </summary>
        public async Task EnsureResultAsync()
        {
            // This is a noop if this has been called before, or if the result is in the packages folder.
            if (!InPackagesFolder && _result == null)
            {
                _packageFetchTime = DateTimeOffset.Now;
                try
                {
                    _result = await _downloadTask;
                }
                catch (Exception ex)
                {
                    _exception = ExceptionDispatchInfo.Capture(ex);
                }

                _taskReturnTime = DateTimeOffset.Now;

                IsComplete = true;
            }
        }

        /// <summary>
        /// Ensure and retrieve the download result.
        /// </summary>
        public async Task<DownloadResourceResult> GetResultAsync()
        {
            DownloadResourceResult result = null;

            if (InPackagesFolder)
            {
                // Results from the packages folder are created on demand
                result = GetPackagesFolderResult(_nupkgPath);
            }
            else
            {
                // Wait for the download to finish
                await EnsureResultAsync();

                if (_exception != null)
                {
                    // Rethrow the exception if the download failed
                    _exception.Throw();
                }

                // Use the downloadTask result
                result = _result;
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // The task should be awaited before calling dispose
                if (_result != null)
                {
                    _result.Dispose();
                }
            }

            _disposed = true;
        }

        public void EmitTelemetryEvent(Guid parentId)
        {
            var telemetryEvent = new TelemetryEvent(PackagePreFetcherInformation);

            telemetryEvent["DownloadStartTime"] = _downloadStartTime;
            telemetryEvent["PackageFetchTime"] = _packageFetchTime;
            telemetryEvent["TaskReturnTime"] = _taskReturnTime;

            telemetryEvent.AddPiiData("PackageId", Package.ToString());

            if (parentId != Guid.Empty)
            {
                telemetryEvent["ParentId"] = parentId.ToString();
            }

            TelemetryActivity.EmitTelemetryEvent(telemetryEvent);
        }

        private DownloadResourceResult GetPackagesFolderResult(string nupkgPath)
        {
            // Create a download result for the package that already exists
            return new DownloadResourceResult(
                File.OpenRead(nupkgPath),
                new PackageArchiveReader(nupkgPath),
                Source?.Source)
            { SignatureVerified = true };
        }
    }
}
