// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.VisualStudio.LanguageServices;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(VisualStudioHostServicesProvider))]
[method: ImportingConstructor]
internal sealed class VisualStudioHostServicesProvider([Import(typeof(VisualStudioWorkspace))] CodeAnalysis.Workspace workspace)
{
    private readonly CodeAnalysis.Workspace _workspace = workspace;

    public HostServices GetServices() => _workspace.Services.HostServices;
}
