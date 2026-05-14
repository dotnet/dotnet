// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using VsMSBuildNuGetProjectSystemThunk = System.Func<
    NuGet.VisualStudio.IVsProjectAdapter,
    NuGet.ProjectManagement.INuGetProjectContext,
    NuGet.PackageManagement.VisualStudio.VsMSBuildProjectSystem>;

namespace NuGet.PackageManagement.VisualStudio
{
    internal static class MSBuildNuGetProjectSystemFactory
    {
        private static Dictionary<string, VsMSBuildNuGetProjectSystemThunk> _factories = new Dictionary<string, VsMSBuildNuGetProjectSystemThunk>(StringComparer.OrdinalIgnoreCase)
        {
            { VsProjectTypes.WebApplicationProjectTypeGuid, (project, nuGetProjectContext) => new WebProjectSystem(project, nuGetProjectContext) },
            { VsProjectTypes.WebSiteProjectTypeGuid, (project, nuGetProjectContext) => new WebSiteProjectSystem(project, nuGetProjectContext) },
            { VsProjectTypes.FsharpProjectTypeGuid, (project, nuGetProjectContext) => new FSharpProjectSystem(project, nuGetProjectContext) },
            { VsProjectTypes.WixProjectTypeGuid, (project, nuGetProjectContext) => new WixProjectSystem(project, nuGetProjectContext) },
            { VsProjectTypes.JsProjectTypeGuid, (project, nuGetProjectContext) => new JsProjectSystem(project, nuGetProjectContext) },
            { VsProjectTypes.WindowsStoreProjectTypeGuid, (project, nuGetProjectContext) => new WindowsStoreProjectSystem(project, nuGetProjectContext) },
            { VsProjectTypes.DeploymentProjectTypeGuid, (project, nuGetProjectContext) => new VsMSBuildProjectSystem(project, nuGetProjectContext) }
        };

        public static VsMSBuildProjectSystem CreateMSBuildNuGetProjectSystem(IVsProjectAdapter vsProjectAdapter, INuGetProjectContext nuGetProjectContext)
        {
            if (vsProjectAdapter == null)
            {
                throw new ArgumentNullException(nameof(vsProjectAdapter));
            }

            if (string.IsNullOrEmpty(vsProjectAdapter.FullName))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                        Strings.DTE_ProjectUnsupported, vsProjectAdapter.ProjectName));
            }

            var guids = vsProjectAdapter.GetProjectTypeGuids();
            if (guids.Contains(VsProjectTypes.CppProjectTypeGuid)) // Got a cpp project
            {
                return new NativeProjectSystem(vsProjectAdapter, nuGetProjectContext);
            }

            // Try to get a factory for the project type guid
            foreach (var guid in guids)
            {
                if (_factories.TryGetValue(guid, out var factory))
                {
                    return factory(vsProjectAdapter, nuGetProjectContext);
                }
            }

            // Fall back to the default if we have no special project types
            return new VsMSBuildProjectSystem(vsProjectAdapter, nuGetProjectContext);
        }
    }
}
