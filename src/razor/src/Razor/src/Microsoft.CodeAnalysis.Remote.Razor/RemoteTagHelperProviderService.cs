// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteTagHelperProviderService : RazorServiceBase, IRemoteTagHelperProviderService
{
    private readonly RemoteTagHelperDeltaProvider _tagHelperDeltaProvider;

    internal RemoteTagHelperProviderService(IServiceBroker serviceBroker, ITelemetryReporter telemetryReporter)
        : base(serviceBroker, telemetryReporter)
    {
        _tagHelperDeltaProvider = new RemoteTagHelperDeltaProvider();
    }

    public ValueTask<TagHelperResolutionResult> GetTagHelpersAsync(RazorPinnedSolutionInfoWrapper solutionInfo, ProjectSnapshotHandle projectHandle, string factoryTypeName, CancellationToken cancellationToken = default)
        => RazorBrokeredServiceImplementation.RunServiceAsync(cancellationToken => GetTagHelpersCoreAsync(solutionInfo, projectHandle, factoryTypeName, cancellationToken), cancellationToken);

    public ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaAsync(RazorPinnedSolutionInfoWrapper solutionInfo, ProjectSnapshotHandle projectHandle, string? factoryTypeName, int lastResultId, CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(cancellationToken => GetTagHelpersDeltaCoreAsync(solutionInfo, projectHandle, factoryTypeName, lastResultId, cancellationToken), cancellationToken);

    private async ValueTask<TagHelperResolutionResult> GetTagHelpersCoreAsync(RazorPinnedSolutionInfoWrapper solutionInfo, ProjectSnapshotHandle projectHandle, string? factoryTypeName, CancellationToken cancellationToken)
    {
        if (projectHandle is null)
        {
            throw new ArgumentNullException(nameof(projectHandle));
        }

        if (string.IsNullOrEmpty(factoryTypeName))
        {
            throw new ArgumentException($"'{nameof(factoryTypeName)}' cannot be null or empty.", nameof(factoryTypeName));
        }

        // We should replace the below call: https://github.com/dotnet/razor-tooling/issues/6316
#pragma warning disable CS0618 // Type or member is obsolete
        var solution = await solutionInfo.GetSolutionAsync(ServiceBrokerClient, cancellationToken).ConfigureAwait(false);
#pragma warning restore CS0618 // Type or member is obsolete
        var projectSnapshot = await GetProjectSnapshotAsync(projectHandle, cancellationToken).ConfigureAwait(false);
        var workspaceProject = solution
            .Projects
            .FirstOrDefault(project => FilePathComparer.Instance.Equals(project.FilePath, projectSnapshot.FilePath));

        if (workspaceProject is null)
        {
            return TagHelperResolutionResult.Empty;
        }

        var resolutionResult = await RazorServices.TagHelperResolver.GetTagHelpersAsync(workspaceProject, projectHandle.Configuration, factoryTypeName, cancellationToken).ConfigureAwait(false);
        return resolutionResult;
    }

    public async ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaCoreAsync(RazorPinnedSolutionInfoWrapper solutionInfo, ProjectSnapshotHandle projectHandle, string? factoryTypeName, int lastResultId, CancellationToken cancellationToken)
    {
        var tagHelperResolutionResult = await GetTagHelpersCoreAsync(solutionInfo, projectHandle, factoryTypeName, cancellationToken).ConfigureAwait(false);
        var currentTagHelpers = tagHelperResolutionResult.Descriptors ?? Array.Empty<TagHelperDescriptor>();
        var deltaResult = _tagHelperDeltaProvider.GetTagHelpersDelta(projectHandle.FilePath, lastResultId, currentTagHelpers);
        return deltaResult;
    }
}
