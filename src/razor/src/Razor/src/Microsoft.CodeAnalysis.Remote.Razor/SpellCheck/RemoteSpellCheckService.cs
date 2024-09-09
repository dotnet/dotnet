// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SpellCheck;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed partial class RemoteSpellCheckService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteSpellCheckService
{
    internal sealed class Factory : FactoryBase<IRemoteSpellCheckService>
    {
        protected override IRemoteSpellCheckService CreateService(in ServiceArgs args)
            => new RemoteSpellCheckService(in args);
    }

    private readonly ISpellCheckService _spellCheckService = args.ExportProvider.GetExportedValue<ISpellCheckService>();

    public ValueTask<int[]> GetSpellCheckRangeTriplesAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetSpellCheckRangeTriplesAsync(context, cancellationToken),
            cancellationToken);

    private async ValueTask<int[]> GetSpellCheckRangeTriplesAsync(RemoteDocumentContext context, CancellationToken cancellationToken)
    {
        return await _spellCheckService.GetSpellCheckRangeTriplesAsync(context, cancellationToken).ConfigureAwait(false);
    }
}
