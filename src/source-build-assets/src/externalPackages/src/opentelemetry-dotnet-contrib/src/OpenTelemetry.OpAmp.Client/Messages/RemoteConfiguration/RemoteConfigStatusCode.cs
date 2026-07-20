// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Messages;

/// <summary>
/// The status of a remote configuration previously received from the OpAMP server.
/// </summary>
public enum RemoteConfigStatusCode
{
    /// <summary>
    /// The status is not set.
    /// </summary>
    Unset = 0,

    /// <summary>
    /// The remote configuration was successfully applied.
    /// </summary>
    Applied = 1,

    /// <summary>
    /// The remote configuration is currently being applied.
    /// </summary>
    Applying = 2,

    /// <summary>
    /// The remote configuration could not be applied.
    /// </summary>
    Failed = 3,
}
