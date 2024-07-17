// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(RazorCohostClientCapabilitiesService))]
[Export(typeof(IClientCapabilitiesService))]
internal class RazorCohostClientCapabilitiesService : IClientCapabilitiesService
{
    private VSInternalClientCapabilities? _clientCapabilities;

    public bool CanGetClientCapabilities => _clientCapabilities is not null;

    public VSInternalClientCapabilities ClientCapabilities => _clientCapabilities ?? throw new InvalidOperationException("Client capabilities requested before initialized.");

    public void SetCapabilities(VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;
    }
}
