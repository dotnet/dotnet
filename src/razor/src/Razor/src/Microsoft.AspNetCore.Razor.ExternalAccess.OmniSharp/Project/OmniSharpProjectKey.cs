// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;

public sealed class OmniSharpProjectKey
{
    public static OmniSharpProjectKey? From(CodeAnalysis.Project workspaceProject)
    {
        var key = ProjectKey.From(workspaceProject);
        return key is null ? null : new(key);
    }

    internal ProjectKey Key { get; }

    internal OmniSharpProjectKey(ProjectKey key)
    {
        this.Key = key;
    }
}
