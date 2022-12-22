// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Microsoft.CodeAnalysis;
using OmniSharp;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public abstract class OmniSharpWorkspaceTestBase : OmniSharpTestBase
{
    protected OmniSharpWorkspace Workspace { get; }
    protected Project Project { get; }

    protected OmniSharpWorkspaceTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        Workspace = TestOmniSharpWorkspace.Create(LoggerFactory);
        AddDisposable(Workspace);

        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, "TestProject", "TestAssembly", LanguageNames.CSharp, filePath: "/path/to/project.csproj");
        Workspace.AddProject(projectInfo);
        Project = Workspace.CurrentSolution.Projects.FirstOrDefault();
    }

    protected Document AddRoslynDocument(string filePath)
    {
        var backgroundDocumentId = DocumentId.CreateNewId(Project.Id);
        var backgroundDocumentInfo = DocumentInfo.Create(backgroundDocumentId, filePath ?? "EmptyFile", filePath: filePath);
        Workspace.AddDocument(backgroundDocumentInfo);
        var addedDocument = Workspace.CurrentSolution.GetDocument(backgroundDocumentId);
        return addedDocument;
    }
}
