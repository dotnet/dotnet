// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Test.Common;
using MonoDevelop.Projects;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

public class VisualStudioMacLSPEditorFeatureDetectorTest : TestBase
{
    public VisualStudioMacLSPEditorFeatureDetectorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void IsLSPEditorAvailable_ProjectSupported_ReturnsTrue()
    {
        // Arrange
        var featureDetector = new TestLSPEditorFeatureDetector()
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
        var featureDetector = new TestLSPEditorFeatureDetector()
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
        var featureDetector = new TestLSPEditorFeatureDetector()
        {
            IsRemoteClientValue = true,
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
        var featureDetector = new TestLSPEditorFeatureDetector()
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
        var featureDetector = new TestLSPEditorFeatureDetector()
        {
            IsRemoteClientValue = true,
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
        var featureDetector = new TestLSPEditorFeatureDetector();

        // Act
        var result = featureDetector.IsRemoteClient();

        // Assert
        Assert.False(result);
    }

#pragma warning disable CS0618 // Type or member is obsolete (Test constructor)
    private class TestLSPEditorFeatureDetector : VisualStudioMacLSPEditorFeatureDetector
    {
        public bool UseLegacyEditor { get; set; }

        public bool IsLiveShareHostValue { get; set; }

        public bool IsRemoteClientValue { get; set; }

        public bool ProjectSupportsLSPEditorValue { get; set; }

        public override bool IsLSPEditorAvailable() => !UseLegacyEditor;

        public override bool IsLiveShareHost() => IsLiveShareHostValue;

        public override bool IsRemoteClient() => IsRemoteClientValue;

        private protected override bool ProjectSupportsLSPEditor(string documentFilePath, DotNetProject project) => ProjectSupportsLSPEditorValue;
    }
#pragma warning restore CS0618 // Type or member is obsolete (Test constructor)
}
