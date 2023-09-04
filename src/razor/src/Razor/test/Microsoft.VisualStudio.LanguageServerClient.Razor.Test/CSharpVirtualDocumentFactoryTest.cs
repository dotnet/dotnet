// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

public class CSharpVirtualDocumentFactoryTest : TestBase
{
    private readonly ITextBuffer _nonRazorLSPBuffer;
    private readonly ITextBuffer _razorLSPBuffer;
    private readonly IContentTypeRegistryService _contentTypeRegistryService;
    private readonly ITextBufferFactoryService _textBufferFactoryService;
    private readonly ITextDocumentFactoryService TextDocumentFactoryService;
    private readonly FilePathService _filePathService;

    public CSharpVirtualDocumentFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var csharpContentType = new Mock<IContentType>(MockBehavior.Strict).Object;
        Mock.Get(csharpContentType).Setup(t => t.TypeName).Returns("CSharp");
        Mock.Get(csharpContentType).Setup(t => t.DisplayName).Returns("CSharp");
        _contentTypeRegistryService = Mock.Of<IContentTypeRegistryService>(
            registry => registry.GetContentType(RazorLSPConstants.CSharpContentTypeName) == csharpContentType, MockBehavior.Strict);
        var textBufferFactoryService = new Mock<ITextBufferFactoryService>(MockBehavior.Strict);
        textBufferFactoryService
            .Setup(factory => factory.CreateTextBuffer())
            .Returns(() =>
            {
                var factoryBuffer = Mock.Of<ITextBuffer>(buffer => buffer.CurrentSnapshot == Mock.Of<ITextSnapshot>(MockBehavior.Strict) && buffer.Properties == new PropertyCollection(), MockBehavior.Strict);
                Mock.Get(factoryBuffer).Setup(b => b.ChangeContentType(It.IsAny<IContentType>(), It.IsAny<object>())).Verifiable();
                return factoryBuffer;
            });
        _textBufferFactoryService = textBufferFactoryService.Object;

        var razorLSPContentType = Mock.Of<IContentType>(contentType => contentType.IsOfType(RazorConstants.RazorLSPContentTypeName) == true, MockBehavior.Strict);
        _razorLSPBuffer = Mock.Of<ITextBuffer>(textBuffer => textBuffer.ContentType == razorLSPContentType, MockBehavior.Strict);

        var nonRazorLSPContentType = Mock.Of<IContentType>(contentType => contentType.IsOfType(It.IsAny<string>()) == false, MockBehavior.Strict);
        _nonRazorLSPBuffer = Mock.Of<ITextBuffer>(textBuffer => textBuffer.ContentType == nonRazorLSPContentType, MockBehavior.Strict);

        TextDocumentFactoryService = new Mock<ITextDocumentFactoryService>(MockBehavior.Strict).Object;
        Mock.Get(TextDocumentFactoryService).Setup(s => s.CreateTextDocument(It.IsAny<ITextBuffer>(), It.IsAny<string>())).Returns((ITextDocument)null);

