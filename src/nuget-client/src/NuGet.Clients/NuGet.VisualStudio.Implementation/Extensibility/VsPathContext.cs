// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    // Implementation of IVsPathContext without support of reference resolving.
    // Used when project is not managed by NuGet or given project doesn't have any packages installed.
    internal class VsPathContext : IVsPathContext2
    {
        private readonly string _userPackageFolder;
        public string UserPackageFolder
        {
            get
            {
                const string eventName = nameof(IVsPathContext) + "." + nameof(UserPackageFolder);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _userPackageFolder;
            }
        }

        private readonly IEnumerable _fallbackPackageFolders;
        public IEnumerable FallbackPackageFolders
        {
            get
            {
                const string eventName = nameof(IVsPathContext) + "." + nameof(FallbackPackageFolders);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _fallbackPackageFolders;
            }
        }

        private readonly string _solutionPackageFolder;
        public string SolutionPackageFolder
        {
            get
            {
                const string eventName = nameof(IVsPathContext2) + "." + nameof(SolutionPackageFolder);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _solutionPackageFolder;
            }
        }

        private INuGetTelemetryProvider _telemetryProvider;

        public VsPathContext(NuGetPathContext pathContext, INuGetTelemetryProvider telemetryProvider, string solutionPackageFolder = null)
        {
            if (pathContext == null)
            {
                throw new ArgumentNullException(nameof(pathContext));
            }

            _telemetryProvider = telemetryProvider ?? throw new ArgumentNullException(nameof(telemetryProvider));

            try
            {
                _userPackageFolder = pathContext.UserPackageFolder;
                _fallbackPackageFolders = pathContext.FallbackPackageFolders;
                _solutionPackageFolder = solutionPackageFolder;
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPathContext).FullName);
                throw;
            }
        }

        public VsPathContext(string userPackageFolder, IEnumerable<string> fallbackPackageFolders, INuGetTelemetryProvider telemetryProvider)
        {
            if (userPackageFolder == null)
            {
                throw new ArgumentNullException(nameof(userPackageFolder));
            }

            if (fallbackPackageFolders == null)
            {
                throw new ArgumentNullException(nameof(fallbackPackageFolders));
            }

            _telemetryProvider = telemetryProvider ?? throw new ArgumentNullException(nameof(telemetryProvider));

            try
            {
                _userPackageFolder = userPackageFolder;
                _fallbackPackageFolders = fallbackPackageFolders.ToList();
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPathContext).FullName);
                throw;
            }
        }

        public bool TryResolvePackageAsset(string packageAssetPath, out string packageDirectoryPath)
        {
            const string eventName = nameof(IVsPathContext) + "." + nameof(IVsPathContext.TryResolvePackageAsset);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            // unable to resolve the reference file path without the index
            packageDirectoryPath = null;
            return false;
        }
    }
}
