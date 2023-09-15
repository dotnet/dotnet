// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public abstract class LanguageServerTestBase : TestBase
{
    // This is marked as legacy because in its current form it's being assigned a "TestProjectSnapshotManagerDispatcher" which takes the
    // synchronization context from the constructing thread and binds to that. We've seen in XUnit how this can unexpectedly lead to flaky
    // tests since it doesn't actually replicate what happens in real scenario (a separate dedicated dispatcher thread). If you're reading
    // this write your tests using the normal Dispatcher property. Eventually this LegacyDispatcher property will go away when we've had
    // the opportunity to re-write our tests correctly.
    private protected ProjectSnapshotManagerDispatcher LegacyDispatcher { get; }
    private protected ProjectSnapshotManagerDispatcher Dispatcher { get; }
    private protected IRazorSpanMappingService SpanMappingService { get; }
    private protected FilePathService FilePathService { get; }

    protected JsonSerializer Serializer { get; }

    public LanguageServerTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        LegacyDispatcher = new TestProjectSnapshotManagerDispatcher();
#pragma warning restore CS0618 // Type or member is obsolete

        Dispatcher = new LSPProjectSnapshotManagerDispatcher(LoggerFactory);
        AddDisposable((IDisposable)Dispatcher);

        SpanMappingService = new ThrowingRazorSpanMappingService();

        Serializer = new JsonSerializer();
        Serializer.AddVSInternalExtensionConverters();
        Serializer.AddVSExtensionConverters();

        FilePathService = new FilePathService(TestLanguageServerFeatureOptions.Instance);
    }

    internal RazorRequestContext CreateRazorRequestContext(VersionedDocumentContext? documentContext, ILspServices? lspServices = null)
    {
        lspServices ??= new Mock<ILspServices>(MockBehavior.Strict).Object;

        var requestContext = new RazorRequestContext(documentContext, Logger, lspServices);

        return requestContext;
    }

    protected static RazorCodeDocument CreateCodeDocument(string text, ImmutableArray<TagHelperDescriptor> tagHelpers = default, string? filePath = null)
    {
        var fileKind = FileKinds.GetFileKindFromFilePath(filePath ?? "test.cshtml");
        tagHelpers = tagHelpers.NullToEmpty();

        if (fileKind == FileKinds.Component)
        {
            tagHelpers = tagHelpers.AddRange(RazorTestResources.BlazorServerAppTagHelpers);
        }

        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath);
        var projectEngine = RazorProjectEngine.Create(RazorExtensions.Register);
        var importDocumentName = fileKind == FileKinds.Legacy ? "_ViewImports.cshtml" : "_Imports.razor";
        var defaultImportDocument = TestRazorSourceDocument.Create(
            """
                @using BlazorApp1
                @using BlazorApp1.Pages
                @using BlazorApp1.Shared
                @using System;
                @using Microsoft.AspNetCore.Components
                @using Microsoft.AspNetCore.Components.Authorization
                @using Microsoft.AspNetCore.Components.Routing
                @using Microsoft.AspNetCore.Components.Web
                """,
            new RazorSourceDocumentProperties(importDocumentName, importDocumentName));
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, new[] { defaultImportDocument }, tagHelpers);
        return codeDocument;
    }

    internal static DocumentContextFactory CreateDocumentContextFactory(Uri documentPath, string sourceText)
    {
        var codeDocument = CreateCodeDocument(sourceText);
        return CreateDocumentContextFactory(documentPath, codeDocument);
    }

    internal static VersionedDocumentContext CreateDocumentContext(Uri documentPath, RazorCodeDocument codeDocument)
    {
        return TestDocumentContext.From(documentPath.GetAbsoluteOrUNCPath(), codeDocument, hostDocumentVersion: 1337);
    }

    internal static string GetString(SourceText sourceText)
    {
        var sourceChars = new char[sourceText.Length];
        sourceText.CopyTo(0, sourceChars, 0, sourceText.Length);
        var sourceString = new string(sourceChars);

        return sourceString;
    }

    internal static DocumentContextFactory CreateDocumentContextFactory(
        Uri documentPath,
        RazorCodeDocument codeDocument,
        bool documentFound = true)
    {
        var documentContextFactory = documentFound
            ? new TestDocumentContextFactory(documentPath.GetAbsoluteOrUNCPath(), codeDocument, version: 1337)
            : new TestDocumentContextFactory();
        return documentContextFactory;
    }

    internal static VersionedDocumentContext CreateDocumentContext(Uri uri, IDocumentSnapshot snapshot)
    {
        return new VersionedDocumentContext(uri, snapshot, projectContext: null, version: 0);
    }

    internal static IOptionsMonitor<RazorLSPOptions> GetOptionsMonitor(bool enableFormatting = true, bool formatOnType = true, bool autoInsertAttributeQuotes = true, bool colorBackground = false)
    {
        var monitor = new Mock<IOptionsMonitor<RazorLSPOptions>>(MockBehavior.Strict);
        monitor.SetupGet(m => m.CurrentValue).Returns(new RazorLSPOptions(default, enableFormatting, true, InsertSpaces: true, TabSize: 4, formatOnType, autoInsertAttributeQuotes, colorBackground));
        return monitor.Object;
    }

    [Obsolete("Use " + nameof(LSPProjectSnapshotManagerDispatcher))]
    private class TestProjectSnapshotManagerDispatcher : ProjectSnapshotManagerDispatcher
    {
        public TestProjectSnapshotManagerDispatcher()
        {
            DispatcherScheduler = SynchronizationContext.Current is null
                ? new ThrowingTaskScheduler()
                : TaskScheduler.FromCurrentSynchronizationContext();
        }

        public override TaskScheduler DispatcherScheduler { get; }

        private Thread Thread { get; } = Thread.CurrentThread;

        public override bool IsDispatcherThread => Thread.CurrentThread == Thread;
    }

    private class ThrowingTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Enumerable.Empty<Task>();
        }

        protected override void QueueTask(Task task)
        {
            throw new NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }
    }

    private class ThrowingRazorSpanMappingService : IRazorSpanMappingService
    {
        public Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(Document document, IEnumerable<TextSpan> spans, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
