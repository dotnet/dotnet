// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.ExternalAccess.RoslynWorkspace;

// Copied from https://github.com/dotnet/project-system/blob/e4db47666e0a49f6c38e701f8630dbc31380fb64/src/Microsoft.VisualStudio.ProjectSystem.Managed/Threading/Tasks/CancellationSeries.cs

internal sealed class CancellationSeries : IDisposable
{
    private CancellationTokenSource? _cts = new();

    private readonly CancellationToken _superToken;

    /// <summary>
    /// Initializes a new instance of <see cref="CancellationSeries"/>.
    /// </summary>
    /// <param name="token">An optional cancellation token that, when cancelled, cancels the last
    /// issued token and causes any subsequent tokens to be issued in a cancelled state.</param>
    public CancellationSeries(CancellationToken token)
    {
        _superToken = token;
    }

    /// <summary>
    /// Creates the next <see cref="CancellationToken"/> in the series, ensuring the last issued
    /// token (if any) is cancelled first.
    /// </summary>
    /// <param name="token">An optional cancellation token that, when cancelled, cancels the
    /// returned token.</param>
    /// <returns>
    /// A cancellation token that will be cancelled when either:
    /// <list type="bullet">
    /// <item><see cref="CreateNext"/> is called again</item>
    /// <item>The token passed to this method (if any) is cancelled</item>
    /// <item>The token passed to the constructor (if any) is cancelled</item>
    /// <item><see cref="Dispose"/> is called</item>
    /// </list>
    /// </returns>
    /// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
    public CancellationToken CreateNext(CancellationToken token)
    {
        var nextSource = CancellationTokenSource.CreateLinkedTokenSource(token, _superToken);

        // Obtain the token before exchange, as otherwise the CTS may be cancelled before
        // we request the Token, which will result in an ObjectDisposedException.
        // This way we would return a cancelled token, which is reasonable.
        var nextToken = nextSource.Token;

        var priorSource = Interlocked.Exchange(ref _cts, nextSource);

        if (priorSource is null)
        {
            nextSource.Dispose();

            throw new ObjectDisposedException(nameof(CancellationSeries));
        }

        try
        {
            priorSource.Cancel();
        }
        finally
        {
            // A registered action on the token may throw, which would surface here.
            // Ensure we always dispose the prior CTS.
            priorSource.Dispose();
        }

        return nextToken;
    }

    public void Dispose()
    {
        var source = Interlocked.Exchange(ref _cts, null);

        if (source is null)
        {
            // Already disposed
            return;
        }

        try
        {
            source.Cancel();
        }
        finally
        {
            source.Dispose();
        }
    }
}
