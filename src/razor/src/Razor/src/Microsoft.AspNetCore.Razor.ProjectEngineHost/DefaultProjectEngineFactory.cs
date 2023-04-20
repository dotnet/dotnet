// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal class DefaultProjectEngineFactory : IProjectEngineFactory
{
    internal static RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure, IProjectEngineFactory fallback, (IProjectEngineFactory, ICustomProjectEngineFactoryMetadata)[] factories)
    {
        var factoryToUse = fallback;
        foreach (var (factory, metadata) in factories)
        {
            if (configuration.ConfigurationName == metadata.ConfigurationName)
            {
                factoryToUse = factory;
            }
        }

        return factoryToUse.Create(configuration, fileSystem, configure);
    }

    public RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure)
    {
        return RazorProjectEngine.Create(configuration, fileSystem, b =>
        {
            CompilerFeatures.Register(b);

            configure?.Invoke(b);
        });
    }
}
