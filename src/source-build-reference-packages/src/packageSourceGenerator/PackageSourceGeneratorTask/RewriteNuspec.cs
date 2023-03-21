// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Packaging;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    public partial class RewriteNuspec : Task
    {
        [Required]
        public string? NuspecPath { get; set; }

        [Required]
        public string? TargetPath { get; set; }

        public string? IncludeTargetFrameworks { get; set; }

        public string? ExcludeTargetFrameworks { get; set; }

        public bool RemoveIcon { get; set; }

        public bool RemoveRuntimeSpecificDependencies { get; set; }

        public override bool Execute()
        {
            string nuspecContent = File.ReadAllText(NuspecPath!);
            NuspecReader nuspecReader = new(NuspecPath!);
            TargetFrameworkRegexFilter targetFrameworkRegexFilter = new(IncludeTargetFrameworks,
                ExcludeTargetFrameworks);

            IEnumerable<FrameworkSpecificGroup> frameworkAssemblyGroups = nuspecReader.GetFrameworkAssemblyGroups();
            foreach (FrameworkSpecificGroup frameworkSpecificGroup in frameworkAssemblyGroups)
            {
                string targetFramework = frameworkSpecificGroup.TargetFramework.GetShortFolderName();
                if (targetFrameworkRegexFilter.IsIncludedAndNotExcluded(targetFramework))
                    continue;

                string framework = frameworkSpecificGroup.TargetFramework.GetFrameworkString();
                string pattern = $@" *<frameworkAssembly.*?targetFramework=""{framework}"" />\r?\n";
                nuspecContent = Regex.Replace(nuspecContent, pattern, string.Empty);
            }

            IEnumerable<PackageDependencyGroup> dependencyGroups = nuspecReader.GetDependencyGroups();
            foreach (PackageDependencyGroup dependencyGroup in dependencyGroups)
            {
                string targetFramework = dependencyGroup.TargetFramework.GetShortFolderName();
                if (targetFrameworkRegexFilter.IsIncludedAndNotExcluded(targetFramework))
                    continue;

                string framework = dependencyGroup.TargetFramework.GetFrameworkString();
                string pattern = @$" *<group targetFramework=""{framework}""(?:>.+?</group>| />)\r?\n";
                nuspecContent = Regex.Replace(nuspecContent, pattern, string.Empty, RegexOptions.Singleline);
            }

            if (RemoveIcon)
            {
                nuspecContent = GetIconRegex().Replace(nuspecContent, string.Empty);
            }

            if (RemoveRuntimeSpecificDependencies)
            {
                nuspecContent = GetRuntimeSpecificDependenciesRegex().Replace(nuspecContent, string.Empty);
            }

            File.WriteAllText(TargetPath!, nuspecContent);
            return true;
        }

        [GeneratedRegex(" *<icon>.+</icon>\r?\n")]
        private static partial Regex GetIconRegex();

        [GeneratedRegex(@" *<dependency id=""runtime.native.+?"".+? />\r?\n")]
        private static partial Regex GetRuntimeSpecificDependenciesRegex();
    }
}
