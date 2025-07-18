﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.Contracts.EditAndContinue;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.EditAndContinue.UnitTests;

internal static class ActiveStatementTestHelpers
{
    public static ImmutableArray<ManagedActiveStatementDebugInfo> GetActiveStatementDebugInfosCSharp(
        string[] markedSources,
        string[]? filePaths = null,
        int[]? methodRowIds = null,
        Guid[]? modules = null,
        int[]? methodVersions = null,
        int[]? ilOffsets = null,
        ActiveStatementFlags[]? flags = null)
    {
        return ActiveStatementsDescription.GetActiveStatementDebugInfos(
            GetUnmappedActiveStatementsCSharp(markedSources, filePaths, flags),
            methodRowIds,
            modules,
            methodVersions,
            ilOffsets);
    }

    public static ImmutableArray<UnmappedActiveStatement> GetUnmappedActiveStatementsCSharp(
        string[] markedSources,
        string[]? filePaths = null,
        ActiveStatementFlags[]? flags = null)
    {
        return ActiveStatementsDescription.GetUnmappedActiveStatements(
            static (source, path) => SyntaxFactory.ParseSyntaxTree(SourceText.From(source, encoding: Encoding.UTF8, SourceHashAlgorithms.Default), path: path),
            markedSources,
            filePaths,
            extension: ".cs",
            flags);
    }

    public static string Delete(string src, string marker)
    {
        while (true)
        {
            var startStr = "/*delete" + marker;
            var endStr = "*/";
            var start = src.IndexOf(startStr);
            if (start == -1)
            {
                return src;
            }

            var end = src.IndexOf(endStr, start + startStr.Length) + endStr.Length;
            src = src[..start] + src[end..];
        }
    }

    /// <summary>
    /// Inserts new lines into the text at the position indicated by /*insert<paramref name="marker"/>[{number-of-lines-to-insert}]*/.
    /// </summary>
    public static string InsertNewLines(string src, string marker)
    {
        while (true)
        {
            var startStr = "/*insert" + marker + "[";
            var endStr = "*/";

            var start = src.IndexOf(startStr);
            if (start == -1)
            {
                return src;
            }

            var startOfLineCount = start + startStr.Length;
            var endOfLineCount = src.IndexOf(']', startOfLineCount);
            var lineCount = int.Parse(src[startOfLineCount..endOfLineCount]);

            var end = src.IndexOf(endStr, endOfLineCount) + endStr.Length;

            src = src[..start] + string.Join("", Enumerable.Repeat(Environment.NewLine, lineCount)) + src[end..];
        }
    }

    public static string Update(string src, string marker)
        => InsertNewLines(Delete(src, marker), marker);

    public static string InspectActiveStatement(ActiveStatement statement)
        => $"{statement.Id.Ordinal}: {statement.FileSpan} flags=[{statement.Flags}]";

    public static string InspectActiveStatementAndInstruction(ActiveStatement statement)
        => InspectActiveStatement(statement) + " " + statement.InstructionId.GetDebuggerDisplay();

    public static string InspectActiveStatementAndInstruction(ActiveStatement statement, SourceText text)
        => InspectActiveStatementAndInstruction(statement) + $" '{GetFirstLineText(statement.Span, text)}'";

    public static string InspectActiveStatementUpdate(ManagedActiveStatementUpdate update)
        => $"{update.Method.GetDebuggerDisplay()} IL_{update.ILOffset:X4}: {update.NewSpan.GetDebuggerDisplay()}";

    public static IEnumerable<string> InspectNonRemappableRegions(ImmutableDictionary<ManagedMethodId, ImmutableArray<NonRemappableRegion>> regions)
        => regions.OrderBy(r => r.Key.Token).Select(r => $"{r.Key.Method.GetDebuggerDisplay()} | {string.Join(", ", r.Value.Select(r => r.GetDebuggerDisplay()))}");

    public static string InspectExceptionRegionUpdate(ManagedExceptionRegionUpdate r)
        => $"{r.Method.GetDebuggerDisplay()} | {r.NewSpan.GetDebuggerDisplay()} Delta={r.Delta}";

    public static string GetFirstLineText(LinePositionSpan span, SourceText text)
        => text.Lines[span.Start.Line].ToString().Trim();

    public static string InspectSequencePointUpdates(SequencePointUpdates updates)
        => $"{updates.FileName}: [{string.Join(", ", updates.LineUpdates.Select(u => $"{u.OldLine} -> {u.NewLine}"))}]";

    public static IEnumerable<string> Inspect(this IEnumerable<SequencePointUpdates> updates)
        => updates.Select(InspectSequencePointUpdates);

}
