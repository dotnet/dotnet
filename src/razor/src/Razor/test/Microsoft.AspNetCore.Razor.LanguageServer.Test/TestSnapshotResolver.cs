// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class TestSnapshotResolver : ISnapshotResolver
{
    private readonly IProjectSnapshot _miscProject = TestProjectSnapshot.Create(@"C:\temp\miscellaneous\project.csproj");
    private readonly string? _filePath;
    private readonly IProjectSnapshot[]? _projects;

    public TestSnapshotResolver()
    {
    }

    public TestSnapshotResolver(string filePath, params IProjectSnapshot[] projects)
    {
        _filePath = filePath;
        _projects = projects;
    }

    public IEnumerable<IProjectSnapshot> FindPotentialProjects(string documentFilePath)
    {
        if (documentFilePath == _filePath)
        {
            return _projects.AssumeNotNull();
        }

        return Array.Empty<IProjectSnapshot>();
    }

    public IProjectSnapshot GetMiscellaneousProject()
    {
        return _miscProject;
    }

    public bool TryResolveDocumentInAnyProject(string documentFilePath, [NotNullWhen(true)] out IDocumentSnapshot? documentSnapshot)
    {
        documentSnapshot = null;
        return false;
    }
}
