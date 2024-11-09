// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NET
using System;
#endif

using System.Diagnostics;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class Extensions
{
    public static DocumentSnapshotHandle ToHandle(this IDocumentSnapshot snapshot)
        => new(snapshot.FilePath, snapshot.TargetPath, snapshot.FileKind);

    public static ProjectKey ToProjectKey(this Project project)
    {
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(project.CompilationOutputInfo.AssemblyPath);
        return new(intermediateOutputPath);
    }

    /// <summary>
    /// Returns <see langword="true"/> if this <see cref="ProjectKey"/> matches the given <see cref="Project"/>.
    /// </summary>
    public static bool Matches(this ProjectKey projectKey, Project project)
    {
        // In order to perform this check, we are relying on the fact that Id will always end with a '/',
        // because it is guaranteed to be normalized. However, CompilationOutputInfo.AssemblyPath will
        // contain the assembly file name, which AreDirectoryPathsEquivalent will shave off before comparing.
        // So, AreDirectoryPathsEquivalent will return true when Id is "C:/my/project/path/"
        // and the assembly path is "C:\my\project\path\assembly.dll"

        Debug.Assert(projectKey.Id.EndsWith('/'), $"This method can't be called if {nameof(projectKey.Id)} is not a normalized directory path.");

        return FilePathNormalizer.AreDirectoryPathsEquivalent(projectKey.Id, project.CompilationOutputInfo.AssemblyPath);
    }
}
