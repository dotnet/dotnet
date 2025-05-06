// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal readonly record struct ServiceArgs(
    IServiceBroker? ServiceBroker,
    ExportProvider ExportProvider,
    ILoggerFactory ServiceLoggerFactory,
    IWorkspaceProvider WorkspaceProvider,
    ServiceRpcDescriptor.RpcConnection? ServerConnection = null,
    IRazorBrokeredServiceInterceptor? Interceptor = null);
