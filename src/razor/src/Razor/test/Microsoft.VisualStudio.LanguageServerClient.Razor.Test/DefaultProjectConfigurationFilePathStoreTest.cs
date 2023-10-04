// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

public class DefaultProjectConfigurationFilePathStoreTest : TestBase
{
    public DefaultProjectConfigurationFilePathStoreTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void Set_ResolvesRelativePaths()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var projectFilePath = @"C:\project.csproj";
        var hostProject = new HostProject(projectFilePath, @"C:\project\obj", RazorConfiguration.Default, null);
        var configurationFilePath = @"C:\project\subpath\..\obj\project.razor.bin";
        var called = false;
        store.Changed += (sender, args) =>
        {
            called = true;
            Assert.Equal(hostProject.Key, args.ProjectKey);
            Assert.Equal(@"C:\project\obj\project.razor.bin", args.ConfigurationFilePath);
        };

        // Act
        store.Set(hostProject.Key, configurationFilePath);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void Set_InvokesChanged()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var projectFilePath = @"C:\project.csproj";
        var hostProject = new HostProject(projectFilePath, @"C:\project\obj", RazorConfiguration.Default, null);
        var configurationFilePath = @"C:\project\obj\project.razor.bin";
        var called = false;
        store.Changed += (sender, args) =>
        {
            called = true;
            Assert.Equal(hostProject.Key, args.ProjectKey);
            Assert.Equal(configurationFilePath, args.ConfigurationFilePath);
        };

        // Act
        store.Set(hostProject.Key, configurationFilePath);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void Set_SameConfigurationFilePath_DoesNotInvokeChanged()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var projectFilePath = @"C:\project.csproj";
        var hostProject = new HostProject(projectFilePath, @"C:\project\obj", RazorConfiguration.Default, null);
        var configurationFilePath = @"C:\project\obj\project.razor.bin";
        store.Set(hostProject.Key, configurationFilePath);
        var called = false;
        store.Changed += (sender, args) => called = true;

        // Act
        store.Set(hostProject.Key, configurationFilePath);

        // Assert
        Assert.False(called);
    }

    [Fact]
    public void Set_AllowsTryGet()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var projectFilePath = @"C:\project.csproj";
        var hostProject = new HostProject(projectFilePath, @"C:\project\obj", RazorConfiguration.Default, null);
        var expectedConfigurationFilePath = @"C:\project\obj\project.razor.bin";
        store.Set(hostProject.Key, expectedConfigurationFilePath);

        // Act
        var result = store.TryGet(hostProject.Key, out var configurationFilePath);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
    }

    [Fact]
    public void Set_OverridesPrevious()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var projectFilePath = @"C:\project.csproj";
        var hostProject = new HostProject(projectFilePath, @"C:\project\obj", RazorConfiguration.Default, null);
        var expectedConfigurationFilePath = @"C:\project\obj\project.razor.bin";

        // Act
        store.Set(hostProject.Key, @"C:\other\obj\project.razor.bin");
        store.Set(hostProject.Key, expectedConfigurationFilePath);

        // Assert
        var result = store.TryGet(hostProject.Key, out var configurationFilePath);
        Assert.True(result);
        Assert.Equal(expectedConfigurationFilePath, configurationFilePath);
    }

    [Fact]
    public void GetMappings_NotMutable()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();

        // Act
        var mappings = store.GetMappings();
        var hostProject = new HostProject(@"C:\project.csproj", @"C:\project\obj", RazorConfiguration.Default, null);
        store.Set(hostProject.Key, @"C:\project\obj\project.razor.bin");

        // Assert
        Assert.Empty(mappings);
    }

    [Fact]
    public void GetMappings_ReturnsAllSetMappings()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var expectedMappings = new Dictionary<ProjectKey, string>()
        {
            [TestProjectKey.Create(@"C:\project1\obj")] = @"C:\project1\obj\project.razor.bin"
        };
        foreach (var mapping in expectedMappings)
        {
            store.Set(mapping.Key, mapping.Value);
        }

        // Act
        var mappings = store.GetMappings();

        // Assert
        Assert.Equal(expectedMappings, mappings);
    }

    [Fact]
    public void Remove_InvokesChanged()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var projectFilePath = @"C:\project.csproj";
        var hostProject = new HostProject(projectFilePath, @"C:\project\obj", RazorConfiguration.Default, null);
        store.Set(hostProject.Key, @"C:\project\obj\project.razor.bin");
        var called = false;
        store.Changed += (sender, args) =>
        {
            called = true;
            Assert.Equal(hostProject.Key, args.ProjectKey);
            Assert.Null(args.ConfigurationFilePath);
        };

        // Act
        store.Remove(hostProject.Key);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void Remove_UntrackedProject_DoesNotInvokeChanged()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var called = false;
        store.Changed += (sender, args) => called = true;

        // Act
        store.Remove(TestProjectKey.Create(@"C:\project\obj"));

        // Assert
        Assert.False(called);
    }

    [Fact]
    public void Remove_RemovesGettability()
    {
        // Arrange
        var store = new DefaultProjectConfigurationFilePathStore();
        var projectFilePath = @"C:\project.csproj";
        var hostProject = new HostProject(projectFilePath, @"C:\project\obj", RazorConfiguration.Default, null);
        store.Set(hostProject.Key, @"C:\project\obj\project.razor.bin");

        // Act
        store.Remove(hostProject.Key);
        var result = store.TryGet(hostProject.Key, out _);

        // Assert
        Assert.False(result);
    }
}
