// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Microsoft.WebTools.Languages.Shared.ContentTypes;
using Microsoft.WebTools.Languages.Shared.Editor.Composition;
using Microsoft.WebTools.Languages.Shared.Editor.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol;
using Xunit;
using FormattingOptions = Microsoft.VisualStudio.LanguageServer.Protocol.FormattingOptions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class FormattingLanguageServerClient : ClientNotifierServiceBase
{
    private readonly Dictionary<string, RazorCodeDocument> _documents = new Dictionary<string, RazorCodeDocument>();

    public InitializeResult ServerSettings => throw new NotImplementedException();

    public void AddCodeDocument(RazorCodeDocument codeDocument)
    {
        var path = FilePathNormalizer.Normalize(codeDocument.Source.FilePath);
        _documents.TryAdd("/" + path, codeDocument);
    }

    private RazorDocumentFormattingResponse Format(DocumentOnTypeFormattingParams @params)
    {
        var generatedHtml = GetGeneratedHtml(@params.TextDocument.Uri);
        var generatedHtmlSource = SourceText.From(generatedHtml, Encoding.UTF8);
        var absoluteIndex = @params.Position.GetRequiredAbsoluteIndex(generatedHtmlSource, null);

        var request = $@"{{
    ""Options"": {{
        ""UseSpaces"": {(@params.Options.InsertSpaces ? "true" : "false")},
        ""TabSize"": {@params.Options.TabSize},
        ""IndentSize"": {@params.Options.TabSize}
    }},
    ""Uri"": ""{@params.TextDocument.Uri}"",
    ""GeneratedChanges"": [
    ],
    ""OperationType"": ""FormatOnType"",
    ""SpanToFormat"": {{ ""Start"": {absoluteIndex}, ""End"": {absoluteIndex} }}
}}
";
        return CallWebToolsApplyFormattedEditsHandler(request, @params.TextDocument.Uri, generatedHtml);
    }

    private RazorDocumentFormattingResponse Format(DocumentFormattingParams @params)
    {
        var generatedHtml = GetGeneratedHtml(@params.TextDocument.Uri);

        var request = $@"{{
    ""Options"": {{
        ""UseSpaces"": {(@params.Options.InsertSpaces ? "true" : "false")},
        ""TabSize"": {@params.Options.TabSize},
        ""IndentSize"": {@params.Options.TabSize}
    }},
    ""Uri"": ""{@params.TextDocument.Uri}"",
    ""GeneratedChanges"": [
    ]
}}
";

        return CallWebToolsApplyFormattedEditsHandler(request, @params.TextDocument.Uri, generatedHtml);
    }

    private string GetGeneratedHtml(Uri uri)
    {
        var codeDocument = _documents[uri.GetAbsoluteOrUNCPath()];
        var generatedHtml = codeDocument.GetHtmlDocument().GeneratedHtml;
        return generatedHtml.Replace("\r", "", StringComparison.Ordinal).Replace("\n", "\r\n", StringComparison.Ordinal);
    }

    private RazorDocumentFormattingResponse CallWebToolsApplyFormattedEditsHandler(string serializedValue, Uri documentUri, string generatedHtml)
    {
        var response = new RazorDocumentFormattingResponse();

        response.Edits = Array.Empty<TextEdit>();

        var editHandlerAssembly = Assembly.Load("Microsoft.WebTools.Languages.LanguageServer.Server, Version=16.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        var editHandlerType = editHandlerAssembly.GetType("Microsoft.WebTools.Languages.LanguageServer.Server.Html.OperationHandlers.ApplyFormatEditsHandler", throwOnError: true);
        var bufferManagerType = editHandlerAssembly.GetType("Microsoft.WebTools.Languages.LanguageServer.Server.Shared.Buffer.BufferManager", throwOnError: true);

        var exportProvider = EditorTestCompositions.Editor.ExportProviderFactory.CreateExportProvider();
        var contentTypeService = exportProvider.GetExportedValue<IContentTypeRegistryService>();

        if (!contentTypeService.ContentTypes.Any(t => t.TypeName == HtmlContentTypeDefinition.HtmlContentType))
        {
            contentTypeService.AddContentType(HtmlContentTypeDefinition.HtmlContentType, new[] { StandardContentTypeNames.Text });
        }

        var textBufferFactoryService = exportProvider.GetExportedValue<ITextBufferFactoryService>();
        var textBufferListeners = Array.Empty<Lazy<IWebTextBufferListener, IOrderedComponentContentTypes>>();
        var bufferManager = Activator.CreateInstance(bufferManagerType, new object[] { contentTypeService, textBufferFactoryService, textBufferListeners });
        var loggerProvider = NullLoggerProvider.Instance;
        var applyFormatEditsHandler = Activator.CreateInstance(editHandlerType, new object[] { bufferManager, textBufferFactoryService, loggerProvider });

        // Make sure the buffer manager knows about the source document
        var contentTypeName = HtmlContentTypeDefinition.HtmlContentType;
        var initialContent = generatedHtml;
        var snapshotVersionFromLSP = 0;
        var oSharpDocUri = DocumentUri.From(documentUri);
        Assert.IsAssignableFrom<ITextSnapshot>(bufferManager.GetType().GetMethod("CreateBuffer").Invoke(bufferManager, new object[] { oSharpDocUri, contentTypeName, initialContent, snapshotVersionFromLSP }));

        var requestType = editHandlerAssembly.GetType("Microsoft.WebTools.Languages.LanguageServer.Server.ContainedLanguage.ApplyFormatEditsParamForOmniSharp", throwOnError: true);
        var request = JsonConvert.DeserializeObject(serializedValue, requestType);

        var resultTask = (Task)applyFormatEditsHandler.GetType()
            .GetRuntimeMethod(
                name: "Handle",
                parameters: new[] { requestType, typeof(CancellationToken) })
            .Invoke(
                obj: applyFormatEditsHandler,
                parameters: new object[] { request, CancellationToken.None });

        var result = resultTask.GetType()
            .GetProperty(nameof(Task<int>.Result))
            .GetValue(resultTask);

        var rawTextChanges = result.GetType()
            .GetProperty("TextChanges")
            .GetValue(result);

        var serializedTextChanges = JsonConvert.SerializeObject(rawTextChanges, Newtonsoft.Json.Formatting.Indented);
        var textChanges = JsonConvert.DeserializeObject<HtmlFormatterTextEdit[]>(serializedTextChanges);
        response.Edits = textChanges.Select(change => change.AsTextEdit(SourceText.From(generatedHtml))).ToArray();

        return response;
    }

    private RazorDocumentFormattingResponse Format(RazorDocumentRangeFormattingParams @params)
    {
        if (@params.Kind == RazorLanguageKind.Razor)
        {
            throw new InvalidOperationException("We shouldn't be asked to format Razor language kind.");
        }

        var options = @params.Options;
        var response = new RazorDocumentFormattingResponse();

        if (@params.Kind == RazorLanguageKind.CSharp)
        {
            var codeDocument = _documents[@params.HostDocumentFilePath];
            var csharpSourceText = codeDocument.GetCSharpSourceText();
            var csharpDocument = GetCSharpDocument(codeDocument, @params.Options);
            if (!csharpDocument.TryGetSyntaxRoot(out var root))
            {
                throw new InvalidOperationException("Couldn't get syntax root.");
            }

            var spanToFormat = @params.ProjectedRange.AsTextSpan(csharpSourceText);

            var changes = Formatter.GetFormattedTextChanges(root, spanToFormat, csharpDocument.Project.Solution.Workspace);

            response.Edits = changes.Select(c => c.AsTextEdit(csharpSourceText)).ToArray();
        }
        else
        {
            throw new InvalidOperationException($"We shouldn't be asked to format {@params.Kind} language kind.");
        }

        return response;
    }

    private struct HtmlFormatterTextEdit
    {
#pragma warning disable CS0649 // Field 'name' is never assigned to, and will always have its default value
#pragma warning disable IDE1006 // Naming Styles - This type is deserialized above so these need to be cased like this
        public int Position;
        public int Length;
        public string NewText;
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS0649 // Field 'name' is never assigned to, and will always have its default value

        public TextEdit AsTextEdit(SourceText sourceText)
        {
            var startLinePosition = sourceText.Lines.GetLinePosition(Position);
            var endLinePosition = sourceText.Lines.GetLinePosition(Position + Length);

            return new TextEdit
            {
                Range = new Range()
                {
                    Start = new Position(startLinePosition.Line, startLinePosition.Character),
                    End = new Position(endLinePosition.Line, endLinePosition.Character),
                },
                NewText = NewText,
            };
        }
    }

    private static Document GetCSharpDocument(RazorCodeDocument codeDocument, FormattingOptions options)
    {
        var adhocWorkspace = new AdhocWorkspace();
        var csharpOptions = adhocWorkspace.Options
            .WithChangedOption(CodeAnalysis.Formatting.FormattingOptions.TabSize, LanguageNames.CSharp, (int)options.TabSize)
            .WithChangedOption(CodeAnalysis.Formatting.FormattingOptions.IndentationSize, LanguageNames.CSharp, (int)options.TabSize)
            .WithChangedOption(CodeAnalysis.Formatting.FormattingOptions.UseTabs, LanguageNames.CSharp, !options.InsertSpaces);
        adhocWorkspace.TryApplyChanges(adhocWorkspace.CurrentSolution.WithOptions(csharpOptions));

        var project = adhocWorkspace.AddProject("TestProject", LanguageNames.CSharp);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocument = adhocWorkspace.AddDocument(project.Id, "TestDocument", csharpSourceText);
        return csharpDocument;
    }

    public override Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
    {
        if (@params is RazorDocumentRangeFormattingParams rangeFormattingParams &&
           string.Equals(method, "razor/rangeFormatting", StringComparison.Ordinal))
        {
            var response = Format(rangeFormattingParams);

            return Task.FromResult(Convert<TResponse>(response));
        }
        else if (@params is DocumentFormattingParams formattingParams &&
            string.Equals(method, "textDocument/formatting", StringComparison.Ordinal))
        {
            var response = Format(formattingParams);

            return Task.FromResult(Convert<TResponse>(response));
        }
        else if (@params is DocumentOnTypeFormattingParams onTypeFormattingParams &&
            string.Equals(method, "textDocument/onTypeFormatting", StringComparison.Ordinal))
        {
            var response = Format(onTypeFormattingParams);

            return Task.FromResult(Convert<TResponse>(response));
        }

        throw new NotImplementedException();
    }

    private static TResponse Convert<TResponse>(RazorDocumentFormattingResponse response)
    {
        if (response is TResponse tResp)
        {
            return tResp;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public object GetService(Type serviceType)
    {
        throw new NotImplementedException();
    }

    public bool TryGetRequest(long id, out string method, out TaskCompletionSource<JToken> pendingTask)
    {
        throw new NotImplementedException();
    }

    public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
