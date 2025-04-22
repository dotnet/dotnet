// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
