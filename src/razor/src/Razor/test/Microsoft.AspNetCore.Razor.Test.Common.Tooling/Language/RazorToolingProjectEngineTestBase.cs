// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract class RazorToolingProjectEngineTestBase : ToolingTestBase
{
    protected abstract RazorLanguageVersion Version { get; }

    protected RazorToolingProjectEngineTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

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
