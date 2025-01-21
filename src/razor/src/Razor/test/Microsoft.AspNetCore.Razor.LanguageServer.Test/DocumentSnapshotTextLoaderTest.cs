// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
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
