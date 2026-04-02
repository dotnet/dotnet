// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace NuGet.CommandLine.XPlat;

/// <summary>
/// We cannot have a dependency on a package from SDK due to source build,
/// hence we invert the relationship and define the interface here,
/// SDK implements it and we load the implementation dynamically.
/// </summary>
public interface IVirtualProjectBuilder
{
    /// <summary>
    /// Whether the given file path can be a file-based app.
    /// </summary>
    /// <remarks>
    /// Currently, files that exist and have the <c>.cs</c> file extension or <c>#!</c> (shebang) are valid file-based apps.
    /// </remarks>
    bool IsValidEntryPointPath(string entryPointFilePath);

    /// <summary>
    /// Returns the virtual project path (e.g., <c>app.csproj</c>) corresponding to the given entry point file
    /// (e.g., <c>app.cs</c>). The returned path is used by MSBuild for DG specs and property evaluation.
    /// </summary>
    string GetVirtualProjectPath(string entryPointFilePath);

    ProjectRootElement CreateProjectRootElement(string entryPointFilePath, ProjectCollection projectCollection);

    void SaveProject(string entryPointFilePath, ProjectRootElement projectRootElement);
}
