// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
