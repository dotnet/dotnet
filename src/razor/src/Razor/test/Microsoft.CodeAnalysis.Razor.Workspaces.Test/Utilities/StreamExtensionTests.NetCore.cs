// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Utilities;

public class StreamExtensionTests(ITestOutputHelper testOutputHelper) : ToolingTestBase(testOutputHelper)
{
    [Theory]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(-500)]
    [InlineData(500)]
    public void SizeFunctions(int size)
    {
        using var stream = new MemoryStream();

        stream.WriteSize(size);
        stream.Position = 0;

        Assert.Equal(size, stream.ReadSize());
    }

    public static TheoryData<string, Encoding?> StringFunctionData = new TheoryData<string, Encoding?>
    {
        { "", null },
        { "hello", null },
        { "", Encoding.UTF8 },
        { "hello", Encoding.UTF8 },
        { "", Encoding.ASCII },
        { "hello", Encoding.ASCII },
        { "", Encoding.UTF32 },
        { "hello", Encoding.UTF32 },
        { "", Encoding.Unicode },
        { "hello", Encoding.Unicode },
        { "", Encoding.BigEndianUnicode },
        { "hello", Encoding.BigEndianUnicode },
    };

    [Theory]
    [MemberData(nameof(StringFunctionData))]
    public async Task StringFunctions(string expected, Encoding? encoding)
    {
        using var stream = new MemoryStream();

        await stream.WriteStringAsync(expected, encoding, DisposalToken);
        stream.Position = 0;

        var actual = await stream.ReadStringAsync(encoding, DisposalToken);
        Assert.Equal(expected, actual);
    }
}
