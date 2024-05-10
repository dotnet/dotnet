// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteLinkedEditingRangeServiceFactory : RazorServiceFactoryBase<IRemoteLinkedEditingRangeService>
{
    // WARNING: We must always have a parameterless constructor in order to be properly handled by ServiceHub.
    public RemoteLinkedEditingRangeServiceFactory()
        : base(RazorServices.Descriptors)
    {
    }

    protected override IRemoteLinkedEditingRangeService CreateService(IServiceBroker serviceBroker, ExportProvider exportProvider)
    {
        var documentSnapshotFactory = exportProvider.GetExportedValue<DocumentSnapshotFactory>();
        var loggerFactory = exportProvider.GetExportedValue<ILoggerFactory>();

        return new RemoteLinkedEditingRangeService(serviceBroker, documentSnapshotFactory, loggerFactory);
    }
}
