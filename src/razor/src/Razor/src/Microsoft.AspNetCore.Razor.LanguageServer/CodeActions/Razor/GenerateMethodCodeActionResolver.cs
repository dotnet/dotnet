// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using CSharpSyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;

internal class GenerateMethodCodeActionResolver : IRazorCodeActionResolver
{
    private readonly DocumentContextFactory _documentContextFactory;
    private readonly RazorLSPOptionsMonitor _razorLSPOptionsMonitor;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly IRazorFormattingService _razorFormattingService;

    private static readonly string s_beginningIndents = $"{FormattingUtilities.InitialIndent}{FormattingUtilities.Indent}";
    private static readonly string s_returnType = "$$ReturnType$$";
    private static readonly string s_methodName = "$$MethodName$$";
    private static readonly string s_eventArgs = "$$EventArgs$$";
    private static readonly string s_generateMethodTemplate =
        $"{s_beginningIndents}private {s_returnType} {s_methodName}({s_eventArgs}){Environment.NewLine}" +
        s_beginningIndents + "{" + Environment.NewLine +
        $"{s_beginningIndents}{FormattingUtilities.Indent}throw new global::System.NotImplementedException();{Environment.NewLine}" +
        s_beginningIndents + "}";

    public string Action => LanguageServerConstants.CodeActions.GenerateEventHandler;

    public GenerateMethodCodeActionResolver(
        DocumentContextFactory documentContextFactory,
        RazorLSPOptionsMonitor razorLSPOptionsMonitor,
        ClientNotifierServiceBase languageServer,
        IRazorDocumentMappingService razorDocumentMappingService,
        IRazorFormattingService razorFormattingService)
    {
        _documentContextFactory = documentContextFactory;
        _razorLSPOptionsMonitor = razorLSPOptionsMonitor;
        _languageServer = languageServer;
        _documentMappingService = razorDocumentMappingService;
        _razorFormattingService = razorFormattingService;
    }

    public async Task<WorkspaceEdit?> ResolveAsync(JObject data, CancellationToken cancellationToken)
    {
        if (data is null)
        {
            return null;
        }

        var actionParams = data.ToObject<GenerateMethodCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var documentContext = _documentContextFactory.TryCreateForOpenDocument(actionParams.Uri);
        if (documentContext is null)
        {
            return null;
        }

        var code = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var uriPath = FilePathNormalizer.Normalize(actionParams.Uri.GetAbsoluteOrUNCPath());
        var razorClassName = Path.GetFileNameWithoutExtension(uriPath);
        var codeBehindPath = $"{uriPath}.cs";

        if (!File.Exists(codeBehindPath)
            || razorClassName is null
            || !code.TryComputeNamespace(fallbackToRootNamespace: true, out var razorNamespace))
        {
            return await GenerateMethodInCodeBlockAsync(
                code,
                actionParams,
                documentContext,
                razorNamespace: null,
                razorClassName,
                cancellationToken).ConfigureAwait(false);
        }

        var content = File.ReadAllText(codeBehindPath);
        if (GetCSharpClassDeclarationSyntax(content, razorNamespace, razorClassName) is not { } @class)
        {
            // The code behind file is malformed, generate the code in the razor file instead.
            return await GenerateMethodInCodeBlockAsync(
                code,
                actionParams,
                documentContext,
                razorNamespace,
                razorClassName,
                cancellationToken).ConfigureAwait(false);
        }

        var codeBehindUri = new UriBuilder
        {
            Scheme = Uri.UriSchemeFile,
            Path = codeBehindPath,
            Host = string.Empty,
        }.Uri;

        var codeBehindTextDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier() { Uri = codeBehindUri };

        var templateWithMethodSignature = PopulateMethodSignature(documentContext, actionParams);
        var classLocationLineSpan = @class.GetLocation().GetLineSpan();
        var formattedMethod = FormattingUtilities.AddIndentationToMethod(
            templateWithMethodSignature,
            _razorLSPOptionsMonitor.CurrentValue,
            @class.SpanStart,
            classLocationLineSpan.StartLinePosition.Character,
            content);

        var insertPosition = new Position(classLocationLineSpan.EndLinePosition.Line, 0);
        var edit = new TextEdit()
        {
            Range = new Range { Start = insertPosition, End = insertPosition },
            NewText = $"{formattedMethod}{Environment.NewLine}"
        };

        var delegatedParams = new DelegatedSimplifyMethodParams(
            new TextDocumentIdentifierAndVersion(new TextDocumentIdentifier() { Uri = codeBehindUri}, 1),
            RequiresVirtualDocument: false,
            edit);

        var result = await _languageServer.SendRequestAsync<DelegatedSimplifyMethodParams, TextEdit[]?>(
            CustomMessageNames.RazorSimplifyMethodEndpointName,
            delegatedParams,
            cancellationToken).ConfigureAwait(false)
            ?? new TextEdit[] { edit };

        var codeBehindTextDocEdit = new TextDocumentEdit()
        {
            TextDocument = codeBehindTextDocumentIdentifier,
            Edits = result
        };

        return new WorkspaceEdit() { DocumentChanges = new[] { codeBehindTextDocEdit } };
    }

