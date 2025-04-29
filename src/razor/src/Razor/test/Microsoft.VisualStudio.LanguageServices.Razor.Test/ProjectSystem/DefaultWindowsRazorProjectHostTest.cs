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
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Xunit;
using Xunit.Abstractions;
using Rules = Microsoft.CodeAnalysis.Razor.ProjectSystem.Rules;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

public class DefaultWindowsRazorProjectHostTest : VisualStudioWorkspaceTestBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ItemCollection _configurationItems;
    private readonly ItemCollection _extensionItems;
    private readonly ItemCollection _razorComponentWithTargetPathItems;
    private readonly ItemCollection _razorGenerateWithTargetPathItems;
    private readonly PropertyCollection _razorGeneralProperties;
    private readonly PropertyCollection _configurationGeneral;
    private readonly TestProjectSnapshotManager _projectManager;

    public DefaultWindowsRazorProjectHostTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _serviceProvider = VsMocks.CreateServiceProvider(static b =>
            b.AddComponentModel(static b =>
            {
                var startupInitializer = new RazorStartupInitializer([]);
                b.AddExport(startupInitializer);
            }));

        _projectManager = CreateProjectSnapshotManager();

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
        Assert.True(expectedConfigurationValue.SequenceEqual(configurationItem.Value));
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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        // Act & Assert
        await host.GetTestAccessor().InitializeAsync();
        Assert.Empty(_projectManager.GetProjects());

        await host.DisposeAsync();
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task DefaultRazorProjectHost_BackgroundThread_CreateAndDispose_Succeeds()
    {
        // Arrange
        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        // Act & Assert
        await Task.Run(async () => await host.GetTestAccessor().InitializeAsync());
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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        // Act & Assert
        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));
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

        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act
        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert
        var project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, project.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_1, project.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.1", project.Configuration.ConfigurationName);
        Assert.Collection(
            project.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.1", e.ExtensionName));

        Assert.Collection(
            project.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UITheory]
    // Standard setup.  BaseIntermediateOutputPath ends in obj and IntermediateOutputPath starts with obj
    [InlineData(@"C:\my repo root\solution folder\projectFolder\obj\", @"obj\Debug\net8.0", @"C:\my repo root\solution folder\projectFolder\obj\Debug\net8.0")]
    // ArtifactsPath in use as ../artifacts
    [InlineData(@"C:\my repo root\solution folder\projectFolder\../artifacts\obj\projectName\", @"C:\my repo root\solution folder\projectFolder\../artifacts\obj\projectName\debug", @"C:\my repo root\solution folder\artifacts\obj\projectName\debug")]
    // .... and ArtifactsPivot is $(ArtifactsPivot)\_MyCustomPivot
    [InlineData(@"C:\my repo root\solution folder\projectFolder\../artifacts\obj\projectName\", @"C:\my repo root\solution folder\projectFolder\../artifacts\obj\projectName\_MyCustomPivot", @"C:\my repo root\solution folder\artifacts\obj\projectName\_MyCustomPivot")]
    // Set BIOP to ..\..\artifacts\obj\$(MSBuildProjectFolder), pre-ArtifactsPath existing
    [InlineData(@"C:\my repo root\solution folder\projectFolder\..\..\artifacts\obj\projectName", @"..\..\artifacts\obj\projectName\Debug\net8.0", @"C:\my repo root\artifacts\obj\projectName\Debug\net8.0")]
    public void IntermediateOutputPathCalculationHandlesRelativePaths(string baseIntermediateOutputPath, string intermediateOutputPath, string expectedCombinedIOP)
    {
        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var state = TestProjectRuleSnapshot.CreateProperties(
            WindowsRazorProjectHostBase.ConfigurationGeneralSchemaName,
            new Dictionary<string, string>()
            {
                [WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName] = intermediateOutputPath,
                [WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName] = baseIntermediateOutputPath,
            });

        var dict = ImmutableDictionary<string, IProjectRuleSnapshot>.Empty;
        dict = dict.Add(WindowsRazorProjectHostBase.ConfigurationGeneralSchemaName, state);

        var result = host.GetTestAccessor().GetIntermediateOutputPathFromProjectChange(dict,
            out var combinedIntermediateOutputPath);

        Assert.True(result);
        Assert.Equal(expectedCombinedIOP, combinedIntermediateOutputPath);
    }

    [UITheory]
    // This is what we see for Razor class libraries
    [InlineData("obj/", @"C:\my repo root\solution folder\projectFolder\", @"obj\Debug\net8.0", @"C:\my repo root\solution folder\projectFolder\obj\Debug\net8.0")]
    [InlineData("../obj/", @"C:\my repo root\solution folder\projectFolder\", @"obj\Debug\net8.0", @"C:\my repo root\solution folder\projectFolder\obj\Debug\net8.0")]
    [InlineData("../obj", @"C:\my repo root\solution folder\projectFolder\", @"obj\Debug\net8.0", @"C:\my repo root\solution folder\projectFolder\obj\Debug\net8.0")]
    public void IntermediateOutputPathCalculationHandlesRelativePaths_BaseIntermediateOutputPath(string baseIntermediateOutputPath, string msbuildProjectDirectoryPropertyName, string intermediateOutputPath, string expectedCombinedIOP)
    {
        var services = new TestProjectSystemServices(TestProjectData.SomeProject.FilePath);
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var state = TestProjectRuleSnapshot.CreateProperties(
            WindowsRazorProjectHostBase.ConfigurationGeneralSchemaName,
            new Dictionary<string, string>()
            {
                [WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName] = intermediateOutputPath,
                [WindowsRazorProjectHostBase.BaseIntermediateOutputPathPropertyName] = baseIntermediateOutputPath,
                [WindowsRazorProjectHostBase.MSBuildProjectDirectoryPropertyName] = msbuildProjectDirectoryPropertyName,
            });

        var dict = ImmutableDictionary<string, IProjectRuleSnapshot>.Empty;
        dict = dict.Add(WindowsRazorProjectHostBase.ConfigurationGeneralSchemaName, state);

        var result = host.GetTestAccessor().GetIntermediateOutputPathFromProjectChange(dict,
            out var combinedIntermediateOutputPath);

        Assert.True(result);
        Assert.Equal(expectedCombinedIOP, combinedIntermediateOutputPath);
    }

    [UIFact]
    public async Task OnProjectChanged_NoVersionFound_DoesNotInitializeProject()
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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act
        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await _projectManager.UpdateAsync(updater =>
        {
            updater.SolutionClosed();
        });

        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        var project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, project.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_1, project.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.1", project.Configuration.ConfigurationName);
        Assert.Collection(
            project.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.1", e.ExtensionName));

        Assert.Collection(
            project.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.ComponentImport, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
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

        await Task.Run(async () => await host.GetTestAccessor().OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 2
        project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, project.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_0, project.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.0", project.Configuration.ConfigurationName);
        Assert.Collection(
            project.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.0", e.ExtensionName));

        Assert.Collection(
            project.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.AnotherProjectNestedFile3.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.AnotherProjectNestedFile3.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.AnotherProjectNestedComponentFile3.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.AnotherProjectNestedComponentFile3.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.ComponentImport, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectChanged_VersionRemoved_DeInitializesProject()
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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

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

        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

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

        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

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

        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        var project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, project.FilePath);
        Assert.Same("MVC-2.1", project.Configuration.ConfigurationName);

        Assert.Collection(
            project.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        // Act - 2
        services.UnconfiguredProject.FullPath = TestProjectData.AnotherProject.FilePath;
        await Task.Run(async () => await testAccessor.OnProjectRenamingAsync(TestProjectData.SomeProject.FilePath, TestProjectData.AnotherProject.FilePath));

        // Assert - 1
        project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.AnotherProject.FilePath, project.FilePath);
        Assert.Same("MVC-2.1", project.Configuration.ConfigurationName);

        Assert.Collection(
            project.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Legacy, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }

    [UIFact]
    public async Task OnProjectChanged_ChangeIntermediateOutputPath_RemovesAndAddsProject()
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
        var host = new DefaultWindowsRazorProjectHost(services, _serviceProvider, _projectManager, TestLanguageServerFeatureOptions.Instance);

        var testAccessor = host.GetTestAccessor();

        await Task.Run(async () => await testAccessor.InitializeAsync());
        Assert.Empty(_projectManager.GetProjects());

        // Act - 1
        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 1
        var project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, project.FilePath);

        Assert.Equal(RazorLanguageVersion.Version_2_1, project.Configuration.LanguageVersion);
        Assert.Equal("MVC-2.1", project.Configuration.ConfigurationName);
        Assert.Collection(
            project.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Another-Thing", e.ExtensionName),
            e => Assert.Equal("MVC-2.1", e.ExtensionName));

        Assert.Collection(
            project.DocumentFilePaths.OrderBy(d => d),
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentImportFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.ComponentImport, document.FileKind);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectFile1.TargetPath, document.TargetPath);
            },
            d =>
            {
                var document = project.GetRequiredDocument(d);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.FilePath, document.FilePath);
                Assert.Equal(TestProjectData.SomeProjectComponentFile1.TargetPath, document.TargetPath);
                Assert.Equal(FileKinds.Component, document.FileKind);
            });

        // Act - 2
        _configurationGeneral.Property(WindowsRazorProjectHostBase.IntermediateOutputPathPropertyName, "obj2");

        changes = new TestProjectChangeDescription[]
        {
            _razorGeneralProperties.ToChange(changes[0].After),
            _configurationItems.ToChange(changes[1].After),
            _extensionItems.ToChange(changes[2].After),
            _razorComponentWithTargetPathItems.ToChange(changes[3].After),
            _razorGenerateWithTargetPathItems.ToChange(changes[4].After),
            _configurationGeneral.ToChange(changes[5].After),
        };

        await Task.Run(async () => await testAccessor.OnProjectChangedAsync(string.Empty, services.CreateUpdate(changes)));

        // Assert - 2
        // Changing intermediate output path is effectively removing the old project and adding a new one.
        project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal(TestProjectData.SomeProject.FilePath, project.FilePath);
        Assert.Equal(TestProjectData.SomeProject.IntermediateOutputPath + "2", project.IntermediateOutputPath);

        await Task.Run(async () => await host.DisposeAsync());
        Assert.Empty(_projectManager.GetProjects());
    }
}
