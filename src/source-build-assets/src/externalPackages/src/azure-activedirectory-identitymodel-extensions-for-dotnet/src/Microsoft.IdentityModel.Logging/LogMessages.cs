// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// System.IdentityModel.Logging
// Range: MIML10000 - MIML10999

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// Log messages and codes for Microsoft.IdentityModel.Logging
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // logging
        internal const string MIML10000 = "MIML10000: eventData.Payload is null or empty. Not logging any messages.";
        internal const string MIML10001 = "MIML10001: Cannot create the fileStream or StreamWriter to write logs. See inner exception.";
        internal const string MIML10002 = "MIML10002: Unknown log level: {0}.";
        internal const string MIML10003 = "MIML10003: Sku and version telemetry cannot be manipulated. They are added by default.";
#pragma warning restore 1591

    }
}
