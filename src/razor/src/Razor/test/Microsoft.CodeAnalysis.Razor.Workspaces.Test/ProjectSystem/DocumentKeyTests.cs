// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.ProjectSystem;
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
