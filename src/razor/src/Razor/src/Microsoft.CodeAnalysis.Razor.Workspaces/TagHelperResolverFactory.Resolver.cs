// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor;

internal partial class TagHelperResolverFactory
{
    private sealed class Resolver(ITelemetryReporter telemetryReporter) : ITagHelperResolver
    {
        private readonly CompilationTagHelperResolver _resolver = new(telemetryReporter);

        public ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
            Project workspaceProject,
            IProjectSnapshot projectSnapshot,
            CancellationToken cancellationToken)
            => projectSnapshot.Configuration is not null
                ? _resolver.GetTagHelpersAsync(workspaceProject, projectSnapshot.GetProjectEngine(), cancellationToken)
                : new(ImmutableArray<TagHelperDescriptor>.Empty);
    }
}
