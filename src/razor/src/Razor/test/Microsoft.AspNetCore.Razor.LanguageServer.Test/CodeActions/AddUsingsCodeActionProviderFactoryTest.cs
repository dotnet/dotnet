// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class AddUsingsCodeActionProviderFactoryTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void GetNamespaceFromFQN_Invalid_ReturnsEmpty()
    {
        // Arrange
        var fqn = "Abc";

        // Act
        var namespaceName = AddUsingsCodeActionProviderHelper.GetNamespaceFromFQN(fqn);

        // Assert
        Assert.Empty(namespaceName);
    }

    [Fact]
    public void GetNamespaceFromFQN_Valid_ReturnsNamespace()
    {
        // Arrange
        var fqn = "Abc.Xyz";

        // Act
        var namespaceName = AddUsingsCodeActionProviderHelper.GetNamespaceFromFQN(fqn);

        // Assert
        Assert.Equal("Abc", namespaceName);
    }

    [Fact]
    public void TryCreateAddUsingResolutionParams_CreatesResolutionParams()
    {
        // Arrange
        var fqn = "Abc.Xyz";
        var docUri = new Uri("c:/path");

        // Act
        var result = AddUsingsCodeActionProviderHelper.TryCreateAddUsingResolutionParams(fqn, docUri, additionalEdit: null, out var @namespace, out var resolutionParams);

        // Assert
        Assert.True(result);
        Assert.Equal("Abc", @namespace);
        Assert.NotNull(resolutionParams);
    }

    [Fact]
    public void TryExtractNamespace_Invalid_ReturnsFalse()
    {
        // Arrange
        var csharpAddUsing = "Abc.Xyz;";

        // Act
        var res = AddUsingsCodeActionProviderHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

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
        var res = AddUsingsCodeActionProviderHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

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
        var res = AddUsingsCodeActionProviderHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

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
        var res = AddUsingsCodeActionProviderHelper.TryExtractNamespace(csharpAddUsing, out var @namespace, out var prefix);

        // Assert
        Assert.True(res);
        Assert.Equal("X.Y.Z", @namespace);
        Assert.Equal("Goo - ", prefix);
    }
}
