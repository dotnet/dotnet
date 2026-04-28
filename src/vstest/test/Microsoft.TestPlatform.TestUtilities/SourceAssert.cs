// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Provides assertion methods that validate PDB-reported line numbers against actual source files.
/// This removes the need for Debug/Release conditional checks in tests, since line numbers
/// from PDBs differ between configurations due to compiler optimizations.
/// </summary>
public static class SourceAssert
{
    /// <summary>
    /// Asserts that <paramref name="actualLineNumber"/> is at the start of <paramref name="methodName"/>'s body.
    /// In Debug builds, the PDB reports the opening brace line. In Release builds, the compiler
    /// optimizes and reports the next line (first statement or closing brace).
    /// This assertion accepts exactly those two possibilities: <c>BodyStartLine</c> or <c>BodyStartLine + 1</c>.
    /// For overloaded methods, succeeds if the line matches any overload.
    /// </summary>
    public static void LineIsAtMethodBodyStart(string sourceFilePath, string methodName, int actualLineNumber, string? message = null)
    {
        var lines = File.ReadAllLines(sourceFilePath);
        var bodyStarts = SourceNavigationParser.FindMethodBodyStartLines(lines, methodName);
        Assert.IsNotEmpty(bodyStarts, $"Method '{methodName}' not found in '{sourceFilePath}'.");

        Assert.Contains(
            bodyStart => actualLineNumber == bodyStart || actualLineNumber == bodyStart + 1, bodyStarts,
            message ?? $"Line {actualLineNumber} is not at the body start of method '{methodName}' in '{Path.GetFileName(sourceFilePath)}'."
                     + $" Expected one of: {string.Join(", ", bodyStarts.SelectMany(b => new[] { b, b + 1 }).Distinct().OrderBy(x => x))}");
    }

    /// <summary>
    /// Asserts that <paramref name="actualLineNumber"/> falls within the declaration range of
    /// <paramref name="methodName"/> in <paramref name="sourceFilePath"/>.
    /// The range spans from a few lines above the method signature (to cover attributes) through
    /// the body start. For overloaded methods, succeeds if the line falls within any overload.
    /// </summary>
    public static void LineIsWithinMethod(string sourceFilePath, string methodName, int actualLineNumber, string? message = null)
    {
        var lines = File.ReadAllLines(sourceFilePath);
        var locations = SourceNavigationParser.FindMethodLocations(lines, methodName);
        Assert.IsNotEmpty(locations, $"Method '{methodName}' not found in '{sourceFilePath}'.");

        // Allow from a few lines before the signature (to cover attributes like [TestMethod]) through body start + 1.
        const int attributeMargin = 5;
        Assert.Contains(
            loc => actualLineNumber >= loc.SignatureLine - attributeMargin && actualLineNumber <= loc.BodyStartLine + 1, locations,
            message ?? $"Line {actualLineNumber} is not within any overload of method '{methodName}' in '{Path.GetFileName(sourceFilePath)}'."
                     + $" Method ranges: {string.Join(", ", locations.Select(loc => $"[{loc.SignatureLine - attributeMargin}-{loc.BodyStartLine + 1}]"))}");
    }

    /// <summary>
    /// Finds the source file containing <paramref name="methodName"/> in the test asset project
    /// identified by <paramref name="assetName"/> (e.g. "NUTestProject.dll").
    /// Looks in <c>test/TestAssets/{projectName}/</c> relative to the repo root.
    /// </summary>
    public static string FindSourceFile(string assetName, string methodName)
    {
        var projectName = Path.GetFileNameWithoutExtension(assetName);
        var testAssetsDir = Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, "test", "TestAssets", projectName);
        Assert.IsTrue(Directory.Exists(testAssetsDir), $"Test asset project directory not found: '{testAssetsDir}'.");

        foreach (var csFile in Directory.GetFiles(testAssetsDir, "*.cs", SearchOption.TopDirectoryOnly))
        {
            var lines = File.ReadAllLines(csFile);
            if (SourceNavigationParser.FindMethodBodyStartLines(lines, methodName).Count > 0)
            {
                return csFile;
            }
        }

        Assert.Fail($"No source file containing method '{methodName}' found in '{testAssetsDir}'.");
        return null!;
    }
}

/// <summary>
/// Parses C# source text to find method body start lines. This class operates on string arrays
/// (lines of text) and has no file system dependencies, making it easy to unit test.
/// </summary>
public static class SourceNavigationParser
{
    /// <summary>
    /// Finds each overload of <paramref name="methodName"/> and returns a <see cref="MethodLocation"/>
    /// with the 1-based signature line and body start line (the line containing the opening brace).
    /// </summary>
    public static IReadOnlyList<MethodLocation> FindMethodLocations(string[] lines, string methodName)
    {
        var results = new List<MethodLocation>();

        for (int i = 0; i < lines.Length; i++)
        {
            if (!ContainsMethodSignature(lines[i], methodName))
            {
                continue;
            }

            int signatureLine = i + 1; // 1-based

            // Find the next '{' starting from the signature line.
            for (int j = i; j < lines.Length; j++)
            {
                if (lines[j].Contains('{'))
                {
                    results.Add(new MethodLocation(signatureLine, j + 1)); // 1-based
                    break;
                }
            }
        }

        return results;
    }

    /// <summary>
    /// Convenience wrapper returning just the body start lines for each overload.
    /// </summary>
    public static IReadOnlyList<int> FindMethodBodyStartLines(string[] lines, string methodName)
    {
        var locations = FindMethodLocations(lines, methodName);
        var results = new List<int>(locations.Count);
        foreach (var loc in locations)
        {
            results.Add(loc.BodyStartLine);
        }

        return results;
    }

    /// <summary>
    /// Checks whether <paramref name="text"/> contains a method signature for <paramref name="methodName"/>,
    /// identified by the pattern <c>methodName(</c> (with optional whitespace before the parenthesis).
    /// </summary>
    public static bool ContainsMethodSignature(string text, string methodName)
    {
        int startIndex = 0;
        while (true)
        {
            int idx = text.IndexOf(methodName, startIndex, StringComparison.Ordinal);
            if (idx < 0)
            {
                return false;
            }

            // Check that the character after the method name (skipping whitespace) is '('.
            for (int i = idx + methodName.Length; i < text.Length; i++)
            {
                if (text[i] == '(')
                {
                    return true;
                }

                if (!char.IsWhiteSpace(text[i]))
                {
                    break;
                }
            }

            startIndex = idx + 1;
        }
    }
}

/// <summary>
/// Represents the location of a method in a source file. All line numbers are 1-based.
/// </summary>
/// <param name="SignatureLine">The line containing the method name and parameters.</param>
/// <param name="BodyStartLine">The line containing the opening brace.</param>
public readonly record struct MethodLocation(int SignatureLine, int BodyStartLine);
