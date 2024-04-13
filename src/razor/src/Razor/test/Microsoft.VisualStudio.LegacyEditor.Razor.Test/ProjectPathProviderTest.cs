// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class ProjectPathProviderTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [UIFact]
    public void TryGetProjectPath_NullLiveShareProjectPathProvider_UsesProjectService()
    {
        // Arrange
        var projectFilePath = "/my/project/path.csproj";
        var documentFilePath = "/my/document/path.razor";

        var vsHierarchy = CreateVsHierarchy(projectFilePath);
        var vsRunningDocumentTable = CreateVsRunningDocumentTable(documentFilePath, vsHierarchy);

        var serviceProvider = VsMocks.CreateServiceProvider(b =>
        {
            b.AddService<SVsRunningDocumentTable>(vsRunningDocumentTable);
        });

        var textBuffer = StrictMock.Of<ITextBuffer>();
        var textDocumentFactoryService = CreateTextDocumentFactoryService(textBuffer, documentFilePath);

        var projectPathProvider = new ProjectPathProvider(
            serviceProvider,
            textDocumentFactoryService,
            liveShareProjectPathProvider: null,
            JoinableTaskContext);

        // Act
        var result = projectPathProvider.TryGetProjectPath(textBuffer, out var filePath);

        // Assert
        Assert.True(result);
        Assert.Equal(projectFilePath, filePath);
    }

    [UIFact]
    public void TryGetProjectPath_PrioritizesLiveShareProjectPathProvider()
    {
        // Arrange
        var liveShareProjectFilePath = "/path/from/liveshare.csproj";

        var liveShareProjectPathProviderMock = new StrictMock<ILiveShareProjectPathProvider>();
        liveShareProjectPathProviderMock
            .Setup(x => x.TryGetProjectPath(It.IsAny<ITextBuffer>(), out liveShareProjectFilePath))
            .Returns(true);

        var projectPathProvider = new ProjectPathProvider(
            StrictMock.Of<IServiceProvider>(),
            StrictMock.Of<ITextDocumentFactoryService>(),
            liveShareProjectPathProviderMock.Object,
            JoinableTaskContext);

        var textBuffer = StrictMock.Of<ITextBuffer>();

        // Act
        var result = projectPathProvider.TryGetProjectPath(textBuffer, out var filePath);

        // Assert
        Assert.True(result);
        Assert.Equal(liveShareProjectFilePath, filePath);
    }

    [UIFact]
    public void TryGetProjectPath_ReturnsFalseIfNoProject()
    {
        // Arrange
        var documentFilePath = "/my/document/path.razor";
        var vsRunningDocumentTable = CreateVsRunningDocumentTable(documentFilePath, vsHierarchy: null);

        var serviceProvider = VsMocks.CreateServiceProvider(b =>
        {
            b.AddService<SVsRunningDocumentTable>(vsRunningDocumentTable);
        });

        var textBuffer = StrictMock.Of<ITextBuffer>();
        var textDocumentFactoryService = CreateTextDocumentFactoryService(textBuffer, documentFilePath);

        var liveShareProjectPathProviderMock = new StrictMock<ILiveShareProjectPathProvider>();
        liveShareProjectPathProviderMock
            .Setup(p => p.TryGetProjectPath(It.IsAny<ITextBuffer>(), out It.Ref<string?>.IsAny))
            .Returns(false);

        var projectPathProvider = new ProjectPathProvider(
            serviceProvider,
            textDocumentFactoryService,
            liveShareProjectPathProviderMock.Object,
            JoinableTaskContext);

        // Act
        var result = projectPathProvider.TryGetProjectPath(textBuffer, out var filePath);

        // Assert
        Assert.False(result);
        Assert.Null(filePath);
    }

    [UIFact]
    public void TryGetProjectPath_ReturnsTrueIfProject()
    {
        // Arrange
        var projectFilePath = "/my/project/path.csproj";
        var documentFilePath = "/my/document/path.razor";

        var vsHierarchy = CreateVsHierarchy(projectFilePath);
        var vsRunningDocumentTable = CreateVsRunningDocumentTable(documentFilePath, vsHierarchy);

        var serviceProvider = VsMocks.CreateServiceProvider(b =>
        {
            b.AddService<SVsRunningDocumentTable>(vsRunningDocumentTable);
        });

        var textBuffer = StrictMock.Of<ITextBuffer>();
        var textDocumentFactoryService = CreateTextDocumentFactoryService(textBuffer, documentFilePath);

        var liveShareProjectPathProviderMock = new StrictMock<ILiveShareProjectPathProvider>();
        liveShareProjectPathProviderMock
            .Setup(p => p.TryGetProjectPath(It.IsAny<ITextBuffer>(), out It.Ref<string?>.IsAny))
            .Returns(false);

        var projectPathProvider = new ProjectPathProvider(
            serviceProvider,
            textDocumentFactoryService,
            liveShareProjectPathProviderMock.Object,
            JoinableTaskContext);

        // Act
        var result = projectPathProvider.TryGetProjectPath(textBuffer, out var filePath);

        // Assert
        Assert.True(result);
        Assert.Equal(projectFilePath, filePath);
    }

    private static IVsHierarchy CreateVsHierarchy(string projectFilePath)
    {
        var vsHierarchyMock = new StrictMock<IVsHierarchy>();
        var vsProjectMock = vsHierarchyMock.As<IVsProject>();
        vsProjectMock
            .Setup(x => x.GetMkDocument((uint)VSConstants.VSITEMID.Root, out projectFilePath))
            .Returns(VSConstants.S_OK);

        return vsHierarchyMock.Object;
    }

    private static IVsRunningDocumentTable CreateVsRunningDocumentTable(string documentFilePath, IVsHierarchy? vsHierarchy)
    {
        var itemid = 19u;
        var punkDocData = IntPtr.Zero;
        var dwCookie = 23u;

        var vsRunningDocumentTableMock = new StrictMock<IVsRunningDocumentTable>();
        vsRunningDocumentTableMock
            .Setup(x => x.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock, documentFilePath, out vsHierarchy, out itemid, out punkDocData, out dwCookie))
            .Returns(VSConstants.S_OK);

        return vsRunningDocumentTableMock.Object;
    }

    private static ITextDocumentFactoryService CreateTextDocumentFactoryService(ITextBuffer textBuffer, string filePath)
    {
        var textDocumentMock = new StrictMock<ITextDocument>();
        textDocumentMock
            .SetupGet(x => x.FilePath)
            .Returns(filePath);

        var textDocument = textDocumentMock.Object;

        var textDocumentFactoryServiceMock = new StrictMock<ITextDocumentFactoryService>();
        textDocumentFactoryServiceMock
            .Setup(x => x.TryGetTextDocument(textBuffer, out textDocument))
            .Returns(true);

        return textDocumentFactoryServiceMock.Object;
    }
}
