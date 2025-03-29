// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal sealed partial class ProjectEngineFactory(string configurationName) : IProjectEngineFactory
{
    public string ConfigurationName => configurationName;

    public RazorProjectEngine Create(
        RazorConfiguration configuration,
        RazorProjectFileSystem fileSystem,
        Action<RazorProjectEngineBuilder>? configure)
        => RazorProjectEngine.Create(configuration, fileSystem, builder =>
        {
            CompilerFeatures.Register(builder);
            builder.RegisterExtensions();
            configure?.Invoke(builder);
        });
}
