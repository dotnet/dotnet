// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal static class FormattingUtilities
{
    public static readonly string Indent = "$$Indent$$";
    public static readonly string InitialIndent = "$$InitialIndent$$";

    /// <summary>
    ///  Adds indenting to the method.
    /// </summary>
    /// <param name="method">
    ///  The method to add indenting to. The method should be marked with <see cref="Indent"/> where an indent is wanted
    /// </param>
    /// <param name="options">
    ///  The <see cref="RazorLSPOptions"/> that contains information about indenting.
    /// </param>
    /// <param name="startingIndent">
    ///  The size of the any existing indent.
    /// </param>
    /// <returns>The indented method.</returns>
    public static string AddIndentationToMethod(string method, RazorLSPOptions options, int startingIndent)
    {
        var indentSize = options.TabSize;
        var insertSpaces = options.InsertSpaces;
        var initialIndent = GetIndentationString(startingIndent, insertSpaces, indentSize);
        var indent = GetIndentationString(indentSize, insertSpaces, indentSize);
        return method.Replace(InitialIndent, initialIndent).Replace(Indent, indent);
    }

    /// <summary>
    ///  Adds indenting to the method.
    /// </summary>
    /// <param name="method">
    ///  The method to add indenting to. The method should be marked with <see cref="Indent"/> where an indent is wanted
    ///  and <see cref="InitialIndent"/> where some initial indent is needed.
    /// </param>
    /// <param name="options">
    ///  The <see cref="RazorLSPOptions"/> that contains information about indenting.
    /// </param>
    /// <param name="startAbsoluteIndex">
    ///  The absolute index of the beginning of the class in the C# file the method will be added to.
    /// </param>
    /// <param name="numCharacterBefore">
    ///  The number of characters on the line before where startAbsoluteIndex is in the source.
    /// </param>
    /// <param name="source">
    ///  The contents of the C# file.
    /// </param>
    /// <returns>The indented method.</returns>
    public static string AddIndentationToMethod(string method, RazorLSPOptions options, int startAbsoluteIndex, int numCharacterBefore, string source)
    {
        var startingIndent = 0;
        for (var i = 1; i <= numCharacterBefore; i++)
        {
            if (source[startAbsoluteIndex - i] == '\t')
            {
                startingIndent += options.TabSize;
            }
            else
            {
                startingIndent++;
            }
        }

        return AddIndentationToMethod(method, options, startingIndent);
    }

    /// <summary>
    ///  Adds indenting to the method.
    /// </summary>
    /// <param name="method">
    ///  The method to add indenting to. The method should be marked with <see cref="Indent"/> where an indent is wanted
    ///  and <see cref="InitialIndent"/> where some initial indent is needed.
    /// </param>
    /// <param name="options">
    ///  The <see cref="RazorLSPOptions"/> that contains information about indenting.
    /// </param>
    /// <param name="startAbsoluteIndex">
    ///  The absolute index of the beginning of the code block in the Razor file where the method will be added to.
    /// </param>
    /// <param name="numCharacterBefore">
    ///  The number of characters on the line before where startAbsoluteIndex is in the source.
    /// </param>
    /// <param name="source">
    ///  The <see cref="RazorSourceDocument"/> of the razor file the method is being added to.
    /// </param>
    /// <returns>The indented method.</returns>
    public static string AddIndentationToMethod(string method, RazorLSPOptions options, int startAbsoluteIndex, int numCharacterBefore, RazorSourceDocument source)
    {
        var startingIndent = 0;
        for (var i = 1; i <= numCharacterBefore; i++)
        {
            if (source[startAbsoluteIndex - i] == '\t')
            {
                startingIndent += options.TabSize;
            }
            else
            {
                startingIndent++;
            }
        }

        return AddIndentationToMethod(method, options, startingIndent);
    }

    /// <summary>
    /// Given a <paramref name="indentation"/> amount of characters, generate a string representing the configured indentation.
    /// </summary>
    /// <param name="indentation">An amount of characters to represent the indentation.</param>
    /// <param name="insertSpaces">Whether spaces are used for indentation.</param>
    /// <param name="tabSize">The size of a tab.</param>
    /// <returns>A whitespace string representation indentation.</returns>
    public static string GetIndentationString(int indentation, bool insertSpaces, int tabSize)
    {
        if (insertSpaces)
        {
            return new string(' ', indentation);
        }

        var tabs = indentation / tabSize;
        var tabPrefix = new string('\t', tabs);

        var spaces = indentation % tabSize;
        var spaceSuffix = new string(' ', spaces);

        var combined = string.Concat(tabPrefix, spaceSuffix);
        return combined;
    }
}
