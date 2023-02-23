// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Document;

public sealed class OmniSharpHostDocument
{
    public OmniSharpHostDocument(string filePath, string targetPath, string kind)
    {
        InternalHostDocument = new HostDocument(filePath, targetPath, kind);

        if (targetPath.Contains("/"))
        {
            throw new FormatException("TargetPath's must use '\\' instead of '/'");
        }

        if (targetPath.StartsWith("\\", StringComparison.Ordinal))
        {
            throw new FormatException("TargetPath's can't start with '\\'");
        }
    }

    public string FilePath => InternalHostDocument.FilePath;

    internal string TargetPath => InternalHostDocument.TargetPath;

    internal string FileKind => InternalHostDocument.FileKind;

    internal HostDocument InternalHostDocument { get; }
}
