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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Remote;

namespace Microsoft.VisualStudio.Razor.Discovery;

public partial class OutOfProcTagHelperResolverTest
{
    private class TestResolver(
        IRemoteServiceInvoker remoteServiceInvoker,
        ILoggerFactory loggerFactory,
        ITelemetryReporter telemetryReporter)
        : OutOfProcTagHelperResolver(remoteServiceInvoker, loggerFactory, telemetryReporter)
    {
        public Func<ProjectSnapshot, ValueTask<ImmutableArray<TagHelperDescriptor>>>? OnResolveOutOfProcess { get; init; }

        public Func<ProjectSnapshot, ValueTask<ImmutableArray<TagHelperDescriptor>>>? OnResolveInProcess { get; init; }

        protected override ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersOutOfProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
            => OnResolveOutOfProcess?.Invoke(projectSnapshot) ?? default;

        protected override ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersInProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
            => OnResolveInProcess?.Invoke(projectSnapshot) ?? default;

        public ImmutableArray<Checksum> PublicProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
            => ProduceChecksumsFromDelta(projectId, lastResultId, deltaResult);
    }
}
