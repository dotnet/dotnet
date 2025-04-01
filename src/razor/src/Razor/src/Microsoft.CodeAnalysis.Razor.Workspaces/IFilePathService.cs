// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