    private async Task<WorkspaceEdit> GenerateMethodInCodeBlockAsync(
        RazorCodeDocument code,
        GenerateMethodCodeActionParams actionParams,
        VersionedDocumentContext documentContext,
        string? razorNamespace,
        string? razorClassName,
        CancellationToken cancellationToken)
    {
        var templateWithMethodSignature = PopulateMethodSignature(documentContext, actionParams);
        var edits = CodeBlockService.CreateFormattedTextEdit(code, templateWithMethodSignature, _razorLSPOptionsMonitor.CurrentValue);

        // If there are 3 edits, this means that there is no existing @code block, so we have an edit for '@code {', the method stub, and '}'.
        // Otherwise, a singular edit means that an @code block does exist and the only edit is adding the method stub.
        var editToSendToRoslyn = edits.Length == 3 ? edits[1] : edits[0];
        if (edits.Length == 3
            && razorClassName is not null
            && (razorNamespace is not null || code.TryComputeNamespace(fallbackToRootNamespace: true, out razorNamespace))
            && GetCSharpClassDeclarationSyntax(code.GetCSharpDocument().GeneratedCode, razorNamespace, razorClassName) is { } @class)
        {
            // There is no existing @code block. This means that there is no code block source mapping in the generated C# document
            // to place the code, so we cannot utilize the document mapping service and the formatting service.
            // We are going to arbitrarily place the method at the end of the class in the generated C# file to
            // just get the simplified text that comes back from Roslyn.

            var classLocationLineSpan = @class.GetLocation().GetLineSpan();
            var insertPosition = new Position(classLocationLineSpan.EndLinePosition.Line, 0);
            var tempTextEdit = new TextEdit()
            {
                NewText = editToSendToRoslyn.NewText,
                Range = new Range() { Start = insertPosition, End = insertPosition }
            };

            var delegatedParams = new DelegatedSimplifyMethodParams(documentContext.Identifier, RequiresVirtualDocument: true, tempTextEdit);
            var result = await _languageServer.SendRequestAsync<DelegatedSimplifyMethodParams, TextEdit[]?>(
                CustomMessageNames.RazorSimplifyMethodEndpointName,
                delegatedParams,
                cancellationToken).ConfigureAwait(false);

            // Roslyn should have passed back 2 edits. One that contains the simplified method stub and the other that contains the new
            // location for the class end brace since we had asked to insert the method stub at the original class end brace location.
            // We will only use the edit that contains the method stub.
            Debug.Assert(result is null || result.Length == 2, $"Unexpected response to {CustomMessageNames.RazorSimplifyMethodEndpointName} from Roslyn");
            var simplificationEdit = result?.FirstOrDefault(edit => edit.NewText.Contains("private"));
            if (simplificationEdit is not null)
            {
                // Roslyn will have removed the beginning formatting, put it back.
                var formatting = editToSendToRoslyn.NewText[0..editToSendToRoslyn.NewText.IndexOf("private")];
                editToSendToRoslyn.NewText = $"{formatting}{simplificationEdit.NewText.TrimEnd()}";
            }
        }
        else if (_documentMappingService.TryMapToGeneratedDocumentRange(code.GetCSharpDocument(), editToSendToRoslyn.Range, out var remappedRange))
        {
            // If the call to Roslyn is successful, the razor formatting service will format incorrectly if our manual formatting is present,
            // strip our manual formatting from the method so we just have a valid method signature.
            var unformattedMethodSignature = templateWithMethodSignature
                .Replace(FormattingUtilities.InitialIndent, string.Empty)
                .Replace(FormattingUtilities.Indent, string.Empty);

            var remappedEdit = new TextEdit()
            {
                NewText = unformattedMethodSignature,
                Range = remappedRange
            };

            var delegatedParams = new DelegatedSimplifyMethodParams(documentContext.Identifier, RequiresVirtualDocument: true, remappedEdit);
            var result = await _languageServer.SendRequestAsync<DelegatedSimplifyMethodParams, TextEdit[]?>(
                CustomMessageNames.RazorSimplifyMethodEndpointName,
                delegatedParams,
                cancellationToken).ConfigureAwait(false);

            if (result is not null)
            {
                var formattingOptions = new FormattingOptions()
                {
                    TabSize = _razorLSPOptionsMonitor.CurrentValue.TabSize,
                    InsertSpaces = _razorLSPOptionsMonitor.CurrentValue.InsertSpaces,
                };

                var formattedEdits = await _razorFormattingService.FormatCodeActionAsync(
                    documentContext,
                    RazorLanguageKind.CSharp,
                    result,
                    formattingOptions,
                    CancellationToken.None).ConfigureAwait(false);

                edits = formattedEdits;
            }
        }

        var razorTextDocEdit = new TextDocumentEdit()
        {
            TextDocument = new OptionalVersionedTextDocumentIdentifier() { Uri = actionParams.Uri },
            Edits = edits,
        };

        return new WorkspaceEdit() { DocumentChanges = new[] { razorTextDocEdit } };
    }

