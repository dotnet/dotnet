﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration;

internal static class CodeWriterExtensions
{
    private const string InstanceMethodFormat = "{0}.{1}";

    private static readonly ReadOnlyMemory<char> s_true = "true".AsMemory();
    private static readonly ReadOnlyMemory<char> s_false = "false".AsMemory();

    private static readonly ReadOnlyMemory<char> s_zeroes = "0000000000".AsMemory(); // 10 zeros

    // This table contains string representations of numbers from 0 to 999.
    private static readonly ImmutableArray<ReadOnlyMemory<char>> s_integerTable = InitializeIntegerTable();

    private static readonly char[] CStyleStringLiteralEscapeChars =
    {
        '\r',
        '\t',
        '\"',
        '\'',
        '\\',
        '\0',
        '\n',
        '\u2028',
        '\u2029',
    };

    private static ImmutableArray<ReadOnlyMemory<char>> InitializeIntegerTable()
    {
        var array = new ReadOnlyMemory<char>[1000];

        // Fill entries 100 to 999.
        for (var i = 100; i < 1000; i++)
        {
            array[i] = i.ToString(CultureInfo.InvariantCulture).AsMemory();
        }

        // Fill entries 10 to 99 with two-digit strings sliced from entries 110 to 199.
        for (var i = 10; i < 100; i++)
        {
            array[i] = array[i + 100][^2..];
        }

        // Fill 1 to 9 with slices of the last character from entries 11 to 19.
        for (var i = 1; i < 10; i++)
        {
            array[i] = array[i + 10][^1..];
        }

        // Finally, fill the entry for 0 with a slice from s_zeroes.
        array[0] = s_zeroes[..1];

        return ImmutableCollectionsMarshal.AsImmutableArray(array);
    }

    public static bool IsAtBeginningOfLine(this CodeWriter writer)
    {
        return writer.LastChar is '\n';
    }

    public static CodeWriter WritePadding(this CodeWriter writer, int offset, SourceSpan? span, CodeRenderingContext context)
    {
        if (span == null)
        {
            return writer;
        }

        if (context.SourceDocument.FilePath != null &&
            !string.Equals(context.SourceDocument.FilePath, span.Value.FilePath, StringComparison.OrdinalIgnoreCase))
        {
            // We don't want to generate padding for nodes from imports.
            return writer;
        }

        var basePadding = CalculatePadding();
        var resolvedPadding = Math.Max(basePadding - offset, 0);

        writer.Indent(resolvedPadding);

        return writer;

        int CalculatePadding()
        {
            var spaceCount = 0;
            for (var i = span.Value.AbsoluteIndex - 1; i >= 0; i--)
            {
                var @char = context.SourceDocument.Text[i];
                if (@char == '\n' || @char == '\r')
                {
                    break;
                }
                else
                {
                    // Note that a tab is also replaced with a single space so character indices match.
                    spaceCount++;
                }
            }

            return spaceCount;
        }
    }

    public static CodeWriter WriteVariableDeclaration(this CodeWriter writer, string type, string name, string value)
    {
        writer.Write(type).Write(" ").Write(name);
        if (!string.IsNullOrEmpty(value))
        {
            writer.Write(" = ").Write(value);
        }
        else
        {
            writer.Write(" = null");
        }

        writer.WriteLine(";");

        return writer;
    }

    public static CodeWriter WriteBooleanLiteral(this CodeWriter writer, bool value)
    {
        return writer.Write(value ? s_true : s_false);
    }

