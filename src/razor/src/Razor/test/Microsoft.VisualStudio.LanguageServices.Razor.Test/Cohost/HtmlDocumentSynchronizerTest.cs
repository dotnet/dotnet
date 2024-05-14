// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.VisualStudio.Razor.LanguageClient.Cohost.HtmlDocumentSynchronizer;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class HtmlDocumentSynchronizerTest(ITestOutputHelper testOutput) : VisualStudioWorkspaceTestBase(testOutput)
{
    private DocumentId? _documentId;

    protected override void ConfigureWorkspace(AdhocWorkspace workspace)
    {
        var project = workspace.CurrentSolution.AddProject("Project", "Project.dll", LanguageNames.CSharp);
        var document = project.AddAdditionalDocument("File.razor", SourceText.From("<div></div>"));
        _documentId = document.Id;

        Assert.True(workspace.TryApplyChanges(document.Project.Solution));
    }

    [Fact]
    public async Task TrySynchronize_NewDocument_Generates()
    {
        var publisher = new TestHtmlDocumentPublisher();
        var synchronizer = new HtmlDocumentSynchronizer(StrictMock.Of<TrackingLSPDocumentManager>(), publisher, LoggerFactory);

        var document = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();

        Assert.True(await synchronizer.TrySynchronizeAsync(document, DisposalToken));

        var version = await RazorDocumentVersion.CreateAsync(document, DisposalToken);

        Assert.Equal(1, version.WorkspaceVersion);

        Assert.Collection(publisher.Publishes,
            i =>
            {
                Assert.Equal(_documentId, i.Item1.Id);
                Assert.Equal("<div></div>", i.Item2);
            });
    }

    [Fact]
    public async Task TrySynchronize_WorkspaceMovedForward_NoDocumentChanges_DoesntGenerate()
    {
        var publisher = new TestHtmlDocumentPublisher();
        var synchronizer = new HtmlDocumentSynchronizer(StrictMock.Of<TrackingLSPDocumentManager>(), publisher, LoggerFactory);

        var document = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();
        var version1 = await RazorDocumentVersion.CreateAsync(document, DisposalToken);

        Assert.True(await synchronizer.TrySynchronizeAsync(document, DisposalToken));

        // Add a new document, moving the workspace forward but leaving our document unaffected
        Assert.True(Workspace.TryApplyChanges(document.Project.AddAdditionalDocument("Foo2.razor", SourceText.From("")).Project.Solution));

        document = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();
        var version2 = await RazorDocumentVersion.CreateAsync(document, DisposalToken);

        Assert.True(await synchronizer.TrySynchronizeAsync(document, DisposalToken));

        // Validate that the workspace moved forward
        Assert.NotEqual(version1.WorkspaceVersion, version2.WorkspaceVersion);

        // Still only one publish
        Assert.Collection(publisher.Publishes,
            i =>
            {
                Assert.Equal(_documentId, i.Item1.Id);
                Assert.Equal("<div></div>", i.Item2);
            });
    }

    [Fact]
    public async Task TrySynchronize_WorkspaceUnchanged_DocumentChanges_Generates()
    {
        var publisher = new TestHtmlDocumentPublisher();
        var synchronizer = new HtmlDocumentSynchronizer(StrictMock.Of<TrackingLSPDocumentManager>(), publisher, LoggerFactory);

        var document = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();
        var version1 = await RazorDocumentVersion.CreateAsync(document, DisposalToken);

        Assert.True(await synchronizer.TrySynchronizeAsync(document, DisposalToken));

        // Change our document directly, but without applying changes (equivalent to LSP didChange)
        var solution = Workspace.CurrentSolution.WithAdditionalDocumentText(_documentId.AssumeNotNull(), SourceText.From("<span></span>"));
        document = solution.GetAdditionalDocument(_documentId).AssumeNotNull();
        var version2 = await RazorDocumentVersion.CreateAsync(document, DisposalToken);

        Assert.True(await synchronizer.TrySynchronizeAsync(document, DisposalToken));

        // Validate that the workspace hasn't moved forward
        Assert.Equal(version1.WorkspaceVersion, version2.WorkspaceVersion);

        // We should have two publishes
        Assert.Collection(publisher.Publishes,
            i =>
            {
                Assert.Equal(_documentId, i.Item1.Id);
                Assert.Equal("<div></div>", i.Item2);
            },
            i =>
            {
                Assert.Equal(_documentId, i.Item1.Id);
                Assert.Equal("<span></span>", i.Item2);
            });
    }

    [Fact]
    public async Task TrySynchronize_RequestOldVersion_ImmediateFail()
    {
        var tcs = new TaskCompletionSource<bool>();
        var publisher = new TestHtmlDocumentPublisher(() => tcs.Task);
        var synchronizer = new HtmlDocumentSynchronizer(StrictMock.Of<TrackingLSPDocumentManager>(), publisher, LoggerFactory);

        var document1 = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();
        var version1 = await RazorDocumentVersion.CreateAsync(document1, DisposalToken);

        Assert.True(Workspace.TryApplyChanges(Workspace.CurrentSolution.WithAdditionalDocumentText(_documentId.AssumeNotNull(), SourceText.From("<span></span>"))));
        var document2 = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();

        var task = synchronizer.TrySynchronizeAsync(document2, DisposalToken);

        Assert.False(await synchronizer.TrySynchronizeAsync(document1, DisposalToken));

        tcs.SetResult(true);

        Assert.True(await task);

        // We should have two publishes
        Assert.Collection(publisher.Publishes,
            i =>
            {
                Assert.Equal(_documentId, i.Item1.Id);
                Assert.Equal("<span></span>", i.Item2);
            });
    }

    [Fact]
    public async Task TrySynchronize_RequestSameVersion_SingleGeneration()
    {
        var tcs = new TaskCompletionSource<bool>();
        var publisher = new TestHtmlDocumentPublisher(() => tcs.Task);
        var synchronizer = new HtmlDocumentSynchronizer(StrictMock.Of<TrackingLSPDocumentManager>(), publisher, LoggerFactory);

        var document = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();

        var task1 = synchronizer.TrySynchronizeAsync(document, DisposalToken);
        var task2 = synchronizer.TrySynchronizeAsync(document, DisposalToken);

        tcs.SetResult(true);

        await Task.WhenAll(task1, task2);

        Assert.Collection(publisher.Publishes,
            i =>
            {
                Assert.Equal(_documentId, i.Item1.Id);
                Assert.Equal("<div></div>", i.Item2);
            });
    }

    [Fact]
    public async Task TrySynchronize_RequestNewVersion_CancelOldTask()
    {
        var tcs = new TaskCompletionSource<bool>();
        var publisher = new TestHtmlDocumentPublisher(() => tcs.Task);
        var synchronizer = new HtmlDocumentSynchronizer(StrictMock.Of<TrackingLSPDocumentManager>(), publisher, LoggerFactory);

        var document = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();

        var task1 = synchronizer.TrySynchronizeAsync(document, DisposalToken);

        // Change our document directly, but without applying changes (equivalent to LSP didChange)
        var solution = Workspace.CurrentSolution.WithAdditionalDocumentText(_documentId.AssumeNotNull(), SourceText.From("<span></span>"));
        document = solution.GetAdditionalDocument(_documentId).AssumeNotNull();

        var task2 = synchronizer.TrySynchronizeAsync(document, DisposalToken);

        tcs.SetResult(true);

        await Task.WhenAll(task1, task2);
#pragma warning disable xUnit1031 // Do not use blocking task operations in test method
        Assert.False(task1.Result);
#pragma warning restore xUnit1031 // Do not use blocking task operations in test method

        Assert.Collection(publisher.Publishes,
            i =>
            {
                Assert.Equal(_documentId, i.Item1.Id);
                Assert.Equal("<span></span>", i.Item2);
            });
    }

    [Fact]
    public async Task GetSynchronizationRequestTask_RequestSameVersion_ReturnsSameTask()
    {
        var tcs = new TaskCompletionSource<bool>();
        var publisher = new TestHtmlDocumentPublisher(() => tcs.Task);
        var synchronizer = new HtmlDocumentSynchronizer(StrictMock.Of<TrackingLSPDocumentManager>(), publisher, LoggerFactory);

        var document = Workspace.CurrentSolution.GetAdditionalDocument(_documentId).AssumeNotNull();
        var version = await RazorDocumentVersion.CreateAsync(document, DisposalToken);

        var accessor = synchronizer.GetTestAccessor();
        var task1 = accessor.GetSynchronizationRequestTaskAsync(document, version);
        var task2 = accessor.GetSynchronizationRequestTaskAsync(document, version);

        Assert.Same(task1, task2);
    }

    private class TestHtmlDocumentPublisher(Func<Task>? generateTask = null) : IHtmlDocumentPublisher
    {
        private List<(TextDocument, string)> _publishes = [];

        public List<(TextDocument, string)> Publishes => _publishes;

        public async Task<string?> GetHtmlSourceFromOOPAsync(TextDocument document, CancellationToken cancellationToken)
        {
            if (generateTask is not null)
            {
                await generateTask();
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var source = await document.GetTextAsync();
            return source.ToString();
        }

        public Task PublishAsync(TextDocument document, string htmlText, CancellationToken cancellationToken)
        {
            _publishes.Add((document, htmlText));
            return Task.CompletedTask;
        }
    }
}
