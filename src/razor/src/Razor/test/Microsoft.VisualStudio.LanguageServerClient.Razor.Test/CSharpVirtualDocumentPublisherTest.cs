// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServerClient.Razor.DocumentMapping;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

public class CSharpVirtualDocumentPublisherTest : TestBase
{
    public CSharpVirtualDocumentPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void DocumentManager_Changed_Added_Noops()
    {
        // Arrange
        var lspDocumentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        var fileInfoProvider = new Mock<RazorDynamicFileInfoProvider>(MockBehavior.Strict);
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, lspDocumentMappingProvider.Object);

        // Act & Assert
        publisher.Changed(old: null, @new: Mock.Of<LSPDocumentSnapshot>(MockBehavior.Strict), virtualOld: null, virtualNew: null, LSPDocumentChangeKind.Added);
    }

    [Fact]
    public void DocumentManager_Changed_Removed_Noops()
    {
        // Arrange
        var lspDocumentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        var fileInfoProvider = new Mock<RazorDynamicFileInfoProvider>(MockBehavior.Strict);
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, lspDocumentMappingProvider.Object);

        // Act & Assert
        publisher.Changed(old: Mock.Of<LSPDocumentSnapshot>(MockBehavior.Strict), @new: null, virtualOld: null, virtualNew: null, LSPDocumentChangeKind.Removed);
    }

    [Fact]
    public void DocumentManager_Changed_VirtualDocumentChanged_NonCSharp_Noops()
    {
        // Arrange
        var lspDocumentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        var fileInfoProvider = new Mock<RazorDynamicFileInfoProvider>(MockBehavior.Strict);
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, lspDocumentMappingProvider.Object);

        // Act & Assert
        publisher.Changed(old: Mock.Of<LSPDocumentSnapshot>(MockBehavior.Strict),
                          @new: Mock.Of<LSPDocumentSnapshot>(MockBehavior.Strict),
                          virtualOld: Mock.Of<VirtualDocumentSnapshot>(MockBehavior.Strict),
                          virtualNew: Mock.Of<VirtualDocumentSnapshot>(MockBehavior.Strict),
                          LSPDocumentChangeKind.VirtualDocumentChanged);
    }

    [Fact]
    public void DocumentManager_Changed_VirtualDocumentChanged_UpdatesFileInfo()
    {
        // Arrange
        var csharpSnapshot = new CSharpVirtualDocumentSnapshot(projectKey: default, new Uri("C:/path/to/something.razor.g.cs"), Mock.Of<ITextSnapshot>(MockBehavior.Strict), hostDocumentSyncVersion: 1337);
        var lspDocument = new TestLSPDocumentSnapshot(new Uri("C:/path/to/something.razor"), 1337, csharpSnapshot);
        var fileInfoProvider = new Mock<RazorDynamicFileInfoProvider>(MockBehavior.Strict);
        var lspDocumentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        fileInfoProvider.Setup(provider => provider.UpdateLSPFileInfo(lspDocument.Uri, It.IsAny<DynamicDocumentContainer>()))
            .Verifiable();
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, lspDocumentMappingProvider.Object);

        // Act
        publisher.Changed(old: Mock.Of<LSPDocumentSnapshot>(MockBehavior.Strict),
                          @new: lspDocument,
                          virtualOld: Mock.Of<VirtualDocumentSnapshot>(MockBehavior.Strict),
                          virtualNew: csharpSnapshot,
                          LSPDocumentChangeKind.VirtualDocumentChanged);

        // Assert
        fileInfoProvider.VerifyAll();
    }
}
