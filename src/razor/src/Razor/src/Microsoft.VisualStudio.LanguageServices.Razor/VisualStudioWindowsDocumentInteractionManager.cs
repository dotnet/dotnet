// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Shared]
[Export(typeof(DocumentInteractionManager))]
internal class VisualStudioWindowsDocumentInteractionManager : DocumentInteractionManager
{
    public override Task OpenDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        VsShellUtilities.OpenDocument(ServiceProvider.GlobalProvider, filePath);

        return Task.CompletedTask;
    }
}
