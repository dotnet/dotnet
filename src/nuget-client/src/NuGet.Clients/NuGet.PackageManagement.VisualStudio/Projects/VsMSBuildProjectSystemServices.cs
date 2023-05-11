// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Implements project services in terms of <see cref="VsMSBuildProjectSystem"/>
    /// </summary>
    internal class VsMSBuildProjectSystemServices
        : INuGetProjectServices
        , IProjectSystemCapabilities
    {
        private readonly IVsProjectAdapter _vsProjectAdapter;
        private readonly IVsProjectThreadingService _threadingService;
        private readonly VsMSBuildProjectSystem _vsProjectSystem;

        #region INuGetProjectServices

        [Obsolete]
        public IProjectBuildProperties BuildProperties => throw new NotImplementedException();

        public IProjectSystemCapabilities Capabilities => this;

        public IProjectSystemReferencesReader ReferencesReader { get; }

        public IProjectSystemReferencesService References => throw new NotSupportedException();

        public IProjectSystemService ProjectSystem => _vsProjectSystem;

        public IProjectScriptHostService ScriptService { get; }

        #endregion INuGetProjectServices

        public bool SupportsPackageReferences
        {
            get
            {
                return _threadingService.JoinableTaskFactory.Run(async () =>
                {
                    await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();
                    return _vsProjectAdapter.Project.Object is VSLangProj150.VSProject4;
                });
            }
        }

        public bool NominatesOnSolutionLoad => false;

        public VsMSBuildProjectSystemServices(
            IVsProjectAdapter vsProjectAdapter,
            VsMSBuildProjectSystem vsProjectSystem,
            IVsProjectThreadingService threadingService,
            Lazy<IScriptExecutor> scriptExecutor)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(vsProjectSystem);
            Assumes.Present(threadingService);
            Assumes.Present(scriptExecutor);

            _vsProjectAdapter = vsProjectAdapter;
            _vsProjectSystem = vsProjectSystem;
            _threadingService = threadingService;

            if (vsProjectSystem is NativeProjectSystem)
            {
                ReferencesReader = new NativeProjectSystemReferencesReader(vsProjectAdapter, _threadingService);
            }
            else if (vsProjectSystem is CpsProjectSystem)
            {
                ReferencesReader = new CpsProjectSystemReferenceReader(vsProjectAdapter, _threadingService);
            }
            else
            {
                ReferencesReader = new VsCoreProjectSystemReferenceReader(vsProjectAdapter, _threadingService);
            }
            ScriptService = new VsProjectScriptHostService(vsProjectAdapter, scriptExecutor);
        }
    }
}
