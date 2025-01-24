// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.Telemetry;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsPackageInstaller))]
    [Export(typeof(IVsPackageInstaller2))]
    public class VsPackageInstaller : IVsPackageInstaller2
    {
        private readonly ISourceRepositoryProvider _sourceRepositoryProvider;
        private readonly ISettings _settings;
        private readonly IVsSolutionManager _solutionManager;
        private readonly IDeleteOnRestartManager _deleteOnRestartManager;
        private readonly INuGetTelemetryProvider _telemetryProvider;
        private readonly IRestoreProgressReporter _restoreProgressReporter;

        private JoinableTaskFactory PumpingJTF { get; set; }

        [ImportingConstructor]
        public VsPackageInstaller(
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISettings settings,
            IVsSolutionManager solutionManager,
            IDeleteOnRestartManager deleteOnRestartManager,
            INuGetTelemetryProvider telemetryProvider,
            IRestoreProgressReporter restoreProgressReporter)
        {
            _sourceRepositoryProvider = sourceRepositoryProvider;
            _settings = settings;
            _solutionManager = solutionManager;
            _deleteOnRestartManager = deleteOnRestartManager;
            _telemetryProvider = telemetryProvider;
            _restoreProgressReporter = restoreProgressReporter;

            PumpingJTF = new PumpingJTF(NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public void InstallLatestPackage(
            string source,
            Project project,
            string packageId,
            bool includePrerelease,
            bool ignoreDependencies)
        {
            const string eventName = nameof(IVsPackageInstaller2) + "." + nameof(InstallLatestPackage);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);
            try
            {
                PumpingJTF.Run(() => InstallPackageAsync(
                    source,
                    project,
                    packageId,
                    version: null,
                    includePrerelease: includePrerelease,
                    ignoreDependencies: ignoreDependencies));
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPackageInstaller).FullName);
                throw;
            }
        }

        public void InstallPackage(string source, Project project, string packageId, Version version, bool ignoreDependencies)
        {
            const string eventName = nameof(IVsPackageInstaller) + "." + nameof(InstallPackage) + ".1";
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName,
                new
                {
                    PackageId = packageId,
                    Version = version?.ToString()
                });
            try
            {
                NuGetVersion semVer = null;

                if (version != null)
                {
                    semVer = new NuGetVersion(version);
                }

                PumpingJTF.Run(() => InstallPackageAsync(
                    source,
                    project,
                    packageId,
                    version: semVer,
                    includePrerelease: false,
                    ignoreDependencies: ignoreDependencies));
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPackageInstaller).FullName);
                throw;
            }
        }

        public void InstallPackage(string source, Project project, string packageId, string version, bool ignoreDependencies)
        {
            const string eventName = nameof(IVsPackageInstaller) + "." + nameof(InstallPackage) + ".2";
            using var __ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName,
                new
                {
                    PackageId = packageId,
                    Version = version
                });
            try
            {
                NuGetVersion semVer = null;

                if (!string.IsNullOrEmpty(version))
                {
                    _ = NuGetVersion.TryParse(version, out semVer);
                }

                PumpingJTF.Run(() => InstallPackageAsync(
                    source,
                    project,
                    packageId,
                    version: semVer,
                    includePrerelease: false,
                    ignoreDependencies: ignoreDependencies));
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPackageInstaller).FullName);
                throw;
            }
        }

        private Task InstallPackageAsync(string source, Project project, string packageId, NuGetVersion version, bool includePrerelease, bool ignoreDependencies)
        {
            IEnumerable<string> sources = null;

            if (!string.IsNullOrEmpty(source) &&
                !StringComparer.OrdinalIgnoreCase.Equals("All", source)) // "All" was supported in V2
            {
                sources = new[] { source };
            }

            var toInstall = new List<PackageIdentity>
            {
                new PackageIdentity(packageId, version)
            };

            var projectContext = new VSAPIProjectContext
            {
                PackageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv2,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    ClientPolicyContext.GetClientPolicy(_settings, NullLogger.Instance),
                    NullLogger.Instance)
            };

            return InstallInternalAsync(project, toInstall, GetSources(sources), projectContext, includePrerelease, ignoreDependencies, CancellationToken.None);
        }

        public void InstallPackage(IPackageRepository repository, Project project, string packageId, string version, bool ignoreDependencies, bool skipAssemblyReferences)
        {
            const string eventName = nameof(IVsPackageInstaller) + "." + nameof(InstallPackage) + ".3";
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            // It would be really difficult for anyone to use this method
            throw new NotSupportedException();
        }

        public void InstallPackagesFromRegistryRepository(string keyName, bool isPreUnzipped, bool skipAssemblyReferences, Project project, IDictionary<string, string> packageVersions)
        {
            const string eventName = nameof(IVsPackageInstaller) + "." + nameof(InstallPackagesFromRegistryRepository) + ".1";
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            InstallPackagesFromRegistryRepositoryImpl(keyName, isPreUnzipped, skipAssemblyReferences, ignoreDependencies: true, project: project, packageVersions: packageVersions);
        }

        public void InstallPackagesFromRegistryRepository(string keyName, bool isPreUnzipped, bool skipAssemblyReferences, bool ignoreDependencies, Project project, IDictionary<string, string> packageVersions)
        {
            const string eventName = nameof(IVsPackageInstaller) + "." + nameof(InstallPackagesFromRegistryRepository) + ".2";
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            InstallPackagesFromRegistryRepositoryImpl(keyName, isPreUnzipped, skipAssemblyReferences, ignoreDependencies, project, packageVersions);
        }

        public void InstallPackagesFromRegistryRepositoryImpl(string keyName, bool isPreUnzipped, bool skipAssemblyReferences, bool ignoreDependencies, Project project, IDictionary<string, string> packageVersions)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(keyName));
            }

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (packageVersions == null
                || !packageVersions.Any())
            {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(packageVersions));
            }

            try
            {
                PumpingJTF.Run(async () =>
                    {
                        // HACK !!! : This is a hack for PCL projects which send isPreUnzipped = true, but their package source 
                        // (located at C:\Program Files (x86)\Microsoft SDKs\NuGetPackages) follows the V3
                        // folder version format.
                        if (isPreUnzipped)
                        {
                            var isProjectJsonProject = await EnvDTEProjectUtility.HasBuildIntegratedConfig(project);
                            isPreUnzipped = isProjectJsonProject ? false : isPreUnzipped;
                        }

                        // create a repository provider with only the registry repository
                        var repoProvider = new PreinstalledRepositoryProvider(ErrorHandler, _sourceRepositoryProvider);
                        repoProvider.AddFromRegistry(keyName, isPreUnzipped);

                        var toInstall = GetIdentitiesFromDict(packageVersions);

                        // Skip assembly references and disable binding redirections should be done together
                        var disableBindingRedirects = skipAssemblyReferences;

                        var projectContext = new VSAPIProjectContext(skipAssemblyReferences, disableBindingRedirects)
                        {
                            PackageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                PackageExtractionBehavior.XmlDocFileSaveMode,
                                ClientPolicyContext.GetClientPolicy(_settings, NullLogger.Instance),
                                NullLogger.Instance)
                        };

                        await InstallInternalAsync(
                            project,
                            toInstall,
                            repoProvider,
                            projectContext,
                            includePrerelease: false,
                            ignoreDependencies: ignoreDependencies,
                            token: CancellationToken.None);
                    });
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPackageInstaller).FullName);
                throw;
            }
        }

        public void InstallPackagesFromVSExtensionRepository(string extensionId, bool isPreUnzipped, bool skipAssemblyReferences, Project project, IDictionary<string, string> packageVersions)
        {
            const string eventName = nameof(IVsPackageInstaller) + "." + nameof(InstallPackagesFromVSExtensionRepository) + ".1";
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            InstallPackagesFromVSExtensionRepositoryImpl(
                extensionId,
                isPreUnzipped,
                skipAssemblyReferences,
                ignoreDependencies: true,
                project: project,
                packageVersions: packageVersions);
        }

        public void InstallPackagesFromVSExtensionRepository(string extensionId, bool isPreUnzipped, bool skipAssemblyReferences, bool ignoreDependencies, Project project, IDictionary<string, string> packageVersions)
        {
            const string eventName = nameof(IVsPackageInstaller) + "." + nameof(InstallPackagesFromVSExtensionRepository) + ".2";
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            InstallPackagesFromVSExtensionRepositoryImpl(
                extensionId,
                isPreUnzipped,
                skipAssemblyReferences,
                ignoreDependencies,
                project,
                packageVersions);
        }

        public void InstallPackagesFromVSExtensionRepositoryImpl(string extensionId, bool isPreUnzipped, bool skipAssemblyReferences, bool ignoreDependencies, Project project, IDictionary<string, string> packageVersions)
        {
            if (string.IsNullOrEmpty(extensionId))
            {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(extensionId));
            }

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (!packageVersions.Any())
            {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(packageVersions));
            }

            try
            {
                PumpingJTF.Run(() =>
                    {
                        var repoProvider = new PreinstalledRepositoryProvider(ErrorHandler, _sourceRepositoryProvider);
                        repoProvider.AddFromExtension(_sourceRepositoryProvider, extensionId);

                        var toInstall = GetIdentitiesFromDict(packageVersions);

                        // Skip assembly references and disable binding redirections should be done together
                        var disableBindingRedirects = skipAssemblyReferences;

                        var projectContext = new VSAPIProjectContext(skipAssemblyReferences, disableBindingRedirects)
                        {
                            PackageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                PackageExtractionBehavior.XmlDocFileSaveMode,
                                ClientPolicyContext.GetClientPolicy(_settings, NullLogger.Instance),
                                NullLogger.Instance)
                        };

                        return InstallInternalAsync(
                            project,
                            toInstall,
                            repoProvider,
                            projectContext,
                            includePrerelease: false,
                            ignoreDependencies: ignoreDependencies,
                            token: CancellationToken.None);
                    });
            }
            catch (Exception exception)
            {
                _telemetryProvider.PostFault(exception, typeof(VsPackageInstaller).FullName);
                throw;
            }
        }

        private static List<PackageIdentity> GetIdentitiesFromDict(IDictionary<string, string> packageVersions)
        {
            var toInstall = new List<PackageIdentity>();

            // create identities
            foreach (var pair in packageVersions)
            {
                // TODO: versions can be null today, should this continue?
                NuGetVersion version = null;

                if (!string.IsNullOrEmpty(pair.Value))
                {
                    _ = NuGetVersion.TryParse(pair.Value, out version);
                }

                toInstall.Add(new PackageIdentity(pair.Key, version));
            }

            return toInstall;
        }

        private Action<string> ErrorHandler => msg =>
        {
            // We don't log anything
        };

        /// <summary>
        /// Creates a repo provider for the given sources. If null is passed all sources will be returned.
        /// </summary>
        private ISourceRepositoryProvider GetSources(IEnumerable<string> sources)
        {
            ISourceRepositoryProvider provider = null;

            // add everything enabled if null
            if (sources == null)
            {
                // Use the default set of sources
                provider = _sourceRepositoryProvider;
            }
            else
            {
                // Create a custom source provider for the VS API install
                var customProvider = new PreinstalledRepositoryProvider(ErrorHandler, _sourceRepositoryProvider);

                // Create sources using the given set of sources
                foreach (var source in sources)
                {
                    customProvider.AddFromSource(GetSource(source));
                }

                provider = customProvider;
            }

            return provider;
        }

        /// <summary>
        /// Convert a source string to a SourceRepository. If one already exists that will be used.
        /// </summary>
        private SourceRepository GetSource(string source)
        {
            var repo = _sourceRepositoryProvider.GetRepositories()
                .FirstOrDefault(e => StringComparer.OrdinalIgnoreCase.Equals(e.PackageSource.Source, source));

            if (repo == null)
            {
                Uri result;
                if (!Uri.TryCreate(source, UriKind.Absolute, out result))
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, VsResources.InvalidSource, source),
                        nameof(source));
                }

                var newSource = new Configuration.PackageSource(source);

                repo = _sourceRepositoryProvider.CreateRepository(newSource);
            }

            return repo;
        }

        /// <summary>
        /// Internal install method. All installs from the VS API and template wizard end up here.
        /// </summary>
        internal async Task InstallInternalAsync(
            Project project,
            List<PackageIdentity> packages,
            ISourceRepositoryProvider repoProvider,
            VSAPIProjectContext projectContext,
            bool includePrerelease,
            bool ignoreDependencies,
            CancellationToken token)
        {
            // Go off the UI thread. This may be called from the UI thread. Only switch to the UI thread where necessary
            // This method installs multiple packages and can likely take more than a few secs
            // So, go off the UI thread explicitly to improve responsiveness
            await TaskScheduler.Default;

            var gatherCache = new GatherCache();
            var sources = repoProvider.GetRepositories().ToList();

            // store expanded node state
            var expandedNodes = await VsHierarchyUtility.GetAllExpandedNodesAsync();

            try
            {
                var depBehavior = ignoreDependencies ? DependencyBehavior.Ignore : DependencyBehavior.Lowest;

                var packageManager = CreatePackageManager(repoProvider);

                // find the project
                var nuGetProject = await _solutionManager.GetOrCreateProjectAsync(project, projectContext);

                var packageManagementFormat = new PackageManagementFormat(_settings);
                // 1 means PackageReference
                var preferPackageReference = packageManagementFormat.SelectedPackageManagementFormat == 1;

                // Check if default package format is set to `PackageReference` and project has no
                // package installed yet then upgrade it to `PackageReference` based project.
                if (preferPackageReference &&
                   (nuGetProject is MSBuildNuGetProject) &&
                   !(await nuGetProject.GetInstalledPackagesAsync(token)).Any() &&
                   await NuGetProjectUpgradeUtility.IsNuGetProjectUpgradeableAsync(nuGetProject, project, needsAPackagesConfig: false))
                {
                    nuGetProject = await _solutionManager.UpgradeProjectToPackageReferenceAsync(nuGetProject);
                }

                // install the package
                foreach (var package in packages)
                {
                    var installedPackageReferences = await nuGetProject.GetInstalledPackagesAsync(token);
                    // Check if the package is already installed
                    if (package.Version != null &&
                        PackageServiceUtilities.IsPackageInList(installedPackageReferences, package.Id, package.Version))
                    {
                        continue;
                    }

                    // Perform the install
                    await InstallInternalCoreAsync(
                        packageManager,
                        gatherCache,
                        nuGetProject,
                        package,
                        sources,
                        projectContext,
                        includePrerelease,
                        ignoreDependencies,
                        token);
                }
            }
            finally
            {
                // collapse nodes
                await VsHierarchyUtility.CollapseAllNodesAsync(expandedNodes);
            }
        }

        /// <summary>
        /// Core install method. All installs from the VS API and template wizard end up here.
        /// This does not check for already installed packages
        /// </summary>
        internal async Task InstallInternalCoreAsync(
            NuGetPackageManager packageManager,
            GatherCache gatherCache,
            NuGetProject nuGetProject,
            PackageIdentity package,
            IEnumerable<SourceRepository> sources,
            VSAPIProjectContext projectContext,
            bool includePrerelease,
            bool ignoreDependencies,
            CancellationToken token)
        {
            await TaskScheduler.Default;

            var depBehavior = ignoreDependencies ? DependencyBehavior.Ignore : DependencyBehavior.Lowest;

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var resolution = new ResolutionContext(
                    depBehavior,
                    includePrerelease,
                    includeUnlisted: false,
                    versionConstraints: VersionConstraints.None,
                    gatherCache: gatherCache,
                    sourceCacheContext: sourceCacheContext);

                // install the package
                if (package.Version == null)
                {
                    await packageManager.InstallPackageAsync(nuGetProject, package.Id, resolution, projectContext, sources, Enumerable.Empty<SourceRepository>(), token);
                }
                else
                {
                    await packageManager.InstallPackageAsync(nuGetProject, package, resolution, projectContext, sources, Enumerable.Empty<SourceRepository>(), token);
                }
            }
        }

        /// <summary>
        /// Create a new NuGetPackageManager with the IVsPackageInstaller settings.
        /// </summary>
        internal NuGetPackageManager CreatePackageManager(ISourceRepositoryProvider repoProvider)
        {
            return new NuGetPackageManager(
                repoProvider,
                _settings,
                _solutionManager,
                _deleteOnRestartManager,
                _restoreProgressReporter);
        }
    }
}
