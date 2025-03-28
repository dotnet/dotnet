// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

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
