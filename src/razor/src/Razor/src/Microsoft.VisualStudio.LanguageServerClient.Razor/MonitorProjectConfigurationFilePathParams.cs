// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class MonitorProjectConfigurationFilePathParams
{
    public required string ProjectFilePath { get; init; }

    public required string? ConfigurationFilePath { get; init; }
}
