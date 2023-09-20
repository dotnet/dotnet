// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.VisualStudio.Editor.Razor;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

public class DefaultDotNetProjectHostTest : ProjectSnapshotManagerDispatcherTestBase
{
    public DefaultDotNetProjectHostTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void UpdateRazorHostProject_UnsupportedProjectNoops()
    {
        // Arrange
        var projectService = new Mock<TextBufferProjectService>(MockBehavior.Strict);
        projectService.Setup(p => p.IsSupportedProject(It.IsAny<object>()))
            .Returns(false);
        var dotNetProjectHost = new DefaultDotNetProjectHost(
            Dispatcher,
            Mock.Of<VisualStudioMacWorkspaceAccessor>(MockBehavior.Strict),
            projectService.Object,
            TestProjectConfigurationFilePathStore.Instance,
            TestVSLanguageServerFeatureOptions.Instance);

        // Act & Assert
        dotNetProjectHost.UpdateRazorHostProject();
    }

    // -------------------------------------------------------------------------------------------
    // Purposefully do not have any more tests here because that would involve mocking MonoDevelop
    // types. The default constructors for the Solution / DotNetProject MonoDevelop types change
    // static classes (they assume they're being created in an IDE).
    // -------------------------------------------------------------------------------------------
}
