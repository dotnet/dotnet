// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.OpAmp.Client.Settings;

namespace OpenTelemetry.OpAmp.Client.Internal.Services;

internal interface IBackgroundService
{
    /// <summary>
    /// Gets the name of the service.
    /// </summary>
    string ServiceName { get; }

    /// <summary>
    /// Starts the background service.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops the background service.
    /// </summary>
    void Stop();

    /// <summary>
    /// Configures the service using the specified OpenAMP settings.
    /// </summary>
    /// <param name="settings">The OpenAMP settings to apply.</param>
    void Configure(OpAmpClientSettings settings);
}
