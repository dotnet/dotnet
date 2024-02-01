// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.VisualStudio;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Provides a method of creating <see cref="CpsPackageReferenceProject"/> instance.
    /// </summary>
    [Export(typeof(INuGetProjectProvider))]
    [Name(nameof(CpsPackageReferenceProjectProvider))]
    public class CpsPackageReferenceProjectProvider : INuGetProjectProvider
    {
        private static readonly string PackageReference = ProjectStyle.PackageReference.ToString();

        private readonly IProjectSystemCache _projectSystemCache;
        private readonly Lazy<IScriptExecutor> _scriptExecutor;

        public RuntimeTypeHandle ProjectType => typeof(CpsPackageReferenceProject).TypeHandle;

        [ImportingConstructor]
        public CpsPackageReferenceProjectProvider(IProjectSystemCache projectSystemCache, Lazy<IScriptExecutor> scriptExecutor)
            : this(AsyncServiceProvider.GlobalProvider, projectSystemCache, scriptExecutor)
        { }

        public CpsPackageReferenceProjectProvider(
            IAsyncServiceProvider vsServiceProvider,
            IProjectSystemCache projectSystemCache,
            Lazy<IScriptExecutor> scriptExecutor)
        {
            Assumes.Present(vsServiceProvider);
            Assumes.Present(projectSystemCache);
            Assumes.Present(scriptExecutor);

            _projectSystemCache = projectSystemCache;
            _scriptExecutor = scriptExecutor;
        }

        public async Task<NuGetProject> TryCreateNuGetProjectAsync(
            IVsProjectAdapter vsProject,
            ProjectProviderContext context,
            bool forceProjectType)
        {
            Assumes.Present(vsProject);
            Assumes.Present(context);

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // The project must be an IVsHierarchy.
            var hierarchy = vsProject.VsHierarchy;

            if (hierarchy == null)
            {
                return null;
            }

            // Check that the project supports both CPS and PackageReferences
            if (!(await vsProject.IsCapabilityMatchAsync(NuGet.VisualStudio.IDE.ProjectCapabilities.Cps) &&
                await vsProject.IsCapabilityMatchAsync(NuGet.VisualStudio.IDE.ProjectCapabilities.PackageReferences)))
            {
                return null;
            }

            var buildProperties = vsProject.BuildProperties;

            // read MSBuild property RestoreProjectStyle
#pragma warning disable CS0618 // Type or member is obsolete
            // Need to validate no project systems get this property via DTE, and if so, switch to GetPropertyValue
            var restoreProjectStyle = buildProperties.GetPropertyValueWithDteFallback(ProjectBuildProperties.RestoreProjectStyle);
#pragma warning restore CS0618 // Type or member is obsolete

            // check for RestoreProjectStyle property is set and if not set to PackageReference then return false
            if (!(string.IsNullOrEmpty(restoreProjectStyle) ||
                restoreProjectStyle.Equals(PackageReference, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var fullProjectPath = vsProject.FullProjectPath;
            var unconfiguredProject = GetUnconfiguredProject(vsProject.Project);

            var projectServices = new CpsProjectSystemServices(vsProject, _scriptExecutor);

            return new CpsPackageReferenceProject(
                vsProject.ProjectName,
                vsProject.CustomUniqueName,
                fullProjectPath,
                _projectSystemCache,
                unconfiguredProject,
                projectServices,
                vsProject.ProjectId);
        }

        private static UnconfiguredProject GetUnconfiguredProject(EnvDTE.Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var context = project as IVsBrowseObjectContext;
            if (context == null)
            {
                // VC implements this on their DTE.Project.Object
                context = project.Object as IVsBrowseObjectContext;
            }
            return context?.UnconfiguredProject;
        }
    }
}