    private static string PopulateMethodSignature(VersionedDocumentContext documentContext, GenerateMethodCodeActionParams actionParams)
    {
        var templateWithMethodSignature = s_generateMethodTemplate.Replace(s_methodName, actionParams.MethodName);

        var returnType = actionParams.IsAsync ? "global::System.Threading.Tasks.Task" : "void";
        templateWithMethodSignature = templateWithMethodSignature.Replace(s_returnType, returnType);

        var eventTagHelper = documentContext.Project.TagHelpers
            .FirstOrDefault(th => th.Name == actionParams.EventName && th.IsEventHandlerTagHelper() && th.GetEventArgsType() is not null);
        var eventArgsType = eventTagHelper is null
            ? string.Empty // Couldn't find the params, generate no params instead.
            : $"global::{eventTagHelper.GetEventArgsType()} e";

        return templateWithMethodSignature.Replace(s_eventArgs, eventArgsType);
    }

    private static ClassDeclarationSyntax? GetCSharpClassDeclarationSyntax(string csharpContent, string razorNamespace, string razorClassName)
    {
        var mock = CSharpSyntaxFactory.ParseCompilationUnit(csharpContent);
        var @namespace = mock.Members
            .FirstOrDefault(m => m is BaseNamespaceDeclarationSyntax { } @namespace && @namespace.Name.ToString() == razorNamespace);
        if (@namespace is null)
        {
            return null;
        }

        var @class = ((BaseNamespaceDeclarationSyntax)@namespace).Members
            .FirstOrDefault(m => m is ClassDeclarationSyntax { } @class && razorClassName == @class.Identifier.Text);
        if (@class is null)
        {
            return null;
        }

        return (ClassDeclarationSyntax)@class;
    }
}
