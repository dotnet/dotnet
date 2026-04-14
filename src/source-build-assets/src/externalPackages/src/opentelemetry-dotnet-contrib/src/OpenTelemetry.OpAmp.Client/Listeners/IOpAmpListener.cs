// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.OpAmp.Client.Messages;

namespace OpenTelemetry.OpAmp.Client.Listeners;

/// <summary>
/// A listener capable of handling OpAMP messages of a specific type.
/// </summary>
/// <typeparam name="TMessage">The <see cref="OpAmpMessage"/> type this listener handles.</typeparam>
public interface IOpAmpListener<TMessage>
    where TMessage : OpAmpMessage
{
    /// <summary>
    /// Handles the specified OpAMP message.
    /// </summary>
    /// <param name="message">The OpAMP message to handle.</param>
    void HandleMessage(TMessage message);
}