    /// <summary>
    ///  Writes an integer literal to the code writer using optimized precomputed lookup tables
    ///  and efficient grouping for large numbers. This avoids string allocation and formatting overhead.
    /// </summary>
    /// <param name="writer">The code writer to write to.</param>
    /// <param name="value">The integer value to write as a literal.</param>
    /// <returns>
    ///  The code writer for method chaining.
    /// </returns>
    /// <remarks>
    ///  Performance optimizations:
    ///  <list type="bullet">
    ///   <item>Zero is written directly from a precomputed slice</item>
    ///   <item>Numbers -999 to 999 use a precomputed lookup table</item>
    ///   <item>Larger numbers are decomposed into groups of 3 digits, each using the lookup table</item>
    ///   <item>Uses long arithmetic to handle int.MinValue correctly (avoids overflow when negating)</item>
    ///  </list>
    /// </remarks>
    public static CodeWriter WriteIntegerLiteral(this CodeWriter writer, int value)
    {
        // Handle zero as a special case
        if (value == 0)
        {
            return writer.Write(s_integerTable[0]);
        }

        var isNegative = value < 0;
        if (isNegative)
        {
            // For negative numbers, write the minus sign first
            writer.Write("-");
        }

        // Fast path: For small numbers (-999 to 999), use the precomputed lookup table directly
        if (value is > -1000 and < 1000)
        {
            var index = isNegative ? -value : value;
            return writer.Write(s_integerTable[index]);
        }

        // Slow path: For larger numbers, decompose into groups of three digits using the precomputed table.
        // This approach avoids string formatting while maintaining readability of the output.

        // Extract digits and write groups from most significant to least significant.
        // Note: Use long to handle int.MinValue correctly. Math.Abs(int.MinValue) would throw.
        var remaining = isNegative ? -(long)value : value;
        long divisor = 1;

        // Find the highest power of 1000 needed (1, 1000, 1000000, 1000000000)
        // This determines how many 3-digit groups we need
        while (remaining >= divisor * 1000)
        {
            divisor *= 1000;
        }

        // Process each group of 3 digits from most significant to least significant
        var first = true;
        while (divisor > 0)
        {
            var group = (int)(remaining / divisor);
            remaining %= divisor;
            divisor /= 1000;

            Debug.Assert(group >= 0 && group < 1000, "Digit group should be in the range [0, 999]");

            if (group == 0)
            {
                Debug.Assert(!first, "The first group should never be 0.");

                // Entire group is zero: add "000" for proper place value
                writer.Write(s_zeroes[..3]);
                continue;
            }

            if (first)
            {
                // First group: no leading zeros needed (e.g., "123" not "0123")
                writer.Write(s_integerTable[group]);
                first = false;
                continue;
            }

            // Groups after the first one with values 1-99 need leading zeros for proper formatting
            // Example: 1234567 becomes "1" + "234" + "567", but 1000067 becomes "1" + "000" + "067"
            var leadingZeros = group switch
            {
                < 10 => 2,  // 1-9: needs "00" prefix (e.g., "007")
                < 100 => 1, // 10-99: needs "0" prefix (e.g., "067") 
                _ => 0      // 100-999: no leading zeros needed
            };

            if (leadingZeros > 0)
            {
                writer.Write(s_zeroes[..leadingZeros]); // Add "00" or "0"
            }

            writer.Write(s_integerTable[group]); // Add the actual digit group
        }

        return writer;
    }

    public static CodeWriter WriteStartAssignment(this CodeWriter writer, string name)
    {
        return writer.Write(name).Write(" = ");
    }

    public static CodeWriter WriteParameterSeparator(this CodeWriter writer)
    {
        return writer.Write(", ");
    }

    public static CodeWriter WriteStartNewObject(this CodeWriter writer, string typeName)
    {
        return writer.Write("new ").Write(typeName).Write("(");
    }

    public static CodeWriter WriteStringLiteral(this CodeWriter writer, string literal)
        => writer.WriteStringLiteral(literal.AsMemory());

    public static CodeWriter WriteStringLiteral(this CodeWriter writer, ReadOnlyMemory<char> literal)
    {
        if (literal.Length >= 256 && literal.Length <= 1500 && literal.Span.IndexOf('\0') == -1)
        {
            WriteVerbatimStringLiteral(writer, literal);
        }
        else
        {
            WriteCStyleStringLiteral(writer, literal);
        }

        return writer;
    }

    public static CodeWriter WriteUsing(this CodeWriter writer, string name)
    {
        return WriteUsing(writer, name, endLine: true);
    }

    public static CodeWriter WriteUsing(this CodeWriter writer, string name, bool endLine)
    {
        writer.Write("using ");
        writer.Write(name);

        if (endLine)
        {
            writer.WriteLine(";");
        }

        return writer;
    }

