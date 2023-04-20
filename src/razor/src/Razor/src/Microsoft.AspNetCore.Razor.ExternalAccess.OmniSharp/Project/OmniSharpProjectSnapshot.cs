// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Document;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.ProjectSystem;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;

public sealed class OmniSharpProjectSnapshot
{
    internal readonly IProjectSnapshot InternalProjectSnapshot;

    internal OmniSharpProjectSnapshot(IProjectSnapshot projectSnapshot)
    {
        InternalProjectSnapshot = projectSnapshot;
    }

    public string FilePath => InternalProjectSnapshot.FilePath;

    public IEnumerable<string> DocumentFilePaths => InternalProjectSnapshot.DocumentFilePaths;

    public ProjectWorkspaceState? ProjectWorkspaceState => InternalProjectSnapshot.ProjectWorkspaceState;

    public OmniSharpDocumentSnapshot? GetDocument(string filePath)
    {
        var documentSnapshot = InternalProjectSnapshot.GetDocument(filePath);
        if (documentSnapshot is null)
        {
            return null;
        }

        var internalDocumentSnapshot = new OmniSharpDocumentSnapshot(documentSnapshot);
        return internalDocumentSnapshot;
    }

    public void Serialize(string publishFilePath, JsonSerializer serializer, StreamWriter writer)
    {
        var projectRazorJson = InternalProjectSnapshot.ToProjectRazorJson(publishFilePath);
        serializer.Serialize(writer, projectRazorJson);
    }

    internal static OmniSharpProjectSnapshot? Convert(IProjectSnapshot? projectSnapshot)
    {
        if (projectSnapshot is null)
        {
            return null;
        }

        return new OmniSharpProjectSnapshot(projectSnapshot);
    }
}
