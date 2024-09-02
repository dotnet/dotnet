// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

public class AddUsingsHelperTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void TryExtractNamespace_Invalid_ReturnsFalse()
    {
        // Arrange
        var csharpAddUsing = "Abc.Xyz;";

        // Act
        var res = AddUsingsHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

        // Assert
        Assert.False(res);
        Assert.Empty(@namespace);
        Assert.Empty(prefix);
    }

    [Fact]
    public void TryExtractNamespace_ReturnsTrue()
    {
        // Arrange
        var csharpAddUsing = "using Abc.Xyz;";

        // Act
        var res = AddUsingsHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

        // Assert
        Assert.True(res);
        Assert.Equal("Abc.Xyz", @namespace);
        Assert.Empty(prefix);
    }

    [Fact]
    public void TryExtractNamespace_WithStatic_ReturnsTrue()
    {
        // Arrange
        var csharpAddUsing = "using static X.Y.Z;";

        // Act
        var res = AddUsingsHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

        // Assert
        Assert.True(res);
        Assert.Equal("static X.Y.Z", @namespace);
        Assert.Empty(prefix);
    }

    [Fact]
    public void TryExtractNamespace_WithTypeNameCorrection_ReturnsTrue()
    {
        // Arrange
        var csharpAddUsing = "Goo - using X.Y.Z;";

        // Act
        var res = AddUsingsHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

        // Assert
        Assert.True(res);
        Assert.Equal("X.Y.Z", @namespace);
        Assert.Equal("Goo - ", prefix);
    }
}
