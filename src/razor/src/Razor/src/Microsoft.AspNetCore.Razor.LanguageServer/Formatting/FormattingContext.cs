// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class FormattingContext : IDisposable
{
    private readonly AdhocWorkspaceFactory _workspaceFactory;
    private Document? _csharpWorkspaceDocument;
    private AdhocWorkspace? _csharpWorkspace;

    private IReadOnlyList<FormattingSpan>? _formattingSpans;
    private IReadOnlyDictionary<int, IndentationContext>? _indentations;
    private RazorProjectEngine? _engine;
    private IReadOnlyList<RazorSourceDocument>? _importSources;

    private FormattingContext(AdhocWorkspaceFactory workspaceFactory, Uri uri, IDocumentSnapshot originalSnapshot, RazorCodeDocument codeDocument, FormattingOptions options,
        bool isFormatOnType, bool automaticallyAddUsings, int hostDocumentIndex, char triggerCharacter)
    {
        _workspaceFactory = workspaceFactory;
        Uri = uri;
        OriginalSnapshot = originalSnapshot;
        CodeDocument = codeDocument;
        Options = options;
        IsFormatOnType = isFormatOnType;
        AutomaticallyAddUsings = automaticallyAddUsings;
        HostDocumentIndex = hostDocumentIndex;
        TriggerCharacter = triggerCharacter;
    }

    private FormattingContext(RazorProjectEngine engine, IReadOnlyList<RazorSourceDocument> importSources, AdhocWorkspaceFactory workspaceFactory, Uri uri, IDocumentSnapshot originalSnapshot, RazorCodeDocument codeDocument, FormattingOptions options,
        bool isFormatOnType, bool automaticallyAddUsings, int hostDocumentIndex, char triggerCharacter)
        : this(workspaceFactory, uri, originalSnapshot, codeDocument, options, isFormatOnType, automaticallyAddUsings, hostDocumentIndex, triggerCharacter)
    {
        _engine = engine;
        _importSources = importSources;
    }

    public static bool SkipValidateComponents { get; set; }

    public Uri Uri { get; }
    public IDocumentSnapshot OriginalSnapshot { get; }
    public RazorCodeDocument CodeDocument { get; }
    public FormattingOptions Options { get; }
    public bool IsFormatOnType { get; }
    public bool AutomaticallyAddUsings { get; }
    public int HostDocumentIndex { get; }
    public char TriggerCharacter { get; }

    public SourceText SourceText => CodeDocument.GetSourceText();

    public SourceText CSharpSourceText => CodeDocument.GetCSharpSourceText();

    public string NewLineString => Environment.NewLine;

    public Document CSharpWorkspaceDocument
    {
        get
        {
            if (_csharpWorkspaceDocument is null)
            {
                var workspace = CSharpWorkspace;
                var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
                var csharpSourceText = CodeDocument.GetCSharpSourceText();
                _csharpWorkspaceDocument = workspace.AddDocument(project.Id, "TestDocument", csharpSourceText);
            }

            return _csharpWorkspaceDocument;
        }
    }

    public AdhocWorkspace CSharpWorkspace
    {
        get
        {
            if (_csharpWorkspace is null)
            {
                var adhocWorkspace = _workspaceFactory.Create();
                var csharpOptions = GetChangedOptionSet(adhocWorkspace.Options);
                adhocWorkspace.TryApplyChanges(adhocWorkspace.CurrentSolution.WithOptions(csharpOptions));
                _csharpWorkspace = adhocWorkspace;
            }

            return _csharpWorkspace;
        }
    }

    public CodeAnalysis.Options.OptionSet GetChangedOptionSet(CodeAnalysis.Options.OptionSet optionsSet)
    {
        return optionsSet.WithChangedOption(CodeAnalysis.Formatting.FormattingOptions.TabSize, LanguageNames.CSharp, Options.TabSize)
                         .WithChangedOption(CodeAnalysis.Formatting.FormattingOptions.IndentationSize, LanguageNames.CSharp, Options.TabSize)
                         .WithChangedOption(CodeAnalysis.Formatting.FormattingOptions.UseTabs, LanguageNames.CSharp, !Options.InsertSpaces);
    }

    /// <summary>A Dictionary of int (line number) to IndentationContext.</summary>
    /// <remarks>
    /// Don't use this to discover the indentation level you should have, use
    /// <see cref="TryGetIndentationLevel(int, out int)"/> which operates on the position rather than just the line.
    /// </remarks>
    public IReadOnlyDictionary<int, IndentationContext> GetIndentations()
    {
        if (_indentations is null)
        {
            var sourceText = this.SourceText;
            var indentations = new Dictionary<int, IndentationContext>();

            var previousIndentationLevel = 0;
            for (var i = 0; i < sourceText.Lines.Count; i++)
            {
                var line = sourceText.Lines[i];
                // Get first non-whitespace character position
                var nonWsPos = line.GetFirstNonWhitespacePosition();
                var existingIndentation = (nonWsPos ?? line.End) - line.Start;

                // The existingIndentation above is measured in characters, and is used to create text edits
                // The below is measured in columns, so takes into account tab size. This is useful for creating
                // new indentation strings
                var existingIndentationSize = line.GetIndentationSize(this.Options.TabSize);

                var emptyOrWhitespaceLine = false;
                if (nonWsPos is null)
                {
                    emptyOrWhitespaceLine = true;
                    nonWsPos = line.Start;
                }

                // position now contains the first non-whitespace character or 0. Get the corresponding FormattingSpan.
                if (TryGetFormattingSpan(nonWsPos.Value, out var span))
                {
                    indentations[i] = new IndentationContext(firstSpan: span)
                    {
                        Line = i,
#if DEBUG
                        DebugOnly_LineText = line.ToString(),
#endif
                        RazorIndentationLevel = span.RazorIndentationLevel,
                        HtmlIndentationLevel = span.HtmlIndentationLevel,
                        RelativeIndentationLevel = span.IndentationLevel - previousIndentationLevel,
                        ExistingIndentation = existingIndentation,
                        ExistingIndentationSize = existingIndentationSize,
                        EmptyOrWhitespaceLine = emptyOrWhitespaceLine,
                    };
                    previousIndentationLevel = span.IndentationLevel;
                }
                else
                {
                    // Couldn't find a corresponding FormattingSpan. Happens if it is a 0 length line.
                    // Let's create a 0 length span to represent this and default it to HTML.
                    var placeholderSpan = new FormattingSpan(
                        new Language.Syntax.TextSpan(nonWsPos.Value, 0),
                        new Language.Syntax.TextSpan(nonWsPos.Value, 0),
                        FormattingSpanKind.Markup,
                        FormattingBlockKind.Markup,
                        razorIndentationLevel: 0,
                        htmlIndentationLevel: 0,
                        isInClassBody: false,
                        componentLambdaNestingLevel: 0);

                    indentations[i] = new IndentationContext(firstSpan: placeholderSpan)
                    {
                        Line = i,
#if DEBUG
                        DebugOnly_LineText = line.ToString(),
#endif
                        RazorIndentationLevel = 0,
                        HtmlIndentationLevel = 0,
                        RelativeIndentationLevel = previousIndentationLevel,
                        ExistingIndentation = existingIndentation,
                        ExistingIndentationSize = existingIndentation,
                        EmptyOrWhitespaceLine = emptyOrWhitespaceLine,
                    };
                }
            }

            _indentations = indentations;
        }

        return _indentations;
    }

    private IReadOnlyList<FormattingSpan> GetFormattingSpans()
    {
        if (_formattingSpans is null)
        {
            var syntaxTree = CodeDocument.GetSyntaxTree();
            _formattingSpans = syntaxTree.GetFormattingSpans();
        }

        return _formattingSpans;
    }

    /// <summary>
    /// Generates a string of indentation based on a specific indentation level. For instance, inside of a C# method represents 1 indentation level. A method within a class would have indentaiton level of 2 by default etc.
    /// </summary>
    /// <param name="indentationLevel">The indentation level to represent</param>
    /// <returns>A whitespace string representing the indentation level based on the configuration.</returns>
    public string GetIndentationLevelString(int indentationLevel)
    {
        var indentation = GetIndentationOffsetForLevel(indentationLevel);
        var indentationString = FormattingUtilities.GetIndentationString(indentation, Options.InsertSpaces, Options.TabSize);
        return indentationString;
    }

    /// <summary>
    /// Given a level, returns the corresponding offset.
    /// </summary>
    /// <param name="level">A value representing the indentation level.</param>
    /// <returns></returns>
    public int GetIndentationOffsetForLevel(int level)
    {
        return level * Options.TabSize;
    }

    public bool TryGetIndentationLevel(int position, out int indentationLevel)
    {
        if (TryGetFormattingSpan(position, out var span))
        {
            indentationLevel = span.IndentationLevel;
            return true;
        }

        indentationLevel = 0;
        return false;
    }

    public bool TryGetFormattingSpan(int absoluteIndex, [NotNullWhen(true)] out FormattingSpan? result)
    {
        result = null;
        var formattingspans = GetFormattingSpans();
        for (var i = 0; i < formattingspans.Count; i++)
        {
            var formattingspan = formattingspans[i];
            var span = formattingspan.Span;

            if (span.Start <= absoluteIndex && span.End >= absoluteIndex)
            {
                if (span.End == absoluteIndex && span.Length > 0)
                {
                    // We're at an edge.
                    // Non-marker spans (spans.length == 0) do not own the edges after it
                    continue;
                }

                result = formattingspan;
                return true;
            }
        }

        return false;
    }

    public void Dispose()
    {
        _csharpWorkspace?.Dispose();
        if (_csharpWorkspaceDocument != null)
        {
            _csharpWorkspaceDocument = null;
        }
    }

    public async Task<FormattingContext> WithTextAsync(SourceText changedText)
    {
        if (changedText is null)
        {
            throw new ArgumentNullException(nameof(changedText));
        }

        if (_engine is null)
        {
            await InitializeProjectEngineAsync().ConfigureAwait(false);
        }

        var changedSourceDocument = changedText.GetRazorSourceDocument(OriginalSnapshot.FilePath, OriginalSnapshot.TargetPath);

        var codeDocument = _engine!.ProcessDesignTime(changedSourceDocument, OriginalSnapshot.FileKind, _importSources, OriginalSnapshot.Project.TagHelpers);

        DEBUG_ValidateComponents(CodeDocument, codeDocument);

        var newContext = new FormattingContext(
            _engine,
            _importSources!,
            _workspaceFactory,
            Uri,
            OriginalSnapshot,
            codeDocument,
            Options,
            IsFormatOnType,
            AutomaticallyAddUsings,
            HostDocumentIndex,
            TriggerCharacter);

        return newContext;
    }

    private async Task InitializeProjectEngineAsync()
    {
        var engine = OriginalSnapshot.Project.GetProjectEngine();
        var importSources = new List<RazorSourceDocument>();

        var imports = OriginalSnapshot.GetImports();
        foreach (var import in imports)
        {
            var sourceText = await import.GetTextAsync().ConfigureAwait(false);
            var source = sourceText.GetRazorSourceDocument(import.FilePath, import.TargetPath);
            importSources.Add(source);
        }

        _engine = engine;
        _importSources = importSources;
    }

    /// <summary>
    /// It can be difficult in the testing infrastructure to correct constructs input files that work consistently across
    /// context changes, so this method validates that the number of components isn't changing due to lost tag help info.
    /// Without this guarantee its hard to reason about test behaviour/failures.
    /// </summary>
    [Conditional("DEBUG")]
    private static void DEBUG_ValidateComponents(RazorCodeDocument oldCodeDocument, RazorCodeDocument newCodeDocument)
    {
        if (SkipValidateComponents)
        {
            return;
        }

        var oldTagHelperElements = oldCodeDocument.GetSyntaxTree().Root.DescendantNodesAndSelf().OfType<Language.Syntax.MarkupTagHelperElementSyntax>().Count();
        var newTagHelperElements = newCodeDocument.GetSyntaxTree().Root.DescendantNodesAndSelf().OfType<Language.Syntax.MarkupTagHelperElementSyntax>().Count();
        Debug.Assert(oldTagHelperElements == newTagHelperElements, $"Previous context had {oldTagHelperElements} components, new only has {newTagHelperElements}.");
    }

    public static FormattingContext CreateForOnTypeFormatting(
        Uri uri,
        IDocumentSnapshot originalSnapshot,
        RazorCodeDocument codeDocument,
        FormattingOptions options,
        AdhocWorkspaceFactory workspaceFactory,
        bool automaticallyAddUsings,
        int hostDocumentIndex,
        char triggerCharacter)
    {
        return CreateCore(uri, originalSnapshot, codeDocument, options, workspaceFactory, isFormatOnType: true, automaticallyAddUsings, hostDocumentIndex, triggerCharacter);
    }

    public static FormattingContext Create(
        Uri uri,
        IDocumentSnapshot originalSnapshot,
        RazorCodeDocument codeDocument,
        FormattingOptions options,
        AdhocWorkspaceFactory workspaceFactory)
    {
        return CreateCore(uri, originalSnapshot, codeDocument, options, workspaceFactory, isFormatOnType: false, automaticallyAddUsings: false, hostDocumentIndex: 0, triggerCharacter: '\0');
    }

    private static FormattingContext CreateCore(
        Uri uri,
        IDocumentSnapshot originalSnapshot,
        RazorCodeDocument codeDocument,
        FormattingOptions options,
        AdhocWorkspaceFactory workspaceFactory,
        bool isFormatOnType,
        bool automaticallyAddUsings,
        int hostDocumentIndex,
        char triggerCharacter)
    {
        if (uri is null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (originalSnapshot is null)
        {
            throw new ArgumentNullException(nameof(originalSnapshot));
        }

        if (codeDocument is null)
        {
            throw new ArgumentNullException(nameof(codeDocument));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (workspaceFactory is null)
        {
            throw new ArgumentNullException(nameof(workspaceFactory));
        }

        // hostDocumentIndex, triggerCharacter and automaticallyAddUsings are only supported in on type formatting
        Debug.Assert(isFormatOnType || (hostDocumentIndex == 0 && triggerCharacter == '\0' && automaticallyAddUsings == false));

        var result = new FormattingContext(
            workspaceFactory,
            uri,
            originalSnapshot,
            codeDocument,
            options,
            isFormatOnType,
            automaticallyAddUsings,
            hostDocumentIndex,
            triggerCharacter
        );

        return result;
    }
}
