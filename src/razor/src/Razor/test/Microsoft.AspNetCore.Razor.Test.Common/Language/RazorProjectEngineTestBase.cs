// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract class RazorProjectEngineTestBase : TestBase
{
    protected abstract RazorLanguageVersion Version { get; }

    protected RazorProjectEngineTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    protected virtual void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
    }

    protected RazorEngine CreateEngine() => CreateProjectEngine().Engine;

    protected RazorProjectEngine CreateProjectEngine()
    {
        var configuration = RazorConfiguration.Create(Version, "test", Array.Empty<RazorExtension>());
        return RazorProjectEngine.Create(configuration, RazorProjectFileSystem.Empty, ConfigureProjectEngine);
    }

    protected RazorProjectEngine CreateProjectEngine(Action<RazorProjectEngineBuilder> configure)
    {
        var configuration = RazorConfiguration.Create(Version, "test", Array.Empty<RazorExtension>());
        return RazorProjectEngine.Create(configuration, RazorProjectFileSystem.Empty, b =>
        {
            ConfigureProjectEngine(b);
            configure?.Invoke(b);
        });
    }
}
