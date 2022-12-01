// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

[Shared]
[ExportWorkspaceServiceFactory(typeof(ErrorReporter), ServiceLayer.Host)]
internal class VisualStudioErrorReporterFactory : IWorkspaceServiceFactory
{
    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        return new VisualStudioErrorReporter();
    }
}
