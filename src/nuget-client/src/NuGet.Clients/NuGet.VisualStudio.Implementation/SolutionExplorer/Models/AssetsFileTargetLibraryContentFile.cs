// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Microsoft;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.SolutionExplorer.Models
{
    /// <summary>
    /// Data about a content file within a package in a given target, from <c>project.assets.json</c>. Immutable.
    /// </summary>
    internal sealed class AssetsFileTargetLibraryContentFile
    {
        public AssetsFileTargetLibraryContentFile(LockFileContentFile file)
        {
            Requires.NotNull(file, nameof(file));

            BuildAction = file.BuildAction.Value;
            CodeLanguage = file.CodeLanguage;
            CopyToOutput = file.CopyToOutput;
            OutputPath = file.OutputPath;
            Path = file.Path ?? ""; // Path should always be present so don't require consumers to null check. Not worth throwing if null though.
            PPOutputPath = file.PPOutputPath;
        }

        public string? BuildAction { get; }
        public string? CodeLanguage { get; }
        public bool CopyToOutput { get; }
        public string? OutputPath { get; }
        public string Path { get; }
        public string? PPOutputPath { get; }
    }
}
