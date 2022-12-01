// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class TestProjectSnapshotProjectEngineFactory : ProjectSnapshotProjectEngineFactory
{
    public Action<RazorProjectEngineBuilder> Configure { get; set; }

    public RazorProjectEngine Engine { get; set; }

    public override RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure)
    {
        return Engine ?? RazorProjectEngine.Create(configuration, fileSystem, b =>
        {
            configure(b);
            Configure(b);
        });
    }

    public override IProjectEngineFactory FindFactory(ProjectSnapshot project)
    {
        throw new NotImplementedException();
    }

    public override IProjectEngineFactory FindSerializableFactory(ProjectSnapshot project)
    {
        throw new NotImplementedException();
    }
}
