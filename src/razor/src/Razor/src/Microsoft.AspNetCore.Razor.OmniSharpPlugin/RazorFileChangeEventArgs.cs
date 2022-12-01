// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.Build.Execution;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

internal class RazorFileChangeEventArgs : EventArgs
{
    public RazorFileChangeEventArgs(
        string filePath,
        ProjectInstance projectInstance,
        RazorFileChangeKind kind)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (projectInstance is null)
        {
            throw new ArgumentNullException(nameof(projectInstance));
        }

        FilePath = filePath;
        UnevaluatedProjectInstance = projectInstance;
        Kind = kind;
    }

    public string FilePath { get; }

    public ProjectInstance UnevaluatedProjectInstance { get; }

    public RazorFileChangeKind Kind { get; }
}
