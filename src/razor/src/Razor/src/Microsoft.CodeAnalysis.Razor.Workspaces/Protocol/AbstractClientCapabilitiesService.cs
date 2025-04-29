// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal abstract class AbstractClientCapabilitiesService : IClientCapabilitiesService
{
    private VSInternalClientCapabilities? _clientCapabilities;

    public bool CanGetClientCapabilities => _clientCapabilities is not null;

    public VSInternalClientCapabilities ClientCapabilities => _clientCapabilities ?? throw new InvalidOperationException("Client capabilities requested before initialized.");

    public void SetCapabilities(VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;
    }
}
