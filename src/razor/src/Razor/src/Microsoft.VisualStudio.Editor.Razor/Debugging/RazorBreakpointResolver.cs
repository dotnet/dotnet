// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor.Debugging;

internal abstract class RazorBreakpointResolver
{
    public abstract Task<Range?> TryResolveBreakpointRangeAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken);
}
