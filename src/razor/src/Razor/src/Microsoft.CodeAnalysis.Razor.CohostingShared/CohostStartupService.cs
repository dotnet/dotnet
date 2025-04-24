// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(ICohostStartupService))]
[method: ImportingConstructor]
internal class CohostStartupService(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    [ImportMany] IEnumerable<Lazy<IRazorCohostStartupService>> lazyStartupServices)
    : ICohostStartupService
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly ImmutableArray<Lazy<IRazorCohostStartupService>> _lazyStartupServices = [.. lazyStartupServices];

    public async Task StartupAsync(string clientCapabilitiesString, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        var clientCapabilities = JsonSerializer.Deserialize<VSInternalClientCapabilities>(clientCapabilitiesString, JsonHelpers.JsonSerializerOptions) ?? new();

        // We want to make sure LanguageServerFeatureOptions is initialized first, so decisions can be made based on flags,
        // and we also ensure we have a RazorClientLanguageServerManager available for anything that needs to call back to the client,
        // so we do those two things first, manually.
        if (_languageServerFeatureOptions is IRazorCohostStartupService startupService)
        {
            await startupService.StartupAsync(clientCapabilities, requestContext, cancellationToken).ConfigureAwait(false);
        }

        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return;
        }

        foreach (var provider in _lazyStartupServices)
        {
            await provider.Value.StartupAsync(clientCapabilities, requestContext, cancellationToken).ConfigureAwait(false);
        }
    }
}
