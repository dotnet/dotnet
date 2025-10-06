// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal static class TestHostDocument
{
    public static HostDocument Create(HostProject hostProject, string documentFilePath)
    {
        var targetPath = FilePathNormalizer.Normalize(documentFilePath);
        var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(hostProject.FilePath);
        if (targetPath.StartsWith(projectDirectory))
        {
            targetPath = targetPath[projectDirectory.Length..];
        }

        return new(documentFilePath, targetPath);
    }
}
