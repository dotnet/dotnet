// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell.Interop;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

public class VisualStudioWindowsLSPEditorFeatureDetectorTest : TestBase
{
    public VisualStudioWindowsLSPEditorFeatureDetectorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void IsLSPEditorAvailable_ProjectSupported_ReturnsTrue()
    {
        // Arrange
        var logger = GetRazorLogger();
        var featureDetector = new TestLSPEditorFeatureDetector(logger)
        {
            ProjectSupportsLSPEditorValue = true,
        };

        // Act
        var result = featureDetector.IsLSPEditorAvailable("testMoniker", hierarchy: null);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsLSPEditorAvailable_LegacyEditorEnabled_ReturnsFalse()
    {
        // Arrange
        var logger = GetRazorLogger();
        var featureDetector = new TestLSPEditorFeatureDetector(logger)
        {
            UseLegacyEditor = true,
            ProjectSupportsLSPEditorValue = true,
        };

        // Act
        var result = featureDetector.IsLSPEditorAvailable("testMoniker", hierarchy: null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsLSPEditorAvailable_IsVSRemoteClient_ReturnsTrue()
    {
        // Arrange
        var logger = GetRazorLogger();
        var featureDetector = new TestLSPEditorFeatureDetector(logger)
        {
            IsVSRemoteClientValue = true,
            ProjectSupportsLSPEditorValue = true,
        };

        // Act
        var result = featureDetector.IsLSPEditorAvailable("testMoniker", hierarchy: null);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsLSPEditorAvailable_UnsupportedProject_ReturnsFalse()
    {
        // Arrange
        var logger = GetRazorLogger();
        var featureDetector = new TestLSPEditorFeatureDetector(logger)
        {
            ProjectSupportsLSPEditorValue = false,
        };

        // Act
        var result = featureDetector.IsLSPEditorAvailable("testMoniker", hierarchy: null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRemoteClient_VSRemoteClient_ReturnsTrue()
    {
        // Arrange
        var logger = GetRazorLogger();
        var featureDetector = new TestLSPEditorFeatureDetector(logger)
        {
            IsVSRemoteClientValue = true,
        };

        // Act
        var result = featureDetector.IsRemoteClient();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRemoteClient_LiveShareGuest_ReturnsTrue()
    {
        // Arrange
        var logger = GetRazorLogger();
        var featureDetector = new TestLSPEditorFeatureDetector(logger)
        {
            IsLiveShareGuestValue = true,
        };

        // Act
        var result = featureDetector.IsRemoteClient();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRemoteClient_UnknownEnvironment_ReturnsFalse()
    {
        // Arrange
        var logger = GetRazorLogger();
        var featureDetector = new TestLSPEditorFeatureDetector(logger);

        // Act
        var result = featureDetector.IsRemoteClient();

        // Assert
        Assert.False(result);
    }

    private static RazorLogger GetRazorLogger()
    {
        var mock = new Mock<RazorLogger>(MockBehavior.Strict);
        mock.Setup(l => l.LogVerbose(It.IsAny<string>()));

        return mock.Object;
    }

#pragma warning disable CS0618 // Type or member is obsolete (Test constructor)
    private class TestLSPEditorFeatureDetector : VisualStudioWindowsLSPEditorFeatureDetector
    {
        public TestLSPEditorFeatureDetector(RazorLogger logger)
            : base(projectCapabilityResolver: null, logger)
        {
        }

        public bool UseLegacyEditor { get; set; }

        public bool IsLiveShareGuestValue { get; set; }

        public bool IsLiveShareHostValue { get; set; }

        public bool IsVSRemoteClientValue { get; set; }

        public bool ProjectSupportsLSPEditorValue { get; set; }

        public override bool IsLSPEditorAvailable() => !UseLegacyEditor;

        public override bool IsLiveShareHost() => IsLiveShareHostValue;

        private protected override bool IsLiveShareGuest() => IsLiveShareGuestValue;

        private protected override bool IsVSRemoteClient() => IsVSRemoteClientValue;

        private protected override bool ProjectSupportsLSPEditor(string documentMoniker, IVsHierarchy hierarchy) => ProjectSupportsLSPEditorValue;
    }
#pragma warning restore CS0618 // Type or member is obsolete (Test constructor)
}
