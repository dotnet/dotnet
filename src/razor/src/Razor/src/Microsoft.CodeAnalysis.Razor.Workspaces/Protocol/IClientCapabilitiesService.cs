// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;

internal interface IClientCapabilitiesService
{
    /// <summary>
    /// Indicates whether capabilities have been sent by the client, and therefore where a call to ClientCapabilities would succeed
    /// </summary>
    bool CanGetClientCapabilities { get; }

    VSInternalClientCapabilities ClientCapabilities { get; }
}
