// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class VSProjectContextExtensions
{
    internal static ProjectKey ToProjectKey(this VSProjectContext projectContext)
    {
        return new ProjectKey(projectContext.Id);
    }
}
