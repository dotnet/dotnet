// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Checksum = Microsoft.AspNetCore.Razor.Utilities.Checksum;

namespace Microsoft.CodeAnalysis.Remote.Razor;

public partial class OOPTagHelperResolverTest
{
    private class TestResolver(
        ProjectSnapshotProjectEngineFactory factory,
        IErrorReporter errorReporter,
        Workspace workspace,
        ITelemetryReporter telemetryReporter)
        : OOPTagHelperResolver(factory, errorReporter, workspace, telemetryReporter)
    {
        public Func<IProjectEngineFactory, IProjectSnapshot, ValueTask<ImmutableArray<TagHelperDescriptor>>>? OnResolveOutOfProcess { get; init; }

        public Func<IProjectSnapshot, ValueTask<ImmutableArray<TagHelperDescriptor>>>? OnResolveInProcess { get; init; }

        protected override ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersOutOfProcessAsync(IProjectEngineFactory factory, Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            Assert.NotNull(OnResolveOutOfProcess);
            return OnResolveOutOfProcess(factory, projectSnapshot);
        }

        protected override ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersInProcessAsync(Project project, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            Assert.NotNull(OnResolveInProcess);
            return OnResolveInProcess(projectSnapshot);
        }

        public ImmutableArray<Checksum> PublicProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
            => ProduceChecksumsFromDelta(projectId, lastResultId, deltaResult);

        protected override ImmutableArray<Checksum> ProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
            => base.ProduceChecksumsFromDelta(projectId, lastResultId, deltaResult);
    }
}
