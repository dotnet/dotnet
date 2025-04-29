// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(IRazorCohostDynamicRegistrationService))]
[method: ImportingConstructor]
internal class RazorCohostDynamicRegistrationService(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    [ImportMany] IEnumerable<Lazy<IDynamicRegistrationProvider>> lazyRegistrationProviders,
    Lazy<RazorCohostClientCapabilitiesService> lazyRazorCohostClientCapabilitiesService)
    : IRazorCohostDynamicRegistrationService
{
    private static readonly DocumentFilter[] s_filter = [new DocumentFilter()
    {
        Language = CodeAnalysis.ExternalAccess.Razor.Cohost.Constants.RazorLanguageName,
        Pattern = "**/*.{razor,cshtml}"
    }];

    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly ImmutableArray<Lazy<IDynamicRegistrationProvider>> _lazyRegistrationProviders = [.. lazyRegistrationProviders];
    private readonly Lazy<RazorCohostClientCapabilitiesService> _lazyRazorCohostClientCapabilitiesService = lazyRazorCohostClientCapabilitiesService;

    public async Task RegisterAsync(string clientCapabilitiesString, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return;
        }

        var clientCapabilities = JsonSerializer.Deserialize<VSInternalClientCapabilities>(clientCapabilitiesString, JsonHelpers.VsLspJsonSerializerOptions) ?? new();

        _lazyRazorCohostClientCapabilitiesService.Value.SetCapabilities(clientCapabilities);

        // We assume most registration providers will just return one, so whilst this isn't completely accurate, it's a
        // reasonable starting point
        using var registrations = new PooledArrayBuilder<Registration>(_lazyRegistrationProviders.Length);

        foreach (var provider in _lazyRegistrationProviders)
        {
            foreach (var registration in provider.Value.GetRegistrations(clientCapabilities, requestContext))
            {
                // We don't unregister anything, so we don't need to do anything interesting with Ids
                registration.Id = Guid.NewGuid().ToString();
                if (registration.RegisterOptions is ITextDocumentRegistrationOptions options)
                {
                    options.DocumentSelector = s_filter;
                }

                registrations.Add(registration);
            }
        }

        var razorCohostClientLanguageServerManager = requestContext.GetRequiredService<IRazorClientLanguageServerManager>();

        await razorCohostClientLanguageServerManager.SendRequestAsync(
            Methods.ClientRegisterCapabilityName,
            new RegistrationParams()
            {
                Registrations = registrations.ToArray()
            },
            cancellationToken).ConfigureAwait(false);
    }
}
