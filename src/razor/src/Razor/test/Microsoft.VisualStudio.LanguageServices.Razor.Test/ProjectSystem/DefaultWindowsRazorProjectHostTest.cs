// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Moq;
using Xunit;
using Xunit.Abstractions;
using ItemCollection = Microsoft.VisualStudio.ProjectSystem.ItemCollection;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DefaultWindowsRazorProjectHostTest : ProjectSnapshotManagerDispatcherWorkspaceTestBase
{
    private readonly ItemCollection _configurationItems;
    private readonly ItemCollection _extensionItems;
    private readonly ProjectConfigurationFilePathStore _projectConfigurationFilePathStore;
    private readonly ItemCollection _razorComponentWithTargetPathItems;
    private readonly ItemCollection _razorGenerateWithTargetPathItems;
    private readonly PropertyCollection _razorGeneralProperties;
    private readonly PropertyCollection _configurationGeneral;
    private readonly TestProjectSnapshotManager _projectManager;

    public DefaultWindowsRazorProjectHostTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = new TestProjectSnapshotManager(Workspace, Dispatcher);

        var projectConfigurationFilePathStore = new Mock<ProjectConfigurationFilePathStore>(MockBehavior.Strict);
        projectConfigurationFilePathStore.Setup(s => s.Remove(It.IsAny<ProjectKey>())).Verifiable();
        _projectConfigurationFilePathStore = projectConfigurationFilePathStore.Object;

        _configurationItems = new ItemCollection(Rules.RazorConfiguration.SchemaName);
        _extensionItems = new ItemCollection(Rules.RazorExtension.SchemaName);
        _razorComponentWithTargetPathItems = new ItemCollection(Rules.RazorComponentWithTargetPath.SchemaName);
        _razorGenerateWithTargetPathItems = new ItemCollection(Rules.RazorGenerateWithTargetPath.SchemaName);
        _razorGeneralProperties = new PropertyCollection(Rules.RazorGeneral.SchemaName);

        _configurationGeneral = new PropertyCollection(WindowsRazorProjectHostBase.ConfigurationGeneralSchemaName);
    }

    [Fact]
    public void TryGetDefaultConfiguration_FailsIfNoRule()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>().ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetDefaultConfiguration(projectState, out var defaultConfiguration);

        // Assert
        Assert.False(result);
        Assert.Null(defaultConfiguration);
    }

    [Fact]
    public void TryGetDefaultConfiguration_FailsIfNoConfiguration()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(Rules.RazorGeneral.SchemaName, new Dictionary<string, string>())
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetDefaultConfiguration(projectState, out var defaultConfiguration);

        // Assert
        Assert.False(result);
        Assert.Null(defaultConfiguration);
    }

    [Fact]
    public void TryGetDefaultConfiguration_FailsIfEmptyConfiguration()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorDefaultConfigurationProperty] = string.Empty
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetDefaultConfiguration(projectState, out var defaultConfiguration);

        // Assert
        Assert.False(result);
        Assert.Null(defaultConfiguration);
    }

    [Fact]
    public void TryGetDefaultConfiguration_SucceedsWithValidConfiguration()
    {
        // Arrange
        var expectedConfiguration = "Razor-13.37";
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorDefaultConfigurationProperty] = expectedConfiguration
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetDefaultConfiguration(projectState, out var defaultConfiguration);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedConfiguration, defaultConfiguration);
    }

    [Fact]
    public void TryGetLanguageVersion_FailsIfNoRule()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>().ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetLanguageVersion(projectState, out var languageVersion);

        // Assert
        Assert.False(result);
        Assert.Null(languageVersion);
    }

    [Fact]
    public void TryGetLanguageVersion_FailsIfNoLanguageVersion()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(Rules.RazorGeneral.SchemaName, new Dictionary<string, string>())
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetLanguageVersion(projectState, out var languageVersion);

        // Assert
        Assert.False(result);
        Assert.Null(languageVersion);
    }

    [Fact]
    public void TryGetLanguageVersion_FailsIfEmptyLanguageVersion()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorLangVersionProperty] = string.Empty
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetLanguageVersion(projectState, out var languageVersion);

        // Assert
        Assert.False(result);
        Assert.Null(languageVersion);
    }

    [Fact]
    public void TryGetLanguageVersion_SucceedsWithValidLanguageVersion()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorLangVersionProperty] = "1.0"
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetLanguageVersion(projectState, out var languageVersion);

        // Assert
        Assert.True(result);
        Assert.Same(RazorLanguageVersion.Version_1_0, languageVersion);
    }

    [Fact]
    public void TryGetLanguageVersion_SucceedsWithUnknownLanguageVersion_DefaultsToLatest()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorLangVersionProperty] = "13.37"
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetLanguageVersion(projectState, out var languageVersion);

        // Assert
        Assert.True(result);
        Assert.Same(RazorLanguageVersion.Latest, languageVersion);
    }

    [Fact]
    public void TryGetConfigurationItem_FailsNoRazorConfigurationRule()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>().ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfigurationItem("Razor-13.37", projectState, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetConfigurationItem_FailsNoRazorConfigurationItems()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorConfiguration.SchemaName] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorConfiguration.SchemaName,
                new Dictionary<string, Dictionary<string, string>>())
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfigurationItem("Razor-13.37", projectState, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetConfigurationItem_FailsNoMatchingRazorConfigurationItems()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorConfiguration.SchemaName] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorConfiguration.SchemaName,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    ["Razor-10.0"] = new Dictionary<string, string>(),
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfigurationItem("Razor-13.37", projectState, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetConfigurationItem_SucceedsForMatchingConfigurationItem()
    {
        // Arrange
        var expectedConfiguration = "Razor-13.37";
        var expectedConfigurationValue = new Dictionary<string, string>()
        {
            [Rules.RazorConfiguration.ExtensionsProperty] = "SomeExtension"
        };
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorConfiguration.SchemaName] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorConfiguration.SchemaName,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    [expectedConfiguration] = expectedConfigurationValue
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfigurationItem(expectedConfiguration, projectState, out var configurationItem);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedConfiguration, configurationItem.Key);
        Assert.True(Enumerable.SequenceEqual(expectedConfigurationValue, configurationItem.Value));
    }

    [Fact]
    public void GetExtensionNames_SucceedsWithNoExtensions()
    {
        // Arrange
        var items = new ItemCollection(Rules.RazorConfiguration.SchemaName);
        items.Item("Test");

        var item = items.ToSnapshot().Items.Single();

        // Act
        var extensionNames = DefaultWindowsRazorProjectHost.GetExtensionNames(item);

        // Assert
        Assert.Empty(extensionNames);
    }

    [Fact]
    public void GetExtensionNames_SucceedsWithEmptyExtensions()
    {
        // Arrange
        var items = new ItemCollection(Rules.RazorConfiguration.SchemaName);
        items.Item("Test");
        items.Property("Test", Rules.RazorConfiguration.ExtensionsProperty, string.Empty);

        var item = items.ToSnapshot().Items.Single();

        // Act
        var extensionNames = DefaultWindowsRazorProjectHost.GetExtensionNames(item);

        // Assert
        Assert.Empty(extensionNames);
    }

    [Fact]
    public void GetExtensionNames_SucceedsIfSingleExtension()
    {
        // Arrange
        var expectedExtensionName = "SomeExtensionName";

        var items = new ItemCollection(Rules.RazorConfiguration.SchemaName);
        items.Item("Test");
        items.Property("Test", Rules.RazorConfiguration.ExtensionsProperty, "SomeExtensionName");

        var item = items.ToSnapshot().Items.Single();

        // Act
        var extensionNames = DefaultWindowsRazorProjectHost.GetExtensionNames(item);

        // Assert
        var extensionName = Assert.Single(extensionNames);
        Assert.Equal(expectedExtensionName, extensionName);
    }

    [Fact]
    public void GetExtensionNames_SucceedsIfMultipleExtensions()
    {
        // Arrange
        var items = new ItemCollection(Rules.RazorConfiguration.SchemaName);
        items.Item("Test");
        items.Property("Test", Rules.RazorConfiguration.ExtensionsProperty, "SomeExtensionName;SomeOtherExtensionName");

        var item = items.ToSnapshot().Items.Single();

        // Act
        var extensionNames = DefaultWindowsRazorProjectHost.GetExtensionNames(item);

        // Assert
        Assert.Collection(
            extensionNames,
            name => Assert.Equal("SomeExtensionName", name),
            name => Assert.Equal("SomeOtherExtensionName", name));
    }

    [Fact]
    public void TryGetExtensions_SucceedsWhenExtensionsNotFound()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>().ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetExtensions(new[] { "Extension1", "Extension2" }, projectState, out var extensions);

        // Assert
        Assert.True(result);
        Assert.Empty(extensions);
    }

    [Fact]
    public void TryGetExtensions_SucceedsWithUnConfiguredExtensionTypes()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorExtension.PrimaryDataSourceItemType] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorExtension.PrimaryDataSourceItemType,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    ["UnconfiguredExtensionName"] = new Dictionary<string, string>()
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetExtensions(new[] { "Extension1", "Extension2" }, projectState, out var extensions);

        // Assert
        Assert.True(result);
        Assert.Empty(extensions);
    }

    [Fact]
    public void TryGetExtensions_SucceedsWithSomeConfiguredExtensions()
    {
        // Arrange
        var expectedExtension1Name = "Extension1";
        var expectedExtension2Name = "Extension2";
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorExtension.PrimaryDataSourceItemType] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorExtension.PrimaryDataSourceItemType,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    ["UnconfiguredExtensionName"] = new Dictionary<string, string>(),
                    [expectedExtension1Name] = new Dictionary<string, string>(),
                    [expectedExtension2Name] = new Dictionary<string, string>(),
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetExtensions(new[] { expectedExtension1Name, expectedExtension2Name }, projectState, out var extensions);

        // Assert
        Assert.True(result);
        Assert.Collection(
            extensions,
            extension => Assert.Equal(expectedExtension2Name, extension.ExtensionName),
            extension => Assert.Equal(expectedExtension1Name, extension.ExtensionName));
    }

    [Fact]
    public void TryGetConfiguration_FailsIfNoDefaultConfiguration()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>().ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfiguration(projectState, out var configuration);

        // Assert
        Assert.False(result);
        Assert.Null(configuration);
    }

    [Fact]
    public void TryGetConfiguration_FailsIfNoLanguageVersion()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorDefaultConfigurationProperty] = "13.37"
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfiguration(projectState, out var configuration);

        // Assert
        Assert.False(result);
        Assert.Null(configuration);
    }

    [Fact]
    public void TryGetConfiguration_FailsIfNoConfigurationItems()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorDefaultConfigurationProperty] = "13.37",
                    [Rules.RazorGeneral.RazorLangVersionProperty] = "1.0",
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfiguration(projectState, out var configuration);

        // Assert
        Assert.False(result);
        Assert.Null(configuration);
    }

    [Fact]
    public void TryGetConfiguration_SucceedsWithNoConfiguredExtensionNames()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorDefaultConfigurationProperty] = "Razor-13.37",
                    [Rules.RazorGeneral.RazorLangVersionProperty] = "1.0",
                }),
            [Rules.RazorConfiguration.SchemaName] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorConfiguration.SchemaName,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    ["Razor-13.37"] = new Dictionary<string, string>()
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfiguration(projectState, out var configuration);

        // Assert
        Assert.True(result);
        Assert.Equal(RazorLanguageVersion.Version_1_0, configuration.LanguageVersion);
        Assert.Equal("Razor-13.37", configuration.ConfigurationName);
        Assert.Empty(configuration.Extensions);
    }

    [Fact]
    public void TryGetConfiguration_IgnoresMissingExtension()
    {
        // Arrange
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorDefaultConfigurationProperty] = "13.37",
                    [Rules.RazorGeneral.RazorLangVersionProperty] = "1.0",
                }),
            [Rules.RazorConfiguration.SchemaName] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorConfiguration.SchemaName,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    ["13.37"] = new Dictionary<string, string>()
                    {
                        ["Extensions"] = "Razor-13.37"
                    }
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfiguration(projectState, out var configuration);

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
        var projectState = new Dictionary<string, IProjectRuleSnapshot>()
        {
            [Rules.RazorGeneral.SchemaName] = TestProjectRuleSnapshot.CreateProperties(
                Rules.RazorGeneral.SchemaName,
                new Dictionary<string, string>()
                {
                    [Rules.RazorGeneral.RazorDefaultConfigurationProperty] = expectedConfigurationName,
                    [Rules.RazorGeneral.RazorLangVersionProperty] = "1.0",
                }),
            [Rules.RazorConfiguration.SchemaName] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorConfiguration.SchemaName,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    ["UnconfiguredRazorConfiguration"] = new Dictionary<string, string>()
                    {
                        ["Extensions"] = "Razor-9.0"
                    },
                    [expectedConfigurationName] = new Dictionary<string, string>()
                    {
                        ["Extensions"] = expectedExtension1Name + ";" + expectedExtension2Name
                    }
                }),
            [Rules.RazorExtension.PrimaryDataSourceItemType] = TestProjectRuleSnapshot.CreateItems(
                Rules.RazorExtension.PrimaryDataSourceItemType,
                new Dictionary<string, Dictionary<string, string>>()
                {
                    [expectedExtension1Name] = new Dictionary<string, string>(),
                    [expectedExtension2Name] = new Dictionary<string, string>(),
                })
        }.ToImmutableDictionary();

        // Act
        var result = DefaultWindowsRazorProjectHost.TryGetConfiguration(projectState, out var configuration);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedLanguageVersion, configuration.LanguageVersion);
        Assert.Equal(expectedConfigurationName, configuration.ConfigurationName);
        Assert.Collection(
            configuration.Extensions.OrderBy(e => e.ExtensionName),
            extension => Assert.Equal(expectedExtension1Name, extension.ExtensionName),
            extension => Assert.Equal(expectedExtension2Name, extension.ExtensionName));
    }

    [UIFact]
    public async Task DefaultRazorProjectHost_UIThread_CreateAndDispose_Succeeds()
    {
        // Arrange
        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);

        // Act & Assert
        await host.LoadAsync();
        Assert.Empty(_projectManager.GetProjects());

        await host.DisposeAsync();
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task DefaultRazorProjectHost_BackgroundThread_CreateAndDispose_Succeeds()
    {
        // Arrange
        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);

        // Act & Assert
        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact] // This can happen if the .xaml files aren't included correctly.
    public async Task DefaultRazorProjectHost_OnProjectChanged_NoRulesDefined()
    {
        // Arrange
        var changes = new TestProjectChangeDescription[]
        {
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);

        // Act & Assert
        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectChanged_ReadsProperties_InitializesProject()
    {
        // Arrange
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.1");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.1");

        _configurationItems.Item("MVC-2.1");
        _configurationItems.Property("MVC-2.1", Rules.RazorConfiguration.ExtensionsProperty, "MVC-2.1;Another-Thing");

        _extensionItems.Item("MVC-2.1");
        _extensionItems.Item("Another-Thing");

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentFile1.TargetPath);

        _razorGenerateWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath));
        _razorGenerateWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectFile1.TargetPath);

        _configurationGeneral.Property(WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName, TestProjectData.SomeProject.IntermediateOutputPath);
        _configurationGeneral.Property(WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName, "obj");

        var changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(),
             _configurationItems.ToChange(),
             _extensionItems.ToChange(),
             _razorComponentWithTargetPathItems.ToChange(),
             _razorGenerateWithTargetPathItems.ToChange(),
             _configurationGeneral.ToChange(),
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);

        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);
        host.SkipIntermediateOutputPathExistCheck_TestOnly = true;

        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act
        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert
        var snapshot = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, snapshot.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_1, snapshot.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.1", snapshot.Configuration.ConfigurationName);
        Assert.Collection(
            snapshot.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.1", e.ExtensionName));

        Assert.Collection(
            snapshot.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectChanged_NoVersionFound_DoesNotIniatializeProject()
    {
        // Arrange
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "");

        _configurationItems.Item("TestConfiguration");

        _extensionItems.Item("TestExtension");

        _razorGenerateWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath));

        var changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(),
             _configurationItems.ToChange(),
             _extensionItems.ToChange(),
             _razorComponentWithTargetPathItems.ToChange(),
             _razorGenerateWithTargetPathItems.ToChange(),
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);

        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act
        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectChanged_UpdateProject_MarksSolutionOpen()
    {
        // Arrange
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.1");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.1");

        _configurationItems.Item("MVC-2.1");
        _configurationItems.Property("MVC-2.1", Rules.RazorConfiguration.ExtensionsProperty, "MVC-2.1;Another-Thing");

        _extensionItems.Item("MVC-2.1");
        _extensionItems.Item("Another-Thing");

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath), Rules.RazorComponentWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentFile1.TargetPath);

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentImportFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentImportFile1.FilePath), Rules.RazorComponentWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentImportFile1.TargetPath);

        _razorGenerateWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath));
        _razorGenerateWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectFile1.TargetPath);

        _configurationGeneral.Property(WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName, TestProjectData.SomeProject.IntermediateOutputPath);
        _configurationGeneral.Property(WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName, "obj");

        var changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(),
             _configurationItems.ToChange(),
             _extensionItems.ToChange(),
             _razorComponentWithTargetPathItems.ToChange(),
             _razorGenerateWithTargetPathItems.ToChange(),
             _configurationGeneral.ToChange(),
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);
        host.SkipIntermediateOutputPathExistCheck_TestOnly = true;

        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        _projectManager.SolutionClosed();

        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        Assert.False(_projectManager.IsSolutionClosing);
    }

    [UIFact]
    public async Task OnProjectChanged_UpdateProject_Succeeds()
    {
        // Arrange
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.1");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.1");

        _configurationItems.Item("MVC-2.1");
        _configurationItems.Property("MVC-2.1", Rules.RazorConfiguration.ExtensionsProperty, "MVC-2.1;Another-Thing");

        _extensionItems.Item("MVC-2.1");
        _extensionItems.Item("Another-Thing");

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath), Rules.RazorComponentWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentFile1.TargetPath);

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentImportFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentImportFile1.FilePath), Rules.RazorComponentWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentImportFile1.TargetPath);

        _razorGenerateWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath));
        _razorGenerateWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectFile1.TargetPath);

        _configurationGeneral.Property(WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName, TestProjectData.SomeProject.IntermediateOutputPath);
        _configurationGeneral.Property(WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName, "obj");

        var changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(),
             _configurationItems.ToChange(),
             _extensionItems.ToChange(),
             _razorComponentWithTargetPathItems.ToChange(),
             _razorGenerateWithTargetPathItems.ToChange(),
             _configurationGeneral.ToChange(),
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);
        host.SkipIntermediateOutputPathExistCheck_TestOnly = true;

        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        var snapshot = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, snapshot.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_1, snapshot.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.1", snapshot.Configuration.ConfigurationName);
        Assert.Collection(
            snapshot.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.1", e.ExtensionName));

        Assert.Collection(
            snapshot.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.ComponentImport, document.FileKind);
            },
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
            },
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        // Act - 2
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.0");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.0");
        _configurationItems.RemoveItem("MVC-2.1");
        _configurationItems.Item("MVC-2.0", new Dictionary<string, string>() { { "Extensions", "MVC-2.0;Another-Thing" }, });
        _extensionItems.Item("MVC-2.0");
        _razorComponentWithTargetPathItems.Item(TestProjectData.AnotherProjectNestedComponentFile3.FilePath, new Dictionary<string, string>()
         {
             { Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.AnotherProjectNestedComponentFile3.TargetPath },
         });
        _razorGenerateWithTargetPathItems.Item(TestProjectData.AnotherProjectNestedFile3.FilePath, new Dictionary<string, string>()
         {
             { Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.AnotherProjectNestedFile3.TargetPath },
         });

        changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(changes[0].After),
             _configurationItems.ToChange(changes[1].After),
             _extensionItems.ToChange(changes[2].After),
             _razorComponentWithTargetPathItems.ToChange(changes[3].After),
             _razorGenerateWithTargetPathItems.ToChange(changes[4].After),
             _configurationGeneral.ToChange(changes[5].After),
        };

        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 2
        snapshot = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, snapshot.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_0, snapshot.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.0", snapshot.Configuration.ConfigurationName);
        Assert.Collection(
            snapshot.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.0", e.ExtensionName));

        Assert.Collection(
            snapshot.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.AnotherProjectNestedFile3.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.AnotherProjectNestedFile3.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.AnotherProjectNestedComponentFile3.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.AnotherProjectNestedComponentFile3.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            },
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.ComponentImport, document.FileKind);
            },
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = snapshot.GetDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectChanged_VersionRemoved_DeinitializesProject()
    {
        // Arrange
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.1");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.1");

        _configurationItems.Item("MVC-2.1");
        _configurationItems.Property("MVC-2.1", Rules.RazorConfiguration.ExtensionsProperty, "MVC-2.1;Another-Thing");

        _extensionItems.Item("MVC-2.1");
        _extensionItems.Item("Another-Thing");

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentFile1.TargetPath);

        _razorGenerateWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath));
        _razorGenerateWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectFile1.TargetPath);

        _configurationGeneral.Property(WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName, TestProjectData.SomeProject.IntermediateOutputPath);
        _configurationGeneral.Property(WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName, "obj");

        var changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(),
             _configurationItems.ToChange(),
             _extensionItems.ToChange(),
             _razorComponentWithTargetPathItems.ToChange(),
             _razorGenerateWithTargetPathItems.ToChange(),
             _configurationGeneral.ToChange(),
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);
        host.SkipIntermediateOutputPathExistCheck_TestOnly = true;

        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        var snapshot = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, snapshot.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_1, snapshot.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.1", snapshot.Configuration.ConfigurationName);
        Assert.Collection(
            snapshot.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.1", e.ExtensionName));

        // Act - 2
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "");

        changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(changes[0].After),
             _configurationItems.ToChange(changes[1].After),
             _extensionItems.ToChange(changes[2].After),
             _razorComponentWithTargetPathItems.ToChange(changes[3].After),
             _razorGenerateWithTargetPathItems.ToChange(changes[4].After),
        };

        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 2
        Assert.Empty(_projectManager.GetProjects());

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectChanged_AfterDispose_IgnoresUpdate()
    {
        // Arrange
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.1");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.1");

        _configurationItems.Item("MVC-2.1");
        _configurationItems.Property("MVC-2.1", Rules.RazorConfiguration.ExtensionsProperty, "MVC-2.1;Another-Thing");

        _extensionItems.Item("MVC-2.1");
        _extensionItems.Item("Another-Thing");

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentFile1.TargetPath);

        _razorGenerateWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath));
        _razorGenerateWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectFile1.TargetPath);

        _configurationGeneral.Property(WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName, TestProjectData.SomeProject.IntermediateOutputPath);
        _configurationGeneral.Property(WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName, "obj");

        var changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(),
             _configurationItems.ToChange(),
             _extensionItems.ToChange(),
             _razorComponentWithTargetPathItems.ToChange(),
             _razorGenerateWithTargetPathItems.ToChange(),
             _configurationGeneral.ToChange(),
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);
        host.SkipIntermediateOutputPathExistCheck_TestOnly = true;

        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        var snapshot = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, snapshot.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_1, snapshot.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.1", snapshot.Configuration.ConfigurationName);
        Assert.Collection(
            snapshot.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.1", e.ExtensionName));

        // Act - 2
        await Task.Run(async () => await host.DisposeAsync());

        // Assert - 2
        Assert.Empty(_projectManager.GetProjects());

        // Act - 3
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.0");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.0");
        _configurationItems.Item("MVC-2.0", new Dictionary<string, string>() { { "Extensions", "MVC-2.0;Another-Thing" }, });

        changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(changes[0].After),
             _configurationItems.ToChange(changes[1].After),
             _extensionItems.ToChange(changes[2].After),
             _razorComponentWithTargetPathItems.ToChange(changes[3].After),
             _razorGenerateWithTargetPathItems.ToChange(changes[4].After),
        };

        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 3
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectRenamed_RemovesHostProject_CopiesConfiguration()
    {
        // Arrange
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorLangVersionProperty, "2.1");
        _razorGeneralProperties.Property(Rules.RazorGeneral.RazorDefaultConfigurationProperty, "MVC-2.1");

        _configurationItems.Item("MVC-2.1");
        _configurationItems.Property("MVC-2.1", Rules.RazorConfiguration.ExtensionsProperty, "MVC-2.1;Another-Thing");

        _extensionItems.Item("MVC-2.1");
        _extensionItems.Item("Another-Thing");

        _razorComponentWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath));
        _razorComponentWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectComponentFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectComponentFile1.TargetPath);

        _razorGenerateWithTargetPathItems.Item(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath));
        _razorGenerateWithTargetPathItems.Property(Path.GetFileName(TestProjectData.SomeProjectFile1.FilePath), Rules.RazorGenerateWithTargetPath.TargetPathProperty, TestProjectData.SomeProjectFile1.TargetPath);

        _configurationGeneral.Property(WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName, TestProjectData.SomeProject.IntermediateOutputPath);
        _configurationGeneral.Property(WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName, "obj");

        var changes = new TestProjectChangeDescription[]
        {
             _razorGeneralProperties.ToChange(),
             _configurationItems.ToChange(),
             _extensionItems.ToChange(),
             _razorComponentWithTargetPathItems.ToChange(),
             _razorGenerateWithTargetPathItems.ToChange(),
             _configurationGeneral.ToChange(),
        };

        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);

        var host = new DefaultWindowsRazorProjectHost(services, Workspace, Dispatcher, _projectConfigurationFilePathStore, _projectManager);
        host.SkipIntermediateOutputPathExistCheck_TestOnly = true;

        await Task.Run(async () => await host.LoadAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await host.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        var snapshot = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, snapshot.FilePath);
        Assert.Same("MVC-2.1", snapshot.Configuration.ConfigurationName);

        Assert.Collection(
           snapshot.DocumentFilePaths.OrderBy(d => d),
           d =>
           {
               var document = snapshot.GetDocument(d);
               Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
               Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
               Assert.Equal(FileKinds.Legacy, document.FileKind);
           },
           d =>
           {
               var document = snapshot.GetDocument(d);
               Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
               Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
               Assert.Equal(FileKinds.Component, document.FileKind);
           });

        // Act - 2
        services.UnconfiguredProject.FullPath = TestProjectData.AnotherProject.FilePath;
        await Task.Run(async () => await host.OnProjectRenamingAsync(TestProjectData.SomeProject.FilePath, TestProjectData.AnotherProject.FilePath));

        // Assert - 1
        snapshot = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.AnotherProject.FilePath, snapshot.FilePath);
        Assert.Same("MVC-2.1", snapshot.Configuration.ConfigurationName);

        Assert.Collection(
           snapshot.DocumentFilePaths.OrderBy(d => d),
           d =>
           {
               var document = snapshot.GetDocument(d);
               Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
               Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
               Assert.Equal(FileKinds.Legacy, document.FileKind);
           },
           d =>
           {
               var document = snapshot.GetDocument(d);
               Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
               Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
               Assert.Equal(FileKinds.Component, document.FileKind);
           });

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    private class TestProjectSnapshotManager : DefaultProjectSnapshotManager
    {
        public TestProjectSnapshotManager(Workspace workspace, ProjectSnapshotManagerDispatcher dispatcher)
            : base(Mock.Of<IErrorReporter>(MockBehavior.Strict), Array.Empty<IProjectSnapshotChangeTrigger>(), workspace, dispatcher)
        {
        }
    }
}
