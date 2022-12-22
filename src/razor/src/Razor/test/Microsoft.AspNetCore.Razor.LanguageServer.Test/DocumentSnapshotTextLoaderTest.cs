// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DocumentSnapshotTextLoaderTest : TestBase
{
    public DocumentSnapshotTextLoaderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task LoadTextAndVersionAsync_CreatesTextAndVersionFromDocumentsText()
    {
        // Arrange
        var expectedSourceText = SourceText.From("Hello World");
        var result = Task.FromResult(expectedSourceText);
        var snapshot = Mock.Of<DocumentSnapshot>(doc => doc.GetTextAsync() == result, MockBehavior.Strict);
        var textLoader = new DocumentSnapshotTextLoader(snapshot);

        // Act
        var actual = await textLoader.LoadTextAndVersionAsync(default, default, default);

        // Assert
        Assert.Same(expectedSourceText, actual.Text);
    }
}
