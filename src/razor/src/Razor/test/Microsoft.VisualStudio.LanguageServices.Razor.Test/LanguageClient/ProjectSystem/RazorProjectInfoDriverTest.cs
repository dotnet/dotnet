// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;

public class RazorProjectInfoDriverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly HostProject s_hostProject1 = new(
        filePath: "C:/path/to/project1/project1.csproj",
        intermediateOutputPath: "C:/path/to/project1/obj",
        configuration: RazorConfiguration.Default,
        rootNamespace: "TestNamespace");

    private static readonly HostDocument s_hostDocument1 = new("C:/path/to/project1/file.razor", "file.razor");

    private static readonly HostProject s_hostProject2 = new(
        filePath: "C:/path/to/project2/project2.csproj",
        intermediateOutputPath: "C:/path/to/project2/obj",
        configuration: RazorConfiguration.Default,
        rootNamespace: "TestNamespace");

    private static readonly HostDocument s_hostDocument2 = new("C:/path/to/project2/file.razor", "file.razor");

    [UIFact]
    public async Task ProcessesExistingProjectsDuringInitialization()
    {
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectAdded(s_hostProject1);
            updater.DocumentAdded(s_hostProject1.Key, s_hostDocument1, TestMocks.CreateTextLoader(s_hostDocument1.FilePath, "<p>Hello World</p>"));

            updater.ProjectAdded(s_hostProject2);
            updater.DocumentAdded(s_hostProject2.Key, s_hostDocument2, TestMocks.CreateTextLoader(s_hostDocument2.FilePath, "<p>Hello World</p>"));
        });

        var (driver, testAccessor) = await CreateDriverAndInitializeAsync(projectManager);

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        var latestProjects = driver.GetLatestProjectInfo();

        // The misc files projects project should be present.
        Assert.Contains(latestProjects, x => x.ProjectKey == MiscFilesHostProject.Instance.Key);

        // Sort the remaining projects by project key.
        var projects = latestProjects
            .WhereAsArray(x => x.ProjectKey != MiscFilesHostProject.Instance.Key)
            .Sort((x, y) => x.ProjectKey.Id.CompareTo(y.ProjectKey.Id));

        Assert.Equal(2, projects.Length);

        var projectInfo1 = projects[0];
        Assert.Equal(s_hostProject1.Key, projectInfo1.ProjectKey);
        var document1 = Assert.Single(projectInfo1.Documents);
        Assert.Equal(s_hostDocument1.FilePath, document1.FilePath);

        var projectInfo2 = projects[1];
        Assert.Equal(s_hostProject2.Key, projectInfo2.ProjectKey);
        var document2 = Assert.Single(projectInfo2.Documents);
        Assert.Equal(s_hostDocument2.FilePath, document2.FilePath);
    }

    [UIFact]
    public async Task ProcessesProjectsAddedAfterInitialization()
    {
        var projectManager = CreateProjectSnapshotManager();

        var (driver, testAccessor) = await CreateDriverAndInitializeAsync(projectManager);

        // The misc files projects project should be present after initialization.
        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        var initialProjects = driver.GetLatestProjectInfo();

        var miscFilesProject = Assert.Single(initialProjects);
        Assert.Equal(MiscFilesHostProject.Instance.Key, miscFilesProject.ProjectKey);

        // Now add some projects
        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectAdded(s_hostProject1);
            updater.DocumentAdded(s_hostProject1.Key, s_hostDocument1, TestMocks.CreateTextLoader(s_hostDocument1.FilePath, "<p>Hello World</p>"));

            updater.ProjectAdded(s_hostProject2);
            updater.DocumentAdded(s_hostProject2.Key, s_hostDocument2, TestMocks.CreateTextLoader(s_hostDocument2.FilePath, "<p>Hello World</p>"));
        });

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Sort the non-misc files projects by project key.
        var projects = driver
            .GetLatestProjectInfo()
            .WhereAsArray(x => x.ProjectKey != MiscFilesHostProject.Instance.Key)
            .Sort((x, y) => x.ProjectKey.Id.CompareTo(y.ProjectKey.Id));

        Assert.Equal(2, projects.Length);

        var projectInfo1 = projects[0];
        Assert.Equal(s_hostProject1.Key, projectInfo1.ProjectKey);
        var document1 = Assert.Single(projectInfo1.Documents);
        Assert.Equal(s_hostDocument1.FilePath, document1.FilePath);

        var projectInfo2 = projects[1];
        Assert.Equal(s_hostProject2.Key, projectInfo2.ProjectKey);
        var document2 = Assert.Single(projectInfo2.Documents);
        Assert.Equal(s_hostDocument2.FilePath, document2.FilePath);
    }

    [UIFact]
    public async Task ProcessesDocumentAddedAfterInitialization()
    {
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectAdded(s_hostProject1);
        });

        var (driver, testAccessor) = await CreateDriverAndInitializeAsync(projectManager);

        await projectManager.UpdateAsync(static updater =>
        {
            updater.DocumentAdded(s_hostProject1.Key, s_hostDocument1, TestMocks.CreateTextLoader(s_hostDocument1.FilePath, "<p>Hello World</p>"));
        });

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Sort the non-misc files projects by project key.
        var projects = driver
            .GetLatestProjectInfo()
            .WhereAsArray(x => x.ProjectKey != MiscFilesHostProject.Instance.Key)
            .Sort((x, y) => x.ProjectKey.Id.CompareTo(y.ProjectKey.Id));

        var projectInfo1 = Assert.Single(projects);
        Assert.Equal(s_hostProject1.Key, projectInfo1.ProjectKey);
        var document1 = Assert.Single(projectInfo1.Documents);
        Assert.Equal(s_hostDocument1.FilePath, document1.FilePath);
    }

    [UIFact]
    public async Task ProcessesProjectRemovedAfterInitialization()
    {
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectAdded(s_hostProject1);
        });

        var (driver, testAccessor) = await CreateDriverAndInitializeAsync(projectManager);

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Sort the non-misc files projects by project key.
        var projects = driver
            .GetLatestProjectInfo()
            .WhereAsArray(x => x.ProjectKey != MiscFilesHostProject.Instance.Key)
            .Sort((x, y) => x.ProjectKey.Id.CompareTo(y.ProjectKey.Id));

        var projectInfo1 = Assert.Single(projects);
        Assert.Equal(s_hostProject1.Key, projectInfo1.ProjectKey);

        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectRemoved(s_hostProject1.Key);
        });

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        var miscFilesProject = Assert.Single(driver.GetLatestProjectInfo());
        Assert.Equal(MiscFilesHostProject.Instance.Key, miscFilesProject.ProjectKey);
    }

    [UIFact]
    public async Task ListenerNotifiedOfUpdates()
    {
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectAdded(s_hostProject1);
        });

        var (driver, testAccessor) = await CreateDriverAndInitializeAsync(projectManager);

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        var listener = new TestListener();
        driver.AddListener(listener);

        await projectManager.UpdateAsync(static updater =>
        {
            updater.DocumentAdded(s_hostProject1.Key, s_hostDocument1, TestMocks.CreateTextLoader(s_hostDocument1.FilePath, "<p>Hello World</p>"));
        });

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        Assert.Empty(listener.Removes);

        var projectInfo1 = Assert.Single(listener.Updates);
        Assert.Equal(s_hostProject1.Key, projectInfo1.ProjectKey);
        var document1 = Assert.Single(projectInfo1.Documents);
        Assert.Equal(s_hostDocument1.FilePath, document1.FilePath);
    }

    [UIFact]
    public async Task ListenerNotifiedOfRemoves()
    {
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectAdded(s_hostProject1);
        });

        var (driver, testAccessor) = await CreateDriverAndInitializeAsync(projectManager);

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        var listener = new TestListener();
        driver.AddListener(listener);

        await projectManager.UpdateAsync(static updater =>
        {
            updater.ProjectRemoved(s_hostProject1.Key);
        });

        await testAccessor.WaitUntilCurrentBatchCompletesAsync();

        var projectKey1 = Assert.Single(listener.Removes);
        Assert.Equal(s_hostProject1.Key, projectKey1);

        Assert.Empty(listener.Updates);
    }

    private async Task<(RazorProjectInfoDriver, AbstractRazorProjectInfoDriver.TestAccessor)> CreateDriverAndInitializeAsync(
        IProjectSnapshotManager projectManager)
    {
        var driver = new RazorProjectInfoDriver(projectManager, LoggerFactory, delay: TimeSpan.FromMilliseconds(5));
        AddDisposable(driver);

        var testAccessor = driver.GetTestAccessor();

        await driver.WaitForInitializationAsync();

        return (driver, testAccessor);
    }

    private sealed class TestListener : IRazorProjectInfoListener
    {
        private readonly ImmutableArray<ProjectKey>.Builder _removes = ImmutableArray.CreateBuilder<ProjectKey>();
        private readonly ImmutableArray<RazorProjectInfo>.Builder _updates = ImmutableArray.CreateBuilder<RazorProjectInfo>();

        public ImmutableArray<ProjectKey> Removes => _removes.ToImmutable();
        public ImmutableArray<RazorProjectInfo> Updates => _updates.ToImmutable();

        public Task RemovedAsync(ProjectKey projectKey, CancellationToken cancellationToken)
        {
            _removes.Add(projectKey);
            return Task.CompletedTask;
        }

        public Task UpdatedAsync(RazorProjectInfo projectInfo, CancellationToken cancellationToken)
        {
            _updates.Add(projectInfo);
            return Task.CompletedTask;
        }
    }
}
