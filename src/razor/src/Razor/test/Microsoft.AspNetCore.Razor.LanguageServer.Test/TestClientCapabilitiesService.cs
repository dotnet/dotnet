// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

internal class TestClientCapabilitiesService(VSInternalClientCapabilities clientCapabilities) : IClientCapabilitiesService
{
    public VSInternalClientCapabilities ClientCapabilities => clientCapabilities;
}
