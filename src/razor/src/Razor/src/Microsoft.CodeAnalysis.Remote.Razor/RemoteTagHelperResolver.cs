// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class RemoteTagHelperResolver(ITelemetryReporter telemetryReporter)
{
    private readonly IFallbackProjectEngineFactory _fallbackFactory = new FallbackProjectEngineFactory();
    private readonly Dictionary<string, IProjectEngineFactory> _typeNameToFactoryMap = new(StringComparer.Ordinal);
    private readonly CompilationTagHelperResolver _compilationTagHelperResolver = new(telemetryReporter);

    public ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project workspaceProject,
        RazorConfiguration? configuration,
        string factoryTypeName,
        CancellationToken cancellationToken)
    {
        if (configuration is null)
        {
            return new(ImmutableArray<TagHelperDescriptor>.Empty);
        }

        return _compilationTagHelperResolver.GetTagHelpersAsync(
            workspaceProject,
            CreateProjectEngine(configuration, factoryTypeName),
            cancellationToken);
    }

    private RazorProjectEngine CreateProjectEngine(RazorConfiguration configuration, string factoryTypeName)
    {
        // If there's no factory to handle the configuration then fall back to a very basic configuration.
        //
        // This will stop a crash from happening in this case (misconfigured project), but will still make
        // it obvious to the user that something is wrong.

        IProjectEngineFactory factory;

        lock (_typeNameToFactoryMap)
        {
            if (!_typeNameToFactoryMap.TryGetValue(factoryTypeName, out factory))
            {
                factory = CreateFactory(factoryTypeName) ?? _fallbackFactory;
                _typeNameToFactoryMap.Add(factoryTypeName, factory);
            }
        }

        return factory.Create(configuration, RazorProjectFileSystem.Empty, static _ => { });

        static IProjectEngineFactory? CreateFactory(string factoryTypeName)
        {
            try
            {
                var factoryType = Type.GetType(factoryTypeName, throwOnError: true);
                return (IProjectEngineFactory)Activator.CreateInstance(factoryType);
            }
            catch
            {
                return null;
            }
        }
    }
}
