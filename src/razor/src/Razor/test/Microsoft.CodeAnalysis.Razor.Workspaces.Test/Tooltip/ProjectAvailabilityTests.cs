// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

public class ProjectAvailabilityTests(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public async Task GetProjectAvailabilityText_NoProjects_ReturnsNull()
    {
        var projectManager = CreateProjectSnapshotManager();
        var componentAvailabilityService = new TestComponentAvailabilityService(projectManager);

        var availability = await componentAvailabilityService.GetProjectAvailabilityTextAsync("file.razor", "MyTagHelper", DisposalToken);

        Assert.Null(availability);
    }

    [Fact]
    public async Task GetProjectAvailabilityText_OneProject_ReturnsNull()
    {
        var builder = TagHelperDescriptorBuilder.CreateComponent("TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        var tagHelperTypeName = "TestNamespace.TestTagHelper";
        builder.TypeName = tagHelperTypeName;
        var projectWorkspaceState = ProjectWorkspaceState.Create([builder.Build()]);

        var hostProject = new HostProject(
            "C:/path/to/project.csproj",
            "C:/path/to/obj/1",
            RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project");

        var hostDocument = new HostDocument(
            "C:/path/to/file.razor",
            "file.razor",
            RazorFileKind.Component);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.UpdateProjectWorkspaceState(hostProject.Key, projectWorkspaceState);
            updater.AddDocument(hostProject.Key, hostDocument, EmptyTextLoader.Instance);
        });

        var componentAvailabilityService = new TestComponentAvailabilityService(projectManager);

        var availability = await componentAvailabilityService.GetProjectAvailabilityTextAsync(hostDocument.FilePath, tagHelperTypeName, DisposalToken);

        Assert.Null(availability);
    }

    [Fact]
    public async Task GetProjectAvailabilityText_AvailableInAllProjects_ReturnsNull()
    {
        var builder = TagHelperDescriptorBuilder.CreateComponent("TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        var tagHelperTypeName = "TestNamespace.TestTagHelper";
        builder.TypeName = tagHelperTypeName;
        var projectWorkspaceState = ProjectWorkspaceState.Create([builder.Build()]);

        var hostProject1 = new HostProject(
            "C:/path/to/project.csproj",
            "C:/path/to/obj/1",
            RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project1");

        var hostProject2 = new HostProject(
            "C:/path/to/project.csproj",
            "C:/path/to/obj/2",
            RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project2");

        var hostDocument = new HostDocument(
            "C:/path/to/file.razor",
            "file.razor",
            RazorFileKind.Component);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject1);
            updater.UpdateProjectWorkspaceState(hostProject1.Key, projectWorkspaceState);
            updater.AddDocument(hostProject1.Key, hostDocument, EmptyTextLoader.Instance);

            updater.AddProject(hostProject2);
            updater.UpdateProjectWorkspaceState(hostProject2.Key, projectWorkspaceState);
            updater.AddDocument(hostProject2.Key, hostDocument, EmptyTextLoader.Instance);
        });

        var componentAvailabilityService = new TestComponentAvailabilityService(projectManager);

        var availability = await componentAvailabilityService.GetProjectAvailabilityTextAsync(hostDocument.FilePath, tagHelperTypeName, DisposalToken);

        Assert.Null(availability);
    }

    [Fact]
    public async Task GetProjectAvailabilityText_NotAvailableInAllProjects_ReturnsText()
    {
        var builder = TagHelperDescriptorBuilder.CreateComponent("TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        var tagHelperTypeName = "TestNamespace.TestTagHelper";
        builder.TypeName = tagHelperTypeName;
        var projectWorkspaceState = ProjectWorkspaceState.Create([builder.Build()]);

        var hostProject1 = new HostProject(
            "C:/path/to/project.csproj",
            "C:/path/to/obj/1",
            RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project1");

        var hostProject2 = new HostProject(
            "C:/path/to/project.csproj",
            "C:/path/to/obj/2",
            RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project2");

        var hostDocument = new HostDocument(
            "C:/path/to/file.razor",
            "file.razor",
            RazorFileKind.Component);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject1);
            updater.UpdateProjectWorkspaceState(hostProject1.Key, projectWorkspaceState);
            updater.AddDocument(hostProject1.Key, hostDocument, EmptyTextLoader.Instance);

            updater.AddProject(hostProject2);
            updater.AddDocument(hostProject2.Key, hostDocument, EmptyTextLoader.Instance);
        });

        var componentAvailabilityService = new TestComponentAvailabilityService(projectManager);

        var availability = await componentAvailabilityService.GetProjectAvailabilityTextAsync(hostDocument.FilePath, tagHelperTypeName, DisposalToken);

        AssertEx.EqualOrDiff("""

            ⚠️ Not available in:
                project2
            """, availability);
    }

    [Fact]
    public async Task GetProjectAvailabilityText_NotAvailableInAnyProject_ReturnsText()
    {
        var hostProject1 = new HostProject(
            "C:/path/to/project.csproj",
            "C:/path/to/obj/1",
            RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project1");

        var hostProject2 = new HostProject(
            "C:/path/to/project.csproj",
            "C:/path/to/obj/2",
            RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project2");

        var hostDocument = new HostDocument(
            "C:/path/to/file.razor",
            "file.razor",
            RazorFileKind.Component);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject1);
            updater.AddDocument(hostProject1.Key, hostDocument, EmptyTextLoader.Instance);

            updater.AddProject(hostProject2);
            updater.AddDocument(hostProject2.Key, hostDocument, EmptyTextLoader.Instance);
        });

        var componentAvailabilityService = new TestComponentAvailabilityService(projectManager);

        var availability = await componentAvailabilityService.GetProjectAvailabilityTextAsync(hostDocument.FilePath, "MyTagHelper", DisposalToken);

        AssertEx.EqualOrDiff("""

            ⚠️ Not available in:
                project1
                project2
            """, availability);
    }
}
