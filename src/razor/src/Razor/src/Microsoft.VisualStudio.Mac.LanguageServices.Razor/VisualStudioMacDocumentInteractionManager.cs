// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Editor.Razor;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

[Shared]
[Export(typeof(DocumentInteractionManager))]
internal class VisualStudioMacDocumentInteractionManager : DocumentInteractionManager
{
    public override async Task OpenDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        var filePathKey = new FilePath(filePath);
        await IdeApp.Workbench.OpenDocument(filePathKey, project: null, bringToFront: true).ConfigureAwait(false);
    }
}
