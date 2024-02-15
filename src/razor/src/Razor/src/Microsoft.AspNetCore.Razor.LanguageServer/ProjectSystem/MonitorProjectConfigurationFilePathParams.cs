// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class MonitorProjectConfigurationFilePathParams
{
    public required string ProjectKeyId { get; set; }

    public required string? ConfigurationFilePath { get; set; }
}
