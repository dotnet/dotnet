// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Export(typeof(IRazorCohostCapabilitiesProvider)), System.Composition.Shared]
[method: ImportingConstructor]
internal class RazorCohostCapabilitiesProvider([ImportMany(typeof(ICapabilitiesProvider))] IEnumerable<Lazy<ICapabilitiesProvider>> cohostCapabilitiesProviders) : IRazorCohostCapabilitiesProvider
{
    private readonly IEnumerable<Lazy<ICapabilitiesProvider>> _cohostCapabilitiesProviders = cohostCapabilitiesProviders;

    public string GetCapabilities(string clientCapabilities)
    {
        var clientCapabilitiesObject = JsonConvert.DeserializeObject<VSInternalClientCapabilities>(clientCapabilities) ?? new();

        var serverCapabilities = new VSInternalServerCapabilities();

        foreach (var provider in _cohostCapabilitiesProviders)
        {
            provider.Value.ApplyCapabilities(serverCapabilities, clientCapabilitiesObject);
        }

        return JsonConvert.SerializeObject(serverCapabilities);
    }
}
