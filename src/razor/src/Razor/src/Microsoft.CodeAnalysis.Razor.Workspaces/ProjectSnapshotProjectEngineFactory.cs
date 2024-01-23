// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal abstract class ProjectSnapshotProjectEngineFactory : IWorkspaceService
{
    public abstract IProjectEngineFactory FindFactory(IProjectSnapshot project);

    public abstract IProjectEngineFactory FindSerializableFactory(IProjectSnapshot project);

    public RazorProjectEngine Create(IProjectSnapshot project)
    {
        return Create(project, RazorProjectFileSystem.Create(Path.GetDirectoryName(project.FilePath)), null);
    }

    public RazorProjectEngine Create(IProjectSnapshot project, RazorProjectFileSystem fileSystem)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        if (fileSystem is null)
        {
            throw new ArgumentNullException(nameof(fileSystem));
        }

        return Create(project, fileSystem, null);
    }

    public RazorProjectEngine Create(IProjectSnapshot project, Action<RazorProjectEngineBuilder> configure)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        return Create(project, RazorProjectFileSystem.Create(Path.GetDirectoryName(project.FilePath)), configure);
    }

    public RazorProjectEngine Create(IProjectSnapshot project, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        if (fileSystem is null)
        {
            throw new ArgumentNullException(nameof(fileSystem));
        }

        return Create(project.Configuration, fileSystem, configure);
    }

    public RazorProjectEngine Create(RazorConfiguration configuration, string directoryPath, Action<RazorProjectEngineBuilder> configure)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (directoryPath is null)
        {
            throw new ArgumentNullException(nameof(directoryPath));
        }

        return Create(configuration, RazorProjectFileSystem.Create(directoryPath), configure);
    }

#nullable enable
    public abstract RazorProjectEngine? Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure);
#nullable disable
}
