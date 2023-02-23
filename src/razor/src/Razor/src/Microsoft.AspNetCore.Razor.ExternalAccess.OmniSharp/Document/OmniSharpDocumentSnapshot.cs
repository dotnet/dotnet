// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Document;

public sealed class OmniSharpDocumentSnapshot
{
    private readonly IDocumentSnapshot _documentSnapshot;
    private readonly object _projectLock;
    private OmniSharpHostDocument? _hostDocument;
    private OmniSharpProjectSnapshot? _project;

    internal OmniSharpDocumentSnapshot(IDocumentSnapshot documentSnapshot)
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
                var defaultDocumentSnapshot = (DocumentSnapshot)_documentSnapshot;
                var hostDocument = defaultDocumentSnapshot.State.HostDocument;
                _hostDocument = new OmniSharpHostDocument(hostDocument.FilePath, hostDocument.TargetPath, hostDocument.FileKind);
            }

            return _hostDocument;
        }
    }

    public string? FileKind => _documentSnapshot.FileKind;

    public string? FilePath => _documentSnapshot.FilePath;

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
