// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.VisualStudio.Editor.Razor.Settings;

internal interface IAdvancedSettingsStorage
{
    ClientAdvancedSettings GetAdvancedSettings();

    Task OnChangedAsync(Action<ClientAdvancedSettings> changed);
}
