// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteSemanticTokensServiceFactory : RazorServiceFactoryBase<IRemoteSemanticTokensService>
{
    // WARNING: We must always have a parameterless constructor in order to be properly handled by ServiceHub.
    public RemoteSemanticTokensServiceFactory()
        : base(RazorServices.Descriptors)
    {
    }

    protected override IRemoteSemanticTokensService CreateService(IServiceBroker serviceBroker, ExportProvider exportProvider)
    {
        var infoService = exportProvider.GetExportedValue<IRazorSemanticTokensInfoService>();
        var documentSnapshotFactory = exportProvider.GetExportedValue<DocumentSnapshotFactory>();
        return new RemoteSemanticTokensService(serviceBroker, infoService, documentSnapshotFactory);
    }
}
