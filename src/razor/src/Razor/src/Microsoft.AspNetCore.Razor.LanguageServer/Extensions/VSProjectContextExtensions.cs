// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class VSProjectContextExtensions
{
    internal static ProjectKey ToProjectKey(this VSProjectContext projectContext)
    {
        return ProjectKey.FromString(projectContext.Id);
    }
}
