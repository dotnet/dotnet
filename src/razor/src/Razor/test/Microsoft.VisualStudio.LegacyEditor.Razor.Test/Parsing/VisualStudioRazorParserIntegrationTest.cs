// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Moq;
using Xunit;
using Xunit.Abstractions;
using SystemDebugger = System.Diagnostics.Debugger;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

public class VisualStudioRazorParserIntegrationTest : VisualStudioTestBase
{
    private const string TestLinePragmaFileName = @"C:\This\Path\Is\Just\For\Line\Pragmas.cshtml";
    private const string TestProjectPath = @"C:\This\Path\Is\Just\For\Project.csproj";

    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly IProjectSnapshot _projectSnapshot;
    private readonly CodeAnalysis.Workspace _workspace;

    public VisualStudioRazorParserIntegrationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _workspace = TestWorkspace.Create();
        AddDisposable(_workspace);

        _projectEngineFactoryProvider = CreateProjectEngineFactoryProvider();
        _projectSnapshot = new EphemeralProjectSnapshot(_projectEngineFactoryProvider, TestProjectPath);
    }

    [UIFact]
    public async Task NoDocumentSnapshotParsesComponentFileCorrectly()
    {
        // Arrange
        var snapshot = new StringTextSnapshot("@code { }");
        var testBuffer = new TestTextBuffer(snapshot);
        var documentTracker = CreateDocumentTracker(testBuffer, filePath: "C:\\This\\Path\\Is\\Just\\For\\component.razor");
        using (var manager = CreateParserManager(documentTracker))
        {
            // Act
            await manager.InitializeWithDocumentAsync(snapshot);

            // Assert
            Assert.Equal(1, manager._parseCount);

            var codeDocument = await manager.InnerParser.GetLatestCodeDocumentAsync(snapshot);
            Assert.Equal(FileKinds.Component, codeDocument.GetFileKind());

            // @code is only applicable in component files so we're verifying that `@code` was treated as a directive.
            var directiveNodes = manager.CurrentSyntaxTree!.Root.DescendantNodes().Where(child => child.Kind == SyntaxKind.RazorDirective);
            Assert.Single(directiveNodes);
        }
    }

    [UIFact]
    public async Task BufferChangeStartsFullReparseIfChangeOverlapsMultipleSpans()
    {
        // Arrange
        var original = new StringTextSnapshot("Foo @bar Baz");
        using (var manager = CreateParserManager(original))
        {
            var changed = new StringTextSnapshot("Foo @bap Daz");
            var edit = new TestEdit(7, 3, original, changed, "p D");

            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // Act - 1
            await manager.ApplyEditAndWaitForParseAsync(edit);

            // Assert - 1
            Assert.Equal(2, manager._parseCount);

            // Act - 2
            await manager.ApplyEditAndWaitForParseAsync(edit);

            // Assert - 2
            Assert.Equal(3, manager._parseCount);
        }
    }

    [UIFact]
    public async Task AwaitPeriodInsertionAcceptedProvisionally()
    {
        // Arrange
        var original = new StringTextSnapshot("foo @await Html baz");
        using (var manager = CreateParserManager(original))
        {
            var changed = new StringTextSnapshot("foo @await Html. baz");
            var edit = new TestEdit(15, 0, original, changed, ".");
            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // Act
            await manager.ApplyEditAndWaitForReparseAsync(edit);

            // Assert
            Assert.Equal(2, manager._parseCount);
            VerifyCurrentSyntaxTree(manager);
        }
    }

    [UIFact]
    public async Task ImpExprAcceptsDCIInStmtBlkAfterIdentifiers()
    {
        // ImplicitExpressionAcceptsDotlessCommitInsertionsInStatementBlockAfterIdentifiers
        var changed = new StringTextSnapshot("""
            @{
                @DateTime.
            }
            """);
        var original = new StringTextSnapshot("""
            @{
                @DateTime
            }
            """);

        var edit = new TestEdit(15 + Environment.NewLine.Length, 0, original, changed, ".");
        using (var manager = CreateParserManager(original))
        {
            void ApplyAndVerifyPartialChange(TestEdit testEdit, string expectedCode)
            {
                manager.ApplyEdit(testEdit);
                Assert.Equal(1, manager._parseCount);

                VerifyPartialParseTree(manager, changed.GetText(), expectedCode);
            }

            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // This is the process of a dotless commit when doing "." insertions to commit intellisense changes.
            ApplyAndVerifyPartialChange(edit, "DateTime.");

            original = changed;
            changed = new StringTextSnapshot("""
                @{
                    @DateTime..
                }
                """);
            edit = new TestEdit(16 + Environment.NewLine.Length, 0, original, changed, ".");

            ApplyAndVerifyPartialChange(edit, "DateTime..");

            original = changed;
            changed = new StringTextSnapshot("""
                @{
                    @DateTime.Now.
                }
                """);
            edit = new TestEdit(16 + Environment.NewLine.Length, 0, original, changed, "Now");

            ApplyAndVerifyPartialChange(edit, "DateTime.Now.");
        }
    }

    [UIFact]
    public async Task ImpExprAcceptsDCIInStatementBlock()
    {
        // ImpExprAcceptsDotlessCommitInsertionsInStatementBlock
        var changed = new StringTextSnapshot("""
            @{
                @DateT.
            }
            """);
        var original = new StringTextSnapshot("""
            @{
                @DateT
            }
            """);

        var edit = new TestEdit(12 + Environment.NewLine.Length, 0, original, changed, ".");
        using (var manager = CreateParserManager(original))
        {
            void ApplyAndVerifyPartialChange(TestEdit testEdit, string expectedCode)
            {
                manager.ApplyEdit(testEdit);
                Assert.Equal(1, manager._parseCount);
                VerifyPartialParseTree(manager, changed.GetText(), expectedCode);
            }

            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // This is the process of a dotless commit when doing "." insertions to commit intellisense changes.
            ApplyAndVerifyPartialChange(edit, "DateT.");

            original = changed;
            changed = new StringTextSnapshot("""
                @{
                    @DateTime.
                }
                """);
            edit = new TestEdit(12 + Environment.NewLine.Length, 0, original, changed, "ime");

            ApplyAndVerifyPartialChange(edit, "DateTime.");
        }
    }

    [UIFact]
    public async Task ImpExprProvisionallyAcceptsDCI()
    {
        // ImpExprProvisionallyAcceptsDotlessCommitInsertions
        var changed = new StringTextSnapshot("foo @DateT. baz");
        var original = new StringTextSnapshot("foo @DateT baz");
        var edit = new TestEdit(10, 0, original, changed, ".");
        using (var manager = CreateParserManager(original))
        {
            void ApplyAndVerifyPartialChange(TestEdit testEdit, string expectedCode)
            {
                manager.ApplyEdit(testEdit);
                Assert.Equal(1, manager._parseCount);

                VerifyPartialParseTree(manager, testEdit.NewSnapshot.GetText(), expectedCode);
            }

            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // This is the process of a dotless commit when doing "." insertions to commit intellisense changes.
            ApplyAndVerifyPartialChange(edit, "DateT.");

            original = changed;
            changed = new StringTextSnapshot("foo @DateTime. baz");
            edit = new TestEdit(10, 0, original, changed, "ime");

            ApplyAndVerifyPartialChange(edit, "DateTime.");

            // Verify the reparse finally comes
            await manager.WaitForReparseAsync();

            Assert.Equal(2, manager._parseCount);
            VerifyCurrentSyntaxTree(manager);
        }
    }

    [UIFact(Skip = "https://github.com/dotnet/aspnetcore/issues/17234")]
    public async Task ImpExprProvisionallyAcceptsDCIAfterIdentifiers_CompletesSyntaxTreeRequest()
    {
        var original = new StringTextSnapshot("foo @DateTime baz", versionNumber: 0);
        var changed = new StringTextSnapshot("foo @DateTime. baz", versionNumber: 1);
        var edit = new TestEdit(13, 0, original, changed, ".");
        using (var manager = CreateParserManager(original))
        {
            void ApplyAndVerifyPartialChange(TestEdit testEdit, string expectedCode)
            {
                manager.ApplyEdit(testEdit);
                Assert.Equal(1, manager._parseCount);

                VerifyPartialParseTree(manager, testEdit.NewSnapshot.GetText(), expectedCode);
            }

            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);
            var codeDocumentTask = manager.InnerParser.GetLatestCodeDocumentAsync(changed);

            Assert.False(codeDocumentTask.IsCompleted);

            // Perform a partially parsed accepted change
            ApplyAndVerifyPartialChange(edit, "DateTime.");

            Assert.True(codeDocumentTask.IsCompleted);
        }
    }

    [UIFact]
    public async Task ImpExprProvisionallyAcceptsDCIAfterIdentifiers()
    {
        // ImplicitExpressionProvisionallyAcceptsDotlessCommitInsertionsAfterIdentifiers
        var changed = new StringTextSnapshot("foo @DateTime. baz");
        var original = new StringTextSnapshot("foo @DateTime baz");
        var edit = new TestEdit(13, 0, original, changed, ".");
        using (var manager = CreateParserManager(original))
        {
            void ApplyAndVerifyPartialChange(TestEdit testEdit, string expectedCode)
            {
                manager.ApplyEdit(testEdit);
                Assert.Equal(1, manager._parseCount);

                VerifyPartialParseTree(manager, testEdit.NewSnapshot.GetText(), expectedCode);
            }

            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // This is the process of a dotless commit when doing "." insertions to commit intellisense changes.
            ApplyAndVerifyPartialChange(edit, "DateTime.");

            original = changed;
            changed = new StringTextSnapshot("foo @DateTime.. baz");
            edit = new TestEdit(14, 0, original, changed, ".");

            ApplyAndVerifyPartialChange(edit, "DateTime..");

            original = changed;
            changed = new StringTextSnapshot("foo @DateTime.Now. baz");
            edit = new TestEdit(14, 0, original, changed, "Now");

            ApplyAndVerifyPartialChange(edit, "DateTime.Now.");

            // Verify the reparse eventually happens
            await manager.WaitForReparseAsync();

            Assert.Equal(2, manager._parseCount);
            VerifyCurrentSyntaxTree(manager);
        }
    }

    [UIFact]
    public async Task ImpExprProvisionallyAccCaseInsensitiveDCI_NewRoslynIntegration()
    {
        // ImplicitExpressionProvisionallyAcceptsCaseInsensitiveDotlessCommitInsertions_NewRoslynIntegration
        var original = new StringTextSnapshot("foo @date baz");
        var changed = new StringTextSnapshot("foo @date. baz");
        var edit = new TestEdit(9, 0, original, changed, ".");
        using (var manager = CreateParserManager(original))
        {
            void ApplyAndVerifyPartialChange(Action applyEdit, string expectedCode)
            {
                applyEdit();
                Assert.Equal(1, manager._parseCount);

                VerifyPartialParseTree(manager, changed.GetText(), expectedCode);
            }

            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // This is the process of a dotless commit when doing "." insertions to commit intellisense changes.

            // @date => @date.
            ApplyAndVerifyPartialChange(() => manager.ApplyEdit(edit), "date.");

            original = changed;
            changed = new StringTextSnapshot("foo @date baz");
            edit = new TestEdit(9, 1, original, changed, "");

            // @date. => @date
            ApplyAndVerifyPartialChange(() => manager.ApplyEdit(edit), "date");

            original = changed;
            changed = new StringTextSnapshot("foo @DateTime baz");
            edit = new TestEdit(5, 4, original, changed, "DateTime");

            // @date => @DateTime
            ApplyAndVerifyPartialChange(() => manager.ApplyEdit(edit), "DateTime");

            original = changed;
            changed = new StringTextSnapshot("foo @DateTime. baz");
            edit = new TestEdit(13, 0, original, changed, ".");

            // @DateTime => @DateTime.
            ApplyAndVerifyPartialChange(() => manager.ApplyEdit(edit), "DateTime.");

            // Verify the reparse eventually happens
            await manager.WaitForReparseAsync();

            Assert.Equal(2, manager._parseCount);
            VerifyCurrentSyntaxTree(manager);
        }
    }

    [UIFact]
    public async Task ImpExprRejectsAcceptableChangeIfPrevWasProvisionallyAccepted()
    {
        // ImplicitExpressionRejectsChangeWhichWouldHaveBeenAcceptedIfLastChangeWasProvisionallyAcceptedOnDifferentSpan
        // Arrange
        var dotTyped = new TestEdit(8, 0, new StringTextSnapshot("foo @foo @bar"), new StringTextSnapshot("foo @foo. @bar"), ".");
        var charTyped = new TestEdit(14, 0, new StringTextSnapshot("foo @foo. @bar"), new StringTextSnapshot("foo @foo. @barb"), "b");
        using (var manager = CreateParserManager(dotTyped.OldSnapshot))
        {
            await manager.InitializeWithDocumentAsync(dotTyped.OldSnapshot);

            // Apply the dot change
            await manager.ApplyEditAndWaitForReparseAsync(dotTyped);

            // Act (apply the identifier start char change)
            await manager.ApplyEditAndWaitForParseAsync(charTyped);

            // Assert
            Assert.Equal(2, manager._parseCount);
            VerifyPartialParseTree(manager, charTyped.NewSnapshot.GetText());
        }
    }

    [UIFact]
    public async Task ImpExprAcceptsIdentifierTypedAfterDotIfLastChangeProvisional()
    {
        // ImplicitExpressionAcceptsIdentifierTypedAfterDotIfLastChangeWasProvisionalAcceptanceOfDot
        // Arrange
        var dotTyped = new TestEdit(8, 0, new StringTextSnapshot("foo @foo bar"), new StringTextSnapshot("foo @foo. bar"), ".");
        var charTyped = new TestEdit(9, 0, new StringTextSnapshot("foo @foo. bar"), new StringTextSnapshot("foo @foo.b bar"), "b");
        using (var manager = CreateParserManager(dotTyped.OldSnapshot))
        {
            await manager.InitializeWithDocumentAsync(dotTyped.OldSnapshot);

            // Apply the dot change
            manager.ApplyEdit(dotTyped);

            // Act (apply the identifier start char change)
            manager.ApplyEdit(charTyped);

            // Assert
            Assert.Equal(1, manager._parseCount);
            VerifyPartialParseTree(manager, charTyped.NewSnapshot.GetText());
        }
    }

    [UIFact]
    public async Task ImpExpr_AcceptsParenthesisAtEnd_SingleEdit()
    {
        // Arrange
        var edit = new TestEdit(8, 0, new StringTextSnapshot("foo @foo bar"), new StringTextSnapshot("foo @foo() bar"), "()");

        using (var manager = CreateParserManager(edit.OldSnapshot))
        {
            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // Apply the () edit
            manager.ApplyEdit(edit);

            // Assert
            Assert.Equal(1, manager._parseCount);
            VerifyPartialParseTree(manager, edit.NewSnapshot.GetText());
        }
    }

    [UIFact]
    public async Task ImpExpr_AcceptsParenthesisAtEnd_TwoEdits()
    {
        // Arrange
        var edit1 = new TestEdit(8, 0, new StringTextSnapshot("foo @foo bar"), new StringTextSnapshot("foo @foo( bar"), "(");
        var edit2 = new TestEdit(9, 0, new StringTextSnapshot("foo @foo( bar"), new StringTextSnapshot("foo @foo() bar"), ")");
        using (var manager = CreateParserManager(edit1.OldSnapshot))
        {
            await manager.InitializeWithDocumentAsync(edit1.OldSnapshot);

            // Apply the ( edit
            manager.ApplyEdit(edit1);

            // Apply the ) edit
            manager.ApplyEdit(edit2);

            // Assert
            Assert.Equal(1, manager._parseCount);
            VerifyPartialParseTree(manager, edit2.NewSnapshot.GetText());
        }
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfIfKeywordTyped()
    {
        await RunTypeKeywordTestAsync("if");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfDoKeywordTyped()
    {
        await RunTypeKeywordTestAsync("do");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfTryKeywordTyped()
    {
        await RunTypeKeywordTestAsync("try");
    }

    [UIFact]
    public async Task ImplicitExpressionCorrectlyTriggersReparseIfForKeywordTyped()
    {
        await RunTypeKeywordTestAsync("for");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfForEachKeywordTyped()
    {
        await RunTypeKeywordTestAsync("foreach");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfWhileKeywordTyped()
    {
        await RunTypeKeywordTestAsync("while");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfSwitchKeywordTyped()
    {
        await RunTypeKeywordTestAsync("switch");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfLockKeywordTyped()
    {
        await RunTypeKeywordTestAsync("lock");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfUsingKeywordTyped()
    {
        await RunTypeKeywordTestAsync("using");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfSectionKeywordTyped()
    {
        await RunTypeKeywordTestAsync("section");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfInheritsKeywordTyped()
    {
        await RunTypeKeywordTestAsync("inherits");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfFunctionsKeywordTyped()
    {
        await RunTypeKeywordTestAsync("functions");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfNamespaceKeywordTyped()
    {
        await RunTypeKeywordTestAsync("namespace");
    }

    [UIFact]
    public async Task ImpExprCorrectlyTriggersReparseIfClassKeywordTyped()
    {
        await RunTypeKeywordTestAsync("class");
    }

    protected override bool EnableSpanEditHandlers => true;

    private void VerifyPartialParseTree(TestParserManager manager, string content, string? expectedCode = null)
    {
        if (expectedCode != null)
        {
            // Verify if the syntax tree represents the expected input.
            var syntaxTreeContent = manager.PartialParsingSyntaxTreeRoot.ToFullString();
            Assert.Contains(expectedCode, syntaxTreeContent, StringComparison.Ordinal);
        }

        var sourceDocument = TestRazorSourceDocument.Create(content);
        var syntaxTree = new RazorSyntaxTree(manager.PartialParsingSyntaxTreeRoot, sourceDocument, manager.CurrentSyntaxTree!.Diagnostics, manager.CurrentSyntaxTree.Options);
        BaselineTest(syntaxTree);
    }

    private void VerifyCurrentSyntaxTree(TestParserManager manager)
    {
        BaselineTest(manager.CurrentSyntaxTree);
    }

    private TestParserManager CreateParserManager(IVisualStudioDocumentTracker documentTracker)
    {
        var parser = new VisualStudioRazorParser(
            documentTracker,
            _projectEngineFactoryProvider,
            new TestCompletionBroker(),
            LoggerFactory,
            JoinableTaskFactory.Context)
        {
            // We block idle work with the below reset events. Therefore, make tests fast and have the idle timer fire as soon as possible.
            _idleDelay = TimeSpan.FromMilliseconds(1),
            NotifyUIIdleStart = new ManualResetEventSlim(),
            BlockBackgroundIdleWork = new ManualResetEventSlim(),
        };

        parser.StartParser();

        return new TestParserManager(parser);
    }

    private TestParserManager CreateParserManager(ITextSnapshot originalSnapshot)
    {
        var textBuffer = new TestTextBuffer(originalSnapshot);
        var documentTracker = CreateDocumentTracker(textBuffer);

        return CreateParserManager(documentTracker);
    }

    private static IProjectEngineFactoryProvider CreateProjectEngineFactoryProvider()
    {
        var fileSystem = new TestRazorProjectFileSystem();
        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, fileSystem, builder =>
        {
            RazorExtensions.Register(builder);

            builder.AddDefaultImports("@addTagHelper *, Test");

            builder.Features.Add(new VisualStudioRazorParser.VisualStudioEnableTagHelpersFeature());
        });

        var factoryMock = new StrictMock<IProjectEngineFactory>();
        factoryMock
            .Setup(x => x.Create(It.IsAny<RazorConfiguration>(), It.IsAny<RazorProjectFileSystem>(), It.IsAny<Action<RazorProjectEngineBuilder>>()))
            .Returns(projectEngine);

        var providerMock = new StrictMock<IProjectEngineFactoryProvider>();
        providerMock
            .Setup(x => x.GetFactory(It.IsAny<RazorConfiguration>()))
            .Returns(factoryMock.Object);

        return providerMock.Object;
    }

    private async Task RunTypeKeywordTestAsync(string keyword)
    {
        // Arrange
        var before = "@" + keyword[..^1];
        var after = "@" + keyword;
        var changed = new StringTextSnapshot(after);
        var old = new StringTextSnapshot(before);
        var change = new SourceChange(keyword.Length, 0, keyword[keyword.Length - 1].ToString());
        var edit = new TestEdit(change, old, changed);
        using (var manager = CreateParserManager(old))
        {
            await manager.InitializeWithDocumentAsync(edit.OldSnapshot);

            // Act
            await manager.ApplyEditAndWaitForParseAsync(edit);

            // Assert
            Assert.Equal(2, manager._parseCount);
        }
    }

    private static void DoWithTimeoutIfNotDebugging(Func<int, bool> withTimeout)
    {
#if DEBUG
        if (SystemDebugger.IsAttached)
        {
            withTimeout(Timeout.Infinite);
        }
        else
        {
#endif
            Assert.True(withTimeout((int)TimeSpan.FromSeconds(5).TotalMilliseconds), "Timeout expired!");
#if DEBUG
        }
#endif
    }

    private IVisualStudioDocumentTracker CreateDocumentTracker(Text.ITextBuffer textBuffer, string filePath = TestLinePragmaFileName)
    {
        var focusedTextView = StrictMock.Of<ITextView>(v =>
            v.HasAggregateFocus == true);
        var documentTracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.TextBuffer == textBuffer &&
            t.TextViews == new[] { focusedTextView } &&
            t.FilePath == filePath &&
            t.ProjectPath == TestProjectPath &&
            t.ProjectSnapshot == _projectSnapshot &&
            t.IsSupportedProject == true);
        textBuffer.Properties.AddProperty(typeof(IVisualStudioDocumentTracker), documentTracker);

        return documentTracker;
    }

    private class TestParserManager : IDisposable
    {
        public int _parseCount;

        private readonly ManualResetEventSlim _parserComplete;
        private readonly ManualResetEventSlim _reparseComplete;
        private readonly TestTextBuffer _testBuffer;
        private readonly VisualStudioRazorParser _parser;

        public TestParserManager(VisualStudioRazorParser parser)
        {
            _parserComplete = new ManualResetEventSlim();
            _reparseComplete = new ManualResetEventSlim();

            _testBuffer = (TestTextBuffer)parser.TextBuffer;
            _parseCount = 0;

            _parser = parser;
            parser.DocumentStructureChanged += (sender, args) =>
            {
                CurrentSyntaxTree = args.CodeDocument.GetSyntaxTree();

                Interlocked.Increment(ref _parseCount);

                if (args.SourceChange is null)
                {
                    // Reparse occurred
                    _reparseComplete.Set();
                }

                _parserComplete.Set();
            };
        }

        public RazorSyntaxTree? CurrentSyntaxTree { get; private set; }

        public SyntaxNode PartialParsingSyntaxTreeRoot => _parser._partialParser!.ModifiedSyntaxTreeRoot;

        public IVisualStudioRazorParser InnerParser => _parser;

        public async Task InitializeWithDocumentAsync(ITextSnapshot snapshot)
        {
            var old = new StringTextSnapshot(string.Empty);
            var initialChange = new SourceChange(0, 0, snapshot.GetText());
            var edit = new TestEdit(initialChange, old, snapshot);
            await ApplyEditAndWaitForParseAsync(edit);
        }

        public void ApplyEdit(TestEdit edit)
        {
            _testBuffer.ApplyEdit(edit);
        }

        public async Task ApplyEditAndWaitForParseAsync(TestEdit edit)
        {
            ApplyEdit(edit);
            await WaitForParseAsync();
        }

        public async Task ApplyEditAndWaitForReparseAsync(TestEdit edit)
        {
            ApplyEdit(edit);
            await WaitForReparseAsync();
        }

        public async Task WaitForParseAsync()
        {
            // Get off of the UI thread so we can wait for the document structure changed event to fire
            await Task.Run(() => DoWithTimeoutIfNotDebugging(_parserComplete.Wait));

            _parserComplete.Reset();
        }

        public async Task WaitForReparseAsync()
        {
            Assert.True(_parser._idleTimer is not null);

            // Allow background idle work to continue
            _parser.BlockBackgroundIdleWork!.Set();

            // Get off of the UI thread so we can wait for the idle timer to fire
            await Task.Run(() => DoWithTimeoutIfNotDebugging(_parser.NotifyUIIdleStart!.Wait));

            Assert.Null(_parser._idleTimer);

            // Get off of the UI thread so we can wait for the document structure changed event to fire for reparse
            await Task.Run(() => DoWithTimeoutIfNotDebugging(_reparseComplete.Wait));

            _reparseComplete.Reset();
            _parser.BlockBackgroundIdleWork.Reset();
            _parser.NotifyUIIdleStart!.Reset();
        }

        public void Dispose()
        {
            _parser.Dispose();
            _parserComplete.Dispose();
            _reparseComplete.Dispose();
        }
    }

    private class TestCompletionBroker : ICompletionBroker
    {
        public ICompletionSession CreateCompletionSession(ITextView textView, ITrackingPoint triggerPoint, bool trackCaret)
            => throw new NotImplementedException();

        public void DismissAllSessions(ITextView textView)
            => throw new NotImplementedException();

        public ReadOnlyCollection<ICompletionSession> GetSessions(ITextView textView)
            => throw new NotImplementedException();

        public bool IsCompletionActive(ITextView textView)
            => false;

        public ICompletionSession TriggerCompletion(ITextView textView)
            => throw new NotImplementedException();

        public ICompletionSession TriggerCompletion(ITextView textView, ITrackingPoint triggerPoint, bool trackCaret)
            => throw new NotImplementedException();
    }
}
