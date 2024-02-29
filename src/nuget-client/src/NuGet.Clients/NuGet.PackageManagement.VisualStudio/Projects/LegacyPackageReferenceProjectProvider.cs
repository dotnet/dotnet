// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.VisualStudio;
using VSLangProj150;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;


namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(INuGetProjectProvider))]
    [Name(nameof(LegacyPackageReferenceProjectProvider))]
    [Order(After = nameof(CpsPackageReferenceProjectProvider))]
    public sealed class LegacyPackageReferenceProjectProvider : INuGetProjectProvider
    {
        private static readonly string PackageReference = ProjectStyle.PackageReference.ToString();

        private readonly IVsProjectThreadingService _threadingService;
        private readonly Lazy<IScriptExecutor> _scriptExecutor;

        public RuntimeTypeHandle ProjectType => typeof(LegacyPackageReferenceProject).TypeHandle;

        [ImportingConstructor]
        public LegacyPackageReferenceProjectProvider(
            IVsProjectThreadingService threadingService,
            Lazy<IScriptExecutor> scriptExecutor)
            : this(AsyncServiceProvider.GlobalProvider,
                   threadingService,
                   scriptExecutor)
        { }

        public LegacyPackageReferenceProjectProvider(
            IAsyncServiceProvider vsServiceProvider,
            IVsProjectThreadingService threadingService,
            Lazy<IScriptExecutor> scriptExecutor)
        {
            Assumes.Present(vsServiceProvider);
            Assumes.Present(threadingService);
            Assumes.Present(scriptExecutor);

            _threadingService = threadingService;
            _scriptExecutor = scriptExecutor;
        }

        public async Task<NuGetProject> TryCreateNuGetProjectAsync(
            IVsProjectAdapter vsProjectAdapter,
            ProjectProviderContext context,
            bool forceProjectType)
        {
            Assumes.Present(vsProjectAdapter);

            var projectServices = await TryCreateProjectServicesAsync(
                vsProjectAdapter,
                forceCreate: forceProjectType);

            if (projectServices == null)
            {
                return null;
            }

            NuGetFramework targetFramework = await vsProjectAdapter.GetTargetFrameworkAsync();

            return new LegacyPackageReferenceProject(
                vsProjectAdapter,
                vsProjectAdapter.ProjectId,
                projectServices,
                _threadingService,
                targetFramework);
        }

        /// <summary>
        /// Is this project a non-CPS package reference based csproj?
        /// </summary>
        private async Task<INuGetProjectServices> TryCreateProjectServicesAsync(
            IVsProjectAdapter vsProjectAdapter, bool forceCreate)
        {
            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsProject4 = vsProjectAdapter.Project.Object as VSProject4;

            // A legacy CSProj must cast to VSProject4 to manipulate package references
            if (vsProject4 == null)
            {
                return null;
            }

            // Check for RestoreProjectStyle property
#pragma warning disable CS0618 // Type or member is obsolete
            // Need to validate no project systems get this property via DTE, and if so, switch to GetPropertyValue
            var restoreProjectStyle = vsProjectAdapter.BuildProperties.GetPropertyValueWithDteFallback(
                ProjectBuildProperties.RestoreProjectStyle);
#pragma warning restore CS0618 // Type or member is obsolete

            // For legacy csproj, either the RestoreProjectStyle must be set to PackageReference or
            // project has atleast one package dependency defined as PackageReference
            if (forceCreate
                || PackageReference.Equals(restoreProjectStyle, StringComparison.OrdinalIgnoreCase)
                || (vsProject4.PackageReferences?.InstalledPackages?.Length ?? 0) > 0)
            {
                var nominatesOnSolutionLoad = vsProjectAdapter.IsCapabilityMatch(NuGet.VisualStudio.IDE.ProjectCapabilities.PackageReferences);
                return new VsManagedLanguagesProjectSystemServices(vsProjectAdapter, _threadingService, vsProject4, nominatesOnSolutionLoad, _scriptExecutor);
            }

            return null;
        }
    }
}
