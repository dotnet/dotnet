// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Creates the <see cref="IMtpServerClient"/> the MTP proxy managers drive, and shuts it down.
///
/// The launch is behind a replaceable delegate purely so the proxy managers can be unit tested
/// without starting a real test application; production code always uses
/// <see cref="MtpServerClient.Launch(string, MtpServerClientOptions?)"/>.
/// </summary>
internal static class MtpServerClientFactory
{
    /// <summary>
    /// How long to wait for the server to acknowledge <c>exit</c> before abandoning the graceful
    /// shutdown and falling back to disposing the client (which terminates the process).
    /// </summary>
    private static readonly TimeSpan ExitTimeout = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Launches an MTP application in server mode. Replaceable for testing only.
    /// </summary>
    internal static Func<string, MtpServerClientOptions, IMtpServerClient> Launch { get; set; }
        = static (source, options) => MtpServerClient.Launch(source, options);

    /// <summary>
    /// Asks the server to exit, on a best-effort basis.
    ///
    /// Two things matter here and neither is served by passing the run's own cancellation token.
    /// First, shutdown must still happen when the run was cancelled or aborted - that is precisely
    /// when the token is already cancelled, so using it would make <c>exit</c> throw immediately and
    /// skip the handshake entirely. Second, <c>exit</c> is a request/response call rather than the
    /// fire-and-forget notification it replaced, so an unresponsive test application would otherwise
    /// block the run forever; it is bounded here instead.
    ///
    /// Failing to exit cleanly is never fatal: the caller disposes the client afterwards, which
    /// tears the process down regardless.
    /// </summary>
    internal static void TryExit(IMtpServerClient client)
    {
        try
        {
            using var timeout = new CancellationTokenSource(ExitTimeout);
            client.ExitAsync(timeout.Token).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            EqtTrace.Warning("MtpServerClientFactory.TryExit: graceful exit failed, disposing instead. {0}", ex);
        }
    }
}
