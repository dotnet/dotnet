// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

internal sealed class OmniSharpDocumentSnapshot
{
    private readonly DocumentSnapshot _documentSnapshot;
    private readonly object _projectLock;
    private OmniSharpHostDocument _hostDocument;
    private OmniSharpProjectSnapshot _project;

    internal OmniSharpDocumentSnapshot(DocumentSnapshot documentSnapshot)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        _documentSnapshot = documentSnapshot;
        _projectLock = new object();
    }

    public OmniSharpHostDocument HostDocument
    {
        get
        {
            if (_hostDocument is null)
            {
                var defaultDocumentSnapshot = (DefaultDocumentSnapshot)_documentSnapshot;
                var hostDocument = defaultDocumentSnapshot.State.HostDocument;
                _hostDocument = new OmniSharpHostDocument(hostDocument.FilePath, hostDocument.TargetPath, hostDocument.FileKind);
            }

            return _hostDocument;
        }
    }

    public string FileKind => _documentSnapshot.FileKind;

    public string FilePath => _documentSnapshot.FilePath;

    public string TargetPath => _documentSnapshot.TargetPath;

    public OmniSharpProjectSnapshot Project
    {
        get
        {
            lock (_projectLock)
            {
                _project ??= new OmniSharpProjectSnapshot(_documentSnapshot.Project);
            }

            return _project;
        }
    }
}
