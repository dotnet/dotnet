// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteUriPresentationServiceFactory : RazorServiceFactoryBase<IRemoteUriPresentationService>
{
    // WARNING: We must always have a parameterless constructor in order to be properly handled by ServiceHub.
    public RemoteUriPresentationServiceFactory()
        : base(RazorServices.Descriptors)
    {
    }

    protected override IRemoteUriPresentationService CreateService(IRazorServiceBroker serviceBroker, ExportProvider exportProvider)
    {
        var documentMappingService = exportProvider.GetExportedValue<IRazorDocumentMappingService>();
        var documentSnapshotFactory = exportProvider.GetExportedValue<DocumentSnapshotFactory>();
        var loggerFactory = exportProvider.GetExportedValue<ILoggerFactory>();
        return new RemoteUriPresentationService(serviceBroker, documentMappingService, documentSnapshotFactory, loggerFactory);
    }
}
