// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal sealed class MiscFilesHostProject : HostProject
{
    public static MiscFilesHostProject Instance { get; } = Create();

    public static bool IsMiscellaneousProject(IProjectSnapshot project)
    {
        return project.Key == Instance.Key;
    }

    public string DirectoryPath { get; }

    private MiscFilesHostProject(
        string directory,
        string projectFilePath,
        string intermediateOutputPath,
        RazorConfiguration razorConfiguration,
        string? rootNamespace, string?
        displayName = null)
        : base(projectFilePath, intermediateOutputPath, razorConfiguration, rootNamespace, displayName)
    {
        DirectoryPath = directory;
    }

    private static MiscFilesHostProject Create()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n"));
        var miscellaneousProjectPath = Path.Combine(tempDirectory, "__MISC_RAZOR_PROJECT__");
        var normalizedPath = FilePathNormalizer.Normalize(miscellaneousProjectPath);

        return new MiscFilesHostProject(
            tempDirectory,
            normalizedPath,
            normalizedPath,
            FallbackRazorConfiguration.Latest,
            rootNamespace: null,
            "Miscellaneous Files");
    }
}
