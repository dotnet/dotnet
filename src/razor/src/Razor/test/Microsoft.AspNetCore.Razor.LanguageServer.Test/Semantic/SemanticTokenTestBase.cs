// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.Test.Utilities;
using StreamJsonRpc;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

public abstract class SemanticTokenTestBase : TagHelperServiceTestBase
{
    private static readonly AsyncLocal<string?> s_fileName = new();
    private static readonly string s_projectPath = TestProject.GetProjectDirectory(typeof(TagHelperServiceTestBase));

    protected static readonly VSInternalServerCapabilities SemanticTokensServerCapabilities = new()
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
    protected bool UsePreciseSemanticTokenRanges { get; set; }

    protected SemanticTokenTestBase(ITestOutputHelper testOutput, bool usePreciseSemanticTokenRanges)
        : base(testOutput)
    {
        UsePreciseSemanticTokenRanges = usePreciseSemanticTokenRanges;
    }

    protected void AssertSemanticTokensMatchesBaseline(SourceText sourceText, int[]? actualSemanticTokens)
    {
        if (FileName is null)
        {
            var message = $"{nameof(AssertSemanticTokensMatchesBaseline)} should only be called from a Semantic test ({nameof(FileName)} is null).";
            throw new InvalidOperationException(message);
        }

        var fileName = BaselineTestCount > 0 ? FileName + $"_{BaselineTestCount}" : FileName;
        var baselineFileName = Path.ChangeExtension(fileName, ".semantic.txt");

        var actualFileContents = GetFileRepresentationOfTokens(sourceText, actualSemanticTokens);

        BaselineTestCount++;
        if (GenerateBaselines)
        {
            GenerateSemanticBaseline(actualFileContents, baselineFileName);
        }

        var expectedFileContents = GetBaselineFileContents(baselineFileName);

        AssertEx.EqualOrDiff(expectedFileContents, actualFileContents);
    }

    protected string GetBaselineFileContents(string baselineFileName)
    {
        var semanticFile = TestFile.Create(baselineFileName, GetType().GetTypeInfo().Assembly);
        if (!semanticFile.Exists())
        {
            return string.Empty;
        }

        return semanticFile.ReadAllText();
    }

    private protected async Task<ProvideSemanticTokensResponse> GetCSharpSemanticTokensResponseAsync(
        string documentText, Range razorRange, bool isRazorFile, int hostDocumentSyncVersion = 0)
    {
        var codeDocument = CreateCodeDocument(documentText, isRazorFile, DefaultTagHelpers);
        var csharpDocumentUri = new Uri("C:\\TestSolution\\TestProject\\TestDocument.cs");
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, SemanticTokensServerCapabilities, SpanMappingService, DisposalToken);

        var csharpRanges = GetMappedCSharpRanges(codeDocument, razorRange, out var minimalRange);
        if (minimalRange == null)
        {
            return new ProvideSemanticTokensResponse(tokens: Array.Empty<int>(), hostDocumentSyncVersion);
        }

        SemanticTokens? result;
        try
        {
            result = await csharpServer.ExecuteRequestAsync<SemanticTokensRangesParams, SemanticTokens>(
            "roslyn/semanticTokenRanges",
            CreateVSSemanticTokensRangesParams(csharpRanges, csharpDocumentUri),
            DisposalToken);
        }
        catch (RemoteMethodNotFoundException)
        {
            // The new endpoint is available in version 4.8.0-3.23471.2 or higher of Roslyn packages
            result = await csharpServer.ExecuteRequestAsync<SemanticTokensRangeParams, SemanticTokens>(
            Methods.TextDocumentSemanticTokensRangeName,
            CreateVSSemanticTokensRangeParams(minimalRange, csharpDocumentUri),
            DisposalToken);
        }

        return new ProvideSemanticTokensResponse(tokens: result?.Data, hostDocumentSyncVersion);
    }

    protected Range[] GetMappedCSharpRanges(RazorCodeDocument codeDocument, Range razorRange, out Range? minimalRange)
    {
        var documentMappingService = new RazorDocumentMappingService(
            FilePathService, new TestDocumentContextFactory(), LoggerFactory);
        if (UsePreciseSemanticTokenRanges)
        {
            if (!RazorSemanticTokensInfoService.TryGetSortedCSharpRanges(codeDocument, razorRange, out var csharpRanges))
            {
                // No C# in the range.
                minimalRange = null;
                return Array.Empty<Range>();
            }

            minimalRange = new Range { Start = csharpRanges[0].Start, End = csharpRanges.Last().End }; ;
            return csharpRanges;
        }

        if (!documentMappingService.TryMapToGeneratedDocumentRange(codeDocument.GetCSharpDocument(), razorRange, out minimalRange) &&
            !RazorSemanticTokensInfoService.TryGetMinimalCSharpRange(codeDocument, razorRange, out minimalRange))
        {
            // No C# in the range.
            return Array.Empty<Range>();
        }

        return new Range[] { minimalRange };
    }

    internal static SemanticTokensRangesParams CreateVSSemanticTokensRangesParams(Range[] ranges, Uri uri)
        => new()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Ranges = ranges
        };

    internal static SemanticTokensRangeParams CreateVSSemanticTokensRangeParams(Range range, Uri uri)
        => new()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Range = range
        };

    private static void GenerateSemanticBaseline(string actualFileContents, string baselineFileName)
    {
        var semanticBaselinePath = Path.Combine(s_projectPath, baselineFileName);
        File.WriteAllText(semanticBaselinePath, actualFileContents);
    }

    private static string GetFileRepresentationOfTokens(SourceText sourceText, int[]? data)
    {
        if (data == null)
        {
            return string.Empty;
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.AppendLine("//line,characterPos,length,tokenType,modifier,text");
        var legendArray = TestRazorSemanticTokensLegend.Instance.Legend.TokenTypes;
        var prevLength = 0;
        var lineIndex = 0;
        var lineOffset = 0;
        for (var i = 0; i < data.Length; i += 5)
        {
            var lineDelta = data[i];
            var charDelta = data[i + 1];
            var length = data[i + 2];

            Assert.False(i != 0 && lineDelta == 0 && charDelta == 0, "line delta and character delta are both 0, which is invalid as we shouldn't be producing overlapping tokens");
            Assert.False(i != 0 && lineDelta == 0 && charDelta < prevLength, "Previous length is longer than char offset from previous start, meaning tokens will overlap");

            if (lineDelta != 0)
            {
                lineOffset = 0;
            }

            lineIndex += lineDelta;
            lineOffset += charDelta;

            var typeString = legendArray[data[i + 3]];
            builder.Append(lineDelta).Append(' ');
            builder.Append(charDelta).Append(' ');
            builder.Append(length).Append(' ');
            builder.Append(typeString).Append(' ');
            builder.Append(data[i + 4]).Append(' ');
            builder.Append('[').Append(sourceText.GetSubTextString(new TextSpan(sourceText.Lines[lineIndex].Start + lineOffset, length))).Append(']');
            builder.AppendLine();

            prevLength = length;
        }

        return builder.ToString();
    }

    private static int[]? ParseSemanticBaseline(string semanticIntStr)
    {
        if (string.IsNullOrEmpty(semanticIntStr))
        {
            return null;
        }

        var tokenTypesList = TestRazorSemanticTokensLegend.Instance.Legend.TokenTypes.ToList();
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
