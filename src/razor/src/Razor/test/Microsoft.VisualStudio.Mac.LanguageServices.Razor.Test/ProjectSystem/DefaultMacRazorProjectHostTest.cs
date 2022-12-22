// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using MonoDevelop.Projects.MSBuild;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

public class DefaultMacRazorProjectHostTest : TestBase
{
    public DefaultMacRazorProjectHostTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void IsRazorDocumentItem_UpperCaseExtension_ReturnsTrue()
    {
        // Arrange
        var item = new TestMSBuildItem("NonContent")
        {
            Include = "\\Path\\To\\File.RAZOR",
        };

        // Act
        var result = DefaultMacRazorProjectHost.IsRazorDocumentItem(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRazorDocumentItem_NonContentItem_ReturnsTrue()
    {
        // Arrange
        var item = new TestMSBuildItem("NonContent")
        {
            Include = "\\Path\\To\\File.razor",
        };

        // Act
        var result = DefaultMacRazorProjectHost.IsRazorDocumentItem(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRazorDocumentItem_NoInclude_ReturnsFalse()
    {
        // Arrange
        var item = new TestMSBuildItem("Content");

        // Act
        var result = DefaultMacRazorProjectHost.IsRazorDocumentItem(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRazorDocumentItem_NonRazorFile_ReturnsFalse()
    {
        // Arrange
        var item = new TestMSBuildItem("Content")
        {
            Include = "\\Path\\To\\File.notrazor",
        };

        // Act
        var result = DefaultMacRazorProjectHost.IsRazorDocumentItem(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRazorDocumentItem_RazorFile_ReturnsTrue()
    {
        // Arrange
        var item = new TestMSBuildItem("Content")
        {
            Include = "\\Path\\To\\File.razor",
        };

        // Act
        var result = DefaultMacRazorProjectHost.IsRazorDocumentItem(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRazorDocumentItem_CSHTMLFile_ReturnsTrue()
    {
        // Arrange
        var item = new TestMSBuildItem("Content")
        {
            Include = "\\Path\\To\\File.cshtml",
        };

        // Act
        var result = DefaultMacRazorProjectHost.IsRazorDocumentItem(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TryGetDefaultConfiguration_FailsIfNoConfiguration()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();

        // Act
        var result = DefaultMacRazorProjectHost.TryGetDefaultConfiguration(projectProperties, out var defaultConfiguration);

        // Assert
        Assert.False(result);
        Assert.Null(defaultConfiguration);
    }

    [Fact]
    public void TryGetDefaultConfiguration_FailsIfEmptyConfiguration()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorDefaultConfiguration", string.Empty);

        // Act
        var result = DefaultMacRazorProjectHost.TryGetDefaultConfiguration(projectProperties, out var defaultConfiguration);

        // Assert
        Assert.False(result);
        Assert.Null(defaultConfiguration);
    }

    [Fact]
    public void TryGetDefaultConfiguration_SucceedsWithValidConfiguration()
    {
        // Arrange
        var expectedConfiguration = "Razor-13.37";
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorDefaultConfiguration", expectedConfiguration);

        // Act
        var result = DefaultMacRazorProjectHost.TryGetDefaultConfiguration(projectProperties, out var defaultConfiguration);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedConfiguration, defaultConfiguration);
    }

    [Fact]
    public void TryGetLanguageVersion_FailsIfNoLanguageVersion()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();

        // Act
        var result = DefaultMacRazorProjectHost.TryGetLanguageVersion(projectProperties, out var languageVersion);

        // Assert
        Assert.False(result);
        Assert.Null(languageVersion);
    }

    [Fact]
    public void TryGetLanguageVersion_FailsIfEmptyLanguageVersion()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorLangVersion", string.Empty);

        // Act
        var result = DefaultMacRazorProjectHost.TryGetLanguageVersion(projectProperties, out var languageVersion);

        // Assert
        Assert.False(result);
        Assert.Null(languageVersion);
    }

    [Fact]
    public void TryGetLanguageVersion_SucceedsWithValidLanguageVersion()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorLangVersion", "1.0");

        // Act
        var result = DefaultMacRazorProjectHost.TryGetLanguageVersion(projectProperties, out var languageVersion);

        // Assert
        Assert.True(result);
        Assert.Same(RazorLanguageVersion.Version_1_0, languageVersion);
    }

    [Fact]
    public void TryGetLanguageVersion_SucceedsWithUnknownLanguageVersion_DefaultsToLatest()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorLangVersion", "13.37");

        // Act
        var result = DefaultMacRazorProjectHost.TryGetLanguageVersion(projectProperties, out var languageVersion);

        // Assert
        Assert.True(result);
        Assert.Same(RazorLanguageVersion.Latest, languageVersion);
    }

    [Fact]
    public void TryGetConfigurationItem_FailsNoRazorConfigurationItems()
    {
        // Arrange
        var projectItems = Enumerable.Empty<IMSBuildItemEvaluated>();

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfigurationItem("Razor-13.37", projectItems, out var configurationItem);

        // Assert
        Assert.False(result);
        Assert.Null(configurationItem);
    }

    [Fact]
    public void TryGetConfigurationItem_FailsNoMatchingRazorConfigurationItems()
    {
        // Arrange
        var projectItems = new IMSBuildItemEvaluated[]
        {
            new TestMSBuildItem("RazorConfiguration")
            {
                Include = "Razor-10.0",
            }
        };

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfigurationItem("Razor-13.37", projectItems, out var configurationItem);

        // Assert
        Assert.False(result);
        Assert.Null(configurationItem);
    }

    [Fact]
    public void TryGetConfigurationItem_SucceedsForMatchingConfigurationItem()
    {
        // Arrange
        var expectedConfiguration = "Razor-13.37";
        var expectedConfigurationItem = new TestMSBuildItem("RazorConfiguration")
        {
            Include = expectedConfiguration,
        };
        var projectItems = new IMSBuildItemEvaluated[]
        {
            new TestMSBuildItem("RazorConfiguration")
            {
                Include = "Razor-10.0-DoesNotMatch",
            },
            expectedConfigurationItem
        };

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfigurationItem(expectedConfiguration, projectItems, out var configurationItem);

        // Assert
        Assert.True(result);
        Assert.Same(expectedConfigurationItem, configurationItem);
    }

    [Fact]
    public void GetExtensionNames_FailsIfNoExtensions()
    {
        // Arrange
        var configurationItem = new TestMSBuildItem("RazorConfiguration");

        // Act
        var extensionNames = DefaultMacRazorProjectHost.GetExtensionNames(configurationItem);

        // Assert
        Assert.Empty(extensionNames);
    }

    [Fact]
    public void GetExtensionNames_FailsIfEmptyExtensions()
    {
        // Arrange
        var configurationItem = new TestMSBuildItem("RazorConfiguration");
        configurationItem.TestMetadata.SetValue("Extensions", string.Empty);

        // Act
        var extensionNames = DefaultMacRazorProjectHost.GetExtensionNames(configurationItem);

        // Assert
        Assert.Empty(extensionNames);
    }

    [Fact]
    public void GetExtensionNames_SucceedsIfSingleExtension()
    {
        // Arrange
        var expectedExtensionName = "SomeExtensionName";
        var configurationItem = new TestMSBuildItem("RazorConfiguration");
        configurationItem.TestMetadata.SetValue("Extensions", expectedExtensionName);

        // Act
        var extensionNames = DefaultMacRazorProjectHost.GetExtensionNames(configurationItem);

        // Assert
        var extensionName = Assert.Single(extensionNames);
        Assert.Equal(expectedExtensionName, extensionName);
    }

    [Fact]
    public void GetExtensionNames_SucceedsIfMultipleExtensions()
    {
        // Arrange
        var configurationItem = new TestMSBuildItem("RazorConfiguration");
        configurationItem.TestMetadata.SetValue("Extensions", "SomeExtensionName;SomeOtherExtensionName");

        // Act
        var extensionNames = DefaultMacRazorProjectHost.GetExtensionNames(configurationItem);

        // Assert
        Assert.Collection(
            extensionNames,
            name => Assert.Equal("SomeExtensionName", name),
            name => Assert.Equal("SomeOtherExtensionName", name));
    }

    [Fact]
    public void GetExtensions_NoExtensionTypes_ReturnsEmptyArray()
    {
        // Arrange
        var projectItems = new IMSBuildItemEvaluated[]
        {
            new TestMSBuildItem("NotAnExtension")
            {
                Include = "Extension1",
            },
        };

        // Act
        var extensions = DefaultMacRazorProjectHost.GetExtensions(new[] { "Extension1", "Extension2" }, projectItems);

        // Assert
        Assert.Empty(extensions);
    }

    [Fact]
    public void GetExtensions_UnConfiguredExtensionTypes_ReturnsEmptyArray()
    {
        // Arrange
        var projectItems = new IMSBuildItemEvaluated[]
        {
            new TestMSBuildItem("RazorExtension")
            {
                Include = "UnconfiguredExtensionName",
            },
        };

        // Act
        var extensions = DefaultMacRazorProjectHost.GetExtensions(new[] { "Extension1", "Extension2" }, projectItems);

        // Assert
        Assert.Empty(extensions);
    }

    [Fact]
    public void GetExtensions_SomeConfiguredExtensions_ReturnsConfiguredExtensions()
    {
        // Arrange
        var expectedExtension1Name = "Extension1";
        var expectedExtension2Name = "Extension2";
        var projectItems = new IMSBuildItemEvaluated[]
        {
            new TestMSBuildItem("RazorExtension")
            {
                Include = "UnconfiguredExtensionName",
            },
            new TestMSBuildItem("RazorExtension")
            {
                Include = expectedExtension1Name,
            },
            new TestMSBuildItem("RazorExtension")
            {
                Include = expectedExtension2Name,
            },
        };

        // Act
        var extensions = DefaultMacRazorProjectHost.GetExtensions(new[] { expectedExtension1Name, expectedExtension2Name }, projectItems);

        // Assert
        Assert.Collection(
            extensions,
            extension => Assert.Equal(expectedExtension1Name, extension.ExtensionName),
            extension => Assert.Equal(expectedExtension2Name, extension.ExtensionName));
    }

    [Fact]
    public void TryGetConfiguration_FailsIfNoDefaultConfiguration()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        var projectItems = Array.Empty<IMSBuildItemEvaluated>();

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfiguration(projectProperties, projectItems, out var configuration);

        // Assert
        Assert.False(result);
        Assert.Null(configuration);
    }

    [Fact]
    public void TryGetConfiguration_FailsIfNoLanguageVersion()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorDefaultConfiguration", "Razor-13.37");
        var projectItems = Array.Empty<IMSBuildItemEvaluated>();

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfiguration(projectProperties, projectItems, out var configuration);

        // Assert
        Assert.False(result);
        Assert.Null(configuration);
    }

    [Fact]
    public void TryGetConfiguration_FailsIfNoConfigurationItems()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorDefaultConfiguration", "Razor-13.37");
        projectProperties.SetValue("RazorLangVersion", "1.0");
        var projectItems = Array.Empty<IMSBuildItemEvaluated>();

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfiguration(projectProperties, projectItems, out var configuration);

        // Assert
        Assert.False(result);
        Assert.Null(configuration);
    }

    [Fact]
    public void TryGetConfiguration_SucceedsWithoutConfiguredExtensionNames()
    {
        // Arrange
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorDefaultConfiguration", "Razor-13.37");
        projectProperties.SetValue("RazorLangVersion", "1.0");
        var projectItems = new IMSBuildItemEvaluated[]
        {
            new TestMSBuildItem("RazorConfiguration")
            {
                Include = "Razor-13.37",
            },
        };

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfiguration(projectProperties, projectItems, out var configuration);

        // Assert
        Assert.True(result);
        Assert.Empty(configuration.Extensions);
    }

    // This is more of an integration test but is here to test the overall flow/functionality
    [Fact]
    public void TryGetConfiguration_SucceedsWithAllPreRequisites()
    {
        // Arrange
        var expectedLanguageVersion = RazorLanguageVersion.Version_1_0;
        var expectedConfigurationName = "Razor-Test";
        var expectedExtension1Name = "Extension1";
        var expectedExtension2Name = "Extension2";
        var expectedRazorConfigurationItem = new TestMSBuildItem("RazorConfiguration")
        {
            Include = expectedConfigurationName,
        };
        expectedRazorConfigurationItem.TestMetadata.SetValue("Extensions", "Extension1;Extension2");
        var projectItems = new IMSBuildItemEvaluated[]
        {
            new TestMSBuildItem("RazorConfiguration")
            {
                Include = "UnconfiguredRazorConfiguration",
            },
            new TestMSBuildItem("RazorExtension")
            {
                Include = "UnconfiguredExtensionName",
            },
            new TestMSBuildItem("RazorExtension")
            {
                Include = expectedExtension1Name,
            },
            new TestMSBuildItem("RazorExtension")
            {
                Include = expectedExtension2Name,
            },
            expectedRazorConfigurationItem,
        };
        var projectProperties = new MSBuildPropertyGroup();
        projectProperties.SetValue("RazorDefaultConfiguration", expectedConfigurationName);
        projectProperties.SetValue("RazorLangVersion", "1.0");

        // Act
        var result = DefaultMacRazorProjectHost.TryGetConfiguration(projectProperties, projectItems, out var configuration);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedLanguageVersion, configuration.LanguageVersion);
        Assert.Equal(expectedConfigurationName, configuration.ConfigurationName);
        Assert.Collection(
            configuration.Extensions,
            extension => Assert.Equal(expectedExtension1Name, extension.ExtensionName),
            extension => Assert.Equal(expectedExtension2Name, extension.ExtensionName));
    }

    private class TestMSBuildItem : IMSBuildItemEvaluated
    {
        public TestMSBuildItem(string name)
        {
            Name = name;
            TestMetadata = new MSBuildPropertyGroup();
        }

        public string Name { get; }

        public string Include { get; set; }

        public MSBuildPropertyGroup TestMetadata { get; }

        public IMSBuildPropertyGroupEvaluated Metadata => TestMetadata;

        public string Condition => throw new NotImplementedException();

        public bool IsImported => throw new NotImplementedException();

        public string UnevaluatedInclude => throw new NotImplementedException();

        public MSBuildItem SourceItem => throw new NotImplementedException();

        public IEnumerable<MSBuildItem> SourceItems => throw new NotImplementedException();
    }
}
