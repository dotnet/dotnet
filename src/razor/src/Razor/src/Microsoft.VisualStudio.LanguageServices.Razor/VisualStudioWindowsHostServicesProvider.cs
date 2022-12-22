// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Export(typeof(VisualStudioHostServicesProvider))]
internal class VisualStudioWindowsHostServicesProvider : VisualStudioHostServicesProvider
{
    private readonly CodeAnalysis.Workspace _workspace;

    [ImportingConstructor]
    public VisualStudioWindowsHostServicesProvider([Import(typeof(VisualStudioWorkspace))] CodeAnalysis.Workspace workspace)
    {
        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        _workspace = workspace;
    }

    public override HostServices GetServices() => _workspace.Services.HostServices;
}
