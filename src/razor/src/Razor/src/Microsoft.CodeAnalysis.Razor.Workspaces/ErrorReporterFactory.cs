// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;

namespace Microsoft.CodeAnalysis.Razor;

[Shared]
[ExportWorkspaceServiceFactory(typeof(IErrorReporter), ServiceLayer.Default)]
internal class ErrorReporterFactory : IWorkspaceServiceFactory
{
    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices) => ErrorReporter.Instance;
}
