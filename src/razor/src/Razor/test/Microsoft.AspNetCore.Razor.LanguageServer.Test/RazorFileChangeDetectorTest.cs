// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorFileChangeDetectorTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task StartAsync_NotifiesListenersOfExistingRazorFiles()
    {
        // Arrange
        var args1 = new List<(string FilePath, RazorFileChangeKind Kind)>();
        var listenerMock1 = new StrictMock<IRazorFileChangeListener>();
        listenerMock1
            .Setup(l => l.RazorFileChangedAsync(It.IsAny<string>(), It.IsAny<RazorFileChangeKind>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback((string filePath, RazorFileChangeKind kind, CancellationToken _) => args1.Add((filePath, kind)));

        var args2 = new List<(string FilePath, RazorFileChangeKind Kind)>();
        var listenerMock2 = new StrictMock<IRazorFileChangeListener>();
        listenerMock2
            .Setup(l => l.RazorFileChangedAsync(It.IsAny<string>(), It.IsAny<RazorFileChangeKind>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback((string filePath, RazorFileChangeKind kind, CancellationToken _) => args2.Add((filePath, kind)));

        ImmutableArray<string> existingRazorFiles = ["c:/path/to/index.razor", "c:/other/path/_Host.cshtml"];
        using var detector = new InitializationSkippingRazorFileChangeDetector(
            [listenerMock1.Object, listenerMock2.Object],
            existingRazorFiles);

        // Act
        await detector.StartAsync("/some/workspacedirectory", DisposalToken);

        // Assert
        Assert.Collection(args1,
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingRazorFiles[0], args.FilePath);
            },
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingRazorFiles[1], args.FilePath);
            });
        Assert.Collection(args2,
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingRazorFiles[0], args.FilePath);
            },
            args =>
            {
                Assert.Equal(RazorFileChangeKind.Added, args.Kind);
                Assert.Equal(existingRazorFiles[1], args.FilePath);
            });
    }

    [Theory]
    [MemberData(nameof(NotificationBehaviorData))]
    internal async Task TestNotificationBehavior((string, RazorFileChangeKind)[] work, (string, RazorFileChangeKind)[] expected)
    {
        var actual = new List<(string, RazorFileChangeKind)>();
        var listenerMock = new StrictMock<IRazorFileChangeListener>();
        listenerMock
            .Setup(l => l.RazorFileChangedAsync(It.IsAny<string>(), It.IsAny<RazorFileChangeKind>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback((string filePath, RazorFileChangeKind kind, CancellationToken _) => actual.Add((filePath, kind)));

        using var detector = new TestRazorFileChangeDetector([listenerMock.Object], TimeSpan.FromMilliseconds(1));
        var detectorAccessor = detector.GetTestAccessor();

        detectorAccessor.AddWork(work);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        Assert.Equal(expected, actual);
    }

    public static TheoryData NotificationBehaviorData
    {
        get
        {
            const string File1 = "C:/path/to/file1.razor";
            const string File2 = "C:/path/to/file2.razor";

            const RazorFileChangeKind Add = RazorFileChangeKind.Added;
            const RazorFileChangeKind Remove = RazorFileChangeKind.Removed;

            return new TheoryData<(string, RazorFileChangeKind)[], (string, RazorFileChangeKind)[]>
            {
                { [(File1, Add)], [(File1, Add)] },
                { [(File1, Add), (File1, Remove)], [] },
                { [(File1, Remove), (File1, Add)], [] },
                { [(File1, Add), (File1, Remove), (File1, Add)], [(File1, Add)] },
                { [(File1, Remove), (File1, Add), (File1, Remove)], [(File1, Remove)] },
                { [(File1, Add), (File2, Remove)], [(File1, Add), (File2, Remove)] },
                { [(File1, Add), (File1, Remove), (File2, Remove)], [(File2, Remove)] },
            };
        }
    }

    private class TestRazorFileChangeDetector(
        IEnumerable<IRazorFileChangeListener> listeners,
        TimeSpan delay)
        : RazorFileChangeDetector(listeners, delay)
    {
    }

    private class InitializationSkippingRazorFileChangeDetector(
        IEnumerable<IRazorFileChangeListener> listeners,
        ImmutableArray<string> existingProjectFiles) : RazorFileChangeDetector(listeners)
    {
        private readonly ImmutableArray<string> _existingProjectFiles = existingProjectFiles;

        protected override bool InitializeFileWatchers => false;

        protected override ImmutableArray<string> GetExistingRazorFiles(string workspaceDirectory)
            => _existingProjectFiles;
    }
}
