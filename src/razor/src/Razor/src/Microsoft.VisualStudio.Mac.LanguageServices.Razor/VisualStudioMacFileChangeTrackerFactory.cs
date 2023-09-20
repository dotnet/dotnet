// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor.Documents;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

internal class VisualStudioMacFileChangeTrackerFactory : FileChangeTrackerFactory
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;

    public VisualStudioMacFileChangeTrackerFactory(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    }

    public override FileChangeTracker Create(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(SR.ArgumentCannotBeNullOrEmpty, nameof(filePath));
        }

        return new VisualStudioMacFileChangeTracker(filePath, _projectSnapshotManagerDispatcher);
    }
}
