// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.AutoInsert;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.LanguageServer.Protocol;
using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert.RemoteAutoInsertTextEdit?>;
using RoslynFormattingOptions = Roslyn.LanguageServer.Protocol.FormattingOptions;
using RoslynInsertTextFormat = Roslyn.LanguageServer.Protocol.InsertTextFormat;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteAutoInsertService(in ServiceArgs args)
    : RazorDocumentServiceBase(in args), IRemoteAutoInsertService
{
    internal sealed class Factory : FactoryBase<IRemoteAutoInsertService>
    {
        protected override IRemoteAutoInsertService CreateService(in ServiceArgs args)
            => new RemoteAutoInsertService(in args);
    }

    private readonly IAutoInsertService _autoInsertService = args.ExportProvider.GetExportedValue<IAutoInsertService>();
    private readonly IFilePathService _filePathService = args.ExportProvider.GetExportedValue<IFilePathService>();
    private readonly IRazorFormattingService _razorFormattingService = args.ExportProvider.GetExportedValue<IRazorFormattingService>();

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferHtmlInAttributeValuesDocumentPositionInfoStrategy.Instance;

    public ValueTask<Response> GetAutoInsertTextEditAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePosition linePosition,
        string character,
        bool autoCloseTags,
        bool formatOnType,
        bool indentWithTabs,
        int indentSize,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => TryResolveInsertionAsync(
                context,
                linePosition,
                character,
                autoCloseTags,
                formatOnType,
                indentWithTabs,
                indentSize,
                cancellationToken),
            cancellationToken);

    private async ValueTask<Response> TryResolveInsertionAsync(
        RemoteDocumentContext remoteDocumentContext,
        LinePosition linePosition,
        string character,
        bool autoCloseTags,
        bool formatOnType,
        bool indentWithTabs,
        int indentSize,
        CancellationToken cancellationToken)
    {
        var sourceText = await remoteDocumentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(linePosition, out var index))
        {
            return Response.NoFurtherHandling;
        }

        var codeDocument = await remoteDocumentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // Always try our own service first, regardless of language
        // E.g. if ">" is typed for html tag, it's actually our auto-insert provider
        // that adds closing tag instead of HTML even though we are in HTML
        if (_autoInsertService.TryResolveInsertion(
                codeDocument,
                VsLspExtensions.ToPosition(linePosition),
                character,
                autoCloseTags,
                out var insertTextEdit))
        {
            return Response.Results(RemoteAutoInsertTextEdit.FromLspInsertTextEdit(insertTextEdit));
        }

        var positionInfo = GetPositionInfo(codeDocument, index);
        var languageKind = positionInfo.LanguageKind;

        switch (languageKind)
        {
            case RazorLanguageKind.Razor:
                // If we are in Razor and got no edit from our own service, there is nothing else to do
                return Response.NoFurtherHandling;
            case RazorLanguageKind.Html:
                return AutoInsertService.HtmlAllowedAutoInsertTriggerCharacters.Contains(character)
                    ? Response.CallHtml
                    : Response.NoFurtherHandling;
            case RazorLanguageKind.CSharp:
                var mappedPosition = positionInfo.Position.ToLinePosition();
                return await TryResolveInsertionInCSharpAsync(
                        remoteDocumentContext,
                        mappedPosition,
                        character,
                        formatOnType,
                        indentWithTabs,
                        indentSize,
                        cancellationToken);
            default:
                Logger.LogError($"Unsupported language {languageKind} in {nameof(RemoteAutoInsertService)}");
                return Response.NoFurtherHandling;
        }
    }

    private async ValueTask<Response> TryResolveInsertionInCSharpAsync(
        RemoteDocumentContext remoteDocumentContext,
        LinePosition mappedPosition,
        string character,
        bool formatOnType,
        bool indentWithTabs,
        int indentSize,
        CancellationToken cancellationToken)
    {
        // Special case for C# where we use AutoInsert for two purposes:
        // 1. For XML documentation comments (filling out the template when typing "///")
        // 2. For "on type formatting" style behavior, like adjusting indentation when pressing Enter inside empty braces
        //
        // If users have turned off on-type formatting, they don't want the behavior of number 2, but its impossible to separate
        // that out from number 1. Typing "///" could just as easily adjust indentation on some unrelated code higher up in the
        // file, which is exactly the behavior users complain about.
        //
        // Therefore we are just going to no-op if the user has turned off on type formatting. Maybe one day we can make this
        // smarter, but at least the user can always turn the setting back on, type their "///", and turn it back off, without
        // having to restart VS. Not the worst compromise (hopefully!)
        if (!formatOnType)
        {
            return Response.NoFurtherHandling;
        }

        if (!AutoInsertService.CSharpAllowedAutoInsertTriggerCharacters.Contains(character))
        {
            return Response.NoFurtherHandling;
        }

        var generatedDocument = await remoteDocumentContext.Snapshot.GetGeneratedDocumentAsync().ConfigureAwait(false);
        var formattingOptions = new RoslynFormattingOptions()
        {
            InsertSpaces = !indentWithTabs,
            TabSize = indentSize
        };

        var autoInsertResponseItem = await OnAutoInsert.GetOnAutoInsertResponseAsync(
            generatedDocument,
            mappedPosition,
            character,
            formattingOptions,
            cancellationToken
        );

        if (autoInsertResponseItem is null)
        {
            return Response.NoFurtherHandling;
        }

        var razorFormattingOptions = new RazorFormattingOptions()
        {
            InsertSpaces = !indentWithTabs,
            TabSize = indentSize
        };

        var vsLspTextEdit = VsLspFactory.CreateTextEdit(
            autoInsertResponseItem.TextEdit.Range.ToLinePositionSpan(),
            autoInsertResponseItem.TextEdit.NewText);
        var mappedEdit = autoInsertResponseItem.TextEditFormat == RoslynInsertTextFormat.Snippet
            ? await _razorFormattingService.GetCSharpSnippetFormattingEditAsync(
                remoteDocumentContext,
                [vsLspTextEdit],
                razorFormattingOptions,
                cancellationToken)
            .ConfigureAwait(false)
            : await _razorFormattingService.GetSingleCSharpEditAsync(
                remoteDocumentContext,
                vsLspTextEdit,
                razorFormattingOptions,
                cancellationToken)
            .ConfigureAwait(false);

        if (mappedEdit is null)
        {
            return Response.NoFurtherHandling;
        }

        return Response.Results(
            new RemoteAutoInsertTextEdit(
                mappedEdit.Range.ToLinePositionSpan(),
                mappedEdit.NewText,
                autoInsertResponseItem.TextEditFormat));
    }
}
