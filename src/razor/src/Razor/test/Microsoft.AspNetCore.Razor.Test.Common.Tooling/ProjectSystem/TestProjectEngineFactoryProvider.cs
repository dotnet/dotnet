// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal class TestProjectEngineFactoryProvider : IProjectEngineFactoryProvider
{
    public static TestProjectEngineFactoryProvider Instance { get; } = new(baseProvider: null, configure: default);

    public IProjectEngineFactoryProvider BaseProvider { get; }
    private readonly ImmutableArray<Action<RazorProjectEngineBuilder>> _configure;

    private TestProjectEngineFactoryProvider(
        IProjectEngineFactoryProvider? baseProvider,
        ImmutableArray<Action<RazorProjectEngineBuilder>> configure)
    {
        BaseProvider = baseProvider ?? ProjectEngineFactories.DefaultProvider;
        _configure = configure.NullToEmpty();
    }

    public TestProjectEngineFactoryProvider WithBaseProvider(IProjectEngineFactoryProvider baseProvider)
        => new(baseProvider, _configure);

    public TestProjectEngineFactoryProvider AddConfigure(Action<RazorProjectEngineBuilder> configure)
        => new(BaseProvider, _configure.Add(configure));

    public IProjectEngineFactory GetFactory(RazorConfiguration configuration)
        => new FactoryWrapper(this, BaseProvider.GetFactory(configuration));

    private sealed class FactoryWrapper(
        TestProjectEngineFactoryProvider parent,
        IProjectEngineFactory factory)
        : IProjectEngineFactory
    {
        public string ConfigurationName => factory.ConfigurationName;

        public RazorProjectEngine Create(
            RazorConfiguration configuration,
            RazorProjectFileSystem fileSystem,
            Action<RazorProjectEngineBuilder>? configure)
        {
            return RazorProjectEngine.Create(configuration, fileSystem, b =>
            {
                configure?.Invoke(b);

                foreach (var c in parent._configure)
                {
                    c.Invoke(b);
                }
            });
        }
    }
}
