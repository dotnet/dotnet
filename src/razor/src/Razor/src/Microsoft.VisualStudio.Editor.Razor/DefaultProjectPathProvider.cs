// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultProjectPathProvider : ProjectPathProvider
{
    private readonly TextBufferProjectService _projectService;
    private readonly LiveShareProjectPathProvider? _liveShareProjectPathProvider;

    public DefaultProjectPathProvider(
        TextBufferProjectService projectService,
        LiveShareProjectPathProvider? liveShareProjectPathProvider)
    {
        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        _projectService = projectService;
        _liveShareProjectPathProvider = liveShareProjectPathProvider;
    }

    public override bool TryGetProjectPath(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out string? filePath)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (_liveShareProjectPathProvider is not null &&
            _liveShareProjectPathProvider.TryGetProjectPath(textBuffer, out filePath))
        {
            return true;
        }

        var project = _projectService.GetHostProject(textBuffer);
        if (project is null)
        {
            filePath = null;
            return false;
        }

        filePath = _projectService.GetProjectPath(project);
        return true;
    }
}
