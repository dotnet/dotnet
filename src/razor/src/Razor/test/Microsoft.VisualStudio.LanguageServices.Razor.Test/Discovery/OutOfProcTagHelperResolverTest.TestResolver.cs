// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Telemetry;

namespace Microsoft.VisualStudio.Razor.Discovery;

public partial class OutOfProcTagHelperResolverTest
{
    private class TestResolver(
        IRemoteServiceInvoker remoteServiceInvoker,
        ILoggerFactory loggerFactory,
        ITelemetryReporter telemetryReporter)
        : OutOfProcTagHelperResolver(remoteServiceInvoker, loggerFactory, telemetryReporter)
    {
        public Func<ProjectSnapshot, TagHelperCollection>? OnResolveOutOfProcess { get; init; }

        public Func<ProjectSnapshot, TagHelperCollection>? OnResolveInProcess { get; init; }

        protected override ValueTask<TagHelperCollection?> ResolveTagHelpersOutOfProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            var handler = OnResolveOutOfProcess;
            if (handler is not null)
            {
                return new(handler.Invoke(projectSnapshot));
            }

            return default;
        }

        protected override ValueTask<TagHelperCollection> ResolveTagHelpersInProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            var handler = OnResolveInProcess;
            if (handler is not null)
            {
                return new(handler.Invoke(projectSnapshot));
            }

            return default;
        }

        public ImmutableArray<Checksum> PublicProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
            => ProduceChecksumsFromDelta(projectId, lastResultId, deltaResult);
    }
}