    public static CodeWriter WriteEnhancedLineNumberDirective(this CodeWriter writer, SourceSpan span, int characterOffset, bool ensurePathBackslashes)
    {
        // All values here need to be offset by 1 since #line uses a 1-indexed numbering system.
        writer.Write("#line (")
            .WriteIntegerLiteral(span.LineIndex + 1)
            .Write(",")
            .WriteIntegerLiteral(span.CharacterIndex + 1)
            .Write(")-(")
            .WriteIntegerLiteral(span.LineIndex + 1 + span.LineCount)
            .Write(",")
            .WriteIntegerLiteral(span.EndCharacterIndex + 1)
            .Write(") ");

        // an offset of zero is indicated by its absence.
        if (characterOffset != 0)
        {
            writer.WriteIntegerLiteral(characterOffset).Write(" ");
        }

        return writer.Write("\"").WriteFilePath(span.FilePath, ensurePathBackslashes).WriteLine("\"");
    }

    public static CodeWriter WriteLineNumberDirective(this CodeWriter writer, SourceSpan span, bool ensurePathBackslashes)
    {
        if (writer.Length >= writer.NewLine.Length && !IsAtBeginningOfLine(writer))
        {
            writer.WriteLine();
        }

        return writer
            .Write("#line ")
            .WriteIntegerLiteral(span.LineIndex + 1)
            .Write(" \"")
            .WriteFilePath(span.FilePath, ensurePathBackslashes)
            .WriteLine("\"");
    }

    private static CodeWriter WriteFilePath(this CodeWriter writer, string filePath, bool ensurePathBackslashes)
    {
        if (!ensurePathBackslashes)
        {
            return writer.Write(filePath);
        }

        // ISSUE: https://github.com/dotnet/razor/issues/9108
        // The razor tooling normalizes paths to be forward slash based, regardless of OS.
        // If you try and use the line pragma in the design time docs to map back to the original file it will fail,
        // as the path isn't actually valid on windows. As a workaround we apply a simple heuristic to switch the
        // paths back when writing out the design time paths.
        var filePathMemory = filePath.AsMemory();
        var forwardSlashIndex = filePathMemory.Span.IndexOf('/');
        while (forwardSlashIndex >= 0)
        {
            writer.Write(filePathMemory[..forwardSlashIndex]);
            writer.Write("\\");

            filePathMemory = filePathMemory[(forwardSlashIndex + 1)..];
            forwardSlashIndex = filePathMemory.Span.IndexOf('/');
        }

        writer.Write(filePathMemory);

        return writer;
    }

    public static CodeWriter WriteStartMethodInvocation(this CodeWriter writer, string methodName)
    {
        writer.Write(methodName);

        return writer.Write("(");
    }

    public static CodeWriter WriteStartMethodInvocation(
        this CodeWriter writer,
        [InterpolatedStringHandlerArgument(nameof(writer))] ref CodeWriter.WriteInterpolatedStringHandler handler)
    {
        writer.Write(ref handler);
        
        return writer.Write("(");
    }

    public static CodeWriter WriteEndMethodInvocation(this CodeWriter writer)
    {
        return WriteEndMethodInvocation(writer, endLine: true);
    }

    public static CodeWriter WriteEndMethodInvocation(this CodeWriter writer, bool endLine)
    {
        writer.Write(")");
        if (endLine)
        {
            writer.WriteLine(";");
        }

        return writer;
    }

    // Writes a method invocation for the given instance name.
    public static CodeWriter WriteInstanceMethodInvocation(
        this CodeWriter writer,
        string instanceName,
        string methodName,
        params string[] parameters)
    {
        if (instanceName == null)
        {
            throw new ArgumentNullException(nameof(instanceName));
        }

        if (methodName == null)
        {
            throw new ArgumentNullException(nameof(methodName));
        }

        return WriteInstanceMethodInvocation(writer, instanceName, methodName, endLine: true, parameters: parameters);
    }

