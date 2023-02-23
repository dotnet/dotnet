// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor;

public readonly struct DocumentKey : IEquatable<DocumentKey>
{
    public string ProjectFilePath { get; }
    public string DocumentFilePath { get; }

    public DocumentKey(string projectFilePath, string documentFilePath)
    {
        ProjectFilePath = projectFilePath;
        DocumentFilePath = documentFilePath;
    }

    public bool Equals(DocumentKey other)
        => FilePathComparer.Instance.Equals(ProjectFilePath, other.ProjectFilePath) &&
           FilePathComparer.Instance.Equals(DocumentFilePath, other.DocumentFilePath);

    public override bool Equals(object? obj)
        => obj is DocumentKey key &&
           Equals(key);

    public override int GetHashCode()
    {
        var hash = new HashCodeCombiner();
        hash.Add(ProjectFilePath, FilePathComparer.Instance);
        hash.Add(DocumentFilePath, FilePathComparer.Instance);
        return hash;
    }

    public static bool operator ==(DocumentKey left, DocumentKey right)
        => left.Equals(right);

    public static bool operator !=(DocumentKey left, DocumentKey right)
        => !(left == right);
}
