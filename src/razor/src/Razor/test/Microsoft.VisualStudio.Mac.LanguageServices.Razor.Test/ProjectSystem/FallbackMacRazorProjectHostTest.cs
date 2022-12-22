// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Test.Common;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

public class FallbackMacRazorProjectHostTest : TestBase
{
    public FallbackMacRazorProjectHostTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsMvcAssembly_FailsIfNullOrEmptyFilePath(string filePath)
    {
        // Arrange
        var assemblyFilePath = new FilePath(filePath);
        var assemblyReference = new AssemblyReference(assemblyFilePath);

        // Act
        var result = FallbackMacRazorProjectHost.IsMvcAssembly(assemblyReference);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMvcAssembly_FailsIfNotMvc()
    {
        // Arrange
        var assemblyFilePath = new FilePath("C:/Path/To/Assembly.dll");
        var assemblyReference = new AssemblyReference(assemblyFilePath);

        // Act
        var result = FallbackMacRazorProjectHost.IsMvcAssembly(assemblyReference);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMvcAssembly_SucceedsIfMvc()
    {
        // Arrange
        var assemblyFilePath = new FilePath("C:/Path/To/Microsoft.AspNetCore.Mvc.Razor.dll");
        var assemblyReference = new AssemblyReference(assemblyFilePath);

        // Act
        var result = FallbackMacRazorProjectHost.IsMvcAssembly(assemblyReference);

        // Assert
        Assert.True(result);
    }

    // -------------------------------------------------------------------------------------------
    // Purposefully do not have any more tests here because that would involve mocking MonoDevelop
    // types. The default constructors for the Solution / DotNetProject MonoDevelop types change
    // static classes (they assume they're being created in an IDE).
    // -------------------------------------------------------------------------------------------
}