    // Writes a method invocation for the given instance name.
    public static CodeWriter WriteInstanceMethodInvocation(
        this CodeWriter writer,
        string instanceName,
        string methodName,
        bool endLine,
        params string[] parameters)
    {
        if (instanceName == null)
        {
            throw new ArgumentNullException(nameof(instanceName));
        }

        if (methodName == null)
        {
            throw new ArgumentNullException(nameof(methodName));
        }

        return WriteMethodInvocation(
            writer,
            string.Format(CultureInfo.InvariantCulture, InstanceMethodFormat, instanceName, methodName),
            endLine,
            parameters);
    }

    public static CodeWriter WriteStartInstanceMethodInvocation(this CodeWriter writer, string instanceName, string methodName)
    {
        if (instanceName == null)
        {
            throw new ArgumentNullException(nameof(instanceName));
        }

        if (methodName == null)
        {
            throw new ArgumentNullException(nameof(methodName));
        }

        return WriteStartMethodInvocation(
            writer,
            string.Format(CultureInfo.InvariantCulture, InstanceMethodFormat, instanceName, methodName));
    }

    public static CodeWriter WriteField(
        this CodeWriter writer,
        ImmutableArray<string> suppressWarnings,
        ImmutableArray<string> modifiers,
        string type,
        string name)
    {
        if (!suppressWarnings.IsDefaultOrEmpty)
        {
            foreach (var suppressWarning in suppressWarnings)
            {
                writer.WriteLine($"#pragma warning disable {suppressWarning}");
            }
        }

        if (!modifiers.IsDefaultOrEmpty)
        {
            foreach (var modifier in modifiers)
            {
                writer.Write($"{modifier} ");
            }
        }

        writer.WriteLine($"{type} {name};");

        if (!suppressWarnings.IsDefaultOrEmpty)
        {
            for (var i = suppressWarnings.Length - 1; i >= 0; i--)
            {
                writer.WriteLine($"#pragma warning restore {suppressWarnings[i]}");
            }
        }

        return writer;
    }

    public static CodeWriter WriteMethodInvocation(this CodeWriter writer, string methodName, params string[] parameters)
    {
        return WriteMethodInvocation(writer, methodName, endLine: true, parameters: parameters);
    }

    public static CodeWriter WriteMethodInvocation(this CodeWriter writer, string methodName, bool endLine, params string[] parameters)
    {
        return
            WriteStartMethodInvocation(writer, methodName)
            .Write(string.Join(", ", parameters))
            .WriteEndMethodInvocation(endLine);
    }

    public static CodeWriter WritePropertyDeclaration(
        this CodeWriter writer,
        ImmutableArray<string> modifiers,
        IntermediateToken type,
        string name,
        string expressionBody,
        CodeRenderingContext context)
    {
        WritePropertyDeclarationPreamble(writer, modifiers, type.Content, name, type.Source, nameSpan: null, context);

        writer.WriteLine($" => {expressionBody};");

        return writer;
    }

    public static CodeWriter WriteAutoPropertyDeclaration(
        this CodeWriter writer,
        ImmutableArray<string> modifiers,
        string type,
        string name,
        SourceSpan? typeSpan = null,
        SourceSpan? nameSpan = null,
        CodeRenderingContext context = null,
        bool privateSetter = false,
        bool defaultValue = false)
    {
        WritePropertyDeclarationPreamble(writer, modifiers, type, name, typeSpan, nameSpan, context);

        writer.Write(" { get;");

        if (privateSetter)
        {
            writer.Write(" private");
        }

        writer.WriteLine(" set; }");

        if (defaultValue && context?.Options is { SuppressNullabilityEnforcement: false, DesignTime: false })
        {
            writer.WriteLine(" = default!;");
        }

        return writer;
    }

