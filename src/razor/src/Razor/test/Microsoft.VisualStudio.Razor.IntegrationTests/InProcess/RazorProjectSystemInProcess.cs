// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Microsoft.VisualStudio.Razor.LanguageClient;
using Xunit;

namespace Microsoft.VisualStudio.Extensibility.Testing;

[TestService]
internal partial class RazorProjectSystemInProcess
{
    public async Task WaitForLSPServerActivatedAsync(CancellationToken cancellationToken)
    {
        await WaitForLSPServerActivationStatusAsync(active: true, cancellationToken);
    }

    public async Task WaitForLSPServerDeactivatedAsync(CancellationToken cancellationToken)
    {
        await WaitForLSPServerActivationStatusAsync(active: false, cancellationToken);
    }

    private async Task WaitForLSPServerActivationStatusAsync(bool active, CancellationToken cancellationToken)
    {
        var tracker = await TestServices.Shell.GetComponentModelServiceAsync<ILspServerActivationTracker>(cancellationToken);
        await Helper.RetryAsync(ct =>
        {
            return Task.FromResult(tracker.IsActive == active);
        }, TimeSpan.FromMilliseconds(50), cancellationToken);
    }

    public async Task WaitForProjectFileAsync(string projectFilePath, CancellationToken cancellationToken)
    {
        var projectManager = await TestServices.Shell.GetComponentModelServiceAsync<ProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectManager);
        await Helper.RetryAsync(ct =>
        {
            var projectKeys = projectManager.GetProjectKeysWithFilePath(projectFilePath);

            return projectKeys is [var projectKey, ..] && projectManager.ContainsProject(projectKey)
                ? SpecializedTasks.True
                : SpecializedTasks.False;
        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }

    public async Task WaitForComponentTagNameAsync(string projectName, string componentName, CancellationToken cancellationToken)
    {
        var projectFilePath = await TestServices.SolutionExplorer.GetProjectFileNameAsync(projectName, cancellationToken);
        var projectManager = await TestServices.Shell.GetComponentModelServiceAsync<ProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectManager);
        await Helper.RetryAsync(async ct =>
        {
            var projectKeys = projectManager.GetProjectKeysWithFilePath(projectFilePath);

            if (projectKeys is [var projectKey, ..] && projectManager.TryGetProject(projectKey, out var project))
            {
                var tagHelpers = await project.GetTagHelpersAsync(cancellationToken);
                return tagHelpers.Any(tagHelper => tagHelper.TagMatchingRules.Any(r => r.TagName.Equals(componentName, StringComparison.Ordinal)));
            }

            return false;
        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }

    public async Task WaitForRazorFileInProjectAsync(string projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        var projectManager = await TestServices.Shell.GetComponentModelServiceAsync<ProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectManager);

        await Helper.RetryAsync(ct =>
        {
            var projectKeys = projectManager.GetProjectKeysWithFilePath(projectFilePath);

            return projectKeys is [var projectKey, ..] && projectManager.ContainsDocument(projectKey, filePath)
                ? SpecializedTasks.True
                : SpecializedTasks.False;
        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }

    public async Task<ImmutableArray<string>> GetProjectKeyIdsForProjectAsync(string projectFilePath, CancellationToken cancellationToken)
    {
        var projectManager = await TestServices.Shell.GetComponentModelServiceAsync<ProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectManager);

        return projectManager.GetProjectKeysWithFilePath(projectFilePath).SelectAsArray(static key => key.Id);
    }

    public async Task WaitForCSharpVirtualDocumentAsync(string razorFilePath, CancellationToken cancellationToken)
    {
        var documentManager = await TestServices.Shell.GetComponentModelServiceAsync<LSPDocumentManager>(cancellationToken);

        var uri = new Uri(razorFilePath, UriKind.Absolute);
        await Helper.RetryAsync(ct =>
        {
            if (documentManager.TryGetDocument(uri, out var snapshot))
            {
                if (snapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocument))
                {
                    var result = !virtualDocument.ProjectKey.IsUnknown &&
                        virtualDocument.Snapshot.Length > 0;
                    return Task.FromResult(result);
                }
            }

            return SpecializedTasks.False;

        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }

    public async Task WaitForCSharpVirtualDocumentUpdateAsync(string projectName, string relativeFilePath, Func<Task> updater, CancellationToken cancellationToken)
    {
        var filePath = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(projectName, relativeFilePath, cancellationToken);

        var documentManager = await TestServices.Shell.GetComponentModelServiceAsync<LSPDocumentManager>(cancellationToken);

        var uri = new Uri(filePath, UriKind.Absolute);

        long? desiredVersion = null;

        await Helper.RetryAsync(async ct =>
        {
            if (documentManager.TryGetDocument(uri, out var snapshot))
            {
                if (snapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocument))
                {
                    if (!virtualDocument.ProjectKey.IsUnknown &&
                        virtualDocument.Snapshot.Length > 0)
                    {
                        if (desiredVersion is null)
                        {
                            desiredVersion = virtualDocument.HostDocumentSyncVersion + 1;
                            await updater();
                        }
                        else if (virtualDocument.HostDocumentSyncVersion == desiredVersion)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            return false;

        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }
}
