// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[ExportLspMethod(Methods.TextDocumentCompletionName)]
internal class CompletionHandler : IRequestHandler<CompletionParams, SumType<CompletionItem[], CompletionList>?>
{
    private static readonly IReadOnlyList<string> s_razorTriggerCharacters = new[] { "@" };
    private static readonly IReadOnlyList<string> s_cSharpTriggerCharacters = new[] { " ", "(", "=", "#", ".", "<", "[", "{", "\"", "/", ":", "~" };
    private static readonly IReadOnlyList<string> s_htmlTriggerCharacters = new[] { ":", "@", "#", ".", "!", "*", ",", "(", "[", "-", "<", "&", "\\", "/", "'", "\"", "=", ":", " ", "`" };

    public static readonly IReadOnlyList<string> AllTriggerCharacters = new HashSet<string>(
        s_cSharpTriggerCharacters
            .Concat(s_htmlTriggerCharacters)
            .Concat(s_razorTriggerCharacters))
        .ToArray();

    private static readonly IReadOnlyCollection<string> s_designTimeHelpers = new string[]
    {
        "__builder",
        "__o",
        "__RazorDirectiveTokenHelpers__",
        "__tagHelperExecutionContext",
        "__tagHelperRunner",
        "__typeHelper",
        "_Imports",
        "BuildRenderTree"
    };

