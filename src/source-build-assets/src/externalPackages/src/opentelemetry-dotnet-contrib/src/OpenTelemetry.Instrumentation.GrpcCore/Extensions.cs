// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// Other useful extensions.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Builds an Action comprised of two calls to Dispose with best effort execution for the second disposable.
    /// </summary>
    /// <param name="first">The first.</param>
    /// <param name="second">The second.</param>
    /// <returns>An Action.</returns>
    internal static Action WithBestEffortDispose(this IDisposable first, IDisposable second)
    {
        return () =>
        {
            try
            {
                first.Dispose();
            }
            finally
            {
                second.Dispose();
            }
        };
    }
}
