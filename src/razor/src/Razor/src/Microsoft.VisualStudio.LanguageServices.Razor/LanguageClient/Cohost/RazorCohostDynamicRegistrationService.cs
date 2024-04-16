// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[Export(typeof(IRazorCohostDynamicRegistrationService))]
[method: ImportingConstructor]
internal class RazorCohostDynamicRegistrationService(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    [ImportMany] IEnumerable<IDynamicRegistrationProvider> registrationProviders,
    RazorCohostClientCapabilitiesService razorCohostClientCapabilitiesService,
    ILoggerFactory loggerFactory)
    : IRazorCohostDynamicRegistrationService
{
#pragma warning restore RS0030 // Do not use banned APIs
    private readonly DocumentFilter[] _filter = [new DocumentFilter()
    {
        Language = Constants.RazorLanguageName,
        Pattern = "**/*.{razor,cshtml}"
    }];

    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IEnumerable<IDynamicRegistrationProvider> _registrationProviders = registrationProviders;
    private readonly RazorCohostClientCapabilitiesService _razorCohostClientCapabilitiesService = razorCohostClientCapabilitiesService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorCohostDynamicRegistrationService>();

    public async Task RegisterAsync(string clientCapabilitiesString, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return;
        }

        var clientCapabilities = JsonConvert.DeserializeObject<VSInternalClientCapabilities>(clientCapabilitiesString) ?? new();

        _razorCohostClientCapabilitiesService.SetCapabilities(clientCapabilities);

        _registrationProviders.TryGetCount(out var providerCount);
        using var registrations = new PooledArrayBuilder<Registration>(providerCount);

        foreach (var provider in _registrationProviders)
        {
            var registration = provider.GetRegistration(clientCapabilities, _filter, requestContext);

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
