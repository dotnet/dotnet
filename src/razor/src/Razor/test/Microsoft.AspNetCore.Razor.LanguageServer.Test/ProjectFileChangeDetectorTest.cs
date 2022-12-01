// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ProjectFileChangeDetectorTest : LanguageServerTestBase
{
    public ProjectFileChangeDetectorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task StartAsync_NotifiesListenersOfExistingProjectFiles()
    {
        // Arrange
        var args1 = new List<(string FilePath, RazorFileChangeKind Kind)>();
        var listener1 = new Mock<IProjectFileChangeListener>(MockBehavior.Strict);
        listener1.Setup(l => l.ProjectFileChanged(It.IsAny<string>(), It.IsAny<RazorFileChangeKind>()))
            .Callback<string, RazorFileChangeKind>((filePath, kind) => args1.Add((filePath, kind)));
        var args2 = new List<(string FilePath, RazorFileChangeKind Kind)>();
        var listener2 = new Mock<IProjectFileChangeListener>(MockBehavior.Strict);
        listener2.Setup(l => l.ProjectFileChanged(It.IsAny<string>(), It.IsAny<RazorFileChangeKind>()))
            .Callback<string, RazorFileChangeKind>((filePath, kind) => args2.Add((filePath, kind)));
        var existingProjectFiles = new[] { "c:/path/to/project.csproj", "c:/other/path/project.csproj" };
        var cts = new CancellationTokenSource();
        var detector = new TestProjectFileChangeDetector(
            cts,
            Dispatcher,
            new[] { listener1.Object, listener2.Object },
            existingProjectFiles);

        // Act
        await detector.StartAsync("/some/workspacedirectory", cts.Token);

        // Assert
        Assert.Collection(args1,
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingProjectFiles[0], args.FilePath);
            },
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingProjectFiles[1], args.FilePath);
            });
        Assert.Collection(args2,
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingProjectFiles[0], args.FilePath);
            },
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingProjectFiles[1], args.FilePath);
            });
    }

    private class TestProjectFileChangeDetector : ProjectFileChangeDetector
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IReadOnlyList<string> _existingProjectFiles;

        public TestProjectFileChangeDetector(
            CancellationTokenSource cancellationTokenSource,
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            IEnumerable<IProjectFileChangeListener> listeners,
            IReadOnlyList<string> existingprojectFiles)
            : base(projectSnapshotManagerDispatcher, listeners)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _existingProjectFiles = existingprojectFiles;
        }

        protected override void OnInitializationFinished()
        {
            // Once initialization has finished we want to ensure that no file watchers are created so cancel!
            _cancellationTokenSource.Cancel();
        }

        protected override IEnumerable<string> GetExistingProjectFiles(string workspaceDirectory)
        {
            return _existingProjectFiles;
        }
    }
}
