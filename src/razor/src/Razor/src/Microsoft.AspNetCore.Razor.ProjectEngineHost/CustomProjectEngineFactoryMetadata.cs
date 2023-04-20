// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal class CustomProjectEngineFactoryMetadata : ICustomProjectEngineFactoryMetadata
{
    public CustomProjectEngineFactoryMetadata(string configurationName)
    {
        ConfigurationName = configurationName ?? throw new ArgumentNullException(nameof(configurationName));
    }

    public string ConfigurationName { get; }

    public bool SupportsSerialization { get; set; }
}
