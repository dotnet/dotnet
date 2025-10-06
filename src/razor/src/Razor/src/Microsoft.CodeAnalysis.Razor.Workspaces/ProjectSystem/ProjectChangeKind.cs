// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal enum ProjectChangeKind
{
    ProjectAdded,
    ProjectRemoved,
    ProjectChanged,
    DocumentAdded,
    DocumentRemoved,

    // This could be a state change (opened/closed) or a content change.
    DocumentChanged,
}
