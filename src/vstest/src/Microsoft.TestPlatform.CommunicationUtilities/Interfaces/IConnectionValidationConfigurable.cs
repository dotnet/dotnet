// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

/// <summary>
/// Implemented by a transport that checks the identity of the peer it accepts, using the secret pair
/// generated for the child process it is expecting.
/// </summary>
public interface IConnectionValidationConfigurable
{
    string? ConnectionValidationError { get; }

    void RequireConnectionValidation();

    void UseConnectionSecrets(string connectionSecrets);

    /// <summary>
    /// Records that no secrets are available, so the endpoint fails promptly with this explanation
    /// instead of waiting for the connection timeout.
    /// </summary>
    void CannotValidateConnection(string error);
}

/// <summary>
/// Implemented by a transport that presents the child half of the secret pair. The side that was
/// started presents first, whichever side listens.
/// </summary>
public interface IConnectionAuthenticationResponder
{
    void UseConnectionSecretsAsChild(string connectionSecrets);
}