// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using LspDiagnostic = Microsoft.VisualStudio.LanguageServer.Protocol.Diagnostic;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteDiagnosticsService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteDiagnosticsService
{
    internal sealed class Factory : FactoryBase<IRemoteDiagnosticsService>
    {
        protected override IRemoteDiagnosticsService CreateService(in ServiceArgs args)
            => new RemoteDiagnosticsService(in args);
    }

    private readonly RazorTranslateDiagnosticsService _translateDiagnosticsService = args.ExportProvider.GetExportedValue<RazorTranslateDiagnosticsService>();

    public ValueTask<ImmutableArray<LspDiagnostic>> GetDiagnosticsAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        LspDiagnostic[] csharpDiagnostics,
        LspDiagnostic[] htmlDiagnostics,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetDiagnosticsAsync(context, csharpDiagnostics, htmlDiagnostics, cancellationToken),
            cancellationToken);

    private async ValueTask<ImmutableArray<LspDiagnostic>> GetDiagnosticsAsync(
        RemoteDocumentContext context,
        LspDiagnostic[] csharpDiagnostics,
        LspDiagnostic[] htmlDiagnostics,
        CancellationToken cancellationToken)
    {
        // We've got C# and Html, lets get Razor diagnostics
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        // Yes, CSharpDocument.Documents are the Razor diagnostics. Don't ask.
        var razorDiagnostics = codeDocument.GetCSharpDocument().Diagnostics;

        return [
            .. RazorDiagnosticConverter.Convert(razorDiagnostics, codeDocument.Source.Text, context.Snapshot),
            .. await _translateDiagnosticsService.TranslateAsync(RazorLanguageKind.CSharp, csharpDiagnostics, context.Snapshot),
            .. await _translateDiagnosticsService.TranslateAsync(RazorLanguageKind.Html, htmlDiagnostics, context.Snapshot)
        ];
    }
}