    private static readonly IReadOnlyCollection<CompletionItem> s_designTimeHelpersCompletionItems = GenerateCompletionItems(s_designTimeHelpers);

    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPProjectionProvider _projectionProvider;
    private readonly ITextStructureNavigatorSelectorService _textStructureNavigator;
    private readonly CompletionRequestContextCache _completionRequestContextCache;
    private readonly FormattingOptionsProvider _formattingOptionsProvider;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public CompletionHandler(
        JoinableTaskContext joinableTaskContext,
        LSPRequestInvoker requestInvoker,
        LSPDocumentManager documentManager,
        LSPProjectionProvider projectionProvider,
        ITextStructureNavigatorSelectorService textStructureNavigator,
        CompletionRequestContextCache completionRequestContextCache,
        FormattingOptionsProvider formattingOptionsProvider,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (projectionProvider is null)
        {
            throw new ArgumentNullException(nameof(projectionProvider));
        }

        if (textStructureNavigator is null)
        {
            throw new ArgumentNullException(nameof(textStructureNavigator));
        }

        if (completionRequestContextCache is null)
        {
            throw new ArgumentNullException(nameof(completionRequestContextCache));
        }

        if (formattingOptionsProvider is null)
        {
            throw new ArgumentNullException(nameof(formattingOptionsProvider));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _joinableTaskFactory = joinableTaskContext.Factory;
        _requestInvoker = requestInvoker;
        _documentManager = documentManager;
        _projectionProvider = projectionProvider;
        _textStructureNavigator = textStructureNavigator;
        _completionRequestContextCache = completionRequestContextCache;
        _formattingOptionsProvider = formattingOptionsProvider;
        _logger = loggerProvider.CreateLogger(nameof(CompletionHandler));
    }

    public async Task<SumType<CompletionItem[], CompletionList>?> HandleRequestAsync(CompletionParams request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (clientCapabilities is null)
        {
            throw new ArgumentNullException(nameof(clientCapabilities));
        }

        _logger.LogInformation("Starting request for {request.TextDocument.Uri}.", request.TextDocument.Uri);

        if (!_documentManager.TryGetDocument(request.TextDocument.Uri, out var documentSnapshot))
        {
            _logger.LogWarning("Failed to find document {request.TextDocument.Uri}.", request.TextDocument.Uri);
            return null;
        }

        if (request.Context is null)
        {
            _logger.LogWarning("No Context available when document was found.");
            return null;
        }

        if (!TryGetWordExtent(request, documentSnapshot, out var wordExtent))
        {
            return null;
        }

        var projectionResult = await _projectionProvider.GetProjectionForCompletionAsync(
            documentSnapshot,
            request.Position,
            cancellationToken).ConfigureAwait(false);
        if (projectionResult is null)
        {
            return null;
        }

        var projectedPosition = projectionResult.Position;
        var projectedDocumentUri = projectionResult.Uri;
        var serverKind = projectionResult.LanguageKind == RazorLanguageKind.CSharp ? LanguageServerKind.CSharp : LanguageServerKind.Html;
        var languageServerName = projectionResult.LanguageKind == RazorLanguageKind.CSharp ? RazorLSPConstants.RazorCSharpLanguageServerName : RazorLSPConstants.HtmlLanguageServerName;
        SumType<CompletionItem[], CompletionList>? result = null;
        var provisionalCompletionResult = await TryGetProvisionalCompletionsAsync(request, documentSnapshot, projectionResult, cancellationToken).ConfigureAwait(false);
        if (provisionalCompletionResult.Success)
        {
            // This means the user has just typed a dot after some identifier such as (cursor is pipe): "DateTime.| "
            // In this case Razor interprets after the dot as Html and before it as C#.
            // We use this criteria to provide a better completion experience for what we call provisional changes.
            serverKind = LanguageServerKind.CSharp;
            if (documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var csharpVirtualDocumentSnapshot))
            {
                result = provisionalCompletionResult.Result;
                projectedDocumentUri = csharpVirtualDocumentSnapshot.Uri;
                projectionResult = provisionalCompletionResult.ProvisionalProjection;
                projectedPosition = projectionResult!.Position;
            }
            else
            {
                _logger.LogError("Could not acquire C# virtual document snapshot after provisional completion.");
            }
        }
        else if (!TriggerAppliesToProjection(request.Context, projectionResult.LanguageKind))
        {
            _logger.LogInformation("Trigger does not apply to projection.");
            return null;
        }
        else
        {
            // This is a valid non-provisional completion request.
            _logger.LogInformation("Searching for non-provisional completions, rewriting context.");

            var completionContext = RewriteContext(request.Context, projectionResult.LanguageKind);

            var completionParams = new CompletionParams()
            {
                Context = completionContext,
                Position = projectedPosition,
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = projectedDocumentUri
                }
            };

            _logger.LogInformation("Requesting non-provisional completions for {projectedDocumentUri}.", projectedDocumentUri);

            var textBuffer = serverKind.GetTextBuffer(documentSnapshot);
            var response = await _requestInvoker.ReinvokeRequestOnServerAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(
                textBuffer,
                Methods.TextDocumentCompletionName,
                languageServerName,
                completionParams,
                cancellationToken).ConfigureAwait(false);

            if (!ReinvocationResponseHelper.TryExtractResultOrLog(response, _logger, languageServerName, out result))
            {
                return null;
            }

            _logger.LogInformation("Found non-provisional completion");
        }

        if (TryConvertToCompletionList(result, out var completionList))
        {
            if (serverKind == LanguageServerKind.CSharp)
            {
                completionList = PostProcessCSharpCompletionList(request, documentSnapshot, wordExtent.Value, completionList);
            }

            var wordRange = wordExtent.HasValue && wordExtent.Value.IsSignificant ? wordExtent?.Span.AsRange() : null;
            completionList = TranslateTextEdits(request.Position, projectedPosition, wordRange, completionList);

            if (completionList.ItemDefaults?.EditRange != null)
            {
                completionList.ItemDefaults.EditRange = TranslateRange(request.Position, projectedPosition, completionList.ItemDefaults.EditRange);
            }

            var requestContext = new CompletionRequestContext(documentSnapshot.Uri, projectedDocumentUri, serverKind);
            var resultId = _completionRequestContextCache.Set(requestContext);
            SetResolveData(resultId, completionList);
        }