        _filePathService = new FilePathService(TestLanguageServerFeatureOptions.Instance);
    }

    [Fact]
    public void TryCreateMultipleFor_NonRazorLSPBuffer_ReturnsFalse()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.razor");
        var uriProvider = Mock.Of<FileUriProvider>(provider => provider.GetOrCreate(It.IsAny<ITextBuffer>()) == uri, MockBehavior.Strict);
        var projectSnapshotManagerAccessor = Mock.Of<ProjectSnapshotManagerAccessor>(MockBehavior.Strict);
        var factory = new CSharpVirtualDocumentFactory(_contentTypeRegistryService, _textBufferFactoryService, TextDocumentFactoryService, uriProvider, _filePathService, projectSnapshotManagerAccessor, TestLanguageServerFeatureOptions.Instance, new TestOutputWindowLogger(), telemetryReporter: null);

        // Act
        var result = factory.TryCreateMultipleFor(_nonRazorLSPBuffer, out var virtualDocuments);

        // Assert
        Assert.False(result);
        Assert.Null(virtualDocuments);
    }

    [Fact]
    public void TryCreateMultipleFor_RazorLSPBuffer_ReturnsCSharpVirtualDocumentAndTrue()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.razor");
        var uriProvider = Mock.Of<FileUriProvider>(provider => provider.GetOrCreate(_razorLSPBuffer) == uri, MockBehavior.Strict);
        Mock.Get(uriProvider).Setup(p => p.AddOrUpdate(It.IsAny<ITextBuffer>(), It.IsAny<Uri>())).Verifiable();

        var projectSnapshotManager = TestProjectSnapshotManager.Create(ErrorReporter, new TestDispatcher());
        var project = projectSnapshotManager.CreateAndAddProject(@"C:\path\to\project.csproj");
        projectSnapshotManager.CreateAndAddDocument(project, @"C:\path\to\file.razor");
        var projectSnapshotManagerAccessor = Mock.Of<ProjectSnapshotManagerAccessor>(a => a.Instance == projectSnapshotManager, MockBehavior.Strict);

        var factory = new CSharpVirtualDocumentFactory(_contentTypeRegistryService, _textBufferFactoryService, TextDocumentFactoryService, uriProvider, _filePathService, projectSnapshotManagerAccessor, TestLanguageServerFeatureOptions.Instance, new TestOutputWindowLogger(), telemetryReporter: null);

        // Act
        var result = factory.TryCreateMultipleFor(_razorLSPBuffer, out var virtualDocuments);

        // Assert
        Assert.True(result);
        using var virtualDocument = Assert.Single(virtualDocuments);
        Assert.EndsWith(TestLanguageServerFeatureOptions.Instance.CSharpVirtualDocumentSuffix, virtualDocument.Uri.OriginalString, StringComparison.Ordinal);
    }

    [Fact]
    public void TryCreateMultipleFor_RazorLSPBuffer_ReturnsMultipleCSharpVirtualDocumentsAndTrue()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.razor");
        var uriProvider = Mock.Of<FileUriProvider>(provider => provider.GetOrCreate(_razorLSPBuffer) == uri, MockBehavior.Strict);
        Mock.Get(uriProvider).Setup(p => p.AddOrUpdate(It.IsAny<ITextBuffer>(), It.IsAny<Uri>())).Verifiable();

        var projectSnapshotManager = TestProjectSnapshotManager.Create(ErrorReporter, new TestDispatcher());
        var project = TestProjectSnapshot.Create(@"C:\path\to\project1.csproj", @"C:\path\to\obj1", Array.Empty<string>(), RazorConfiguration.Default, projectWorkspaceState: null);
        projectSnapshotManager.ProjectAdded(project.HostProject);
        projectSnapshotManager.CreateAndAddDocument(project, @"C:\path\to\file.razor");
        project = TestProjectSnapshot.Create(@"C:\path\to\project2.csproj", @"C:\path\to\obj2", Array.Empty<string>(), RazorConfiguration.Default, projectWorkspaceState: null);
        projectSnapshotManager.ProjectAdded(project.HostProject);
        projectSnapshotManager.CreateAndAddDocument(project, @"C:\path\to\file.razor");
        var projectSnapshotManagerAccessor = Mock.Of<ProjectSnapshotManagerAccessor>(a => a.Instance == projectSnapshotManager, MockBehavior.Strict);

        var languageServerFeatureOptions = new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true);
        var filePathService = new FilePathService(languageServerFeatureOptions);
        var factory = new CSharpVirtualDocumentFactory(_contentTypeRegistryService, _textBufferFactoryService, TextDocumentFactoryService, uriProvider, filePathService, projectSnapshotManagerAccessor, languageServerFeatureOptions, new TestOutputWindowLogger(), telemetryReporter: null);

        // Act
        var result = factory.TryCreateMultipleFor(_razorLSPBuffer, out var virtualDocuments);

        // Assert
        Assert.True(result);
        Assert.Equal(2, virtualDocuments.Length);
        Assert.Collection(virtualDocuments,
            item => Assert.Equal("C:/path/to/file.razor.ooJmNcWMKXNlf5MK.ide.g.cs", item.Uri.OriginalString),
            item => Assert.Equal("C:/path/to/file.razor.jGYrFHvWEciJi85y.ide.g.cs", item.Uri.OriginalString));
    }

    private class TestOutputWindowLogger : IOutputWindowLogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public void SetTestLogger(ILogger testOutputLogger)
        {
        }
    }

    private class TestDispatcher : ProjectSnapshotManagerDispatcher
    {
        // The tests run synchronously without the dispatcher, so just assert that
        // we're always on the right thread
        public override bool IsDispatcherThread => true;

        public override TaskScheduler DispatcherScheduler => TaskScheduler.Default;
    }
}
