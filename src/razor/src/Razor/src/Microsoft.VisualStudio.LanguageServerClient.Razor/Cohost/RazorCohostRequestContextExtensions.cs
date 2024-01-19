// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Cohost;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

internal static class RazorCohostRequestContextExtensions
{
    /// <summary>
    /// Simple method to allow cohosted endpoints to get a document context that is API compatible with <see cref="RazorRequestContext"/>
    /// </summary>
    public static VersionedDocumentContext GetRequiredDocumentContext(this RazorCohostRequestContext context)
    {
        var documentContextFactory = context.GetRequiredService<CohostDocumentContextFactory>();

        return documentContextFactory.Create(context.Uri.AssumeNotNull(), context.TextDocument.AssumeNotNull());
    }
}
