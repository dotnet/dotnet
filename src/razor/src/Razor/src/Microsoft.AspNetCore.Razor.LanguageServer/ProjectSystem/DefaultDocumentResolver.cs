// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class DefaultDocumentResolver : DocumentResolver
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly ProjectResolver _projectResolver;

    public DefaultDocumentResolver(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ProjectResolver projectResolver)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectResolver is null)
        {
            throw new ArgumentNullException(nameof(projectResolver));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectResolver = projectResolver;
    }

    public override bool TryResolveDocument(string documentFilePath, [NotNullWhen(true)] out DocumentSnapshot? document)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var normalizedPath = FilePathNormalizer.Normalize(documentFilePath);
        if (!_projectResolver.TryResolveProject(normalizedPath, out var project))
        {
            // Neither the potential project determined by file path,
            // nor the Miscellaneous project contain the document.
            document = null;
            return false;
        }

        document = project.GetDocument(normalizedPath);
        return true;
    }
}
