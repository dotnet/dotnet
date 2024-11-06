// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal sealed record class MiscFilesHostProject : HostProject
{
    public static MiscFilesHostProject Instance { get; } = Create();

    public static bool IsMiscellaneousProject(IProjectSnapshot project)
    {
        return project.Key == Instance.Key;
    }

    public string DirectoryPath { get; }

    private MiscFilesHostProject(
        string directory,
        string filePath,
        string intermediateOutputPath,
        RazorConfiguration razorConfiguration,
        string? rootNamespace, string?
        displayName = null)
        : base(filePath, intermediateOutputPath, razorConfiguration, rootNamespace, displayName)
    {
        DirectoryPath = directory;
    }

    private static MiscFilesHostProject Create()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n"));
        var filePath = Path.Combine(tempDirectory, "__MISC_RAZOR_PROJECT__");
        var normalizedPath = FilePathNormalizer.Normalize(filePath);

        return new MiscFilesHostProject(
            tempDirectory,
            normalizedPath,
            normalizedPath,
            FallbackRazorConfiguration.Latest,
            rootNamespace: null,
            "Miscellaneous Files");
    }

    public bool Equals(MiscFilesHostProject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) &&
               FilePathComparer.Instance.Equals(DirectoryPath, other.DirectoryPath);
    }

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();

        hash.Add(base.GetHashCode());
        hash.Add(DirectoryPath, FilePathComparer.Instance);

        return hash.CombinedHash;
    }
}
