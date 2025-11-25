// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.CodeActions.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public abstract class CodeActionEndToEndTestBase(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    protected const string CodeBehindTestReplaceNamespace = "$$Replace_Namespace$$";

    internal async Task ValidateCodeBehindFileAsync(
        TestCode input,
        string initialCodeBehindContent,
        string expectedRazorContent,
        string expectedCodeBehindContent,
        string codeAction,
        int childActionIndex = 0)
    {
        var textSpan = input.Span;
        TextSpan? selectionRange = input.TryGetNamedSpans("selection", out var spans)
            ? spans.Single()
            : null;

        var razorFilePath = FilePathNormalizer.Normalize($"{Path.GetTempPath()}test.razor");
        var codeBehindFilePath = FilePathNormalizer.Normalize($"{Path.GetTempPath()}test.razor.cs");
        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };

        var codeDocument = CreateCodeDocument(input.Text, filePath: razorFilePath, rootNamespace: "Test", tagHelpers: CreateTagHelperDescriptors());
        var razorSourceText = codeDocument.Source.Text;
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, null!, "lsp/method", uri: null);
        File.Create(codeBehindFilePath).Close();
        try
        {
            codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);
            initialCodeBehindContent = initialCodeBehindContent.Replace(CodeBehindTestReplaceNamespace, @namespace);
            File.WriteAllText(codeBehindFilePath, initialCodeBehindContent);

            var result = await GetCodeActionsAsync(
                uri,
                textSpan,
                razorSourceText,
                requestContext,
                languageServer,
                razorProviders: [new GenerateMethodCodeActionProvider()],
                diagnostics,
                selectionRange: selectionRange);

            var codeActionToRun = GetCodeActionToRun(codeAction, childActionIndex, result);
            Assert.NotNull(codeActionToRun);

            var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, TestOutputHelper);
            var roslynCodeActionHelpers = new RoslynCodeActionHelpers(languageServer);
            var changes = await GetEditsAsync(
                codeActionToRun,
                requestContext,
                languageServer,
                optionsMonitor: null,
                CreateRazorCodeActionResolvers(roslynCodeActionHelpers, formattingService));

            var razorEdits = new List<TextChange>();
            var codeBehindEdits = new List<TextChange>();
            var codeBehindSourceText = SourceText.From(initialCodeBehindContent);
            foreach (var change in changes)
            {
                if (FilePathNormalizer.Normalize(change.TextDocument.DocumentUri.GetAbsoluteOrUNCPath()) == codeBehindFilePath)
                {
                    codeBehindEdits.AddRange(change.Edits.Select(e => codeBehindSourceText.GetTextChange((TextEdit)e)));
                }
                else
                {
                    razorEdits.AddRange(change.Edits.Select(e => razorSourceText.GetTextChange((TextEdit)e)));
                }
            }

            var actualRazorContent = razorSourceText.WithChanges(razorEdits).ToString();
            AssertEx.EqualOrDiff(expectedRazorContent, actualRazorContent);

            var actualCodeBehindContent = codeBehindSourceText.WithChanges(codeBehindEdits).ToString();
            AssertEx.EqualOrDiff(expectedCodeBehindContent.Replace(CodeBehindTestReplaceNamespace, @namespace), actualCodeBehindContent);
        }
        finally
        {
            File.Delete(codeBehindFilePath);
        }
    }

    internal Task ValidateCodeActionAsync(
        TestCode input,
        string codeAction,
        int childActionIndex = 0,
        IRazorCodeActionProvider[]? razorCodeActionProviders = null,
        Func<IRoslynCodeActionHelpers, IRazorFormattingService, IRazorCodeActionResolver[]>? codeActionResolversCreator = null,
        RazorLSPOptionsMonitor? optionsMonitor = null,
        Diagnostic[]? diagnostics = null)
    {
        return ValidateCodeActionAsync(input, expected: null, codeAction, childActionIndex, razorCodeActionProviders, codeActionResolversCreator, optionsMonitor, diagnostics);
    }

    internal async Task ValidateCodeActionAsync(
        TestCode input,
        string? expected,
        string codeAction,
        int childActionIndex = 0,
        IRazorCodeActionProvider[]? razorCodeActionProviders = null,
        Func<IRoslynCodeActionHelpers, IRazorFormattingService, IRazorCodeActionResolver[]>? codeActionResolversCreator = null,
        RazorLSPOptionsMonitor? optionsMonitor = null,
        Diagnostic[]? diagnostics = null)
    {
        var textSpan = input.Span;
        TextSpan? selectionRange = input.TryGetNamedSpans("selection", out var spans)
            ? spans.Single()
            : null;

        var razorFilePath = "C:/path/test.razor";
        var codeDocument = CreateCodeDocument(input.Text, filePath: razorFilePath, tagHelpers: CreateTagHelperDescriptors());
        var sourceText = codeDocument.Source.Text;
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, null!, "lsp/method", uri: null);

        var result = await GetCodeActionsAsync(
            uri,
            textSpan,
            sourceText,
            requestContext,
            languageServer,
            razorCodeActionProviders,
            diagnostics,
            selectionRange: selectionRange);

        var codeActionToRun = GetCodeActionToRun(codeAction, childActionIndex, result);

        if (expected is null)
        {
            Assert.Null(codeActionToRun);
            return;
        }

        Assert.NotNull(codeActionToRun);

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, TestOutputHelper, codeDocument, optionsMonitor?.CurrentValue);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(languageServer);
        var changes = await GetEditsAsync(
            codeActionToRun,
            requestContext,
            languageServer,
            optionsMonitor,
            codeActionResolversCreator?.Invoke(roslynCodeActionHelpers, formattingService) ?? []);

        var edits = new List<TextChange>();
        foreach (var change in changes)
        {
            edits.AddRange(change.Edits.Select(e => sourceText.GetTextChange((TextEdit)e)));
        }

        var actual = sourceText.WithChanges(edits).ToString();
        AssertEx.EqualOrDiff(expected, actual);
    }

    internal static VSInternalCodeAction? GetCodeActionToRun(string codeAction, int childActionIndex, SumType<Command, CodeAction>[] result)
    {
        var codeActionToRun = (VSInternalCodeAction?)result.SingleOrDefault(e => ((RazorVSInternalCodeAction)e.Value!).Name == codeAction).Value;
        if (codeActionToRun?.Children?.Length > 0)
        {
            codeActionToRun = codeActionToRun.Children[childActionIndex];
        }

        return codeActionToRun;
    }

    internal async Task<SumType<Command, CodeAction>[]> GetCodeActionsAsync(
        Uri uri,
        TextSpan textSpan,
        SourceText sourceText,
        RazorRequestContext requestContext,
        IClientConnection clientConnection,
        IRazorCodeActionProvider[]? razorProviders = null,
        Diagnostic[]? diagnostics = null,
        TextSpan? selectionRange = null)
    {
        var delegatedCodeActionsProvider = new DelegatedCodeActionsProvider(clientConnection, NoOpTelemetryReporter.Instance, LoggerFactory);

        var codeActionsService = new CodeActionsService(
            DocumentMappingService.AssumeNotNull(),
            razorCodeActionProviders: razorProviders ?? [],
            csharpCodeActionProviders:
            [
                new CSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance),
                new TypeAccessibilityCodeActionProvider()
            ],
            htmlCodeActionProviders: [],
            LanguageServerFeatureOptions.AssumeNotNull());

        var endpoint = new CodeActionEndpoint(
            codeActionsService,
            delegatedCodeActionsProvider,
            NoOpTelemetryReporter.Instance);

        // Call GetRegistration, so the endpoint knows we support resolve
        endpoint.ApplyCapabilities(new(), new VSInternalClientCapabilities
        {
            TextDocument = new TextDocumentClientCapabilities
            {
                CodeAction = new CodeActionSetting
                {
                    ResolveSupport = new CodeActionResolveSupportSetting()
                }
            }
        });

        var @params = new VSCodeActionParams
        {
            TextDocument = new VSTextDocumentIdentifier { DocumentUri = new(uri) },
            Range = sourceText.GetRange(textSpan),
            Context = new VSInternalCodeActionContext() { Diagnostics = diagnostics ?? [] }
        };

        if (selectionRange is { } selection)
        {
            // Simulate VS range vs selection range
            @params.Context.SelectionRange = sourceText.GetRange(selection);
        }

        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);
        Assert.NotNull(result);
        return result;
    }

    internal async Task<TextDocumentEdit[]> GetEditsAsync(
        VSInternalCodeAction codeActionToRun,
        RazorRequestContext requestContext,
        IClientConnection clientConnection,
        RazorLSPOptionsMonitor? optionsMonitor,
        IRazorCodeActionResolver[] razorResolvers)
    {
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, TestOutputHelper);

        var delegatedCodeActionResolver = new DelegatedCodeActionResolver(clientConnection);
        var csharpResolvers = new ICSharpCodeActionResolver[]
        {
            new CSharpCodeActionResolver(formattingService)
        };

        var htmlResolvers = Array.Empty<IHtmlCodeActionResolver>();

        optionsMonitor ??= TestRazorLSPOptionsMonitor.Create();
        var codeActionResolveService = new CodeActionResolveService(razorResolvers, csharpResolvers, htmlResolvers, LoggerFactory);
        var resolveEndpoint = new CodeActionResolveEndpoint(codeActionResolveService, delegatedCodeActionResolver, optionsMonitor);

        var resolveResult = await resolveEndpoint.HandleRequestAsync(codeActionToRun, requestContext, DisposalToken);

        Assert.NotNull(resolveResult.Edit);

        var workspaceEdit = resolveResult.Edit;
        Assert.True(workspaceEdit.TryGetTextDocumentEdits(out var documentEdits));

        return documentEdits;
    }

    internal static TagHelperCollection CreateTagHelperDescriptors()
    {
        return [.. BuildTagHelpers()];

        static IEnumerable<TagHelperDescriptor> BuildTagHelpers()
        {
            var builder = TagHelperDescriptorBuilder.Create(TagHelperKind.EventHandler, "oncontextmenu", "Microsoft.AspNetCore.Components");
            builder.SetMetadata(new EventHandlerMetadata()
            {
                EventArgsType = "Microsoft.AspNetCore.Components.Web.MouseEventArgs"
            });

            yield return builder.Build();

            builder = TagHelperDescriptorBuilder.Create(TagHelperKind.EventHandler, "onclick", "Microsoft.AspNetCore.Components");
            builder.SetMetadata(new EventHandlerMetadata()
            {
                EventArgsType = "Microsoft.AspNetCore.Components.Web.MouseEventArgs"
            });

            yield return builder.Build();

            builder = TagHelperDescriptorBuilder.Create(TagHelperKind.EventHandler, "oncopy", "Microsoft.AspNetCore.Components");
            builder.SetMetadata(new EventHandlerMetadata()
            {
                EventArgsType = "Microsoft.AspNetCore.Components.Web.ClipboardEventArgs"
            });

            yield return builder.Build();

            builder = TagHelperDescriptorBuilder.Create(TagHelperKind.Ref, "ref", "Microsoft.AspNetCore.Components");

            yield return builder.Build();

            // Sets up a component to make the following available
            // <TestGenericComponent
            //   TItem="string"
            //   OnDragStart="OnDragStart" />
            //
            //
            // @code
            // {
            //     void OnDragStart(<Microsoft.AspNetCore.Components.Web.DragEventArgs<string> args) {}
            // }
            builder = TagHelperDescriptorBuilder.CreateComponent("TestGenericComponent", "Microsoft.AspNetCore.Components");
            builder.SetTypeName(
                fullName: "Microsoft.AspNetCore.Components.TestGenericComponent",
                typeNamespace: "Microsoft.AspNetCore.Components",
                typeNameIdentifier: "TestGenericComponent");

            builder.BoundAttributeDescriptor(configure => configure
                .Name("OnDragStart")
                .TypeName("System.Action<Microsoft.AspNetCore.Components.Web.DragEventArgs<TItem>>")
                .Metadata(new PropertyMetadata
                {
                    GloballyQualifiedTypeName = "global::System.Action<global::Microsoft.AspNetCore.Components.Web.DragEventArgs<TItem>>",
                    IsDelegateSignature = true,
                    IsGenericTyped = true
                }));
            builder.BoundAttributeDescriptor(configure => configure
                .Name("TItem")
                .TypeName(typeof(Type).FullName)
                .PropertyName("TItem")
                .Metadata(new TypeParameterMetadata()));
            builder.TagMatchingRule(rule => rule.RequireTagName("TestGenericComponent"));
            yield return builder.Build();

            // Sets up a component to make the following available
            // <TestComponent OnDragStart="OnDragStart" />
            //
            //
            // @code
            // {
            //     void OnDragStart(<Microsoft.AspNetCore.Components.Web.DragEventArgs args) {}
            // }
            builder = TagHelperDescriptorBuilder.CreateComponent("TestComponent", "Microsoft.AspNetCore.Components");
            builder.SetTypeName(
                fullName: "Microsoft.AspNetCore.Components.TestComponent",
                typeNamespace: "Microsoft.AspNetCore.Components",
                typeNameIdentifier: "TestComponent");
            builder.BoundAttributeDescriptor(configure => configure
                .Name("OnDragStart")
                .TypeName("System.Action<Microsoft.AspNetCore.Components.Web.DragEventArgs>")
                .Metadata(new PropertyMetadata
                {
                    GloballyQualifiedTypeName = "global::System.Action<global::Microsoft.AspNetCore.Components.Web.DragEventArgs>",
                    IsDelegateSignature = true
                }));
            builder.TagMatchingRule(rule => rule.RequireTagName("TestComponent"));
            yield return builder.Build();
        }
    }

    internal GenerateMethodCodeActionResolver[] CreateRazorCodeActionResolvers(
        IRoslynCodeActionHelpers roslynCodeActionHelpers,
        IRazorFormattingService razorFormattingService)
            =>
            [
                new GenerateMethodCodeActionResolver(
                        roslynCodeActionHelpers,
                        new LspDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory),
                        razorFormattingService,
                        new FileSystem())
            ];
}