        if (completionList != null)
        {
            completionList = completionList is VSInternalCompletionList vsCompletionList
                ? new OptimizedVSCompletionList(vsCompletionList)
                : new OptimizedVSCompletionList(completionList);
        }

        _logger.LogInformation("Returning completion list.");
        return completionList;

        static bool TryConvertToCompletionList(SumType<CompletionItem[], CompletionList>? original, [NotNullWhen(true)] out CompletionList? completionList)
        {
            if (!original.HasValue)
            {
                completionList = null;
                return false;
            }

            if (original.Value.TryGetFirst(out var completionItems))
            {
                completionList = new CompletionList()
                {
                    Items = completionItems,
                    IsIncomplete = false
                };
            }
            else if (!original.Value.TryGetSecond(out completionList))
            {
                throw new InvalidOperationException("Could not convert Razor completion set to a completion list. This should be impossible, the completion result should be either a completion list, a set of completion items or `null`.");
            }

            return true;
        }
    }

    private bool TryGetWordExtent(CompletionParams request, LSPDocumentSnapshot documentSnapshot, [NotNullWhen(true)] out TextExtent? wordExtent)
    {
        var wordCharacterPosition = request.Position.Character;
        var invokeKind = (request.Context as VSInternalCompletionContext)?.InvokeKind;
        if (invokeKind != null && invokeKind.Value == VSInternalCompletionInvokeKind.Typing)
        {
            // Grab the character right before the word. Reason why this is important is that
            // when completion is requested it gives us the position after the typed character
            // i.e: @D|
            // Therefore, in order to properly detect the word we need to inspect the character that
            // was just typed.

            wordCharacterPosition = Math.Max(0, wordCharacterPosition - 1);
        }

        wordExtent = documentSnapshot.Snapshot.GetWordExtent(request.Position.Line, wordCharacterPosition, _textStructureNavigator);

        if (wordExtent is null)
        {
            return false;
        }

        return true;
    }

    // For C# scenarios we want to do a few post-processing steps to the completion list.
    //
    // 1. Do not pre-select any C# items. Razor is complex and just because C# may think something should be "pre-selected" doesn't mean it makes sense based off of the top-level Razor view.
    // 2. Incorporate C# keywords. Prevoiusly C# keywords like if, using, for etc. were added via VS' "Snippet" support in Razor scenarios. VS' LSP implementation doesn't currently support snippets
    //    so those keywords will be missing from the completion list if we don't forcefully add them in Razor scenarios.
    //
    //    Razor is unique here because when you type @fo| it generates something like:
    //
    //    __o = fo|;
    //
    //    This isn't an applicable scenario for C# to provide the "for" keyword; however, Razor still wants that to be applicable because if you type out the full @for keyword it will generate the
    //    appropriate C# code.
    // 3. Remove Razor intrinsic design time items. Razor adds all sorts of C# helpers like __o, __builder etc. to aid in C# compilation/understanding; however, these aren't useful in regards to C# completion.
    private CompletionList PostProcessCSharpCompletionList(
        CompletionParams request,
        LSPDocumentSnapshot documentSnapshot,
        TextExtent wordExtent,
        CompletionList completionList)
    {
        var formattingOptions = _formattingOptionsProvider.GetOptions(documentSnapshot.Uri);
        if (formattingOptions is null)
        {
            return completionList;
        }

        if (IsSimpleImplicitExpression(request, documentSnapshot, wordExtent))
        {
            completionList = RemovePreselection(completionList);

            // -1 is to account for the transition so base indentation is "|@if" instead of "@|if"
            var baseIndentation = Math.Max(GetBaseIndentation(wordExtent, formattingOptions) - 1, 0);
            completionList = IncludeCSharpSnippets(baseIndentation, completionList, formattingOptions);
        }
        //if all completion items are properties then completion is requested inside initializer syntax and we don't need to add snippets
        else if (IsWordOnEmptyLine(wordExtent, documentSnapshot) && !IsForPropertyInitializer(completionList))
        {
            var baseIndentation = GetBaseIndentation(wordExtent, formattingOptions);
            completionList = IncludeCSharpSnippets(baseIndentation, completionList, formattingOptions);
        }

        completionList = RemoveDesignTimeItems(documentSnapshot, wordExtent, completionList);

        return completionList;
    }

    private static bool IsForPropertyInitializer(CompletionList completionList)
    {
        for (var i = 0; i < completionList.Items.Length; i++)
        {
            if (completionList.Items[i].Kind != CompletionItemKind.Property)
            {
                return false;
            }
        }

        return true;
    }

    private static CompletionList IncludeCSharpSnippets(int baseIndentation, CompletionList completionList, FormattingOptions formattingOptions)
    {
        var baseIndentationString = GetIndentationString(baseIndentation, formattingOptions);
        var baseIndentationPlus1String = GetIndentationString(baseIndentation + formattingOptions.TabSize, formattingOptions);

        var forSnippet = new CompletionItem()
        {
            Label = "for (...)",
            InsertText =
                @$"for (var ${{1:i}} = 0; ${{1:i}} < ${{2:length}}; ${{1:i}}++)
{baseIndentationString}{{
{baseIndentationPlus1String}$0
{baseIndentationString}}}",
            InsertTextFormat = InsertTextFormat.Snippet,
            Kind = CompletionItemKind.Snippet,
        };
        var foreachSnippet = new CompletionItem()
        {
            Label = "foreach (...)",
            InsertText =
                @$"foreach (${{1:var}} ${{2:item}} in ${{3:collection}})
{baseIndentationString}{{
{baseIndentationPlus1String}$0
{baseIndentationString}}}",
            InsertTextFormat = InsertTextFormat.Snippet,
            Kind = CompletionItemKind.Snippet,
        };
        var ifSnippet = new CompletionItem()
        {
            Label = "if (...)",
            InsertText =
                @$"if (${{1:true}})
{baseIndentationString}{{
{baseIndentationPlus1String}$0
{baseIndentationString}}}",
            InsertTextFormat = InsertTextFormat.Snippet,
            Kind = CompletionItemKind.Snippet,
        };
        var propSnippet = new CompletionItem()
        {
            Label = "prop",
            InsertText = "public ${1:int} ${2:MyProperty} { get; set; }$0",
            InsertTextFormat = InsertTextFormat.Snippet,
            Kind = CompletionItemKind.Snippet,
        };

        var snippets = new[]
        {
            forSnippet,
            foreachSnippet,
            ifSnippet,
            propSnippet,
        };
        var newList = completionList.Items.Union(snippets, CompletionItemComparer.Instance);
        completionList.Items = newList.ToArray();
        return completionList;
    }

    private static IReadOnlyCollection<CompletionItem> GenerateCompletionItems(IReadOnlyCollection<string> completionItems)
        => completionItems.Select(item => new CompletionItem { Label = item }).ToArray();

    private static bool IsSimpleImplicitExpression(CompletionParams request, LSPDocumentSnapshot documentSnapshot, TextExtent? wordExtent)
    {
        if (request.Context is null)
        {
            return false;
        }

        if (string.Equals(request.Context.TriggerCharacter, "@", StringComparison.Ordinal))
        {
            // Completion was triggered with `@` this is always a simple implicit expression
            return true;
        }

        if (wordExtent is null)
        {
            return false;
        }

        if (!wordExtent.Value.IsSignificant)
        {
            // Word is only whitespace, definitely not an implicit expresison
            return false;
        }

        // We need to look at the item before the word because `@` at the beginning of a word is not encapsulated in that word.
        var leadingWordCharacterIndex = Math.Max(0, wordExtent.Value.Span.Start.Position - 1);
        var leadingWordCharacter = documentSnapshot.Snapshot[leadingWordCharacterIndex];
        if (leadingWordCharacter == '@')
        {
            // This means that completion was requested at something like @for|e and the word was "fore" with the previous character index being "@"
            return true;
        }

        return false;
    }

    // We should remove Razor design time helpers from C#'s completion list. If the current identifier being targeted does not start with a double
    // underscore, we trim out all items starting with "__" from the completion list. If the current identifier does start with a double underscore
    // (e.g. "__ab[||]"), we only trim out common design time helpers from the completion list.
    private static CompletionList RemoveDesignTimeItems(
        LSPDocumentSnapshot documentSnapshot,
        TextExtent? wordExtent,
        CompletionList completionList)
    {
        var filteredItems = completionList.Items.Except(s_designTimeHelpersCompletionItems, CompletionItemComparer.Instance).ToArray();

        // If the current identifier starts with "__", only trim out common design time helpers from the list.
        // In all other cases, trim out both common design time helpers and all completion items starting with "__".
        if (ShouldRemoveAllDesignTimeItems(documentSnapshot, wordExtent))
        {
            filteredItems = filteredItems.Where(item => item.Label != null && !item.Label.StartsWith("__", StringComparison.Ordinal)).ToArray();
        }

        completionList.Items = filteredItems;

        return completionList;

        static bool ShouldRemoveAllDesignTimeItems(LSPDocumentSnapshot documentSnapshot, TextExtent? wordExtent)
        {
            if (!wordExtent.HasValue)
            {
                return true;
            }

            var wordSpan = wordExtent.Value.Span;
            if (wordSpan.Length < 2)
            {
                return true;
            }

            var snapshot = documentSnapshot.Snapshot;
            var startIndex = wordSpan.Start.Position;

            if (snapshot[startIndex] == '_' && snapshot[startIndex + 1] == '_')
            {
                return false;
            }

            return true;
        }
    }

    // Internal for testing only
    internal static CompletionContext RewriteContext(CompletionContext context, RazorLanguageKind languageKind)
    {
        if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter)
        {
            // Non-triggered based completion, the existing context is valid.
            return context;
        }

        if (languageKind == RazorLanguageKind.CSharp && s_cSharpTriggerCharacters.Contains(context.TriggerCharacter))
        {
            // C# trigger character for C# content
            return context;
        }

        if (languageKind == RazorLanguageKind.Html && s_htmlTriggerCharacters.Contains(context.TriggerCharacter))
        {
            // HTML trigger character for HTML content
            return context;
        }

        // Trigger character not associated with the current langauge. Transform the context into an invoked context.
        var rewrittenContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.Invoked,
        };

        var invokeKind = (context as VSInternalCompletionContext)?.InvokeKind;
        if (invokeKind.HasValue)
        {
            rewrittenContext.InvokeKind = invokeKind.Value;
        }

        if (languageKind == RazorLanguageKind.CSharp && s_razorTriggerCharacters.Contains(context.TriggerCharacter))
        {
            // The C# language server will not return any completions for the '@' character unless we
            // send the completion request explicitly.
            rewrittenContext.InvokeKind = VSInternalCompletionInvokeKind.Explicit;
        }

        return rewrittenContext;
    }

    internal async Task<ProvisionalCompletionResult> TryGetProvisionalCompletionsAsync(
        CompletionParams request,
        LSPDocumentSnapshot documentSnapshot,
        ProjectionResult projection,
        CancellationToken cancellationToken)
    {
        if (projection.LanguageKind != RazorLanguageKind.Html ||
            request.Context is null ||
            request.Context.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
            request.Context.TriggerCharacter != ".")
        {
            _logger.LogInformation("Invalid provisional completion context.");
            return default;
        }

        if (projection.Position.Character == 0)
        {
            // We're at the start of line. Can't have provisional completions here.
            _logger.LogInformation("Start of line, invalid completion location.");
            return default;
        }

        var previousCharacterPosition = new Position(projection.Position.Line, projection.Position.Character - 1);
        var previousCharacterProjection = await _projectionProvider.GetProjectionForCompletionAsync(
            documentSnapshot,
            previousCharacterPosition,
            cancellationToken).ConfigureAwait(false);
        if (previousCharacterProjection is null ||
            previousCharacterProjection.LanguageKind != RazorLanguageKind.CSharp ||
            previousCharacterProjection.HostDocumentVersion is null)
        {
            _logger.LogInformation("Failed to find previous char projection in {previousCharacterProjection?.LanguageKind:G} at version {previousCharacterProjection?.HostDocumentVersion}.",
                previousCharacterProjection?.LanguageKind, previousCharacterProjection?.HostDocumentVersion);
            return default;
        }

        if (_documentManager is not TrackingLSPDocumentManager trackingDocumentManager)
        {
            _logger.LogInformation("Not a tracking document manager.");
            return default;
        }

        // Edit the CSharp projected document to contain a '.'. This allows C# completion to provide valid
        // completion items for moments when a user has typed a '.' that's typically interpreted as Html.
        var addProvisionalDot = new VisualStudioTextChange(previousCharacterProjection.PositionIndex, 0, ".");

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        _logger.LogInformation("Adding provisional dot.");
        trackingDocumentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
            documentSnapshot.Uri,
            new[] { addProvisionalDot },
            previousCharacterProjection.HostDocumentVersion.Value,
            state: null);

        try
        {
            var provisionalCompletionParams = new CompletionParams()
            {
                Context = request.Context,
                Position = new Position(
                    previousCharacterProjection.Position.Line,
                    previousCharacterProjection.Position.Character + 1),
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = previousCharacterProjection.Uri
                }
            };

            _logger.LogInformation("Requesting provisional completion for {previousCharacterProjection.Uri}.", previousCharacterProjection.Uri);

            var textBuffer = LanguageServerKind.CSharp.GetTextBuffer(documentSnapshot);
            var response = await _requestInvoker.ReinvokeRequestOnServerAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(
                textBuffer,
                Methods.TextDocumentCompletionName,
                RazorLSPConstants.RazorCSharpLanguageServerName,
                provisionalCompletionParams,
                cancellationToken).ConfigureAwait(true);

            if (!ReinvocationResponseHelper.TryExtractResultOrLog(response, _logger, RazorLSPConstants.RazorCSharpLanguageServerName, out var result))
            {
                return default;
            }

            var provisionalProjection = new ProjectionResult()
            {
                HostDocumentVersion = previousCharacterProjection.HostDocumentVersion,
                LanguageKind = RazorLanguageKind.CSharp,
                Position = new Position(
                    line: previousCharacterProjection.Position.Line,
                    character: previousCharacterProjection.Position.Character + 1 /* +1 for provisional dot */),
                PositionIndex = previousCharacterProjection.PositionIndex + 1 /* +1 for provisional dot */,
                Uri = previousCharacterProjection.Uri,
            };

            _logger.LogInformation("Found provisional completion.");
            return new ProvisionalCompletionResult(Success: true, provisionalProjection, result);
        }
        finally
        {
            // We no longer need the provisional change. Revert.
            var removeProvisionalDot = new VisualStudioTextChange(previousCharacterProjection.PositionIndex, 1, string.Empty);

            _logger.LogInformation("Removing provisional dot.");
            trackingDocumentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                documentSnapshot.Uri,
                new[] { removeProvisionalDot },
                previousCharacterProjection.HostDocumentVersion.Value,
                state: null);
        }
    }

    // In cases like "@{" preselection can lead to unexpected behavior, so let's exclude it.
    private static CompletionList RemovePreselection(CompletionList completionList)
    {
        foreach (var item in completionList.Items)
        {
            item.Preselect = false;
        }

        return completionList;
    }

    // The TextEdit positions returned to us from the C#/HTML language servers are positions correlating to the virtual document.
    // We need to translate these positions to apply to the Razor document instead. Performance is a big concern here, so we want to
    // make the logic as simple as possible, i.e. no asynchronous calls.
    // The current logic takes the approach of assuming the original request's position (Razor doc) correlates directly to the positions
    // returned by the C#/HTML language servers. We use this assumption (+ math) to map from the virtual (projected) doc positions ->
    // Razor doc positions.
    internal static CompletionList TranslateTextEdits(
        Position hostDocumentPosition,
        Position projectedPosition,
        Range? wordRange,
        CompletionList completionList)
    {
        var newItems = completionList.Items.Select(item => TranslateTextEdits(hostDocumentPosition, projectedPosition, wordRange, item)).ToArray();
        completionList.Items = newItems;

        return completionList;

        static CompletionItem TranslateTextEdits(Position hostDocumentPosition, Position projectedPosition, Range? wordRange, CompletionItem item)
        {
            if (item.TextEdit != null)
            {
                var translatedRange = TranslateRange(hostDocumentPosition, projectedPosition, item.TextEdit.Range);
                item.TextEdit = new TextEdit
                {
                    NewText = item.TextEdit.NewText,
                    Range = translatedRange,
                };
            }
            else if (item.AdditionalTextEdits != null)
            {
                // Additional text edits should typically only be provided at resolve time. We don't support them in the normal completion flow.
                item.AdditionalTextEdits = null;
            }

            return item;
        }
    }

    internal static Range TranslateRange(Position hostDocumentPosition, Position projectedPosition, Range textEditRange)
    {
        var offset = projectedPosition.Character - hostDocumentPosition.Character;

        var editStartPosition = textEditRange.Start;
        var translatedStartPosition = TranslatePosition(offset, hostDocumentPosition, editStartPosition);
        var editEndPosition = textEditRange.End;
        var translatedEndPosition = TranslatePosition(offset, hostDocumentPosition, editEndPosition);
        var translatedRange = new Range()
        {
            Start = translatedStartPosition,
            End = translatedEndPosition,
        };

        return translatedRange;

        static Position TranslatePosition(int offset, Position hostDocumentPosition, Position editPosition)
        {
            var translatedCharacter = editPosition.Character - offset;

            // Note: If this completion handler ever expands to deal with multi-line TextEdits, this logic will likely need to change since
            // it assumes we're only dealing with single-line TextEdits.
            var translatedPosition = new Position(hostDocumentPosition.Line, translatedCharacter);
            return translatedPosition;
        }
    }

    internal static void SetResolveData(long resultId, CompletionList completionList)
    {
        if (completionList is VSInternalCompletionList vsCompletionList && vsCompletionList.Data != null)
        {
            // Provided completion list is already wrapping completion list data, lets wrap that instead of each completion item.

            var data = new CompletionResolveData()
            {
                ResultId = resultId,
                OriginalData = vsCompletionList.Data,
            };
            vsCompletionList.Data = data;
        }
        else
        {
            for (var i = 0; i < completionList.Items.Length; i++)
            {
                var item = completionList.Items[i];
                var data = new CompletionResolveData()
                {
                    ResultId = resultId,
                    OriginalData = item.Data!,
                };
                item.Data = data;
            }
        }
    }

    // Internal for testing
    internal static bool TriggerAppliesToProjection(CompletionContext context, RazorLanguageKind languageKind)
    {
        if (languageKind == RazorLanguageKind.Razor)
        {
            // We don't handle any type of triggers in Razor pieces of the document
            return false;
        }

        if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter)
        {
            // Not a trigger character completion, allow it.
            return true;
        }

        if (!AllTriggerCharacters.Contains(context.TriggerCharacter))
        {
            // This is an auto-invoked completion from the VS LSP platform. Completions are automatically invoked upon typing identifiers
            // and are represented as CompletionTriggerKind.TriggerCharacter and have a trigger character that we have not registered for.
            return true;
        }

        if (IsApplicableTriggerCharacter(context.TriggerCharacter, languageKind))
        {
            // Trigger character is associated with the langauge at the current cursor position
            return true;
        }

        // We were triggered but the trigger character doesn't make sense for the current cursor position. Bail.
        return false;
    }

    private static bool IsApplicableTriggerCharacter(string? triggerCharacter, RazorLanguageKind languageKind)
    {
        if (s_razorTriggerCharacters.Contains(triggerCharacter))
        {
            // Razor trigger characters always transition into either C# or HTML, always note as "applicable".
            return true;
        }
        else if (languageKind == RazorLanguageKind.CSharp)
        {
            return s_cSharpTriggerCharacters.Contains(triggerCharacter);
        }
        else if (languageKind == RazorLanguageKind.Html)
        {
            return s_htmlTriggerCharacters.Contains(triggerCharacter);
        }

        // Unknown trigger character.
        return false;
    }

    private static string GetIndentationString(int indentation, FormattingOptions options)
    {
        if (options.InsertSpaces)
        {
            return new string(' ', indentation);
        }
        else
        {
            var tabs = indentation / options.TabSize;
            var tabPrefix = new string('\t', (int)tabs);

            var spaces = indentation % options.TabSize;
            var spaceSuffix = new string(' ', (int)spaces);

            var combined = string.Concat(tabPrefix, spaceSuffix);
            return combined;
        }
    }

    private static bool IsWordOnEmptyLine(TextExtent? wordExtent, LSPDocumentSnapshot documentSnapshot)
    {
        if (wordExtent is null)
        {
            return false;
        }

        var line = wordExtent.Value.Span.Start.GetContainingLine();
        var lineStart = line.Start.Position;
        var wordStart = wordExtent.Value.Span.Start.Position;

        // Is the word prefixed by whitespace?
        for (var i = lineStart; i < wordStart; i++)
        {
            if (!char.IsWhiteSpace(documentSnapshot.Snapshot[i]))
            {
                return false;
            }
        }

        var lineEnd = line.End.Position;
        var wordEnd = wordExtent.Value.Span.End.Position;

        // Is the word suffixed by whitespace?
        for (var i = wordEnd; i < lineEnd; i++)
        {
            if (!char.IsWhiteSpace(documentSnapshot.Snapshot[i]))
            {
                return false;
            }
        }

        return true;
    }

    // Internal for testing
    internal static int GetBaseIndentation(TextExtent wordExtent, FormattingOptions formattingOptions)
    {
        var line = wordExtent.Span.Start.GetContainingLine();
        var lineStart = line.Start.Position;
        var wordStart = wordExtent.Span.Start.Position;

        if (formattingOptions.InsertSpaces)
        {
            var baseIndentation = wordStart - lineStart;
            return baseIndentation;
        }
        else
        {
            var leadingTabs = 0;
            var firstNonTab = wordStart;
            for (var i = lineStart; i < wordStart; i++)
            {
                if (line.Snapshot[i] == '\t')
                {
                    leadingTabs++;
                }
                else
                {
                    firstNonTab = i;
                    break;
                }
            }

            var nonTabIndentation = wordStart - firstNonTab;
            var leadingIndentation = leadingTabs * formattingOptions.TabSize;
            var baseIndentation = leadingIndentation + nonTabIndentation;
            return baseIndentation;
        }
    }

    private class CompletionItemComparer : IEqualityComparer<CompletionItem>
    {
        public static CompletionItemComparer Instance = new();

        public bool Equals(CompletionItem x, CompletionItem y)
        {
            if (x is null && y is null)
            {
                return true;
            }
            else if (x is null || y is null)
            {
                return false;
            }

            return x.Label.Equals(y.Label, StringComparison.Ordinal);
        }

        public int GetHashCode(CompletionItem obj) => obj?.Label?.GetHashCode() ?? 0;
    }

    // Internal for testing
    internal record struct ProvisionalCompletionResult(
        bool Success,
        ProjectionResult? ProvisionalProjection,
        SumType<CompletionItem[], CompletionList>? Result);
}
