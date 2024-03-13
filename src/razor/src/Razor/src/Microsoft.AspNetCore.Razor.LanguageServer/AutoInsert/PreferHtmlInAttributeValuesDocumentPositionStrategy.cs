// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

// The main reason for this service is auto-insert of empty double quotes when a user types
// equals "=" after Blazor component attribute. We think this is Razor (correctly I guess)
// and wouldn't forward auto-insert request to HTML in this case. By essentially overriding
// language info here we allow the request to be sent over to HTML where it will insert empty
// double-quotes as it would for any other attribute value
internal class PreferHtmlInAttributeValuesDocumentPositionInfoStrategy : IDocumentPositionInfoStrategy
{
    public static IDocumentPositionInfoStrategy Instance { get; } = new PreferHtmlInAttributeValuesDocumentPositionInfoStrategy();

    public async Task<DocumentPositionInfo?> TryGetPositionInfoAsync(IRazorDocumentMappingService documentMappingService, DocumentContext documentContext, Position position, ILogger logger, CancellationToken cancellationToken)
    {
        var defaultDocumentPositionInfo = await DefaultDocumentPositionInfoStrategy.Instance.TryGetPositionInfoAsync(documentMappingService, documentContext, position, logger, cancellationToken).ConfigureAwait(false);
        if (defaultDocumentPositionInfo is null)
        {
            return null;
        }

        var absolutePosition = defaultDocumentPositionInfo.HostDocumentIndex;
        if (defaultDocumentPositionInfo.LanguageKind != RazorLanguageKind.Razor ||
            absolutePosition < 1)
        {
            return defaultDocumentPositionInfo;
        }

        // Get the node at previous position to see if we are after markup tag helper attribute,
        // and more specifically after the EqualsToken of it
        var previousPosition = absolutePosition - 1;
        var owner = await documentContext.GetSyntaxNodeAsync(previousPosition, cancellationToken).ConfigureAwait(false);
        if (owner is MarkupTagHelperAttributeSyntax { EqualsToken: { IsMissing: false } equalsToken } &&
            equalsToken.EndPosition == defaultDocumentPositionInfo.HostDocumentIndex)
        {
            return new DocumentPositionInfo(RazorLanguageKind.Html, defaultDocumentPositionInfo.Position, defaultDocumentPositionInfo.HostDocumentIndex);
        }

        return defaultDocumentPositionInfo;
    }
}
