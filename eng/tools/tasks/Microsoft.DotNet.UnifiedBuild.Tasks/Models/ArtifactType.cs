// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.UnifiedBuild.Tasks.Models;

public enum ArtifactType
{
    Tarball,
    Zipball
}

public static class Extensions
{
    public static string GetArtifactExtension(this ArtifactType artifactType)
    {
        return artifactType switch
        {
            ArtifactType.Tarball => "tar.gz",
            ArtifactType.Zipball => "zip",
            _ => throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null)
        };
    }

    public static string GetGitArchiveArgs(this ArtifactType artifactType, string artifactFilePath, string githubRepoName, string artifactPrefixVersion, string sourceCommit)
    {
        string baseArgs = $"archive --format={artifactType.GetArtifactExtension()} --output \"{artifactFilePath}\" --prefix \"{githubRepoName}-{artifactPrefixVersion}/\" {sourceCommit}";
        return artifactType switch
        {
            ArtifactType.Tarball => $"-c \"tar.tar.gz.command=gzip -cn\" {baseArgs}",
            ArtifactType.Zipball => baseArgs,
            _ => throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null)
        };
    }
}
