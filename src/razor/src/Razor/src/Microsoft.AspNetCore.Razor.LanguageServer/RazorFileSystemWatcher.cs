// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using FileSystemWatcher = System.IO.FileSystemWatcher;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[DesignerCategory("code")]
internal class RazorFileSystemWatcher : FileSystemWatcher
{
    // Without trimming trailing `/`, `\\` from the workspace directory, the FileSystemWatcher
    // returns with paths of the form   "workspaceDirectory/\\Pages\\Counter.razor"
    // which are normalized to          "workspaceDirectory//Pages/Counter.razor" (Invalid `//`)
    //
    // This format doesn't match the directoryFilePaths we store as part of the Project Snapshot ->
    //                                  "workspaceDirectory/Pages/Counter.razor"
    // https://github.com/dotnet/aspnetcore-tooling/blob/488cf6e/src/Razor/src/Microsoft.AspNetCore.Razor.LanguageServer/ProjectSystem/DefaultRazorProjectService.cs#L328
    // Consequently, files are being discarded into the MISC project and subsequently re-generated
    public RazorFileSystemWatcher(string path)
        : base(path.TrimEnd('/', '\\'))
    {
    }

    public RazorFileSystemWatcher(string path, string filter)
        : base(path.TrimEnd('/', '\\'), filter)
    {
    }
}
