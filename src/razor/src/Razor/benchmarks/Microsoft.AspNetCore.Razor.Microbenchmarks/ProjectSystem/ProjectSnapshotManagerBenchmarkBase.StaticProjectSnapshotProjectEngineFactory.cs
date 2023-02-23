// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract partial class ProjectSnapshotManagerBenchmarkBase
{
    private class StaticProjectSnapshotProjectEngineFactory : ProjectSnapshotProjectEngineFactory
    {
        public override IProjectEngineFactory FindFactory(IProjectSnapshot project)
            => throw new NotImplementedException();

        public override IProjectEngineFactory FindSerializableFactory(IProjectSnapshot project)
            => throw new NotImplementedException();

        public override RazorProjectEngine Create(
            RazorConfiguration configuration,
            RazorProjectFileSystem fileSystem,
            Action<RazorProjectEngineBuilder> configure)
            => RazorProjectEngine.Create(configuration, fileSystem, RazorExtensions.Register);
    }
}
