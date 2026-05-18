// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Commands.Restore;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.SolutionRestoreManager.PackageSpecAdapter
{
    internal class NominationProjectAdapter : IProject
    {
        public NominationProjectAdapter(ProjectNames projectNames, IVsProjectRestoreInfo3 vsProjectRestoreInfo)
        {
            FullPath = projectNames.FullName;
            Directory = System.IO.Path.GetDirectoryName(FullPath) ?? throw new System.ArgumentNullException(nameof(FullPath), "Path.GetDirectoryName returned null");
            OuterBuild = new OuterBuildAdapter(Directory, projectNames.ShortName, vsProjectRestoreInfo);

            var targetFrameworks = new Dictionary<string, ITargetFramework>(vsProjectRestoreInfo.TargetFrameworks.Count, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < vsProjectRestoreInfo.TargetFrameworks.Count; i++)
            {
                var targetFramework = vsProjectRestoreInfo.TargetFrameworks[i];
                if (!targetFramework.Properties.TryGetValue(ProjectBuildProperties.TargetFramework, out var alias)
                    || string.IsNullOrEmpty(alias))
                {
                    // native C++ projects do not have TargetFramework property, use empty string as alias.
                    alias = string.Empty;
                }
                targetFrameworks.Add(alias, new InnerBuildAdapter(targetFramework));
            }
            TargetFrameworks = targetFrameworks;
        }

        public string FullPath { get; }

        public string Directory { get; }

        public ITargetFramework OuterBuild { get; }

        public IReadOnlyDictionary<string, ITargetFramework> TargetFrameworks { get; }

        // Global properties are MSBuild command-line arguments, which don't exist in VS nominations.
        public string? GetGlobalProperty(string propertyName) => null;
    }
}
