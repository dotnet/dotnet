// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor;

internal readonly struct DocumentKey : IEquatable<DocumentKey>
{
    public ProjectKey ProjectKey { get; }
    public string DocumentFilePath { get; }

    public DocumentKey(ProjectKey projectKey, string documentFilePath)
    {
        ProjectKey = projectKey;
        DocumentFilePath = documentFilePath;
    }

    public bool Equals(DocumentKey other)
        => ProjectKey.Equals(other.ProjectKey) &&
           FilePathComparer.Instance.Equals(DocumentFilePath, other.DocumentFilePath);

    public override bool Equals(object? obj)
        => obj is DocumentKey key &&
           Equals(key);

    public override int GetHashCode()
    {
        var hash = new HashCodeCombiner();
        hash.Add(ProjectKey);
        hash.Add(DocumentFilePath, FilePathComparer.Instance);
        return hash;
    }

    public static bool operator ==(DocumentKey left, DocumentKey right)
        => left.Equals(right);

    public static bool operator !=(DocumentKey left, DocumentKey right)
        => !(left == right);
}
