// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Dpop;

/// <summary>
/// Abstraction for DPoP <c>jti</c> (JWT ID) replay detection per RFC 9449 §4.3 step 11.
/// Implementations must be thread-safe.
/// </summary>
public interface IJtiReplayCache
{
    /// <summary>
    /// Attempts to record a <c>jti</c> value in the cache.
    /// </summary>
    /// <param name="jti">The unique identifier from the DPoP proof.</param>
    /// <param name="expiration">
    /// The time after which the <c>jti</c> entry may be evicted.
    /// Typically set to the proof's <c>iat</c> plus the maximum proof lifetime plus clock skew.
    /// </param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>
    /// <see langword="true"/> if the <c>jti</c> was successfully added (first use, not a replay);
    /// <see langword="false"/> if the <c>jti</c> was already present (replay detected).
    /// </returns>
    Task<bool> TryAddAsync(string jti, DateTimeOffset expiration, CancellationToken cancellationToken = default);
}
