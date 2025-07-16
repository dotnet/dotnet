// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

internal sealed class DefaultProjectEngineFactory : IProjectEngineFactory
{
    public string ConfigurationName => "Default";

    public RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder>? configure)
    {
        return RazorProjectEngine.Create(configuration, fileSystem, b =>
        {
            CompilerFeatures.Register(b);

            configure?.Invoke(b);
        });
    }
}
