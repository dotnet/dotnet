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
            ArtifactType.Tarball => ".tar.gz",
            ArtifactType.Zipball => ".zip",
            _ => throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null)
        };
    }
}
