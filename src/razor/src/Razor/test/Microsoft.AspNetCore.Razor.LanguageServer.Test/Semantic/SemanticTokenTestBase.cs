// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

public abstract class SemanticTokenTestBase : TagHelperServiceTestBase
{
    private static readonly AsyncLocal<string?> s_fileName = new();
    private static readonly string s_projectPath = TestProject.GetProjectDirectory(typeof(TagHelperServiceTestBase));

    protected static readonly ServerCapabilities SemanticTokensServerCapabilities = new()
    {
        SemanticTokensOptions = new()
        {
            Full = false,
            Range = true
        }
    };
    // Used by the test framework to set the 'base' name for test files.
    public static string? FileName
    {
        get { return s_fileName.Value; }
        set { s_fileName.Value = value; }
    }

#if GENERATE_BASELINES
    protected bool GenerateBaselines { get; set; } = true;
#else
    protected bool GenerateBaselines { get; set; } = false;
#endif

    protected int BaselineTestCount { get; set; }
    protected int BaselineEditTestCount { get; set; }

    protected SemanticTokenTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    protected void AssertSemanticTokensMatchesBaseline(IEnumerable<int>? actualSemanticTokens)
    {
        if (FileName is null)
        {
            var message = $"{nameof(AssertSemanticTokensMatchesBaseline)} should only be called from a Semantic test ({nameof(FileName)} is null).";
            throw new InvalidOperationException(message);
        }

        var fileName = BaselineTestCount > 0 ? FileName + $"_{BaselineTestCount}" : FileName;
        var baselineFileName = Path.ChangeExtension(fileName, ".semantic.txt");
        var actual = actualSemanticTokens?.ToArray();

        BaselineTestCount++;
        if (GenerateBaselines)
        {
            GenerateSemanticBaseline(actual, baselineFileName);
        }

        var semanticArray = GetBaselineTokens(baselineFileName);

        if (semanticArray is null && actual is null)
        {
            return;
        }
        else if (semanticArray is null || actual is null)
        {
            Assert.False(true, $"Expected: {semanticArray}; Actual: {actual}");
        }

        for (var i = 0; i < Math.Min(semanticArray!.Length, actual!.Length); i += 5)
        {
            var end = i + 5;
            var actualTokens = actual[i..end];
            var expectedTokens = semanticArray[i..end];
            Assert.True(Enumerable.SequenceEqual(expectedTokens, actualTokens), $"Expected: {string.Join(',', expectedTokens)} Actual: {string.Join(',', actualTokens)} index: {i}");
        }

        Assert.True(semanticArray.Length == actual.Length, $"Expected length: {semanticArray.Length}, Actual length: {actual.Length}");
    }

    protected int[]? GetBaselineTokens(string baselineFileName)
    {
        var semanticFile = TestFile.Create(baselineFileName, GetType().GetTypeInfo().Assembly);
        if (!semanticFile.Exists())
        {
            throw new XunitException($"The resource {baselineFileName} was not found.");
        }

        var semanticIntStr = semanticFile.ReadAllText();
        var semanticArray = ParseSemanticBaseline(semanticIntStr);
        return semanticArray;
    }

    private protected async Task<ProvideSemanticTokensResponse> GetCSharpSemanticTokensResponseAsync(
        string documentText, Range razorRange, bool isRazorFile, int hostDocumentSyncVersion = 0)
    {
        var codeDocument = CreateCodeDocument(documentText, isRazorFile, DefaultTagHelpers);
        var csharpRange = GetMappedCSharpRange(codeDocument, razorRange);
        var csharpTokens = Array.Empty<int>();

        if (csharpRange is not null)
        {
            var csharpDocumentUri = new Uri("C:\\TestSolution\\TestProject\\TestDocument.cs");
            var csharpSourceText = codeDocument.GetCSharpSourceText();

            await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
                csharpSourceText, csharpDocumentUri, SemanticTokensServerCapabilities, SpanMappingService, DisposalToken);
            var result = await csharpServer.ExecuteRequestAsync<SemanticTokensRangeParams, SemanticTokens>(
                Methods.TextDocumentSemanticTokensRangeName,
                CreateVSSemanticTokensRangeParams(csharpRange, csharpDocumentUri),
                DisposalToken);

            csharpTokens = result?.Data;
        }

        var csharpResponse = new ProvideSemanticTokensResponse(tokens: csharpTokens, hostDocumentSyncVersion);
        return csharpResponse;
    }

    protected Range? GetMappedCSharpRange(RazorCodeDocument codeDocument, Range razorRange)
    {
        var documentMappingService = new DefaultRazorDocumentMappingService(
            TestLanguageServerFeatureOptions.Instance, new TestDocumentContextFactory(), LoggerFactory);
        if (!documentMappingService.TryMapToProjectedDocumentRange(codeDocument, razorRange, out var csharpRange) &&
            !DefaultRazorSemanticTokensInfoService.TryGetMinimalCSharpRange(codeDocument, razorRange, out csharpRange))
        {
            // No C# in the range.
            return null;
        }

        return csharpRange;
    }

    internal static SemanticTokensRangeParams CreateVSSemanticTokensRangeParams(Range range, Uri uri)
        => new()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Range = range
        };

    private static void GenerateSemanticBaseline(IEnumerable<int>? actual, string baselineFileName)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        if (actual != null)
        {
            var actualArray = actual.ToArray();
            builder.AppendLine("//line,characterPos,length,tokenType,modifier");
            var legendArray = RazorSemanticTokensLegend.TokenTypes.ToArray();
            for (var i = 0; i < actualArray.Length; i += 5)
            {
                var typeString = legendArray[actualArray[i + 3]];
                builder.Append(actualArray[i]).Append(' ');
                builder.Append(actualArray[i + 1]).Append(' ');
                builder.Append(actualArray[i + 2]).Append(' ');
                builder.Append(typeString).Append(' ');
                builder.Append(actualArray[i + 4]);
                builder.AppendLine();
            }
        }

        var semanticBaselinePath = Path.Combine(s_projectPath, baselineFileName);
        File.WriteAllText(semanticBaselinePath, builder.ToString());
    }

    private static int[]? ParseSemanticBaseline(string semanticIntStr)
    {
        if (string.IsNullOrEmpty(semanticIntStr))
        {
            return null;
        }

        var tokenTypesList = RazorSemanticTokensLegend.TokenTypes.ToList();
        var strArr = semanticIntStr.Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var results = new List<int>();
        for (var i = 0; i < strArr.Length; i++)
        {
            if (int.TryParse(strArr[i], System.Globalization.NumberStyles.Integer, Thread.CurrentThread.CurrentCulture, out var intResult))
            {
                results.Add(intResult);
                continue;
            }

            // Needed to handle token types with spaces in their names, e.g. C#'s 'local name' type
            var tokenTypeStr = strArr[i];
            while (i + 1 < strArr.Length && !int.TryParse(strArr[i + 1], System.Globalization.NumberStyles.Integer, Thread.CurrentThread.CurrentCulture, out _))
            {
                tokenTypeStr += $" {strArr[i + 1]}";
                i++;
            }

            var tokenIndex = tokenTypesList.IndexOf(tokenTypeStr);
            if (tokenIndex != -1)
            {
                results.Add(tokenIndex);
                continue;
            }
        }

        return results.ToArray();
    }
}
