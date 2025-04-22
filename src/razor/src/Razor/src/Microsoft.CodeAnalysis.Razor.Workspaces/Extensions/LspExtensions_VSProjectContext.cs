// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    internal static ProjectKey ToProjectKey(this VSProjectContext projectContext)
    {
        return new ProjectKey(projectContext.Id);
    }
}
