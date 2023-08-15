// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to retrieve the user's latest settings.
    // NOTE: This method is a poly-fill for VS. We only intend to do it this way until VS formally
    // supports sending workspace configuration requests.
    [JsonRpcMethod(Methods.WorkspaceConfigurationName, UseSingleObjectParameterDeserialization = true)]
    public Task<object[]> WorkspaceConfigurationAsync(ConfigurationParams configParams, CancellationToken _)
    {
        if (configParams is null)
        {
            throw new ArgumentNullException(nameof(configParams));
        }

        var result = new List<object>();
        foreach (var item in configParams.Items)
        {
            // Right now in VS we only care about editor settings, but we should update this logic later if
            // we want to support Razor and HTML settings as well.
            var setting = item.Section switch
            {
                "vs.editor.razor" => _editorSettingsManager.GetClientSettings(),
                _ => new object()
            };

            result.Add(setting);
        }

        return Task.FromResult(result.ToArray());
    }
}
