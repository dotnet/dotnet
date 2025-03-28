// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost.Test;

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

    [Fact]
    public async Task SerializeProjectInfo()
    {
        using var stream = new MemoryStream();

        var configuration = new RazorConfiguration(
            RazorLanguageVersion.Latest,
            "TestConfiguration",
            ImmutableArray<RazorExtension>.Empty);

        var tagHelper = TagHelperDescriptorBuilder.Create("TypeName", "AssemblyName")
            .TagMatchingRuleDescriptor(rule => rule.RequireTagName("tag-name"))
            .Build();

        var projectWorkspaceState = ProjectWorkspaceState.Create([tagHelper]);

        var projectInfo = new RazorProjectInfo(
            new ProjectKey("TestProject"),
            @"C:\test\test.csproj",
            configuration,
            "TestNamespace",
            "Test",
            projectWorkspaceState,
            [new DocumentSnapshotHandle(@"C:\test\document.razor", @"document.razor", FileKinds.Component)]);

        var bytesToSerialize = projectInfo.Serialize();

        await stream.WriteProjectInfoAsync(projectInfo, DisposalToken);

        // WriteProjectInfoAsync prepends the size before writing which is 4 bytes long
        Assert.Equal(bytesToSerialize.Length + 4, stream.Position);

        var streamContents = stream.ToArray();
        var expectedSize = BitConverter.GetBytes(bytesToSerialize.Length);
        Assert.Equal(expectedSize, streamContents.Take(4).ToArray());

        Assert.Equal(bytesToSerialize, streamContents.Skip(4).ToArray());

        stream.Position = 0;
        var deserialized = await stream.ReadProjectInfoAsync(default);

        Assert.Equal(projectInfo, deserialized);
    }

    [Theory]
    [CombinatorialData]
    internal void ProjectInfoActionFunctions(ProjectInfoAction infoAction)
    {
        using var stream = new MemoryStream();
        stream.WriteProjectInfoAction(infoAction);

        stream.Position = 0;
        Assert.Equal(infoAction, stream.ReadProjectInfoAction());
    }
}
