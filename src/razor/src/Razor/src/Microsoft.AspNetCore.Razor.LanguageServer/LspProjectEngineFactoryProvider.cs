// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// In the language server, we provide an <see cref="IProjectEngineFactoryProvider"/> that wraps
/// <see cref="ProjectEngineFactories.DefaultProvider"/> and configure every <see cref="RazorProjectEngine"/>
/// with the current code-gen options.
/// </summary>
internal sealed class LspProjectEngineFactoryProvider(RazorLSPOptionsMonitor optionsMonitor) : IProjectEngineFactoryProvider
{
    public IProjectEngineFactory GetFactory(RazorConfiguration configuration)
    {
        var factory = ProjectEngineFactories.DefaultProvider.GetFactory(configuration);

        return new Factory(factory, optionsMonitor);
    }

    private class Factory(IProjectEngineFactory innerFactory, RazorLSPOptionsMonitor optionsMonitor) : IProjectEngineFactory
    {
        public string ConfigurationName => innerFactory.ConfigurationName;

        public RazorProjectEngine Create(
            RazorConfiguration configuration,
            RazorProjectFileSystem fileSystem,
            Action<RazorProjectEngineBuilder>? configure)
        {
            if (fileSystem is not DefaultRazorProjectFileSystem defaultFileSystem)
            {
                throw new InvalidOperationException("Unexpected file system.");
            }

            var remoteFileSystem = new RemoteRazorProjectFileSystem(defaultFileSystem.Root);
            return innerFactory.Create(configuration, remoteFileSystem, Configure);

            void Configure(RazorProjectEngineBuilder builder)
            {
                configure?.Invoke(builder);

                builder.ConfigureCodeGenerationOptions(builder =>
                {
                    // We don't need to explicitly subscribe to options changing because this method will be run on every parse.
                    var currentOptions = optionsMonitor.CurrentValue;

                    builder.IndentSize = currentOptions.TabSize;
                    builder.IndentWithTabs = !currentOptions.InsertSpaces;
                    builder.RemapLinePragmaPathsOnWindows = true;
                });
            }
        }
    }
}
