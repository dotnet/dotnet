// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.DynamicFiles;
using Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

public class CSharpVirtualDocumentPublisherTest : ToolingTestBase
{
    private readonly LSPDocumentMappingProvider _documentMappingProvider;

    public CSharpVirtualDocumentPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var requestInvoker = new StrictMock<LSPRequestInvoker>();
        var lazyDocumentManager = new Lazy<LSPDocumentManager>(() => (new StrictMock<LSPDocumentManager>()).Object);
        _documentMappingProvider = new(requestInvoker.Object, lazyDocumentManager);
    }

    [Fact]
    public void DocumentManager_Changed_Added_Noops()
    {
        // Arrange
        var fileInfoProvider = new StrictMock<IRazorDynamicFileInfoProviderInternal>();
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, _documentMappingProvider, TestLanguageServerFeatureOptions.Instance);

        // Act & Assert
        publisher.Changed(old: null, @new: Mock.Of<LSPDocumentSnapshot>(MockBehavior.Strict), virtualOld: null, virtualNew: null, LSPDocumentChangeKind.Added);
    }

    [Fact]
    public void DocumentManager_Changed_Removed_Noops()
    {
        // Arrange
        var fileInfoProvider = new StrictMock<IRazorDynamicFileInfoProviderInternal>();
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, _documentMappingProvider, TestLanguageServerFeatureOptions.Instance);

        // Act & Assert
        publisher.Changed(old: Mock.Of<LSPDocumentSnapshot>(MockBehavior.Strict), @new: null, virtualOld: null, virtualNew: null, LSPDocumentChangeKind.Removed);
    }

    [Fact]
    public void DocumentManager_Changed_VirtualDocumentChanged_NonCSharp_Noops()
    {
        // Arrange
        var fileInfoProvider = new StrictMock<IRazorDynamicFileInfoProviderInternal>();
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, _documentMappingProvider, TestLanguageServerFeatureOptions.Instance);

        // Act & Assert
        publisher.Changed(old: StrictMock.Of<LSPDocumentSnapshot>(),
                          @new: StrictMock.Of<LSPDocumentSnapshot>(),
                          virtualOld: StrictMock.Of<VirtualDocumentSnapshot>(),
                          virtualNew: StrictMock.Of<VirtualDocumentSnapshot>(),
                          LSPDocumentChangeKind.VirtualDocumentChanged);
    }

    [Fact]
    public void DocumentManager_Changed_VirtualDocumentChanged_UpdatesFileInfo()
    {
        // Arrange
        var csharpSnapshot = new CSharpVirtualDocumentSnapshot(projectKey: default, new Uri("C:/path/to/something.razor.g.cs"), Mock.Of<ITextSnapshot>(MockBehavior.Strict), hostDocumentSyncVersion: 1337);
        var lspDocument = new TestLSPDocumentSnapshot(new Uri("C:/path/to/something.razor"), 1337, csharpSnapshot);
        var fileInfoProvider = new Mock<IRazorDynamicFileInfoProviderInternal>(MockBehavior.Strict);
        fileInfoProvider.Setup(provider => provider.UpdateLSPFileInfo(lspDocument.Uri, It.IsAny<IDynamicDocumentContainer>()))
            .Verifiable();
        var publisher = new CSharpVirtualDocumentPublisher(fileInfoProvider.Object, _documentMappingProvider, TestLanguageServerFeatureOptions.Instance);

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
