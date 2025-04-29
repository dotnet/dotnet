// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteCodeActionsService : IRemoteJsonService
{
    ValueTask<CodeActionRequestInfo> GetCodeActionRequestInfoAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId razorDocumentId,
        VSCodeActionParams request,
        CancellationToken cancellationToken);

    ValueTask<SumType<Command, CodeAction>[]?> GetCodeActionsAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId razorDocumentId,
        VSCodeActionParams request,
        RazorVSInternalCodeAction[] delegatedCodeActions,
        CancellationToken cancellationToken);

    ValueTask<CodeAction> ResolveCodeActionAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId razorDocumentId,
        CodeAction request,
        CodeAction? delegatedCodeAction,
        RazorFormattingOptions options,
        CancellationToken cancellationToken);
}
