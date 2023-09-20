// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Editor.Razor.Logging;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.Logging;

[Shared]
[Export(typeof(RazorLogHubTraceProvider))]
internal class VisualStudioMacLogHubTraceProvider : RazorLogHubTraceProvider
{
    public override Task<TraceSource?> InitializeTraceAsync(string logIdentifier, int logHubSessionId, CancellationToken cancellationToken)
    {
        // VS4Mac doesn't really support trace source logging today well. For now we'll generate a dummy trace source to ensure dependencies don't fall over.
        return Task.FromResult<TraceSource?>(new TraceSource(logIdentifier));
    }
}
