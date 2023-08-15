// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteTagHelperProviderService : RazorServiceBase, IRemoteTagHelperProviderService
{
    private readonly RemoteTagHelperResolver _tagHelperResolver;
    private readonly RemoteTagHelperDeltaProvider _tagHelperDeltaProvider;

    internal RemoteTagHelperProviderService(IServiceBroker serviceBroker, ITelemetryReporter telemetryReporter)
        : base(serviceBroker)
    {
        _tagHelperResolver = new RemoteTagHelperResolver(telemetryReporter);
        _tagHelperDeltaProvider = new RemoteTagHelperDeltaProvider();
    }

    public ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        ProjectSnapshotHandle projectHandle,
        string factoryTypeName,
        int lastResultId,
        CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => GetTagHelpersDeltaCoreAsync(solution, projectHandle, factoryTypeName, lastResultId, cancellationToken),
            cancellationToken);

    private async ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaCoreAsync(
        Solution solution,
        ProjectSnapshotHandle projectHandle,
        string factoryTypeName,
        int lastResultId,
        CancellationToken cancellationToken)
    {
        var currentTagHelpers = solution.GetProject(projectHandle.ProjectId) is Project workspaceProject
            ? await _tagHelperResolver.GetTagHelpersAsync(workspaceProject, projectHandle.Configuration, factoryTypeName, cancellationToken).ConfigureAwait(false)
            : ImmutableArray<TagHelperDescriptor>.Empty;

        return _tagHelperDeltaProvider.GetTagHelpersDelta(projectHandle.ProjectId, lastResultId, currentTagHelpers);
    }
}