    private static void WritePropertyDeclarationPreamble(
        CodeWriter writer,
        ImmutableArray<string> modifiers,
        string type,
        string name,
        SourceSpan? typeSpan,
        SourceSpan? nameSpan,
        CodeRenderingContext context)
    {
        if (!modifiers.IsDefaultOrEmpty)
        {
            foreach (var modifier in modifiers)
            {
                writer.Write($"{modifier} ");
            }
        }

        WriteToken(writer, type, typeSpan, context);
        writer.Write(" ");
        WriteToken(writer, name, nameSpan, context);

        static void WriteToken(CodeWriter writer, string content, SourceSpan? span, CodeRenderingContext context)
        {
            if (span is not null && context?.Options.DesignTime == false)
            {
                using (writer.BuildEnhancedLinePragma(span, context))
                {
                    writer.Write(content);
                }
            }
            else
            {
                writer.Write(content);
            }
        }
    }

    /// <summary>
    /// Writes an "@" character if the provided identifier needs escaping in c#
    /// </summary>
    public static CodeWriter WriteIdentifierEscapeIfNeeded(this CodeWriter writer, string identifier)
    {
        if (IdentifierRequiresEscaping(identifier))
        {
            writer.Write("@");
        }
        return writer;
    }

    public static bool IdentifierRequiresEscaping(this string identifier)
    {
        return CodeAnalysis.CSharp.SyntaxFacts.GetKeywordKind(identifier) != CodeAnalysis.CSharp.SyntaxKind.None ||
            CodeAnalysis.CSharp.SyntaxFacts.GetContextualKeywordKind(identifier) != CodeAnalysis.CSharp.SyntaxKind.None;
    }

    public static CSharpCodeWritingScope BuildScope(this CodeWriter writer)
    {
        return new CSharpCodeWritingScope(writer);
    }

    public static CSharpCodeWritingScope BuildLambda(this CodeWriter writer)
        => writer.WriteLambdaHeader().BuildScope();

    public static CSharpCodeWritingScope BuildLambda(this CodeWriter writer, string parameterName)
        => writer.WriteLambdaHeader(parameterName).BuildScope();

    public static CSharpCodeWritingScope BuildLambda<T>(this CodeWriter writer, T parameterName)
        where T : IWriteableValue
        => writer.WriteLambdaHeader(parameterName).BuildScope();

    public static CSharpCodeWritingScope BuildAsyncLambda(this CodeWriter writer)
        => writer.WriteAsyncLambdaHeader().BuildScope();

    public static CSharpCodeWritingScope BuildAsyncLambda(this CodeWriter writer, string parameterName)
        => writer.WriteAsyncLambdaHeader(parameterName).BuildScope();

    public static CSharpCodeWritingScope BuildAsyncLambda<T>(this CodeWriter writer, T parameterName)
        where T : IWriteableValue
        => writer.WriteAsyncLambdaHeader(parameterName).BuildScope();

    public static CodeWriter WriteLambdaHeader(this CodeWriter writer)
        => writer.Write("() => ");

    public static CodeWriter WriteLambdaHeader(this CodeWriter writer, string parameterName)
        => writer.Write($"({parameterName}) => ");

    public static CodeWriter WriteLambdaHeader<T>(this CodeWriter writer, T parameterName)
        where T : IWriteableValue
    {
        writer.Write("(");
        parameterName.WriteTo(writer);
        writer.Write(") => ");

        return writer;
    }

    public static CodeWriter WriteAsyncLambdaHeader(this CodeWriter writer)
        => writer.Write($"async() => ");

    public static CodeWriter WriteAsyncLambdaHeader(this CodeWriter writer, string parameterName)
        => writer.Write($"async({parameterName}) => ");

    public static CodeWriter WriteAsyncLambdaHeader<T>(this CodeWriter writer, T parameterName)
        where T : IWriteableValue
    {
        writer.Write("async(");
        parameterName.WriteTo(writer);
        writer.Write(") => ");

        return writer;
    }

#nullable enable
    public static CSharpCodeWritingScope BuildNamespace(this CodeWriter writer, string? name, SourceSpan? span, CodeRenderingContext context)
    {
        if (name.IsNullOrEmpty())
        {
            return new CSharpCodeWritingScope(writer, writeBraces: false);
        }

        writer.Write("namespace ");
        if (context.Options.DesignTime || span is null)
        {
            writer.WriteLine(name);
        }
        else
        {
            writer.WriteLine();
            using (writer.BuildEnhancedLinePragma(span, context))
            {
                writer.WriteLine(name);
            }
        }
        return new CSharpCodeWritingScope(writer);
    }
#nullable disable

