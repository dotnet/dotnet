// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal class ProjectEngineFactoryProvider(ImmutableArray<IProjectEngineFactory> factories) : IProjectEngineFactoryProvider
{
    public IProjectEngineFactory GetFactory(RazorConfiguration configuration)
    {
        foreach (var factory in factories)
        {
            if (factory.ConfigurationName == configuration.ConfigurationName)
            {
                return factory;
            }
        }

        return ProjectEngineFactories.Empty;
    }
}
