// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract partial class ProjectSnapshotManagerBenchmarkBase
{
    private class StaticProjectEngineFactoryProvider : IProjectEngineFactoryProvider
    {
        public static readonly StaticProjectEngineFactoryProvider Instance = new();

        private StaticProjectEngineFactoryProvider()
        {
        }

        public IProjectEngineFactory GetFactory(RazorConfiguration configuration)
            => Factory.Instance;

        private sealed class Factory : IProjectEngineFactory
        {
            public static readonly Factory Instance = new();

            private Factory()
            {
            }

            public string ConfigurationName => "Static";

            public RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder>? configure)
                => RazorProjectEngine.Create(configuration, fileSystem, RazorExtensions.Register);
        }
    }
}
