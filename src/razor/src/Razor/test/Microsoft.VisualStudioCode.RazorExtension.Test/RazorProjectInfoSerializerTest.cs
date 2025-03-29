// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudioCode.RazorExtension.Test;

public class RazorProjectInfoSerializerTest(ITestOutputHelper testOutputHelper) : ToolingTestBase(testOutputHelper)
{
    [Fact]
    public void GeneratedDocument()
    {
        using var workspace = new AdhocWorkspace(MefHostServices.DefaultHost);
        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        project = workspace.AddDocument(project.Id, "Goo.cshtml__virtual.cs", SourceText.From("Hi there"))
                    .WithFilePath("virtualcsharp-razor:///e:/Scratch/RazorInConsole/Goo.cshtml__virtual.cs")
                    .Project;

        var documents = RazorProjectInfoFactory.GetDocuments(project, "temp");

        Assert.Single(documents);
    }

    [Fact]
    public void AdditionalDocument()
    {
        using var workspace = new AdhocWorkspace(MefHostServices.DefaultHost);
        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        workspace.TryApplyChanges(project.AddAdditionalDocument("Goo.cshtml", SourceText.From("Hi there"), filePath: @"\E:\Scratch\RazorInConsole\Goo.cshtml").Project.Solution);

        project = workspace.CurrentSolution.GetProject(project.Id)!;

        var documents = RazorProjectInfoFactory.GetDocuments(project, "temp");

        Assert.Single(documents);
    }

    [Fact]
    public void AdditionalAndGeneratedDocument()
    {
        using var workspace = new AdhocWorkspace(MefHostServices.DefaultHost);
        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        workspace.TryApplyChanges(project.AddAdditionalDocument("Goo.cshtml", SourceText.From("Hi there"), filePath: @"\E:\Scratch\RazorInConsole\Goo.cshtml").Project.Solution);

        project = workspace.CurrentSolution.GetProject(project.Id)!;

        project = workspace.AddDocument(project.Id, "Another.cshtml__virtual.cs", SourceText.From("Hi there"))
            .WithFilePath("virtualcsharp-razor:///e:/Scratch/RazorInConsole/Another.cshtml__virtual.cs")
            .Project;

        var documents = RazorProjectInfoFactory.GetDocuments(project, "temp");

        Assert.Single(documents);
    }

    [Fact]
    public void AdditionalNonRazorAndGeneratedDocument()
    {
        using var workspace = new AdhocWorkspace(MefHostServices.DefaultHost);
        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        workspace.TryApplyChanges(project.AddAdditionalDocument("Goo.txt", SourceText.From("Hi there"), filePath: @"\E:\Scratch\RazorInConsole\Goo.txt").Project.Solution);

        project = workspace.CurrentSolution.GetProject(project.Id)!;

        project = workspace.AddDocument(project.Id, "Another.cshtml__virtual.cs", SourceText.From("Hi there"))
            .WithFilePath("virtualcsharp-razor:///e:/Scratch/RazorInConsole/Another.cshtml__virtual.cs")
            .Project;

        var documents = RazorProjectInfoFactory.GetDocuments(project, "temp");

        Assert.Single(documents);
    }

    [Fact]
    public void NormalDocument()
    {
        using var workspace = new AdhocWorkspace(MefHostServices.DefaultHost);
        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        project = workspace.AddDocument(project.Id, "Goo.cs", SourceText.From("Hi there"))
            .WithFilePath("e:/Scratch/RazorInConsole/Goo.cs")
            .Project;

        var documents = RazorProjectInfoFactory.GetDocuments(project, "temp");

        Assert.Empty(documents);
    }
}
