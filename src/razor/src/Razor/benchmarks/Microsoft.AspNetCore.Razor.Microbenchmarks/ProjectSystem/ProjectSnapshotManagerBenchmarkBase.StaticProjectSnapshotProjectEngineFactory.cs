// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

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
