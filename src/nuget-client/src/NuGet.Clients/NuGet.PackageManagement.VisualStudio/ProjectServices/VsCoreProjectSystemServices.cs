// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Represents Visual Studio core project system in the integrated development environment (IDE).
    /// </summary>
    internal class VsCoreProjectSystemServices :
        INuGetProjectServices,
        IProjectSystemCapabilities
    {
        private readonly IVsProjectAdapter _vsProjectAdapter;

        public bool SupportsPackageReferences => false;

        public bool NominatesOnSolutionLoad => false;

        #region INuGetProjectServices

        [Obsolete]
        public IProjectBuildProperties BuildProperties => throw new NotImplementedException();

        public IProjectSystemCapabilities Capabilities => this;

        public IProjectSystemReferencesReader ReferencesReader { get; }

        public IProjectSystemReferencesService References => throw new NotSupportedException();

        public IProjectSystemService ProjectSystem { get; }

        public IProjectScriptHostService ScriptService { get; }

        #endregion INuGetProjectServices

        public VsCoreProjectSystemServices(
            IVsProjectAdapter vsProjectAdapter,
            IVsProjectThreadingService threadingService,
            Lazy<IScriptExecutor> _scriptExecutor)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(threadingService);

            _vsProjectAdapter = vsProjectAdapter;
            ProjectSystem = new VsCoreProjectSystem(_vsProjectAdapter);
            ReferencesReader = new VsCoreProjectSystemReferenceReader(vsProjectAdapter, threadingService);
            ScriptService = new VsProjectScriptHostService(vsProjectAdapter, _scriptExecutor);
        }
    }
}
