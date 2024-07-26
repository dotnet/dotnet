// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    internal static ProjectKey ToProjectKey(this VSProjectContext projectContext)
    {
        return new ProjectKey(projectContext.Id);
    }
}
