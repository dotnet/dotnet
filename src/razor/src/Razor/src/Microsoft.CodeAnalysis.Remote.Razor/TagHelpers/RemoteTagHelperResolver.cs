// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.Telemetry;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(RemoteTagHelperResolver)), Shared]
[method: ImportingConstructor]
internal class RemoteTagHelperResolver(ITelemetryReporter telemetryReporter)
{
    /// <summary>
    /// A map of configuration names to <see cref="IProjectEngineFactory"/> instances.
    /// </summary>
    private static readonly Dictionary<string, IProjectEngineFactory> s_configurationNameToFactoryMap = CreateConfigurationNameToFactoryMap();

    private readonly CompilationTagHelperResolver _compilationTagHelperResolver = new(telemetryReporter);

    private static Dictionary<string, IProjectEngineFactory> CreateConfigurationNameToFactoryMap()
    {
        var map = new Dictionary<string, IProjectEngineFactory>(StringComparer.Ordinal);

        foreach (var factory in ProjectEngineFactories.All)
        {
            map.Add(factory.ConfigurationName, factory);
        }

        return map;
    }

    public ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project workspaceProject,
        RazorConfiguration? configuration,
        CancellationToken cancellationToken)
    {
        if (configuration is null)
        {
            return new(ImmutableArray<TagHelperDescriptor>.Empty);
        }

        return _compilationTagHelperResolver.GetTagHelpersAsync(
            workspaceProject,
            CreateProjectEngine(configuration),
            cancellationToken);
    }

    private RazorProjectEngine CreateProjectEngine(RazorConfiguration configuration)
    {
        // If there's no factory to handle the configuration then fall back to a very basic configuration.
        //
        // This will stop a crash from happening in this case (misconfigured project), but will still make
        // it obvious to the user that something is wrong.

        IProjectEngineFactory factory;

        lock (s_configurationNameToFactoryMap)
        {
            if (!s_configurationNameToFactoryMap.TryGetValue(configuration.ConfigurationName, out factory))
            {
                factory = ProjectEngineFactories.Empty;
                s_configurationNameToFactoryMap.Add(configuration.ConfigurationName, factory);
            }
        }

        return factory.Create(configuration, RazorProjectFileSystem.Empty, configure: null);
    }
}
