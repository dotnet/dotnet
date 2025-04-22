// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.Debugging;

internal interface IRazorBreakpointResolver
{
    Task<LspRange?> TryResolveBreakpointRangeAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken);
}
