// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Core;
using NuGet.VisualStudio.Etw;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    internal class VsPackageMetadata : IVsPackageMetadata
    {
        private readonly PackageIdentity _package;
        private readonly string _title;
        private readonly IEnumerable<string> _authors;
        private readonly string _description;
        private readonly string _installPath;

        public VsPackageMetadata(PackageIdentity package, string installPath)
            :
                this(package, string.Empty, Enumerable.Empty<string>(), string.Empty, installPath)
        {
        }

        public VsPackageMetadata(PackageIdentity package, string title, IEnumerable<string> authors, string description, string installPath)
        {
            _package = package;
            _installPath = installPath ?? string.Empty;
            _title = title ?? package.Id;
            _authors = authors ?? Enumerable.Empty<string>();
            _description = description ?? string.Empty;
        }

        public string Id
        {
            get
            {
                const string eventName = nameof(IVsPackageMetadata) + "." + nameof(Id);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _package.Id;
            }
        }

        public SemanticVersion Version
        {
            get
            {
                const string eventName = nameof(IVsPackageMetadata) + "." + nameof(Version);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return new SemanticVersion(_package.Version.ToNormalizedString());
            }
        }

        public string VersionString
        {
            get
            {
                const string eventName = nameof(IVsPackageMetadata) + "." + nameof(VersionString);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _package.Version.ToString();
            }
        }

        public string Title
        {
            get
            {
                const string eventName = nameof(IVsPackageMetadata) + "." + nameof(Title);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _title;
            }
        }

        public IEnumerable<string> Authors
        {
            get
            {
                const string eventName = nameof(IVsPackageMetadata) + "." + nameof(Authors);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _authors;
            }
        }

        public string Description
        {
            get
            {
                const string eventName = nameof(IVsPackageMetadata) + "." + nameof(Description);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _description;
            }
        }

        public string InstallPath
        {
            get
            {
                const string eventName = nameof(IVsPackageMetadata) + "." + nameof(InstallPath);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _installPath;
            }
        }
    }
}
