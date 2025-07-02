// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal readonly record struct DocumentKey : IComparable<DocumentKey>
{
    public ProjectKey ProjectKey { get; }
    public string FilePath { get; }

    public DocumentKey(ProjectKey projectKey, string filePath)
    {
        ProjectKey = projectKey;
        FilePath = filePath;
    }

    public bool Equals(DocumentKey other)
        => ProjectKey.Equals(other.ProjectKey) &&
           PathUtilities.OSSpecificPathComparer.Equals(FilePath, other.FilePath);

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();
        hash.Add(ProjectKey);
        hash.Add(FilePath, PathUtilities.OSSpecificPathComparer);

        return hash;
    }

    public int CompareTo(DocumentKey other)
    {
        var comparison = ProjectKey.CompareTo(other.ProjectKey);
        if (comparison != 0)
        {
            return comparison;
        }

        return PathUtilities.OSSpecificPathComparer.Compare(FilePath, other.FilePath);
    }
}
