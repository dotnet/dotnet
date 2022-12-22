// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

namespace NuGet.VisualStudio.SolutionExplorer
{
    internal static class ConfigurationGeneralRule
    {
        public const string SchemaName = "ConfigurationGeneral";

        public const string ProjectAssetsFileProperty = "ProjectAssetsFile";
        public const string TargetFrameworkProperty = "TargetFramework";
        public const string TargetFrameworkMonikerProperty = "TargetFrameworkMoniker";
    }

    internal static class NuGetRestoreRule
    {
        public const string SchemaName = "NuGetRestore";

        public const string NuGetTargetMonikerProperty = "NuGetTargetMoniker";
    }
}
