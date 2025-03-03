// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using RoslynLspDiagnostic = Microsoft.VisualStudio.LanguageServer.Protocol.Diagnostic;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteDiagnosticsService : IRemoteJsonService
{
    ValueTask<ImmutableArray<RoslynLspDiagnostic>> GetDiagnosticsAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        RoslynLspDiagnostic[] csharpDiagnostics,
        RoslynLspDiagnostic[] htmlDiagnostics,
        CancellationToken cancellationToken);

    ValueTask<ImmutableArray<RoslynLspDiagnostic>> GetTaskListDiagnosticsAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        ImmutableArray<string> taskListDescriptors,
        CancellationToken cancellationToken);
}
