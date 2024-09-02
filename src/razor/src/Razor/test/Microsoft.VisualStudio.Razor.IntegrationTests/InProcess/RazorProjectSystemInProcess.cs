// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Xunit;
using Microsoft.VisualStudio.Razor.LanguageClient;
using Microsoft.AspNetCore.Razor.Threading;
using System.Collections.Immutable;
using System.Linq;

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
        var projectManager = await TestServices.Shell.GetComponentModelServiceAsync<IProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectManager);
        await Helper.RetryAsync(ct =>
        {
            var projectKeys = projectManager.GetAllProjectKeys(projectFilePath);
            if (projectKeys.Length == 0)
            {
                return SpecializedTasks.False;
            }

            return Task.FromResult(projectManager.TryGetLoadedProject(projectKeys[0], out _));
        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }

    public async Task WaitForComponentTagNameAsync(string projectName, string componentName, CancellationToken cancellationToken)
    {
        var projectFileName = await TestServices.SolutionExplorer.GetProjectFileNameAsync(projectName, cancellationToken);
        var projectManager = await TestServices.Shell.GetComponentModelServiceAsync<IProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectManager);
        await Helper.RetryAsync(async ct =>
        {
            var projectKeys = projectManager.GetAllProjectKeys(projectFileName);
            if (projectKeys.Length == 0)
            {
                return false;
            }

            if (!projectManager.TryGetLoadedProject(projectKeys[0], out var project))
            {
                return false;
            }

            var tagHelpers = await project.GetTagHelpersAsync(cancellationToken);
            return tagHelpers.Any(tagHelper => tagHelper.TagMatchingRules.Any(r => r.TagName.Equals(componentName, StringComparison.Ordinal)));
        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }

    public async Task WaitForRazorFileInProjectAsync(string projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        var projectSnapshotManager = await TestServices.Shell.GetComponentModelServiceAsync<IProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectSnapshotManager);
        await Helper.RetryAsync(ct =>
        {
            var projectKeys = projectSnapshotManager.GetAllProjectKeys(projectFilePath);
            if (projectKeys.Length == 0 ||
                !projectSnapshotManager.TryGetLoadedProject(projectKeys[0], out var project))
            {
                return SpecializedTasks.False;
            }

            var document = project.GetDocument(filePath);

            return Task.FromResult(document is not null);
        }, TimeSpan.FromMilliseconds(100), cancellationToken);
    }

    public async Task<ImmutableArray<string>> GetProjectKeyIdsForProjectAsync(string projectFilePath, CancellationToken cancellationToken)
    {
        var projectManager = await TestServices.Shell.GetComponentModelServiceAsync<IProjectSnapshotManager>(cancellationToken);
        Assert.NotNull(projectManager);

        var projectKeys = projectManager.GetAllProjectKeys(projectFilePath);

        return projectKeys.SelectAsArray(key => key.Id);
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
