// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;

public class OmniSharpProjectChangeEventArgs : EventArgs
{
    internal OmniSharpProjectChangeEventArgs(ProjectChangeEventArgs args) : this(
        OmniSharpProjectSnapshot.Convert(args.Older),
        OmniSharpProjectSnapshot.Convert(args.Newer),
        args.DocumentFilePath,
        (OmniSharpProjectChangeKind)args.Kind)
    {
    }

    public OmniSharpProjectChangeEventArgs(OmniSharpProjectSnapshot? older, OmniSharpProjectSnapshot? newer, string? documentFilePath, OmniSharpProjectChangeKind kind)
    {
        if (older is null && newer is null)
        {
            throw new ArgumentException("Both projects cannot be null.");
        }

        Older = older;
        Newer = newer;
        DocumentFilePath = documentFilePath;
        Kind = kind;
    }

    public OmniSharpProjectSnapshot? Older { get; }

    public OmniSharpProjectSnapshot? Newer { get; }

    public string? DocumentFilePath { get; }

    public OmniSharpProjectChangeKind Kind { get; }
}
