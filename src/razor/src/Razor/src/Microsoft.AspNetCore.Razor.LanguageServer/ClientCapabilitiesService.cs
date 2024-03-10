// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class ClientCapabilitiesService(IInitializeManager<InitializeParams, InitializeResult> initializeManager) : IClientCapabilitiesService
{
    private readonly IInitializeManager<InitializeParams, InitializeResult> _initializeManager = initializeManager;

    public VSInternalClientCapabilities ClientCapabilities => _initializeManager.GetInitializeParams().Capabilities.ToVSInternalClientCapabilities();
}
