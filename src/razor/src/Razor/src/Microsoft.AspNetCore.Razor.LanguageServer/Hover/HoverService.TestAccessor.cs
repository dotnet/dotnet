// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

internal sealed partial class HoverService
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(HoverService instance)
    {
        public Task<VSInternalHover?> GetHoverInfoAsync(
            string documentFilePath,
            RazorCodeDocument codeDocument,
            SourceLocation location,
            VSInternalClientCapabilities clientCapabilities,
            CancellationToken cancellationToken)
            => instance.GetHoverInfoAsync(documentFilePath, codeDocument, location, clientCapabilities, cancellationToken);
    }
}
