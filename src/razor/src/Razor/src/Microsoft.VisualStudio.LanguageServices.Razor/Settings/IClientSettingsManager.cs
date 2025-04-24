// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

namespace Microsoft.VisualStudio.Razor.Settings;

internal interface IClientSettingsManager : IClientSettingsReader
{
    void Update(ClientSpaceSettings updateSettings);
    void Update(ClientCompletionSettings updateSettings);
    void Update(ClientAdvancedSettings updateSettings);

    event EventHandler<ClientSettingsChangedEventArgs> ClientSettingsChanged;
}
