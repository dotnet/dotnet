// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static class MiscFilesProject
{
    public static HostProject HostProject { get; }
    public static string DirectoryPath { get; }

    public static ProjectKey Key => HostProject.Key;
    public static string FilePath => HostProject.FilePath;

    static MiscFilesProject()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n"));
        var filePath = Path.Combine(tempDirectory, "__MISC_RAZOR_PROJECT__");
        var normalizedPath = FilePathNormalizer.Normalize(filePath);

        DirectoryPath = tempDirectory;

        HostProject = new(
            normalizedPath,
            normalizedPath,
            FallbackRazorConfiguration.Latest,
            rootNamespace: null,
            "Miscellaneous Files");
    }

    public static ProjectSnapshot GetMiscellaneousProject(this ProjectSnapshotManager projectManager)
        => projectManager.GetRequiredProject(Key);

    public static bool IsMiscellaneousProject(this ProjectSnapshot project)
        => project.Key == Key;
}
