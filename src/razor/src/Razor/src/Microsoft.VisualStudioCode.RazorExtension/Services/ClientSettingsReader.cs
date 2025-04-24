// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[Shared]
[Export(typeof(IClientSettingsReader))]
internal class ClientSettingsReader : IClientSettingsReader
{
    public ClientSettings GetClientSettings()
    {
        // TODO: Implement logic to read client settings
        return ClientSettings.Default;
    }
}
