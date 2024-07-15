// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteHtmlDocumentServiceFactory : RazorServiceFactoryBase<IRemoteHtmlDocumentService>
{
    // WARNING: We must always have a parameterless constructor in order to be properly handled by ServiceHub.
    public RemoteHtmlDocumentServiceFactory()
        : base(RazorServices.Descriptors)
    {
    }

    protected override IRemoteHtmlDocumentService CreateService(IRazorServiceBroker serviceBroker, ExportProvider exportProvider)
    {
        var documentSnapshotFactory = exportProvider.GetExportedValue<DocumentSnapshotFactory>();
        return new RemoteHtmlDocumentService(serviceBroker, documentSnapshotFactory);
    }
}
