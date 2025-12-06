// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Shell;

namespace NuGet.PackageManagement.VisualStudio.IDE
{
    internal sealed class VSDocumentOpener : IDocumentOpener
    {
        public void OpenDocument(string path)
        {
            VsShellUtilities.OpenDocument(ServiceProvider.GlobalProvider, path);
        }
    }
}
