// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.FoldingRanges;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteFoldingRangeServiceFactory : RazorServiceFactoryBase<IRemoteFoldingRangeService>
{
    // WARNING: We must always have a parameterless constructor in order to be properly handled by ServiceHub.
    public RemoteFoldingRangeServiceFactory()
        : base(RazorServices.Descriptors)
    {
    }

    protected override IRemoteFoldingRangeService CreateService(IServiceBroker serviceBroker, ExportProvider exportProvider)
    {
        var infoService = exportProvider.GetExportedValue<IFoldingRangeService>();
        var documentSnapshotFactory = exportProvider.GetExportedValue<DocumentSnapshotFactory>();
        var filePathService = exportProvider.GetExportedValue<IFilePathService>();
        return new RemoteFoldingRangeService(serviceBroker, infoService, documentSnapshotFactory, filePathService);
    }
}
