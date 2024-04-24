// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ProjectConfigurationFileChangeDetectorTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task StartAsync_NotifiesListenersOfExistingConfigurationFiles()
    {
        // Arrange
        var eventArgs1 = new List<ProjectConfigurationFileChangeEventArgs>();
        var listenerMock1 = new StrictMock<IProjectConfigurationFileChangeListener>();
        listenerMock1
            .Setup(l => l.ProjectConfigurationFileChanged(It.IsAny<ProjectConfigurationFileChangeEventArgs>()))
            .Callback<ProjectConfigurationFileChangeEventArgs>(eventArgs1.Add);

        var eventArgs2 = new List<ProjectConfigurationFileChangeEventArgs>();
        var listenerMock2 = new StrictMock<IProjectConfigurationFileChangeListener>();
        listenerMock2
            .Setup(l => l.ProjectConfigurationFileChanged(It.IsAny<ProjectConfigurationFileChangeEventArgs>()))
            .Callback<ProjectConfigurationFileChangeEventArgs>(eventArgs2.Add);

        ImmutableArray<string> existingConfigurationFiles = ["c:/path/to/project.razor.json", "c:/other/path/project.razor.bin"];

        var detector = new TestProjectConfigurationFileChangeDetector(
            [listenerMock1.Object, listenerMock2.Object],
            existingConfigurationFiles,
            LoggerFactory);

        // Act
        await detector.StartAsync("/some/workspace+directory", DisposalToken);

        // Assert
        Assert.Collection(eventArgs1,
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingConfigurationFiles[0], args.ConfigurationFilePath);
            },
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingConfigurationFiles[1], args.ConfigurationFilePath);
            });
        Assert.Collection(eventArgs2,
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingConfigurationFiles[0], args.ConfigurationFilePath);
            },
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingConfigurationFiles[1], args.ConfigurationFilePath);
            });
    }

    private class TestProjectConfigurationFileChangeDetector(
        IEnumerable<IProjectConfigurationFileChangeListener> listeners,
        ImmutableArray<string> existingConfigurationFiles,
        ILoggerFactory loggerFactory)
        : ProjectConfigurationFileChangeDetector(listeners, TestLanguageServerFeatureOptions.Instance, loggerFactory)
    {
        private readonly ImmutableArray<string> _existingConfigurationFiles = existingConfigurationFiles;

        protected override bool InitializeFileWatchers => false;

        protected override ImmutableArray<string> GetExistingConfigurationFiles(string workspaceDirectory)
            => _existingConfigurationFiles;
    }
}
