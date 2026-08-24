// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Telemetry;

/// <summary>
/// A no-op implementation of <see cref="ITelemetryClient"/> that performs no operations.
/// </summary>
[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is a no-op implementation of ITelemetryClient interface.")]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This is a no-op implementation of ITelemetryClient interface.")]
internal class NullTelemetryClient : ITelemetryClient
{
    internal static readonly NullTelemetryClient Instance = new NullTelemetryClient();

    void ITelemetryClient.IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus, string configurationSource) { }

    void ITelemetryClient.IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus, string configurationSource, Exception exception) { }

    void ITelemetryClient.IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus) { }

    void ITelemetryClient.IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus, Exception exception) { }

    void ITelemetryClient.IncrementSignatureValidationCounter(string errorType, string issuer, string algorithm, SecurityKey key) { }

    void ITelemetryClient.LogBackgroundConfigurationRefreshFailure(string metadataAddress, string configurationSource, Exception exception) { }

    void ITelemetryClient.LogBackgroundConfigurationRefreshFailure(string metadataAddress, Exception exception) { }

    void ITelemetryClient.LogConfigurationRetrievalDuration(string metadataAddress, string configurationSource, TimeSpan operationDuration) { }

    void ITelemetryClient.LogConfigurationRetrievalDuration(string metadataAddress, string configurationSource, TimeSpan operationDuration, Exception exception) { }

    void ITelemetryClient.LogConfigurationRetrievalDuration(string metadataAddress, TimeSpan operationDuration) { }

    void ITelemetryClient.LogConfigurationRetrievalDuration(string metadataAddress, TimeSpan operationDuration, Exception exception) { }
}
