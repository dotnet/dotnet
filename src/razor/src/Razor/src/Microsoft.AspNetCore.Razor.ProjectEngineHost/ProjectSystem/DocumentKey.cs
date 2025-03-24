// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

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
           FilePathComparer.Instance.Equals(FilePath, other.FilePath);

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();
        hash.Add(ProjectKey);
        hash.Add(FilePath, FilePathComparer.Instance);

        return hash;
    }

    public int CompareTo(DocumentKey other)
    {
        var comparison = ProjectKey.CompareTo(other.ProjectKey);
        if (comparison != 0)
        {
            return comparison;
        }

        return FilePathComparer.Instance.Compare(FilePath, other.FilePath);
    }
}
