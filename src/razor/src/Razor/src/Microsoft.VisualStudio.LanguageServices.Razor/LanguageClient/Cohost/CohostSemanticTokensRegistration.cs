// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Text.Json;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[Export(typeof(IDynamicRegistrationProvider))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostSemanticTokensRegistration(ISemanticTokensLegendService semanticTokensLegendService) : IDynamicRegistrationProvider
{
    private readonly ISemanticTokensLegendService _semanticTokensLegendService = semanticTokensLegendService;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.SemanticTokens?.DynamicRegistration == true)
        {
            var semanticTokensRefreshQueue = requestContext.GetRequiredService<IRazorSemanticTokensRefreshQueue>();
            var clientCapabilitiesString = JsonSerializer.Serialize(clientCapabilities);
            semanticTokensRefreshQueue.Initialize(clientCapabilitiesString);

            return [new Registration()
            {
                Method = Methods.TextDocumentSemanticTokensRangeName,
                RegisterOptions = new SemanticTokensRegistrationOptions()
                    .EnableSemanticTokens(_semanticTokensLegendService)
            }];
        }

        return [];
    }
}