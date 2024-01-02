// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Editor.Razor.Logging;

internal abstract class RazorLogHubTraceProvider
{
    public abstract Task InitializeTraceAsync(string logIdentifier, int logHubSessionId, CancellationToken cancellationToken);
    public abstract TraceSource? TryGetTraceSource();
}
