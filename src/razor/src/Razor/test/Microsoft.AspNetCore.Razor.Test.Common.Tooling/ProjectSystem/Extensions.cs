// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal static class Extensions
{
    public static ProjectInfo ToProjectInfo(this HostProject hostProject)
    {
        var assemblyPath = Path.Combine(
            Path.GetDirectoryName(hostProject.FilePath).AssumeNotNull(),
            "obj",
            Path.ChangeExtension(Path.GetFileName(hostProject.FilePath), ".dll"));

        var projectId = ProjectId.CreateNewId(debugName: hostProject.DisplayName);

        return ProjectInfo
            .Create(
                projectId,
                VersionStamp.Create(),
                name: hostProject.DisplayName,
                assemblyName: hostProject.DisplayName,
                language: LanguageNames.CSharp,
                filePath: hostProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(assemblyPath));
    }

    public static DocumentInfo CreateDocumentInfo(this ProjectInfo projectInfo, string fileName)
    {
        var filePath = Path.Combine(
            Path.GetDirectoryName(projectInfo.FilePath).AssumeNotNull(),
            fileName);

        return DocumentInfo.Create(
            DocumentId.CreateNewId(projectInfo.Id, debugName: fileName),
            Path.GetFileNameWithoutExtension(fileName),
            filePath: filePath);
    }

    public static ProjectInfo WithProjectReferences(this ProjectInfo projectInfo, params IEnumerable<ProjectId> projectIds)
    {
        return projectInfo.WithProjectReferences(
            projectIds.Select(id =>
                id != projectInfo.Id
                    ? new ProjectReference(id)
                    : throw new InvalidOperationException("Can't add reference from project to itself.")));
    }
}
