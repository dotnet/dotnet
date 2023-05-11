// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Represent net core project systems in visual studio.
    /// </summary>
    internal class CpsProjectSystemServices :
        INuGetProjectServices
    {
        public CpsProjectSystemServices(
            IVsProjectAdapter vsProjectAdapter,
            Lazy<IScriptExecutor> scriptExecutor)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(scriptExecutor);

            ScriptService = new VsProjectScriptHostService(vsProjectAdapter, scriptExecutor);
        }

        [Obsolete]
        public IProjectBuildProperties BuildProperties => throw new NotSupportedException();

        public IProjectSystemCapabilities Capabilities => throw new NotSupportedException();

        public IProjectSystemReferencesReader ReferencesReader => throw new NotSupportedException();

        public IProjectSystemReferencesService References => throw new NotSupportedException();

        public IProjectSystemService ProjectSystem => throw new NotSupportedException();

        public IProjectScriptHostService ScriptService { get; }

    }
}
