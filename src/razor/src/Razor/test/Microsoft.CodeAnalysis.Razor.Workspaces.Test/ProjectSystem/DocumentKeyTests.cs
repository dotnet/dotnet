// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.CodeAnalysis.Razor.ProjectSystem.CompareKeysTestData;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DocumentKeyTests(ITestOutputHelper testOuput) : ToolingTestBase(testOuput)
{
    [Theory]
    [MemberData(nameof(CompareDocumentKeysData))]
    internal void CompareDocumentKeys(DocumentKey key1, DocumentKey key2, CompareResult result)
    {
        switch (result)
        {
            case CompareResult.Equal:
                Assert.Equal(0, key1.CompareTo(key2));
                break;

            case CompareResult.LessThan:
                Assert.True(key1.CompareTo(key2) < 0);
                break;

            case CompareResult.GreaterThan:
                Assert.True(key1.CompareTo(key2) > 0);
                break;

            default:
                Assumed.Unreachable();
                break;
        }
    }

    public static TheoryData CompareDocumentKeysData => CompareKeysTestData.DocumentKeys;
}
