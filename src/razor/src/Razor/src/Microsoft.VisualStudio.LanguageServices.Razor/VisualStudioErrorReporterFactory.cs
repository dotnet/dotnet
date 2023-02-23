// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Shared]
[ExportWorkspaceServiceFactory(typeof(IErrorReporter), ServiceLayer.Host)]
internal class VisualStudioErrorReporterFactory : IWorkspaceServiceFactory
{
    private readonly SVsServiceProvider _serviceProvider;

    [ImportingConstructor]
    public VisualStudioErrorReporterFactory(SVsServiceProvider serviceProvider)
    {
        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        _serviceProvider = serviceProvider;
    }

    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        return new VisualStudioErrorReporter(_serviceProvider);
    }
}
