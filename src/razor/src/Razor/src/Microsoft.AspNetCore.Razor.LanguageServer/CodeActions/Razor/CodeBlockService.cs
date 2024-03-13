// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;

internal static class CodeBlockService
{
    /// <summary>
    ///  Creates a <see cref="TextEdit"/> that will place the formatted generated method within a @code block in the file.
    /// </summary>
    /// <param name="code">
    ///  The <see cref="RazorCodeDocument"/> of the file where the generated method will be placed.
    /// </param>
    /// <param name="templateWithMethodSignature">
    ///  The skeleton of the generated method where a <see cref="FormattingUtilities.Indent"/> should be placed
    ///  anywhere that needs to have some indenting, <see cref="FormattingUtilities.InitialIndent"/> anywhere that
    ///  needs some initial indenting.
    ///  It should look something like:
    ///   <see cref="FormattingUtilities.InitialIndent"/><see cref="FormattingUtilities.Indent"/>public void MethodName()
    ///   <see cref="FormattingUtilities.InitialIndent"/><see cref="FormattingUtilities.Indent"/>{
    ///   <see cref="FormattingUtilities.InitialIndent"/><see cref="FormattingUtilities.Indent"/><see cref="FormattingUtilities.Indent"/>throw new NotImplementedException();
    ///   <see cref="FormattingUtilities.InitialIndent"/><see cref="FormattingUtilities.Indent"/>}
    /// </param>
    /// <param name="options">
    ///  The <see cref="RazorLSPOptions"/> that contains information about indentation.
    /// </param>
    /// <returns>
    ///  A <see cref="TextEdit"/> that will place the formatted generated method within a @code block in the file.
    /// </returns>
    public static TextEdit[] CreateFormattedTextEdit(RazorCodeDocument code, string templateWithMethodSignature, RazorLSPOptions options)
    {
        var csharpCodeBlock = code.GetSyntaxTree().Root.DescendantNodes()
            .Select(RazorSyntaxFacts.TryGetCSharpCodeFromCodeBlock)
            .FirstOrDefault(n => n is not null);
        if (csharpCodeBlock is null
            || !csharpCodeBlock.Children.TryGetOpenBraceNode(out var openBrace)
            || !csharpCodeBlock.Children.TryGetCloseBraceNode(out var closeBrace))
        {
            // No well-formed @code block exists. Generate the method within an @code block at the end of the file and conduct manual formatting.
            var indentedMethod = FormattingUtilities.AddIndentationToMethod(templateWithMethodSignature, options, startingIndent: 0);
            var codeBlockStartText = "@code {" + Environment.NewLine;
            var sourceText = code.Source.Text;
            var lastCharacterLocation = sourceText.Lines[^1];
            var insertCharacterIndex = 0;
            if (!IsLineEmpty(lastCharacterLocation))
            {
                // The last line of the file is not empty so we need to place the code at the end of that line with a new line at the beginning.
                insertCharacterIndex = lastCharacterLocation.EndIncludingLineBreak - lastCharacterLocation.Start;
                codeBlockStartText = $"{Environment.NewLine}{codeBlockStartText}";
            }

            var eofPosition = new Position(lastCharacterLocation.LineNumber, insertCharacterIndex);
            var eofRange = new Range { Start = eofPosition, End = eofPosition };
            var start = new TextEdit()
            {
                NewText = codeBlockStartText,
                Range = eofRange
            };

            var method = new TextEdit()
            {
                NewText = indentedMethod,
                Range = eofRange
            };

            var end = new TextEdit()
            {
                NewText = Environment.NewLine + "}",
                Range = eofRange
            };

            return new TextEdit[] { start, method, end };
        }

        // A well-formed @code block exists, generate the method within it.

        var openBraceLocation = openBrace.GetSourceLocation(code.Source);
        var closeBraceLocation = closeBrace.GetSourceLocation(code.Source);
        var previousLineAbsoluteIndex = closeBraceLocation.AbsoluteIndex - closeBraceLocation.CharacterIndex - 1;
        var previousLine = code.Source.Text.Lines.GetLinePosition(previousLineAbsoluteIndex);

        var insertLineLocation =
            openBraceLocation.LineIndex == closeBraceLocation.LineIndex || !IsLineEmpty(code.Source.Text.Lines[previousLine.Line])
            ? closeBraceLocation
            : new SourceLocation(previousLineAbsoluteIndex, previousLine.Line, previousLine.Character);

        var formattedGeneratedMethod = FormatMethodInCodeBlock(
            code.Source,
            csharpCodeBlock.GetSourceLocation(code.Source),
            openBraceLocation.LineIndex,
            closeBraceLocation.LineIndex,
            insertLineLocation,
            options,
            templateWithMethodSignature);

        var insertCharacter = openBraceLocation.LineIndex == closeBraceLocation.LineIndex
            ? closeBraceLocation.CharacterIndex
            : 0;
        var insertPosition = new Position(insertLineLocation.LineIndex, insertCharacter);

        var edit = new TextEdit()
        {
            Range = new Range { Start = insertPosition, End = insertPosition },
            NewText = formattedGeneratedMethod
        };

        return new TextEdit[] { edit };
    }

    private static string FormatMethodInCodeBlock(
        RazorSourceDocument source,
        SourceLocation codeBlockSourceLocation,
        int openBraceLineIndex,
        int closeBraceLineIndex,
        SourceLocation insertLocation,
        RazorLSPOptions options,
        string method)
    {
        // The absolute index and character index of the code block's location points to the end of '@code'.
        // For indenting, we want to know about what characters there are before that, so we need to - 5.
        var codeBlockStartAbsoluteIndex = codeBlockSourceLocation.AbsoluteIndex - 5;
        var numCharacterBefore = codeBlockSourceLocation.CharacterIndex - 5;
        var formattedGeneratedMethod = FormattingUtilities.AddIndentationToMethod(
            method,
            options,
            codeBlockStartAbsoluteIndex,
            numCharacterBefore,
            source);
        if (openBraceLineIndex == closeBraceLineIndex)
        {
            // The @code block's '{' and '}' are on the same line, we'll need to add a new line to both the beginning and end of the generated code.
            return $"{Environment.NewLine}{formattedGeneratedMethod}{Environment.NewLine}";
        }

        if (insertLocation.LineIndex == closeBraceLineIndex)
        {
            // We will be inserting the code on the same line as the '}' of the code block. Make sure to add a new line to separate these.
            formattedGeneratedMethod += Environment.NewLine;
        }

        if (insertLocation.LineIndex - 1 == openBraceLineIndex)
        {
            // There is no other code in the @code block, no need to continue formatting.
            return formattedGeneratedMethod;
        }

        // There is other code that exists in the @code block. Look at what is above the line we are going to insert at.
        // If there is code above it, we need to add a new line at the beginning the generated code.
        var previousLine = source.Text.Lines.GetLineFromPosition(insertLocation.AbsoluteIndex - insertLocation.CharacterIndex - 1);
        if (!IsLineEmpty(previousLine))
        {
            formattedGeneratedMethod = $"{Environment.NewLine}{formattedGeneratedMethod}";
        }

        return formattedGeneratedMethod;
    }

    /// <summary>
    ///  Determines whether the line is empty.
    /// </summary>
    /// <param name="textLine">The line to check.</param>
    /// <returns>true if the line is empty, otherwise false.</returns>
    private static bool IsLineEmpty(TextLine textLine) => textLine.Start == textLine.End;
}
