// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(INuGetProjectProvider))]
    [Name(nameof(MSBuildNuGetProjectProvider))]
    [Order(After = nameof(ProjectJsonProjectProvider))]
    internal class MSBuildNuGetProjectProvider : INuGetProjectProvider
    {
        private readonly IVsProjectThreadingService _threadingService;
        private readonly Lazy<IScriptExecutor> _scriptExecutor;

        public RuntimeTypeHandle ProjectType => typeof(VsMSBuildNuGetProject).TypeHandle;

        [ImportingConstructor]
        public MSBuildNuGetProjectProvider(IVsProjectThreadingService threadingService, Lazy<IScriptExecutor> scriptExecutor)
            : this(AsyncServiceProvider.GlobalProvider,
                   threadingService, scriptExecutor)
        { }

        public MSBuildNuGetProjectProvider(
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

        public NuGetProject TryCreateNuGetProject(
            IVsProjectAdapter vsProjectAdapter,
            ProjectProviderContext context,
            bool forceProjectType)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(context);

            var projectSystem = MSBuildNuGetProjectSystemFactory.CreateMSBuildNuGetProjectSystem(
                vsProjectAdapter,
                context.ProjectContext);

            projectSystem.InitializeProperties();

            var projectServices = new VsMSBuildProjectSystemServices(vsProjectAdapter, projectSystem, _threadingService, _scriptExecutor);

            var folderNuGetProjectFullPath = context.PackagesPathFactory();

            // Project folder path is the packages config folder path
            var packagesConfigFolderPath = vsProjectAdapter.ProjectDirectory;

            return new VsMSBuildNuGetProject(
                vsProjectAdapter,
                projectSystem,
                folderNuGetProjectFullPath,
                packagesConfigFolderPath,
                projectServices);
        }
    }
}