    public static CSharpCodeWritingScope BuildClassDeclaration(
        this CodeWriter writer,
        ImmutableArray<string> modifiers,
        string name,
        BaseTypeWithModel baseType,
        ImmutableArray<IntermediateToken> interfaces,
        ImmutableArray<TypeParameter> typeParameters,
        CodeRenderingContext context,
        bool useNullableContext = false)
    {
        Debug.Assert(context == null || context.CodeWriter == writer);

        if (useNullableContext)
        {
            writer.WriteLine("#nullable restore");
        }

        foreach (var modifier in modifiers)
        {
            writer.Write(modifier);
            writer.Write(" ");
        }

        writer.Write("class ");
        writer.Write(name);

        if (!typeParameters.IsDefaultOrEmpty)
        {
            writer.Write("<");

            for (var i = 0; i < typeParameters.Length; i++)
            {
                var typeParameter = typeParameters[i];
                WriteToken(typeParameter.Name);

                // Write ',' between parameters, but not after them
                if (i < typeParameters.Length - 1)
                {
                    writer.Write(",");
                }
            }

            writer.Write(">");
        }

        var hasBaseType = !string.IsNullOrWhiteSpace(baseType?.BaseType.Content);
        var hasInterfaces = !interfaces.IsDefaultOrEmpty;

        if (hasBaseType || hasInterfaces)
        {
            writer.Write(" : ");

            if (hasBaseType)
            {
                WriteToken(baseType.BaseType);
                WriteOptionalToken(baseType.GreaterThan);
                WriteOptionalToken(baseType.ModelType);
                WriteOptionalToken(baseType.LessThan);

                if (hasInterfaces)
                {
                    WriteParameterSeparator(writer);
                }
            }

            if (hasInterfaces)
            {
                WriteToken(interfaces[0]);
                for (var i = 1; i < interfaces.Length; i++)
                {
                    writer.Write(", ");
                    WriteToken(interfaces[i]);
                }
            }
        }

        writer.WriteLine();

        if (!typeParameters.IsDefaultOrEmpty)
        {
            foreach (var typeParameter in typeParameters)
            {
                WriteOptionalToken(typeParameter.Constraints, addLineBreak: true);
            }
        }

        if (useNullableContext)
        {
            writer.WriteLine("#nullable disable");
        }

        return new CSharpCodeWritingScope(writer);

        void WriteOptionalToken(IntermediateToken token, bool addLineBreak = false)
        {
            if (token is not null)
            {
                WriteToken(token, addLineBreak);
            }
        }

        void WriteToken(IntermediateToken token, bool addLineBreak = false)
        {
            if (token.Source is { } source)
            {
                WriteWithPragma(writer, token.Content, context, source);
            }
            else
            {
                writer.Write(token.Content);

                if (addLineBreak)
                {
                    writer.WriteLine();
                }
            }
        }

        static void WriteWithPragma(CodeWriter writer, string content, CodeRenderingContext context, SourceSpan source)
        {
            if (context.Options.DesignTime)
            {
                using (writer.BuildLinePragma(source, context))
                {
                    context.AddSourceMappingFor(source);
                    writer.Write(content);
                }
            }
            else
            {
                using (writer.BuildEnhancedLinePragma(source, context))
                {
                    writer.Write(content);
                }
            }
        }
    }

    public static CSharpCodeWritingScope BuildMethodDeclaration(
        this CodeWriter writer,
        string accessibility,
        string returnType,
        string name,
        IEnumerable<KeyValuePair<string, string>> parameters)
    {
        writer.Write(accessibility)
            .Write(" ")
            .Write(returnType)
            .Write(" ")
            .Write(name)
            .Write("(")
            .Write(string.Join(", ", parameters.Select(p => p.Key + " " + p.Value)))
            .WriteLine(")");

        return new CSharpCodeWritingScope(writer);
    }

