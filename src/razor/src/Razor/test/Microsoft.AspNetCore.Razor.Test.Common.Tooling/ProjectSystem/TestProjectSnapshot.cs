// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal sealed class TestProjectSnapshot : IProjectSnapshot
{
    public ProjectSnapshot RealSnapshot { get; }

    private TestProjectSnapshot(ProjectState state)
    {
        RealSnapshot = new ProjectSnapshot(state);
    }

    public static TestProjectSnapshot Create(string filePath, ProjectWorkspaceState? projectWorkspaceState = null)
    {
        var hostProject = TestHostProject.Create(filePath);
        var state = ProjectState.Create(hostProject, ProjectEngineFactories.DefaultProvider);

        if (projectWorkspaceState is not null)
        {
            state = state.WithProjectWorkspaceState(projectWorkspaceState);
        }

        return new TestProjectSnapshot(state);
    }

    public HostProject HostProject => RealSnapshot.HostProject;

    public ProjectKey Key => RealSnapshot.Key;
    public IEnumerable<string> DocumentFilePaths => RealSnapshot.DocumentFilePaths;
    public string FilePath => RealSnapshot.FilePath;
    public string IntermediateOutputPath => RealSnapshot.IntermediateOutputPath;
    public string? RootNamespace => RealSnapshot.RootNamespace;
    public string DisplayName => RealSnapshot.DisplayName;
    public LanguageVersion CSharpLanguageVersion => RealSnapshot.CSharpLanguageVersion;

    public ValueTask<TagHelperCollection> GetTagHelpersAsync(CancellationToken cancellationToken)
        => RealSnapshot.GetTagHelpersAsync(cancellationToken);

    public bool ContainsDocument(string filePath)
        => RealSnapshot.ContainsDocument(filePath);

    public bool TryGetDocument(string filePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        if (RealSnapshot.TryGetDocument(filePath, out var result))
        {
            document = result;
            return true;
        }

        document = null;
        return false;
    }
}
