// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class EmptyTextLoaderTest : TestBase
{
    public EmptyTextLoaderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    // See https://github.com/dotnet/aspnetcore/issues/7997
    [Fact]
    public async Task LoadAsync_SpecifiesEncoding()
    {
        // Arrange
        var loader = new EmptyTextLoader("file.cshtml");

        // Act
        var textAndVersion = await loader.LoadTextAndVersionAsync(default, default);

        // Assert
        Assert.True(textAndVersion.Text.CanBeEmbedded);
        Assert.Same(Encoding.UTF8, textAndVersion.Text.Encoding);
    }
}
