// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    // Full functional implementation of IVsPathContext providing support for
    // reference resolving via reference lookup trie structure.
    internal class VsIndexedPathContext : IVsPathContext
    {
        private readonly PathLookupTrie<string> _referenceLookupIndex;
        private readonly INuGetTelemetryProvider _telemetryProvider;
        private readonly string _userPackageFolder;
        private readonly IEnumerable _fallbackPackageFolders;

        public string UserPackageFolder
        {
            get
            {
                const string eventName = nameof(IVsPathContext) + "." + nameof(UserPackageFolder);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _userPackageFolder;
            }
        }

        public IEnumerable FallbackPackageFolders
        {
            get
            {
                const string eventName = nameof(IVsPathContext) + "." + nameof(FallbackPackageFolders);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _fallbackPackageFolders;
            }
        }

        public VsIndexedPathContext(
            string userPackageFolder,
            IEnumerable<string> fallbackPackageFolders,
            PathLookupTrie<string> index,
            INuGetTelemetryProvider telemetryProvider)
        {
            if (index == null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            _userPackageFolder = userPackageFolder ?? throw new ArgumentNullException(nameof(userPackageFolder));
            _fallbackPackageFolders = fallbackPackageFolders?.ToList() ?? throw new ArgumentNullException(nameof(fallbackPackageFolders));
            _referenceLookupIndex = index ?? throw new ArgumentNullException(nameof(index));
            _telemetryProvider = telemetryProvider ?? throw new ArgumentNullException(nameof(telemetryProvider));
        }

        public bool TryResolvePackageAsset(string packageAssetPath, out string packageDirectoryPath)
        {
            const string eventName = nameof(IVsPathContext) + "." + nameof(TryResolvePackageAsset);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            try
            {
                packageDirectoryPath = _referenceLookupIndex[packageAssetPath];
                return true;
            }
            catch (KeyNotFoundException)
            {
                packageDirectoryPath = null;
                return false;
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsIndexedPathContext).FullName);
                throw;
            }
        }
    }
}
