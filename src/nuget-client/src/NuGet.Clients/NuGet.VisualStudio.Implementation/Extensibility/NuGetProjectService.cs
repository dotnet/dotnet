// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;
using NuGet.PackageManagement.VisualStudio.Exceptions;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.VisualStudio.Contracts;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    public sealed class NuGetProjectService : INuGetProjectService
    {
        private readonly IVsSolutionManager _solutionManager;
        private readonly ISettings _settings;
        private readonly INuGetTelemetryProvider _telemetryProvider;

        public NuGetProjectService(IVsSolutionManager solutionManager, ISettings settings, INuGetTelemetryProvider telemetryProvider)
        {
            _solutionManager = solutionManager ?? throw new ArgumentNullException(nameof(solutionManager));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _telemetryProvider = telemetryProvider ?? throw new ArgumentNullException(nameof(telemetryProvider));
        }

        [System.Diagnostics.Tracing.EventData]
        private struct GetInstalledPackagesAsyncEventData
        {
            [System.Diagnostics.Tracing.EventField]
            public Guid Project { get; set; }
        }

        public async Task<InstalledPackagesResult> GetInstalledPackagesAsync(Guid projectId, CancellationToken cancellationToken)
        {
            const string etwEventName = nameof(INuGetProjectService) + "." + nameof(GetInstalledPackagesAsync);
            var eventData = new GetInstalledPackagesAsyncEventData()
            {
                Project = projectId
            };
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(etwEventName, eventData);

            try
            {
                // Just in case we're on the UI thread, switch to background thread. Very low cost (does not schedule new task) if already on background thread.
                await TaskScheduler.Default;

                NuGetProject project = await _solutionManager.GetNuGetProjectAsync(projectId.ToString());
                if (project == null)
                {
                    return NuGetContractsFactory.CreateInstalledPackagesResult(InstalledPackageResultStatus.ProjectNotReady, packages: null);
                }

                InstalledPackageResultStatus status;
                IReadOnlyCollection<NuGetInstalledPackage> installedPackages;

                switch (project)
                {
                    case BuildIntegratedNuGetProject packageReferenceProject:
                        (status, installedPackages) = await GetInstalledPackagesAsync(packageReferenceProject, cancellationToken);
                        break;

                    case MSBuildNuGetProject packagesConfigProject:
                        (status, installedPackages) = await GetInstalledPackagesAsync(packagesConfigProject, cancellationToken);
                        break;

                    default:
                        (status, installedPackages) = await GetInstalledPackagesAsync(project, cancellationToken);
                        break;
                }

                return NuGetContractsFactory.CreateInstalledPackagesResult(status, installedPackages);
            }
            catch (Exception exception)
            {
                var extraProperties = new Dictionary<string, object>();
                extraProperties["projectId"] = projectId.ToString();
                await _telemetryProvider.PostFaultAsync(exception, typeof(NuGetProjectService).FullName, extraProperties: extraProperties);
                throw;
            }
        }

        private async Task<(InstalledPackageResultStatus, IReadOnlyCollection<NuGetInstalledPackage>)> GetInstalledPackagesAsync(BuildIntegratedNuGetProject project, CancellationToken cancellationToken)
        {
            NuGetInstalledPackage ToNuGetInstalledPackage(PackageReference packageReference, FallbackPackagePathResolver pathResolver, bool directDependency)
            {
                var id = packageReference.PackageIdentity.Id;

                string requestedRange = null;
                if (directDependency)
                {
                    requestedRange =
                        packageReference.AllowedVersions?.OriginalString // When Version is specified
                        ?? packageReference.AllowedVersions?.ToShortString(); // Probably only when Version is not specified in msbuild
                }

                string version =
                    packageReference.PackageIdentity.Version?.ToNormalizedString()
                    ?? string.Empty;

                var installPath =
                    version != null
                    ? pathResolver.GetPackageDirectory(id, version)
                    : null;

                return NuGetContractsFactory.CreateNuGetInstalledPackage(id, requestedRange, version, installPath, directDependency);
            }

            InstalledPackageResultStatus status;
            List<NuGetInstalledPackage> installedPackages;

            (InstalledPackageResultStatus, IReadOnlyCollection<NuGetInstalledPackage>) ErrorResult(InstalledPackageResultStatus status)
            {
                return (status, null);
            }

            var cacheContext = new DependencyGraphCacheContext();
            IReadOnlyList<ProjectModel.PackageSpec> packageSpecs;
            IReadOnlyList<ProjectModel.IAssetsLogMessage> messages;
            try
            {
                (packageSpecs, messages) = await project.GetPackageSpecsAndAdditionalMessagesAsync(cacheContext);
            }
            catch (ProjectNotNominatedException)
            {
                return ErrorResult(InstalledPackageResultStatus.ProjectNotReady);
            }
            catch (InvalidDataException)
            {
                return ErrorResult(InstalledPackageResultStatus.ProjectInvalid);
            }

            if (messages?.Any(m => m.Level == LogLevel.Error) == true)
            {
                // Although we know that the project will fail to restore, we may still know about some direct dependencies, so let's return the packages that we know about.
                status = InstalledPackageResultStatus.ProjectInvalid;
            }
            else
            {
                status = InstalledPackageResultStatus.Successful;
            }

            var packageSpec = packageSpecs.Single(s => s.RestoreMetadata.ProjectStyle == ProjectModel.ProjectStyle.PackageReference || s.RestoreMetadata.ProjectStyle == ProjectModel.ProjectStyle.ProjectJson);
            var packagesPath = VSRestoreSettingsUtilities.GetPackagesPath(_settings, packageSpec);
            FallbackPackagePathResolver pathResolver = new FallbackPackagePathResolver(packagesPath, VSRestoreSettingsUtilities.GetFallbackFolders(_settings, packageSpec));

            IReadOnlyCollection<PackageReference> directPackages;
            IReadOnlyCollection<PackageReference> transitivePackages;

            if (project is IPackageReferenceProject packageReferenceProject)
            {
                var installed = await packageReferenceProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, cancellationToken);
                directPackages = installed.InstalledPackages;
                transitivePackages = installed.TransitivePackages;
            }
            else
            {
                directPackages = (await project.GetInstalledPackagesAsync(cancellationToken)).ToList();
                transitivePackages = Array.Empty<PackageReference>();
            }

            installedPackages = new List<NuGetInstalledPackage>(directPackages.Count + (transitivePackages?.Count ?? 0));

            installedPackages.AddRange(directPackages.Select(p => ToNuGetInstalledPackage(p, pathResolver, directDependency: true)));
            if (transitivePackages != null)
            {
                installedPackages.AddRange(transitivePackages.Select(p => ToNuGetInstalledPackage(p, pathResolver, directDependency: false)));
            }

            return (status, installedPackages);
        }

        private async Task<(InstalledPackageResultStatus, IReadOnlyCollection<NuGetInstalledPackage>)> GetInstalledPackagesAsync(MSBuildNuGetProject project, CancellationToken cancellationToken)
        {
            NuGetInstalledPackage ToNuGetInstalledPackage(Packaging.PackageReference packageReference, FolderNuGetProject packagesFolder)
            {
                var id = packageReference.PackageIdentity.Id;
                string requestedRange = packageReference.PackageIdentity.Version.OriginalVersion;
                string version = requestedRange;
                var installPath = packagesFolder.GetInstalledPath(packageReference.PackageIdentity);
                bool directDependency = true;

                return NuGetContractsFactory.CreateNuGetInstalledPackage(id, requestedRange, version, installPath, directDependency);
            }

            FolderNuGetProject packagesFolder = project.FolderNuGetProject;

            var packageReferences = await project.GetInstalledPackagesAsync(cancellationToken);

            var installedPackages = packageReferences.Select(p => ToNuGetInstalledPackage(p, packagesFolder))
                .ToList();
            var status = InstalledPackageResultStatus.Successful;

            return (status, installedPackages);
        }

        private async Task<(InstalledPackageResultStatus, IReadOnlyCollection<NuGetInstalledPackage>)> GetInstalledPackagesAsync(NuGetProject project, CancellationToken cancellationToken)
        {
            // NuGetProject type that doesn't extend MSBuildNuGetProject or BuildIntegratedNuGetProject?
            // At the time of writing, this codepath is impossible to reach.

            NuGetInstalledPackage ToNuGetInstalledPackage(PackageReference package)
            {
                string id = package.PackageIdentity.Id;
                string version = package.PackageIdentity.Version?.OriginalVersion
                    ?? package.PackageIdentity.Version?.ToNormalizedString()
                    ?? package.AllowedVersions?.MinVersion?.OriginalVersion
                    ?? package.AllowedVersions?.MinVersion?.ToNormalizedString();
                string requestedRange = package.AllowedVersions.OriginalString ?? version;
                string installPath = null;
                bool directDependency = true;

                return NuGetContractsFactory.CreateNuGetInstalledPackage(id, requestedRange, version, installPath, directDependency);
            }

            var status = InstalledPackageResultStatus.Unknown;

            var notImplementedException = new NotImplementedException($"Project type {project.GetType().Name} is not implemented");
            await TelemetryUtility.PostFaultAsync(notImplementedException, nameof(NuGetProjectService));

            IEnumerable<PackageReference> projectPackages = await project.GetInstalledPackagesAsync(cancellationToken);

            List<NuGetInstalledPackage> installedPackages =
                projectPackages.Select(ToNuGetInstalledPackage).ToList();

            return (status, installedPackages);
        }
    }
}
