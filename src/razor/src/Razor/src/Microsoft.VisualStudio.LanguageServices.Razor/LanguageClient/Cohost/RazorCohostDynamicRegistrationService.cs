// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[Export(typeof(IRazorCohostDynamicRegistrationService))]
[method: ImportingConstructor]
internal class RazorCohostDynamicRegistrationService(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    [ImportMany] IEnumerable<Lazy<IDynamicRegistrationProvider>> lazyRegistrationProviders,
    Lazy<RazorCohostClientCapabilitiesService> lazyRazorCohostClientCapabilitiesService)
    : IRazorCohostDynamicRegistrationService
{
#pragma warning restore RS0030 // Do not use banned APIs
    private readonly DocumentFilter[] _filter = [new DocumentFilter()
    {
        Language = Constants.RazorLanguageName,
        Pattern = "**/*.{razor,cshtml}"
    }];

    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IEnumerable<Lazy<IDynamicRegistrationProvider>> _lazyRegistrationProviders = lazyRegistrationProviders;
    private readonly Lazy<RazorCohostClientCapabilitiesService> _lazyRazorCohostClientCapabilitiesService = lazyRazorCohostClientCapabilitiesService;

    public async Task RegisterAsync(string clientCapabilitiesString, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return;
        }

        // TODO: Should we delay everything below this line until a Razor file is opened?

        var clientCapabilities = JsonSerializer.Deserialize<VSInternalClientCapabilities>(clientCapabilitiesString) ?? new();

        _lazyRazorCohostClientCapabilitiesService.Value.SetCapabilities(clientCapabilities);

        _lazyRegistrationProviders.TryGetCount(out var providerCount);
        using var registrations = new PooledArrayBuilder<Registration>(providerCount);

        foreach (var provider in _lazyRegistrationProviders)
        {
            var registration = provider.Value.GetRegistration(clientCapabilities, _filter, requestContext);

            if (registration is not null)
            {
                // We don't unregister anything, so we don't need to do anything interesting with Ids
                registration.Id = Guid.NewGuid().ToString();
                registrations.Add(registration);
            }
        }

        var razorCohostClientLanguageServerManager = requestContext.GetRequiredService<IRazorCohostClientLanguageServerManager>();

        await razorCohostClientLanguageServerManager.SendRequestAsync(
            Methods.ClientRegisterCapabilityName,
            new RegistrationParams()
            {
                Registrations = registrations.ToArray()
            },
            cancellationToken).ConfigureAwait(false);
    }
}
