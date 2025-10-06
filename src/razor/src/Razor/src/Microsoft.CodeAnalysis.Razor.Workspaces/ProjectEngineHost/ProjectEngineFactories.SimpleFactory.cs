// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

internal static partial class ProjectEngineFactories
{
    private sealed class SimpleFactory(string configurationName) : IProjectEngineFactory
    {
        public string ConfigurationName => configurationName;

        public RazorProjectEngine Create(
            RazorConfiguration configuration,
            RazorProjectFileSystem fileSystem,
            Action<RazorProjectEngineBuilder>? configure)
            => RazorProjectEngine.Create(configuration, fileSystem, builder =>
            {
                Debug.Assert(configuration.ConfigurationName == ConfigurationName);

                CompilerFeatures.Register(builder);
                builder.RegisterExtensions();
                configure?.Invoke(builder);
            });
    }
}
