// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    internal static ProjectKey ToProjectKey(this VSProjectContext projectContext)
    {
        return new ProjectKey(projectContext.Id);
    }
}
