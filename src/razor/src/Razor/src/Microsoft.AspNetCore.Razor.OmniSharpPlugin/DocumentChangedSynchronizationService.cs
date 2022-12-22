// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Composition;
using System.Threading;
using Microsoft.AspNetCore.Razor.OmniSharpPlugin;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

[Shared]
[Export(typeof(IRazorDocumentChangeListener))]
[Export(typeof(IOmniSharpProjectSnapshotManagerChangeTrigger))]
internal class DocumentChangedSynchronizationService : IRazorDocumentChangeListener, IOmniSharpProjectSnapshotManagerChangeTrigger
{
    private readonly OmniSharpProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private OmniSharpProjectSnapshotManagerBase _projectManager;

    [ImportingConstructor]
    public DocumentChangedSynchronizationService(OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    }

    public void Initialize(OmniSharpProjectSnapshotManagerBase projectManager)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectManager = projectManager;
    }

    public void RazorDocumentChanged(RazorFileChangeEventArgs args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        if (args.Kind != RazorFileChangeKind.Changed)
        {
            return;
        }

        var projectFilePath = args.UnevaluatedProjectInstance.ProjectFileLocation.File;
        var documentFilePath = args.FilePath;

        _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            () => _projectManager.DocumentChanged(projectFilePath, documentFilePath),
            CancellationToken.None).ConfigureAwait(false);
    }
}
