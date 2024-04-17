// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Test;

public class RazorProjectInfoPublisherTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/35945")]
    public async Task ProjectManager_Changed_Remove_Change_NoopsOnDelayedPublish()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var tagHelpers = ImmutableArray.Create(
            new TagHelperDescriptor(FileKinds.Component, "Namespace.FileNameOther", "Assembly", "FileName", "FileName document", "FileName hint",
                caseSensitive: false, tagMatchingRules: default, attributeDescriptors: default, allowedChildTags: default, metadata: null!, diagnostics: default));

        var initialProjectSnapshot = CreateProjectSnapshot(
            @"C:\path\to\project.csproj", ProjectWorkspaceState.Create(tagHelpers, CodeAnalysis.CSharp.LanguageVersion.Preview));
        var expectedProjectSnapshot = CreateProjectSnapshot(
            @"C:\path\to\project.csproj", ProjectWorkspaceState.Create(CodeAnalysis.CSharp.LanguageVersion.Preview));
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Same(expectedProjectSnapshot, snapshot);
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            })
        {
            EnqueueDelay = 10,
            _active = true,
        };

        projectConfigurationFilePathStore.Set(expectedProjectSnapshot.Key, expectedConfigurationFilePath);
        var documentRemovedArgs = ProjectChangeEventArgs.CreateTestInstance(
            initialProjectSnapshot, initialProjectSnapshot, documentFilePath: @"C:\path\to\file.razor", ProjectChangeKind.DocumentRemoved);
        var projectChangedArgs = ProjectChangeEventArgs.CreateTestInstance(
            initialProjectSnapshot, expectedProjectSnapshot, documentFilePath: null!, ProjectChangeKind.ProjectChanged);

        // Act
        publisher.ProjectManager_Changed(null!, documentRemovedArgs);
        publisher.ProjectManager_Changed(null!, projectChangedArgs);

        // Assert
        var stalePublishTask = Assert.Single(publisher.DeferredPublishTasks);
        await stalePublishTask.Value;
        Assert.True(serializationSuccessful);
    }

    [Fact]
    public async Task ProjectManager_Changed_NotActive_Noops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var attemptedToSerialize = false;
        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
        });

        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) => attemptedToSerialize = true)
        {
            EnqueueDelay = 10,
        };

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });

        // Assert
        Assert.Empty(publisher.DeferredPublishTasks);
        Assert.False(attemptedToSerialize);
    }

    [Fact]
    public async Task ProjectManager_Changed_DocumentOpened_UninitializedProject_NotActive_Noops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var attemptedToSerialize = false;
        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });

        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) => attemptedToSerialize = true)
        {
            EnqueueDelay = 10,
        };

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(hostProject.Key, hostDocument.FilePath, SourceText.From(string.Empty));
        });

        // Assert
        Assert.Empty(publisher.DeferredPublishTasks);
        Assert.False(attemptedToSerialize);
    }

    [Fact]
    public async Task ProjectManager_Changed_DocumentOpened_InitializedProject_NotActive_Publishes()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.ProjectWorkspaceStateChanged(hostProject.Key, ProjectWorkspaceState.Default);
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });

        var projectSnapshot = projectManager.GetProjects()[0];
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        projectConfigurationFilePathStore.Set(projectSnapshot.Key, expectedConfigurationFilePath);
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            })
        {
            EnqueueDelay = 10,
        };

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(hostProject.Key, hostDocument.FilePath, SourceText.From(string.Empty));
        });

        // Assert
        Assert.Empty(publisher.DeferredPublishTasks);
        Assert.True(serializationSuccessful);
    }

    [Fact]
    public async Task ProjectManager_Changed_DocumentOpened_InitializedProject_NoFile_Active_Publishes()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.ProjectWorkspaceStateChanged(hostProject.Key, ProjectWorkspaceState.Default);
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });

        var projectSnapshot = projectManager.GetProjects()[0];
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        projectConfigurationFilePathStore.Set(projectSnapshot.Key, expectedConfigurationFilePath);
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            },
            configurationFileExists: false)
        {
            EnqueueDelay = 10000, // Long enqueue delay to make sure this test doesn't pass due to slow running, but broken product code
            _active = true
        };

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(hostProject.Key, hostDocument.FilePath, SourceText.From(string.Empty));
        });

        // Assert
        Assert.Empty(publisher.DeferredPublishTasks);
        Assert.True(serializationSuccessful);
    }

    [Theory]
    [InlineData(ProjectChangeKind.DocumentAdded)]
    [InlineData(ProjectChangeKind.DocumentRemoved)]
    [InlineData(ProjectChangeKind.ProjectChanged)]
    internal async Task ProjectManager_Changed_EnqueuesPublishAsync(ProjectChangeKind changeKind)
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var projectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", ProjectWorkspaceState.Create(CodeAnalysis.CSharp.LanguageVersion.CSharp7_3));
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Same(projectSnapshot, snapshot);
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            })
        {
            EnqueueDelay = 10,
            _active = true,
        };

        projectConfigurationFilePathStore.Set(projectSnapshot.Key, expectedConfigurationFilePath);
        var args = ProjectChangeEventArgs.CreateTestInstance(projectSnapshot, projectSnapshot, documentFilePath: null!, changeKind);

        // Act
        publisher.ProjectManager_Changed(null!, args);

        // Assert
        var kvp = Assert.Single(publisher.DeferredPublishTasks);
        await kvp.Value;
        Assert.True(serializationSuccessful);
    }

    [Fact]
    internal async Task ProjectManager_ChangedTagHelpers_PublishesImmediately()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var projectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", ProjectWorkspaceState.Default);
        var changedProjectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", ProjectWorkspaceState.Create(CodeAnalysis.CSharp.LanguageVersion.CSharp8));
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        var aboutToChange = false;
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                if (!aboutToChange)
                {
                    return;
                }

                Assert.Same(changedProjectSnapshot, snapshot);
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            })
        {
            EnqueueDelay = 10,
            _active = true,
        };

        projectConfigurationFilePathStore.Set(projectSnapshot.Key, expectedConfigurationFilePath);
        var args = ProjectChangeEventArgs.CreateTestInstance(projectSnapshot, projectSnapshot, documentFilePath: null!, ProjectChangeKind.ProjectChanged);
        publisher.ProjectManager_Changed(null!, args);

        // Flush publish task
        var kvp = Assert.Single(publisher.DeferredPublishTasks);
        await kvp.Value;
        aboutToChange = true;
        publisher.DeferredPublishTasks.Clear();

        var changedTagHelpersArgs = ProjectChangeEventArgs.CreateTestInstance(
            projectSnapshot, changedProjectSnapshot, documentFilePath: null!, ProjectChangeKind.ProjectChanged);

        // Act
        publisher.ProjectManager_Changed(null!, changedTagHelpersArgs);

        // Assert
        Assert.Empty(publisher.DeferredPublishTasks);
        Assert.True(serializationSuccessful);
    }

    [Fact]
    public async Task ProjectManager_Changed_ProjectRemoved_AfterEnqueuedPublishAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var attemptedToSerialize = false;
        var projectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) => attemptedToSerialize = true)
        {
            EnqueueDelay = 10,
            _active = true,
        };

        projectConfigurationFilePathStore.Set(projectSnapshot.Key, expectedConfigurationFilePath);
        publisher.EnqueuePublish(projectSnapshot);
        var args = ProjectChangeEventArgs.CreateTestInstance(projectSnapshot, newer: null!, documentFilePath: null!, ProjectChangeKind.ProjectRemoved);

        // Act
        publisher.ProjectManager_Changed(null!, args);

        // Assert
        var kvp = Assert.Single(publisher.DeferredPublishTasks);
        await kvp.Value;

        Assert.False(attemptedToSerialize);
    }

    [Fact]
    public async Task EnqueuePublish_BatchesPublishRequestsAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var firstSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");
        var secondSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", [@"C:\path\to\file.cshtml"]);
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Same(secondSnapshot, snapshot);
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            })
        {
            EnqueueDelay = 10,
            _active = true,
        };

        projectConfigurationFilePathStore.Set(firstSnapshot.Key, expectedConfigurationFilePath);

        // Act
        publisher.EnqueuePublish(firstSnapshot);
        publisher.EnqueuePublish(secondSnapshot);

        // Assert
        var kvp = Assert.Single(publisher.DeferredPublishTasks);
        await kvp.Value;
        Assert.True(serializationSuccessful);
    }

    [Fact]
    public async Task EnqueuePublish_OnProjectWithoutRazor_Publishes()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var firstSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");
        var secondSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", [@"C:\path\to\file.cshtml"]);
        var expectedConfigurationFilePath = @"C:\path\to\objbin\Debug\project.razor.bin";
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Same(secondSnapshot, snapshot);
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            },
            useRealShouldSerialize: true)
        {
            EnqueueDelay = 10,
            _active = true,
        };

        projectConfigurationFilePathStore.Set(secondSnapshot.Key, expectedConfigurationFilePath);

        // Act
        publisher.EnqueuePublish(secondSnapshot);

        // Assert
        var kvp = Assert.Single(publisher.DeferredPublishTasks);
        await kvp.Value;
        Assert.True(serializationSuccessful);
    }

    [Fact]
    public async Task EnqueuePublish_OnProjectBeforeTagHelperProcessed_DoesNotPublish()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var firstSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");
        var tagHelpers = ImmutableArray.Create<TagHelperDescriptor>(
            new TagHelperDescriptor(FileKinds.Component, "Namespace.FileNameOther", "Assembly", "FileName", "FileName document", "FileName hint",
                caseSensitive: false, tagMatchingRules: default, attributeDescriptors: default, allowedChildTags: default, metadata: null!, diagnostics: default));

        var secondSnapshot = CreateProjectSnapshot(
            @"C:\path\to\project.csproj",
            ProjectWorkspaceState.Create(tagHelpers, CodeAnalysis.CSharp.LanguageVersion.CSharp8),
            ["FileName.razor"]);

        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Same(secondSnapshot, snapshot);
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            },
            useRealShouldSerialize: true)
        {
            EnqueueDelay = 10,
            _active = true,
        };

        projectConfigurationFilePathStore.Set(firstSnapshot.Key, expectedConfigurationFilePath);

        // Act
        publisher.EnqueuePublish(secondSnapshot);

        // Assert
        var kvp = Assert.Single(publisher.DeferredPublishTasks);
        await kvp.Value;
        Assert.False(serializationSuccessful);
    }

    [Fact]
    public void Publish_UnsetConfigurationFilePath_Noops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var publisher = new TestRazorProjectInfoPublisher(projectManager, projectConfigurationFilePathStore)
        {
            _active = true,
        };

        var omniSharpProjectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");

        // Act & Assert
        publisher.Publish(omniSharpProjectSnapshot);
    }

    [Fact]
    public void Publish_PublishesToSetPublishFilePath()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var omniSharpProjectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";
        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Same(omniSharpProjectSnapshot, snapshot);
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            })
        {
            _active = true,
        };

        projectConfigurationFilePathStore.Set(omniSharpProjectSnapshot.Key, expectedConfigurationFilePath);

        // Act
        publisher.Publish(omniSharpProjectSnapshot);

        // Assert
        Assert.True(serializationSuccessful);
    }

    [UIFact]
    public async Task ProjectAdded_PublishesToCorrectFilePathAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";

        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            })
        {
            _active = true,
        };

        var projectFilePath = @"C:\path\to\project.csproj";
        var hostProject = new HostProject(projectFilePath, Path.Combine(Path.GetDirectoryName(projectFilePath), "obj"), RazorConfiguration.Default, "TestRootNamespace");
        projectConfigurationFilePathStore.Set(hostProject.Key, expectedConfigurationFilePath);
        var projectWorkspaceState = ProjectWorkspaceState.Default;

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.ProjectWorkspaceStateChanged(hostProject.Key, projectWorkspaceState);
        });

        // Assert
        Assert.True(serializationSuccessful);
    }

    [UIFact]
    public async Task ProjectAdded_DoesNotPublishWithoutProjectWorkspaceStateAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";

        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Fail("Serialization should not have been attempted because there is no ProjectWorkspaceState.");
                serializationSuccessful = true;
            })
        {
            _active = true,
        };

        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, "TestRootNamespace");
        projectConfigurationFilePathStore.Set(hostProject.Key, expectedConfigurationFilePath);

        // Act
        await projectManager.UpdateAsync(
            updater => updater.ProjectAdded(hostProject));

        Assert.Empty(publisher.DeferredPublishTasks);

        // Assert
        Assert.False(serializationSuccessful);
    }

    [UIFact]
    public async Task ProjectRemoved_UnSetPublishFilePath_NoopsAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var publisher = new TestRazorProjectInfoPublisher(projectManager, projectConfigurationFilePathStore)
        {
            _active = true,
        };

        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, "TestRootNamespace");
        await projectManager.UpdateAsync(
            updater => updater.ProjectAdded(hostProject));

        // Act & Assert
        await projectManager.UpdateAsync(
            updater => updater.ProjectRemoved(hostProject.Key));

        Assert.Empty(publisher.DeferredPublishTasks);
    }

    [UIFact]
    public async Task ProjectAdded_DoesNotFireWhenNotReadyAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var serializationSuccessful = false;
        var expectedConfigurationFilePath = @"C:\path\to\obj\bin\Debug\project.razor.bin";

        var publisher = new TestRazorProjectInfoPublisher(
            projectManager,
            projectConfigurationFilePathStore,
            onSerializeToFile: (snapshot, configurationFilePath) =>
            {
                Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
                serializationSuccessful = true;
            },
            shouldSerialize: false)
        {
            _active = true,
        };

        var projectFilePath = @"C:\path\to\project.csproj";
        var hostProject = new HostProject(
            projectFilePath,
            Path.Combine(Path.GetDirectoryName(projectFilePath), "obj"),
            RazorConfiguration.Default,
            "TestRootNamespace");
        projectConfigurationFilePathStore.Set(hostProject.Key, expectedConfigurationFilePath);
        var projectWorkspaceState = ProjectWorkspaceState.Default;

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.ProjectWorkspaceStateChanged(hostProject.Key, projectWorkspaceState);
        });

        // Assert
        Assert.False(serializationSuccessful);
    }

    internal static IProjectSnapshot CreateProjectSnapshot(
        string projectFilePath,
        ProjectWorkspaceState? projectWorkspaceState = null,
        string[]? documentFilePaths = null)
    {
        return TestProjectSnapshot.Create(projectFilePath, documentFilePaths ?? [], projectWorkspaceState);
    }

    internal static IProjectSnapshot CreateProjectSnapshot(string projectFilePath, string[] documentFilePaths)
    {
        return TestProjectSnapshot.Create(projectFilePath, documentFilePaths);
    }

    private class TestRazorProjectInfoPublisher(
        IProjectSnapshotManager projectManager,
        ProjectConfigurationFilePathStore projectStatePublishFilePathStore,
        Action<IProjectSnapshot, string>? onSerializeToFile = null,
        bool shouldSerialize = true,
        bool useRealShouldSerialize = false,
        bool configurationFileExists = true)
        : RazorProjectInfoPublisher(s_lspEditorFeatureDetector.Object, projectManager, projectStatePublishFilePathStore, TestRazorLogger.Instance)
    {
        private static readonly StrictMock<LSPEditorFeatureDetector> s_lspEditorFeatureDetector = new();

        private readonly Action<IProjectSnapshot, string> _onSerializeToFile = onSerializeToFile ?? ((_1, _2) => throw new XunitException("SerializeToFile should not have been called."));

        private readonly bool _shouldSerialize = shouldSerialize;
        private readonly bool _useRealShouldSerialize = useRealShouldSerialize;
        private readonly bool _configurationFileExists = configurationFileExists;

        static TestRazorProjectInfoPublisher()
        {
            s_lspEditorFeatureDetector
                .Setup(t => t.IsLSPEditorAvailable())
                .Returns(true);
        }

        protected override bool FileExists(string file)
        {
            return _configurationFileExists;
        }

        protected override void SerializeToFile(IProjectSnapshot projectSnapshot, string configurationFilePath) => _onSerializeToFile?.Invoke(projectSnapshot, configurationFilePath);

        protected override bool ShouldSerialize(IProjectSnapshot projectSnapshot, string configurationFilePath)
        {
            if (_useRealShouldSerialize)
            {
                return base.ShouldSerialize(projectSnapshot, configurationFilePath);
            }
            else
            {
                return _shouldSerialize;
            }
        }
    }
}
