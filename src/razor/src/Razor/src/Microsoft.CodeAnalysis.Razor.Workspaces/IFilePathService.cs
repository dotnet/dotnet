// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IFilePathService
{
    string GetRazorCSharpFilePath(ProjectKey projectKey, string razorFilePath);

    Uri GetRazorDocumentUri(Uri virtualDocumentUri);

    bool IsVirtualCSharpFile(Uri uri);

    bool IsVirtualDocumentUri(Uri uri);

    bool IsVirtualHtmlFile(Uri uri);
}
