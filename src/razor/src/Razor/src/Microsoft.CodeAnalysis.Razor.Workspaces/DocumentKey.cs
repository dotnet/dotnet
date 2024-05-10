// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor;

internal readonly record struct DocumentKey
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

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();
        hash.Add(ProjectKey);
        hash.Add(DocumentFilePath, FilePathComparer.Instance);
        return hash;
    }
}
