// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// This class houses locale-agnostic error identifiers which aid
    /// users searching for solutions to problems that may be in a different
    /// language than the human-readable error message accompanying them.
    /// </summary>
    internal static class NuGetErrorId
    {
        public const string NoCompatibleProjects = "NuGetNoCompatibleProjects";
        public const string ProjectNotFound = "NuGetProjectNotFound";
        public const string NoActiveSolution = "NuGetNoActiveSolution";
        public const string UnsavedSolution = "NuGetUnsavedSolution";
        public const string FileNotFound = "NuGetFileNotFound";
        public const string FileExistsNoClobber = "NuGetFileExistsNoClobber";
        public const string TooManySpecFiles = "NuGetTooManySpecFiles";
        public const string NuspecFileNotFound = "NuGetNuspecFileNotFound";
        public const string CmdletUnhandledException = "NuGetCmdletUnhandledException";
        public const string MissingPackages = "NuGetMissingPackages";
    }
}
