// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

internal interface IClientSettingsReader
{
    ClientSettings GetClientSettings();
}
