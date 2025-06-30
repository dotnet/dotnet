// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DocumentSnapshotTextLoaderTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public async Task LoadTextAndVersionAsync_CreatesTextAndVersionFromDocumentsText()
    {
        // Arrange
        var expectedSourceText = SourceText.From("Hello World");
        var snapshotMock = new StrictMock<IDocumentSnapshot>();
        snapshotMock
            .Setup(x => x.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedSourceText);
        var textLoader = new DocumentSnapshotTextLoader(snapshotMock.Object);

        // Act
        var actual = await textLoader.LoadTextAndVersionAsync(options: default, DisposalToken);

        // Assert
        Assert.Same(expectedSourceText, actual.Text);
    }
}
