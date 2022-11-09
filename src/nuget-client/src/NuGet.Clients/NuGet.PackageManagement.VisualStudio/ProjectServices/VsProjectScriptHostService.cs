// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    internal class VsProjectScriptHostService : IProjectScriptHostService
    {
        private readonly IVsProjectAdapter _vsProjectAdapter;
        private readonly Lazy<IScriptExecutor> _scriptExecutor;

        public VsProjectScriptHostService(
            IVsProjectAdapter vsProjectAdapter,
            Lazy<IScriptExecutor> scriptExecutor)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(scriptExecutor);

            _vsProjectAdapter = vsProjectAdapter;
            _scriptExecutor = scriptExecutor;
        }

        public Task ExecutePackageScriptAsync(
            PackageIdentity packageIdentity,
            string packageInstallPath,
            string scriptRelativePath,
            INuGetProjectContext projectContext,
            bool throwOnFailure,
            CancellationToken token)
        {
            var scriptExecutor = _scriptExecutor.Value;
            Assumes.Present(scriptExecutor);

            return scriptExecutor.ExecuteAsync(
                packageIdentity,
                packageInstallPath,
                scriptRelativePath,
                _vsProjectAdapter.Project,
                projectContext,
                throwOnFailure);
        }

        public async Task<bool> ExecutePackageInitScriptAsync(
            PackageIdentity packageIdentity,
            string packageInstallPath,
            INuGetProjectContext projectContext,
            bool throwOnFailure,
            CancellationToken token)
        {
            var scriptExecutor = _scriptExecutor.Value;
            Assumes.Present(scriptExecutor);

            using (var packageReader = new PackageFolderReader(packageInstallPath))
            {
                var toolItemGroups = packageReader.GetToolItems();

                if (toolItemGroups != null)
                {
                    // Init.ps1 must be found at the root folder, target frameworks are not recognized here,
                    // since this is run for the solution.
                    var toolItemGroup = toolItemGroups
                        .FirstOrDefault(group => group.TargetFramework.IsAny);

                    if (toolItemGroup != null)
                    {
                        var initPS1RelativePath = toolItemGroup
                            .Items
                            .FirstOrDefault(p => p.StartsWith(
                                PowerShellScripts.InitPS1RelativePath,
                                StringComparison.OrdinalIgnoreCase));

                        if (!string.IsNullOrEmpty(initPS1RelativePath))
                        {
                            initPS1RelativePath = PathUtility.ReplaceAltDirSeparatorWithDirSeparator(
                                initPS1RelativePath);

                            return await scriptExecutor.ExecuteAsync(
                                packageIdentity,
                                packageInstallPath,
                                initPS1RelativePath,
                                _vsProjectAdapter.Project,
                                projectContext,
                                throwOnFailure);
                        }
                    }
                }
            }

            return false;
        }
    }
}
