// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Threading;
using NuGet.Frameworks;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    internal class PackageLoadContext
    {
        private readonly Task<PackageCollection> _installedPackagesTask;

        public PackageLoadContext(bool isSolution, INuGetUIContext uiContext)
        {
            IsSolution = isSolution;
            PackageManager = uiContext.PackageManager;
            Projects = (uiContext.Projects ?? Enumerable.Empty<IProjectContextInfo>()).ToArray();
            SolutionManager = uiContext.SolutionManagerService;
            ServiceBroker = uiContext.ServiceBroker;

            _installedPackagesTask = PackageCollection.FromProjectsAsync(
                ServiceBroker,
                Projects,
                CancellationToken.None);
        }

        public NuGetPackageManager PackageManager { get; }

        public IProjectContextInfo[] Projects { get; }

        // Indicates whether the loader is created by solution package manager.
        public bool IsSolution { get; }

        public PackageSearchMetadataCache CachedPackages { get; set; }

        public INuGetSolutionManagerService SolutionManager { get; }

        internal IServiceBroker ServiceBroker { get; }

        public Task<PackageCollection> GetInstalledPackagesAsync() => _installedPackagesTask;

        // Returns the list of frameworks that we need to pass to the server during search
        public async Task<IReadOnlyCollection<string>> GetSupportedFrameworksAsync()
        {
            var frameworks = new HashSet<NuGetFramework>();

            foreach (IProjectContextInfo project in Projects)
            {
                if (project.ProjectStyle == ProjectModel.ProjectStyle.PackageReference)
                {
                    IReadOnlyCollection<NuGetFramework> targetFrameworks = await project.GetTargetFrameworksAsync(ServiceBroker, CancellationToken.None);
                    foreach (NuGetFramework targetFramework in targetFrameworks)
                    {
                        frameworks.Add(targetFramework);
                    }
                }
                else
                {
                    IProjectMetadataContextInfo projectMetadata = await project.GetMetadataAsync(
                        ServiceBroker,
                        CancellationToken.None);
                    NuGetFramework framework = projectMetadata.TargetFramework;

                    if (framework is null)
                    {
                        IReadOnlyCollection<NuGetFramework> supportedFrameworks = projectMetadata.SupportedFrameworks;

                        if (supportedFrameworks != null && supportedFrameworks.Count > 0)
                        {
                            foreach (NuGetFramework supportedFramework in supportedFrameworks)
                            {
                                if (supportedFramework.IsAny)
                                {
                                    return Array.Empty<string>();
                                }

                                frameworks.Add(supportedFramework);
                            }
                        }
                    }
                    else
                    {
                        if (framework.IsAny)
                        {
                            return Array.Empty<string>();
                        }

                        if (framework.IsSpecificFramework)
                        {
                            frameworks.Add(framework);
                        }
                    }
                }
            }

            return frameworks.Select(framework => framework.ToString())
                .ToArray();
        }
    }
}