    public static IDisposable BuildLinePragma(this CodeWriter writer, SourceSpan? span, CodeRenderingContext context, bool suppressLineDefaultAndHidden = false)
    {
        if (string.IsNullOrEmpty(span?.FilePath))
        {
            // Can't build a valid line pragma without a file path.
            return NullDisposable.Default;
        }

        return new LinePragmaWriter(writer, span.Value, context, 0, useEnhancedLinePragma: false, suppressLineDefaultAndHidden);
    }

    public static IDisposable BuildEnhancedLinePragma(this CodeWriter writer, SourceSpan? span, CodeRenderingContext context, int characterOffset = 0, bool suppressLineDefaultAndHidden = false)
    {
        if (string.IsNullOrEmpty(span?.FilePath))
        {
            // Can't build a valid line pragma without a file path.
            return NullDisposable.Default;
        }

        return new LinePragmaWriter(writer, span.Value, context, characterOffset, useEnhancedLinePragma: true, suppressLineDefaultAndHidden);
    }

    private static void WriteVerbatimStringLiteral(CodeWriter writer, ReadOnlyMemory<char> literal)
    {
        writer.Write("@\"");

        // We need to suppress indenting during the writing of the string's content. A
        // verbatim string literal could contain newlines that don't get escaped.
        var oldIndent = writer.CurrentIndent;
        writer.CurrentIndent = 0;

        // We need to find the index of each '"' (double-quote) to escape it.
        int index;
        while ((index = literal.Span.IndexOf('"')) >= 0)
        {
            writer.Write(literal[..index]);
            writer.Write("\"\"");

            literal = literal[(index + 1)..];
        }

        Debug.Assert(index == -1); // We've hit all of the double-quotes.

        // Write the remainder after the last double-quote.
        writer.Write(literal);

        writer.Write("\"");

        writer.CurrentIndent = oldIndent;
    }

    private static void WriteCStyleStringLiteral(CodeWriter writer, ReadOnlyMemory<char> literal)
    {
        // From CSharpCodeGenerator.QuoteSnippetStringCStyle in CodeDOM
        writer.Write("\"");

        // We need to find the index of each escapable character to escape it.
        int index;
        while ((index = literal.Span.IndexOfAny(CStyleStringLiteralEscapeChars)) >= 0)
        {
            writer.Write(literal[..index]);

            switch (literal.Span[index])
            {
                case '\r':
                    writer.Write("\\r");
                    break;
                case '\t':
                    writer.Write("\\t");
                    break;
                case '\"':
                    writer.Write("\\\"");
                    break;
                case '\'':
                    writer.Write("\\\'");
                    break;
                case '\\':
                    writer.Write("\\\\");
                    break;
                case '\0':
                    writer.Write("\\\0");
                    break;
                case '\n':
                    writer.Write("\\n");
                    break;
                case '\u2028':
                    writer.Write("\\u2028");
                    break;
                case '\u2029':
                    writer.Write("\\u2029");
                    break;
                default:
                    Debug.Assert(false, "Unknown escape character.");
                    break;
            }

            literal = literal[(index + 1)..];
        }

        Debug.Assert(index == -1); // We've hit all of chars that need escaping.

        // Write the remainder after the last escaped char.
        writer.Write(literal);

        writer.Write("\"");
    }

    public struct CSharpCodeWritingScope : IDisposable
    {
        private readonly CodeWriter _writer;
        private readonly bool _autoSpace;
        private readonly bool _writeBraces;
        private readonly int _tabSize;
        private int _startIndent;

        public CSharpCodeWritingScope(CodeWriter writer, bool autoSpace = true, bool writeBraces = true)
        {
            _writer = writer;
            _autoSpace = autoSpace;
            _writeBraces = writeBraces;
            _tabSize = writer.TabSize;
            _startIndent = -1; // Set in WriteStartScope

            WriteStartScope();
        }

        public void Dispose()
        {
            if (_writer is null)
            {
                return;
            }

            WriteEndScope();
        }

        private void WriteStartScope()
        {
            TryAutoSpace(" ");

            if (_writeBraces)
            {
                _writer.WriteLine("{");
            }
            else
            {
                _writer.WriteLine();
            }

            _writer.CurrentIndent += _tabSize;
            _startIndent = _writer.CurrentIndent;
        }

