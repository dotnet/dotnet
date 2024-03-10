// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract class RazorProjectEngineTestBase
{
    protected abstract RazorLanguageVersion Version { get; }

    protected virtual void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
    }

    protected RazorEngine CreateEngine() => CreateProjectEngine().Engine;

    protected RazorProjectEngine CreateProjectEngine()
    {
        var configuration = new RazorConfiguration(Version, "test", Extensions: []);
        return RazorProjectEngine.Create(configuration, RazorProjectFileSystem.Empty, ConfigureProjectEngine);
    }

    protected RazorProjectEngine CreateProjectEngine(Action<RazorProjectEngineBuilder> configure)
    {
        var configuration = new RazorConfiguration(Version, "test", Extensions: []);
        return RazorProjectEngine.Create(configuration, RazorProjectFileSystem.Empty, b =>
        {
            ConfigureProjectEngine(b);
            configure?.Invoke(b);
        });
    }
}
