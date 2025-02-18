// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
}
