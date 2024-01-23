// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;

#pragma warning disable VSTHRD110 // Observe result of async calls

namespace Microsoft.CodeAnalysis.Remote.Razor;

public partial class OOPTagHelperResolverTest
{
    private class TestResolver(
        Workspace workspace,
        IErrorReporter errorReporter,
        ITelemetryReporter telemetryReporter)
        : OOPTagHelperResolver(workspace, errorReporter, telemetryReporter)
    {
        public Func<IProjectSnapshot, ValueTask<ImmutableArray<TagHelperDescriptor>>>? OnResolveOutOfProcess { get; init; }

        public Func<IProjectSnapshot, ValueTask<ImmutableArray<TagHelperDescriptor>>>? OnResolveInProcess { get; init; }

        protected override ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersOutOfProcessAsync(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            return OnResolveOutOfProcess?.Invoke(projectSnapshot)
                ?? new ValueTask<ImmutableArray<TagHelperDescriptor>>(default(ImmutableArray<TagHelperDescriptor>));
        }

        protected override ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersInProcessAsync(Project project, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            return OnResolveInProcess?.Invoke(projectSnapshot)
                ?? new ValueTask<ImmutableArray<TagHelperDescriptor>>(default(ImmutableArray<TagHelperDescriptor>));
        }

        public ImmutableArray<Checksum> PublicProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
            => ProduceChecksumsFromDelta(projectId, lastResultId, deltaResult);

        protected override ImmutableArray<Checksum> ProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
            => base.ProduceChecksumsFromDelta(projectId, lastResultId, deltaResult);
    }
}
