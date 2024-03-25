// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Export(typeof(IRazorCohostCapabilitiesProvider)), System.Composition.Shared]
[Export(typeof(IClientCapabilitiesService))]
[method: ImportingConstructor]
internal class RazorCohostCapabilitiesProvider([ImportMany(typeof(ICapabilitiesProvider))] IEnumerable<Lazy<ICapabilitiesProvider>> cohostCapabilitiesProviders)
    : IRazorCohostCapabilitiesProvider, IClientCapabilitiesService
{
    private readonly IEnumerable<Lazy<ICapabilitiesProvider>> _cohostCapabilitiesProviders = cohostCapabilitiesProviders;

    private VSInternalClientCapabilities? _clientCapabilities;

    public bool CanGetClientCapabilities => _clientCapabilities is not null;

    public VSInternalClientCapabilities ClientCapabilities => _clientCapabilities.AssumeNotNull();

    public string GetCapabilities(string clientCapabilities)
    {
        var clientCapabilitiesObject = JsonConvert.DeserializeObject<VSInternalClientCapabilities>(clientCapabilities) ?? new();

        _clientCapabilities = clientCapabilitiesObject;

        var serverCapabilities = new VSInternalServerCapabilities();

        foreach (var provider in _cohostCapabilitiesProviders)
        {
            provider.Value.ApplyCapabilities(serverCapabilities, clientCapabilitiesObject);
        }

        return JsonConvert.SerializeObject(serverCapabilities);
    }
}
