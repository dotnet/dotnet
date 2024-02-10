// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Configuration;

namespace NuGet.Commands.Test
{
    /// <summary>
    /// Provider that only returns V3 as a source
    /// </summary>
    public class TestPackageSourceProvider : IPackageSourceProvider
    {
        private IEnumerable<PackageSource> PackageSources { get; set; }

        public TestPackageSourceProvider(IEnumerable<PackageSource> packageSources)
        {
            PackageSources = packageSources;
        }

        public IEnumerable<PackageSource> LoadPackageSources() => PackageSources;

        public IReadOnlyList<PackageSource> LoadAuditSources() => Array.Empty<PackageSource>();

        public event EventHandler PackageSourcesChanged;

        public void SavePackageSources(IEnumerable<PackageSource> sources)
        {
            PackageSources = sources;
            PackageSourcesChanged?.Invoke(this, null);
        }

        public string ActivePackageSourceName => throw new NotImplementedException();

        public string DefaultPushSource => throw new NotImplementedException();

        public void SaveActivePackageSource(PackageSource source) => throw new NotImplementedException();

        public PackageSource GetPackageSource(string name) => throw new NotImplementedException();

        public void RemovePackageSource(string name) => throw new NotImplementedException();

        public PackageSource GetPackageSourceByName(string name) => throw new NotImplementedException();

        public PackageSource GetPackageSourceBySource(string source) => throw new NotImplementedException();

        public void EnablePackageSource(string name) => throw new NotImplementedException();

        public void DisablePackageSource(string name) => throw new NotImplementedException();

        public void UpdatePackageSource(PackageSource source, bool updateCredentials, bool updateEnabled) => throw new NotImplementedException();

        public void AddPackageSource(PackageSource source) => throw new NotImplementedException();

        public bool IsPackageSourceEnabled(string name) => throw new NotImplementedException();

        public void DisablePackageSource(PackageSource source) => throw new NotImplementedException();

        public bool IsPackageSourceEnabled(PackageSource source) => throw new NotImplementedException();
    }
}
