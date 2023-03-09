// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Test.Utility.ProjectManagement;

namespace Test.Utility
{
    public class NuGetPackageMoniker : INuGetPackageMoniker
    {
        public string Id { get; set; }

        public string Version { get; set; }
    }

    public class TestProjectKProject : INuGetPackageManager
    {
        private readonly List<NuGetPackageMoniker> _installedPackages;

        public TestProjectKProject()
        {
            _installedPackages = new List<NuGetPackageMoniker>();
        }

        public bool CanSupport(string optionName, NuGetOperation operation)
        {
            return true;
        }

        public Task<IReadOnlyCollection<object>> GetInstalledPackagesAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                {
                    IReadOnlyCollection<object> result = _installedPackages.Cast<object>().ToList();
                    return result;
                });
        }

        public Task<IReadOnlyCollection<FrameworkName>> GetSupportedFrameworksAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                {
                    var frameworks = new List<FrameworkName>();
                    IReadOnlyCollection<FrameworkName> result = frameworks;
                    return result;
                });
        }

        public Task InstallPackageAsync(INuGetPackageMoniker package, IReadOnlyDictionary<string, object> options, TextWriter logger, IProgress<INuGetPackageInstallProgress> progress, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                {
                    _installedPackages.Add(new NuGetPackageMoniker
                    {
                        Id = package.Id,
                        Version = package.Version
                    });
                });
        }

        public Task UninstallPackageAsync(INuGetPackageMoniker package, IReadOnlyDictionary<string, object> options, TextWriter logger, IProgress<INuGetPackageInstallProgress> progress, CancellationToken cancellationToken)
        {
            return Task.Run(() => { _installedPackages.RemoveAll(p => p.Id == package.Id && p.Version == package.Version); });
        }
    }
}
