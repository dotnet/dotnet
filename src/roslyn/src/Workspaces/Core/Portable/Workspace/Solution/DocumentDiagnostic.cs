﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.CodeAnalysis;

public class DocumentDiagnostic(WorkspaceDiagnosticKind kind, string message, DocumentId documentId) : WorkspaceDiagnostic(kind, message)
{
    public DocumentId DocumentId { get; } = documentId;
}
