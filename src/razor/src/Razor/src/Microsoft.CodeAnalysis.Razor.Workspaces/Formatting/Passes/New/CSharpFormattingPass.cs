// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting.New;

internal sealed partial class CSharpFormattingPass(IHostServicesProvider hostServicesProvider, ILoggerFactory loggerFactory) : IFormattingPass
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CSharpFormattingPass>();
    private readonly IHostServicesProvider _hostServicesProvider = hostServicesProvider;

    private RazorCSharpSyntaxFormattingOptions? _csharpSyntaxFormattingOptionsOverride;

    public async Task<ImmutableArray<TextChange>> ExecuteAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        // Process changes from previous passes
        var changedText = context.SourceText.WithChanges(changes);
        var changedContext = await context.WithTextAsync(changedText, cancellationToken).ConfigureAwait(false);

        // To format C# code we generate a C# document that represents the indentation semantics the user would be
        // expecting in their Razor file. See the doc comments on CSharpDocumentGenerator for more info
        var generatedDocument = CSharpDocumentGenerator.Generate(changedContext.CodeDocument, context.Options);

        var generatedCSharpText = generatedDocument.SourceText;
        _logger.LogTestOnly($"Generated C# document:\r\n{generatedCSharpText}");
        var formattedCSharpText = await FormatCSharpAsync(generatedCSharpText, context.Options.ToIndentationOptions(), cancellationToken).ConfigureAwait(false);
        _logger.LogTestOnly($"Formatted generated C# document:\r\n{formattedCSharpText}");

        // We now have a formatted C# document, and an original document, but we can't just apply the changes to the original
        // document as they come from very different places. What we want to do is go through each line of the generated document,
        // take the indentation that is in it, and apply it to the original document, and then take any formatting changes
        // on that line, and translate them across to the original document.
        // Essentially each line is split in two, with indentation on the left of the first non-whitespace char, and formatting
        // changes on the right. Sometimes we need to skip parts of the right (eg, skip the `@` in `@if`), and sometimes we skip
        // one side entirely.

        using var formattingChanges = new PooledArrayBuilder<TextChange>();
        var iFormatted = 0;
        for (var iOriginal = 0; iOriginal < changedText.Lines.Count; iOriginal++, iFormatted++)
        {
            var lineInfo = generatedDocument.LineInfo[iOriginal];

            if (lineInfo.SkipPreviousLine)
            {
                iFormatted++;
            }

            if (iFormatted >= formattedCSharpText.Lines.Count)
            {
                break;
            }

            var formattedLine = formattedCSharpText.Lines[iFormatted];
            if (lineInfo.ProcessIndentation &&
                formattedLine.GetFirstNonWhitespaceOffset() is { } formattedIndentation)
            {
                var originalLine = changedText.Lines[iOriginal];
                Debug.Assert(originalLine.GetFirstNonWhitespaceOffset().HasValue);

                var originalLineOffset = originalLine.GetFirstNonWhitespaceOffset().GetValueOrDefault();

                // First up, we take the indentation from the formatted file, and add on the Html indentation level from the line info, and
                // replace whatever was in the original file with it.
                var htmlIndentString = context.GetIndentationLevelString(lineInfo.HtmlIndentLevel);
                var indentationString = formattedCSharpText.ToString(new TextSpan(formattedLine.Start, formattedIndentation))
                    + htmlIndentString
                    + lineInfo.AdditionalIndentation;
                formattingChanges.Add(new TextChange(new TextSpan(originalLine.Start, originalLineOffset), indentationString));

                // Now we handle the formatting, which is changes to the right of the first non-whitespace character.
                if (lineInfo.ProcessFormatting)
                {
                    // The offset and length properties of the line info are relative to the indented content in their respective documents.
                    // In other words, relative to the first non-whitespace character on the line.
                    var originalStart = originalLine.Start + originalLineOffset + lineInfo.OriginOffset;
                    var length = lineInfo.FormattedLength == 0
                        ? originalLine.End - originalStart
                        : lineInfo.FormattedLength;
                    var formattedStart = formattedLine.Start + formattedIndentation + lineInfo.FormattedOffset;
                    formattingChanges.Add(new TextChange(new TextSpan(originalStart, length), formattedCSharpText.ToString(TextSpan.FromBounds(formattedStart, formattedLine.End - lineInfo.FormattedOffsetFromEndOfLine))));

                    if (lineInfo.CheckForNewLines)
                    {
                        Debug.Assert(lineInfo.FormattedLength == 0, "Can't have a FormattedLength if we're looking for new lines. The logic is incompatible.");
                        Debug.Assert(lineInfo.FormattedOffsetFromEndOfLine == 0, "Can't have a FormattedOffsetFromEndOfLine if we're looking for new lines. The logic is incompatible.");

                        // We assume Roslyn won't change anything but whitespace, so we can just apply the changes directly, but
                        // it could very well be adding whitespace in the form of newlines, for example taking "if (true) {" and
                        // making it run over two lines, or even "string Prop { get" and making it span three lines.
                        // Since we assume Roslyn won't change anything non-whitespace, we just keep inserting the formatted lines
                        // of C# until we match the original line contents.
                        // Of course, Roslyn could just as easily remove whitespace, eg making a "class Goo {" into "class Goo\n{",
                        // so whilst the same theory applies, instead of inserting formatted lines, we eat the original lines.
                        while (!changedText.NonWhitespaceContentEquals(formattedCSharpText, originalStart, originalLine.End, formattedStart, formattedLine.End))
                        {
                            // If there are more non-whitespace chars in the original line, then its something like "if (true) {" to "if (true)", so keep inserting formatted lines until we're past the brace.
                            if (FormattingUtilities.CountNonWhitespaceChars(changedText, originalStart, originalLine.End) >= FormattingUtilities.CountNonWhitespaceChars(formattedCSharpText, formattedStart, formattedLine.End))
                            {
                                iFormatted++;
                                if (iFormatted >= formattedCSharpText.Lines.Count)
                                {
                                    _logger.LogError($"Ran out of formatted lines while trying to process formatted changes after {iOriginal} lines. Abandoning further formatting to not corrupt the source file, please report this issue.");
                                    break;
                                }

                                formattedLine = formattedCSharpText.Lines[iFormatted];
                                formattingChanges.Add(new TextChange(new(originalLine.EndIncludingLineBreak, 0), htmlIndentString + formattedCSharpText.ToString(formattedLine.SpanIncludingLineBreak)));
                            }
                            else
                            {
                                // Otherwise, there are more whitespace chars in the formatted line, so "if (true)" to "if (true) {", so we need to remove the original lines until we're past the brace.
                                var oldEnd = originalLine.End;
                                iOriginal++;
                                if (iOriginal >= changedText.Lines.Count)
                                {
                                    _logger.LogError("Ran out of lines while trying to process formatted changes. Abandoning further formatting to not corrupt the source file, please report this issue.");
                                    break;
                                }

                                originalLine = changedText.Lines[iOriginal];
                                formattingChanges.Add(new TextChange(TextSpan.FromBounds(oldEnd, originalLine.End), ""));
                            }
                        }
                    }
                }
            }

            if (lineInfo.SkipNextLine)
            {
                iFormatted++;
            }
            else if (lineInfo.SkipNextLineIfBrace)
            {
                // If the next line is a brace, we skip it. This is used to skip the opening brace of a class
                // that we insert, but Roslyn settings might place on the same like as the class declaration.
                if (iFormatted + 1 < formattedCSharpText.Lines.Count &&
                    formattedCSharpText.Lines[iFormatted + 1] is { Span.Length: > 0 } nextLine &&
                    nextLine.CharAt(0) == '{')
                {
                    iFormatted++;
                }

                // On the other hand, we might insert the opening brace of a class, and Roslyn might collapse
                // it up to the previous line, so we would want to skip the next line in the original document
                // in that case. Fortunately its illegal to have `@code {\r\n {` in a Razor file, so there can't
                // be false positives here.
                if (iOriginal + 1 < changedText.Lines.Count &&
                    changedText.Lines[iOriginal + 1] is { } nextOriginalLine &&
                    nextOriginalLine.GetFirstNonWhitespaceOffset() is { } firstChar &&
                    nextOriginalLine.CharAt(firstChar) == '{')
                {
                    iOriginal++;
                }
            }
        }

        // We're finished processing the original file, which means we've done all of the indentation for the file, and we've done
        // the formatting changes for lines that are entirely C#, or start with C#, and lines that are Html or Razor. Now we process
        // the "additional changes", which is formatting for C# that is inside Html, via implicit or explicit expressions.

        // Previous to this step, all of our changes will have been in order by definition of how we go through the document, so
        // we haven't had to worry about overlaps, but now we do. In order to not loop constantly, we keep track of an extra index
        // variable for where we are in the changes, to check for overlaps.
        var iChanges = 0;
        for (; iFormatted < formattedCSharpText.Lines.Count; iFormatted++)
        {
            // Any C# that is in the middle of a line of Html/Razor will be emitted at the end of the generated document, with a
            // comment above it that encodes where it came from in the original file. We just look for the comment, and then apply
            // the next line as formatted content.
            if (CSharpDocumentGenerator.TryParseAdditionalLineComment(formattedCSharpText.Lines[iFormatted], out var start, out var length))
            {
                iFormatted++;

                // Skip ahead to where changes are likely to become relevant, to save looping the whole set every time
                while (iChanges < formattingChanges.Count)
                {
                    if (formattingChanges[iChanges].Span.End > start)
                    {
                        break;
                    }

                    iChanges++;
                }

                if (iChanges < formattingChanges.Count &&
                    formattingChanges[iChanges].Span.Contains(start))
                {
                    // To avoid overlapping changes, which Roslyn will throw on, we just have to drop this change. It gives the user
                    // something at least, and hopefully they'll report a bug for this case so we can find it.
                    _logger.LogTestOnly($"Skipping a change that would have overlapped an existing change, starting at {start} for {length} chars, overlapping a change at {formattingChanges[iChanges].Span}");
                    continue;
                }

                formattingChanges.Add(new TextChange(new TextSpan(start, length), formattedCSharpText.Lines[iFormatted].ToString()));
            }
        }

        changedText = changedText.WithChanges(formattingChanges.ToArray());
        _logger.LogTestOnly($"Final formatted document:\r\n{changedText}");

        // And we're done, we have a final set of changes to apply. BUT these are changes to the document after Html and Razor
        // formatting, and the return from this method must be changes relative to the original passed in document. The algorithm
        // above is fairly naive anyway, and a lot of them will be no-ops, so it's nice to have this final step as a filter.
        return changedText.GetTextChangesArray(context.SourceText);
    }

    private async Task<SourceText> FormatCSharpAsync(SourceText generatedCSharpText, RazorIndentationOptions options, CancellationToken cancellationToken)
    {
        using var helper = new RoslynWorkspaceHelper(_hostServicesProvider);

        var tree = CSharpSyntaxTree.ParseText(generatedCSharpText, cancellationToken: cancellationToken);
        var csharpRoot = await tree.GetRootAsync(cancellationToken).ConfigureAwait(false);
        var csharpChanges = RazorCSharpFormattingInteractionService.GetFormattedTextChanges(helper.HostWorkspaceServices, csharpRoot, csharpRoot.FullSpan, options, _csharpSyntaxFormattingOptionsOverride, cancellationToken);

        return generatedCSharpText.WithChanges(csharpChanges);
    }

    [Obsolete("Only for the syntax visualizer, do not call")]
    internal static string GetFormattingDocumentContentsForSyntaxVisualizer(RazorCodeDocument codeDocument)
        => CSharpDocumentGenerator.Generate(codeDocument, new()).SourceText.ToString();

    internal TestAccessor GetTestAccessor() => new TestAccessor(this);

    internal readonly struct TestAccessor(CSharpFormattingPass instance)
    {
        public void SetCSharpSyntaxFormattingOptionsOverride(RazorCSharpSyntaxFormattingOptions? optionsOverride)
        {
            instance._csharpSyntaxFormattingOptionsOverride = optionsOverride;
        }
    }
}
