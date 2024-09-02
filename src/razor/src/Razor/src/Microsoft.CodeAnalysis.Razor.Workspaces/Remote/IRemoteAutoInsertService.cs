// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;

using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert.RemoteAutoInsertTextEdit?>;

internal interface IRemoteAutoInsertService
{
    ValueTask<Response> GetAutoInsertTextEditAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePosition position,
        string character,
        bool autoCloseTags,
        bool formatOnType,
        bool indentWithTabs,
        int indentSize,
        CancellationToken cancellationToken);
}
