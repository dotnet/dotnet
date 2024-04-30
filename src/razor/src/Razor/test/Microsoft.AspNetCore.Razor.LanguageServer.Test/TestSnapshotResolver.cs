// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class TestSnapshotResolver : ISnapshotResolver
{
    private readonly IProjectSnapshot _miscProject = TestProjectSnapshot.Create(@"C:\temp\miscellaneous\project.csproj");
    private readonly string? _filePath;
    private readonly ImmutableArray<IProjectSnapshot> _projects;

    public TestSnapshotResolver()
    {
        _projects = [];
    }

    public TestSnapshotResolver(string filePath, params IProjectSnapshot[] projects)
    {
        _filePath = filePath;
        _projects = [.. projects];
    }

    public ImmutableArray<IProjectSnapshot> FindPotentialProjects(string documentFilePath)
        => documentFilePath == _filePath
            ? _projects
            : [];

    public IProjectSnapshot GetMiscellaneousProject()
        => _miscProject;

    public bool TryResolveDocumentInAnyProject(string documentFilePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        document = null;
        return false;
    }
}
