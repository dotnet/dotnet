// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

internal class OmniSharpProjectChangeEventArgs : EventArgs
{
    internal OmniSharpProjectChangeEventArgs(ProjectChangeEventArgs args) : this(
        OmniSharpProjectSnapshot.Convert(args.Older),
        OmniSharpProjectSnapshot.Convert(args.Newer),
        args.DocumentFilePath,
        (OmniSharpProjectChangeKind)args.Kind)
    {
        InternalProjectChangeEventArgs = args;
    }

    private OmniSharpProjectChangeEventArgs(OmniSharpProjectSnapshot older, OmniSharpProjectSnapshot newer, string documentFilePath, OmniSharpProjectChangeKind kind)
    {
        if (older is null && newer is null)
        {
            throw new ArgumentException("Both projects cannot be null.");
        }

        Older = older;
        Newer = newer;
        DocumentFilePath = documentFilePath;
        Kind = kind;

        ProjectFilePath = older?.FilePath ?? newer.FilePath;
    }

    internal ProjectChangeEventArgs InternalProjectChangeEventArgs { get; }

    public OmniSharpProjectSnapshot Older { get; }

    public OmniSharpProjectSnapshot Newer { get; }

    public string ProjectFilePath { get; }

    public string DocumentFilePath { get; }

    public OmniSharpProjectChangeKind Kind { get; }

    public static OmniSharpProjectChangeEventArgs CreateTestInstance(OmniSharpProjectSnapshot older, OmniSharpProjectSnapshot newer, string documentFilePath, OmniSharpProjectChangeKind kind) =>
        new(older, newer, documentFilePath, kind);
}
