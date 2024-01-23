// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal class TestProjectSnapshotProjectEngineFactory : ProjectSnapshotProjectEngineFactory
{
    public Action<RazorProjectEngineBuilder>? Configure { get; set; }

    public RazorProjectEngine? Engine { get; set; }

    public override RazorProjectEngine Create(
        RazorConfiguration configuration,
        RazorProjectFileSystem fileSystem,
        Action<RazorProjectEngineBuilder>? configure)
    {
        return Engine ?? RazorProjectEngine.Create(configuration, fileSystem, b =>
        {
            configure?.Invoke(b);
            Configure?.Invoke(b);
        });
    }

    public override IProjectEngineFactory FindFactory(IProjectSnapshot project)
    {
        throw new NotImplementedException();
    }

    public override IProjectEngineFactory FindSerializableFactory(IProjectSnapshot project)
    {
        throw new NotImplementedException();
    }
}
