// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Common;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IProjectFileChangeListener
{
    void ProjectFileChanged(string filePath, RazorFileChangeKind kind);
}
