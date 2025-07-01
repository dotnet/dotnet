// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed record class HostDocument
{
    public RazorFileKind FileKind { get; init; }
    public string FilePath { get; init; }
    public string TargetPath { get; init; }

    public HostDocument(string filePath, string targetPath, RazorFileKind? fileKind = null)
    {
        FilePath = filePath;
        TargetPath = targetPath;
        FileKind = fileKind ?? FileKinds.GetFileKindFromPath(filePath);
    }
}
