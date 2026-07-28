// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Telemetry.Tests
{
    public class MockTelemetryClient : ITelemetryClient
    {
        public ConcurrentDictionary<string, object> ExportedItems = new ConcurrentDictionary<string, object>();
        public ConcurrentDictionary<string, object> ExportedHistogramItems = new ConcurrentDictionary<string, object>();

        internal int _requestRefreshCounter;

        public int RequestRefreshCounter => _requestRefreshCounter;

        public void ClearExportedItems()
        {
            ExportedItems.Clear();
            ExportedHistogramItems.Clear();
        }

        public void IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus, string configurationSource)
        {
            Interlocked.Increment(ref _requestRefreshCounter);
            ExportedItems.TryAdd(TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer);
            ExportedItems.TryAdd(TelemetryConstants.MetadataAddressTag, metadataAddress);
            ExportedItems.TryAdd(TelemetryConstants.OperationStatusTag, operationStatus);
            ExportedItems.TryAdd(TelemetryConstants.ConfigurationSourceTag, configurationSource);
        }

        public void IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus, string configurationSource, Exception exception)
        {
            Interlocked.Increment(ref _requestRefreshCounter);
            ExportedItems.TryAdd(TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer);
            ExportedItems.TryAdd(TelemetryConstants.MetadataAddressTag, metadataAddress);
            ExportedItems.TryAdd(TelemetryConstants.OperationStatusTag, operationStatus);
            ExportedItems.TryAdd(TelemetryConstants.ConfigurationSourceTag, configurationSource);
            ExportedItems.TryAdd(TelemetryConstants.ExceptionTypeTag, exception.GetType().ToString());
        }

        public void LogConfigurationRetrievalDuration(string metadataAddress, string configurationSource, TimeSpan operationDuration)
        {
            ExportedHistogramItems.TryAdd(TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer);
            ExportedHistogramItems.TryAdd(TelemetryConstants.MetadataAddressTag, metadataAddress);
            ExportedHistogramItems.TryAdd(TelemetryConstants.ConfigurationSourceTag, configurationSource);
        }

        public void LogConfigurationRetrievalDuration(string metadataAddress, string configurationSource, TimeSpan operationDuration, Exception exception)
        {
            ExportedHistogramItems.TryAdd(TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer);
            ExportedHistogramItems.TryAdd(TelemetryConstants.MetadataAddressTag, metadataAddress);
            ExportedHistogramItems.TryAdd(TelemetryConstants.ConfigurationSourceTag, configurationSource);
            ExportedHistogramItems.TryAdd(TelemetryConstants.ExceptionTypeTag, exception.GetType().ToString());
        }

        void ITelemetryClient.LogBackgroundConfigurationRefreshFailure(string metadataAddress, string configurationSource, Exception exception)
        {
            ExportedItems.TryAdd(TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer);
            ExportedItems.TryAdd(TelemetryConstants.MetadataAddressTag, metadataAddress);
            ExportedItems.TryAdd(TelemetryConstants.ConfigurationSourceTag, configurationSource);
            ExportedItems.TryAdd(TelemetryConstants.ExceptionTypeTag, exception.GetType().ToString());
        }

        void ITelemetryClient.IncrementSignatureValidationCounter(string errorType, string issuer, string algorithm, SecurityKey key)
        {
            // Stub implementation for testing
        }

        void ITelemetryClient.IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus, Exception exception) => throw new NotImplementedException("This method shouldn't be called. It is kept only as back-compat mechanism in case of assembly version mismatch");
        void ITelemetryClient.LogBackgroundConfigurationRefreshFailure(string metadataAddress, Exception exception) => throw new NotImplementedException("This method shouldn't be called. It is kept only as back-compat mechanism in case of assembly version mismatch");
        void ITelemetryClient.LogConfigurationRetrievalDuration(string metadataAddress, TimeSpan operationDuration) => throw new NotImplementedException("This method shouldn't be called. It is kept only as back-compat mechanism in case of assembly version mismatch");
        void ITelemetryClient.LogConfigurationRetrievalDuration(string metadataAddress, TimeSpan operationDuration, Exception exception) => throw new NotImplementedException("This method shouldn't be called. It is kept only as back-compat mechanism in case of assembly version mismatch");
        void ITelemetryClient.IncrementConfigurationRefreshRequestCounter(string metadataAddress, string operationStatus) => throw new NotImplementedException("This method shouldn't be called. It is kept only as back-compat mechanism in case of assembly version mismatch");
    }
}