        private void WriteEndScope()
        {
            TryAutoSpace(_writer.NewLine);

            // Ensure the scope hasn't been modified
            if (_writer.CurrentIndent == _startIndent)
            {
                _writer.CurrentIndent -= _tabSize;
            }

            if (_writeBraces)
            {
                _writer.WriteLine("}");
            }
            else
            {
                _writer.WriteLine();
            }
        }

        private void TryAutoSpace(string spaceCharacter)
        {
            if (_autoSpace &&
                _writer.LastChar is char ch &&
                !char.IsWhiteSpace(ch))
            {
                _writer.Write(spaceCharacter);
            }
        }
    }

    private class LinePragmaWriter : IDisposable
    {
        private readonly CodeWriter _writer;
        private readonly CodeRenderingContext _context;
        private readonly int _startIndent;
        private readonly int _startLineIndex;
        private readonly SourceSpan _span;
        private readonly bool _suppressLineDefaultAndHidden;

        public LinePragmaWriter(
            CodeWriter writer,
            SourceSpan span,
            CodeRenderingContext context,
            int characterOffset,
            bool useEnhancedLinePragma = false,
            bool suppressLineDefaultAndHidden = false)
        {
            Debug.Assert(context.Options.DesignTime || useEnhancedLinePragma, "Runtime generation should only use enhanced line pragmas");

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _writer = writer;
            _context = context;
            _suppressLineDefaultAndHidden = suppressLineDefaultAndHidden;
            _startIndent = _writer.CurrentIndent;
            _writer.CurrentIndent = 0;
            _span = span;

            var endsWithNewline = _writer.LastChar is '\n';
            if (!endsWithNewline)
            {
                _writer.WriteLine();
            }

            if (!_context.Options.SuppressNullabilityEnforcement)
            {
                _writer.WriteLine("#nullable restore");
            }

            var ensurePathBackslashes = context.Options.RemapLinePragmaPathsOnWindows && PlatformInformation.IsWindows;
            if (useEnhancedLinePragma && _context.Options.UseEnhancedLinePragma)
            {
                WriteEnhancedLineNumberDirective(writer, span, characterOffset, ensurePathBackslashes);
            }
            else
            {
                WriteLineNumberDirective(writer, span, ensurePathBackslashes);
            }

            // Capture the line index after writing the #line directive.
            _startLineIndex = writer.Location.LineIndex;

            if (useEnhancedLinePragma)
            {
                // If the caller requested an enhanced line directive, but we fell back to regular ones, write out the extra padding that is required
                if (!_context.Options.UseEnhancedLinePragma)
                {
                    context.CodeWriter.WritePadding(0, span, context);
                    characterOffset = 0;
                }

                context.AddSourceMappingFor(span, characterOffset);
            }
        }

        public void Dispose()
        {
            // Need to add an additional line at the end IF there wasn't one already written.
            // This is needed to work with the C# editor's handling of #line ...
            var endsWithNewline = _writer.LastChar is '\n';

            // Always write at least 1 empty line to potentially separate code from pragmas.
            _writer.WriteLine();

            // Check if the previous empty line wasn't enough to separate code from pragmas.
            if (!endsWithNewline)
            {
                _writer.WriteLine();
            }

            var lineCount = _writer.Location.LineIndex - _startLineIndex;
            var linePragma = new LinePragma(_span.LineIndex, lineCount, _span.FilePath, _span.EndCharacterIndex, _span.EndCharacterIndex, _span.CharacterIndex);
            _context.AddLinePragma(linePragma);

            if (!_suppressLineDefaultAndHidden)
            {
                _writer
                    .WriteLine("#line default")
                    .WriteLine("#line hidden");
            }

            if (!_context.Options.SuppressNullabilityEnforcement)
            {
                _writer.WriteLine("#nullable disable");
            }

            _writer.CurrentIndent = _startIndent;

        }
    }

    private class NullDisposable : IDisposable
    {
        public static readonly NullDisposable Default = new NullDisposable();

        private NullDisposable()
        {
        }

        public void Dispose()
        {
        }
    }
}
