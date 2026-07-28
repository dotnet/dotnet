// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Telemetry
{
    internal static class TelemetryConstants
    {
        // Static attribute tags

        /// <summary>
        /// Telemetry tag indicating the version of the IdentityModel library.
        /// </summary>
        public const string IdentityModelVersionTag = "IdentityModelVersion";

        /// <summary>
        /// Telemetry tag indicating the endpoint from which a configuration is retrieved.
        /// </summary>
        public const string MetadataAddressTag = "MetadataAddress";

        /// <summary>
        /// Telemetry tag describing the operation being performed.
        /// </summary>
        public const string OperationStatusTag = "OperationStatus";

        /// <summary>
        /// Telemetry tag indicating the source of the configuration.
        /// </summary>
        public const string ConfigurationSourceTag = "ConfigurationSource";

        /// <summary>
        /// Telemetry tag indicating the type of exception that occurred.
        /// </summary>
        public const string ExceptionTypeTag = "ExceptionType";

        /// <summary>
        /// Telemetry tag indicating if the update was blocking.
        /// </summary>
        public const string BlockingTypeTag = "Blocking";

        /// <summary>
        /// Telemetry tag indicating the status of an operation (success or failure).
        /// </summary>
        public const string StatusTag = "Status";

        /// <summary>
        /// Telemetry tag indicating the cryptographic algorithm used.
        /// </summary>
        public const string AlgorithmTag = "Algorithm";

        /// <summary>
        /// Telemetry tag indicating the key algorithm used.
        /// </summary>
        public const string KeyAlgorithmTag = "KeyAlgorithm";

        /// <summary>
        /// Telemetry tag indicating the error that occurred on failure.
        /// </summary>
        public const string ErrorTag = "Error";

        /// <summary>
        /// Telemetry tag indicating the token issuer.
        /// </summary>
        public const string IssuerTag = "Issuer";

        /// <summary>
        /// Telemetry value indicating a successful operation.
        /// </summary>
        public const string SuccessValue = "Success";

        /// <summary>
        /// Telemetry value indicating a failed operation.
        /// </summary>
        public const string FailureValue = "Failure";

        public static class Protocols
        {
            // Configuration manager refresh statuses

            /// <summary>
            /// Telemetry tag indicating configuration retrieval after the refresh interval has expired.
            /// </summary>
            public const string Automatic = "Automatic";

            /// <summary>
            /// Telemetry tag indicating configuration retrieval per a call to RequestRefresh.
            /// </summary>
            public const string Manual = "Manual";

            /// <summary>
            /// Telemetry tag indicating configuration retrieval when there is no previously cached configuration.
            /// </summary>
            public const string FirstRefresh = "FirstRefresh";

            /// <summary>
            /// Telemetry tag indicating configuration retrieval when the last known good configuration is needed.
            /// </summary>
            public const string Lkg = "LastKnownGood";

            // Configuration manager exception types

            /// <summary>
            /// Telemetry tag indicating that configuration could not be sucessfully validated after retrieval.
            /// </summary>
            public const string ConfigurationInvalid = "ConfigurationInvalid";

            /// <summary>
            /// Telemetry tag indicating that configuration could not be retrieved successfully.
            /// </summary>
            public const string ConfigurationRetrievalFailed = "ConfigurationRetrievalFailed";

            /// <summary>
            /// Telemetry tag indicating that metadata endpoint is the source of the configuration.
            /// </summary>
            public const string ConfigurationSourceRetriever = "Retriever";

            /// <summary>
            /// Telemetry tag indicating that a configuration handler is the source of the configuration.
            /// </summary>
            public const string ConfigurationSourceHandler = "Handler";

            /// <summary>
            /// Telemetry tag indicating that the configuration source is unknown for the scenario.
            /// </summary>
            /// <remarks>
            /// This is used when the configuration source cannot be determined upfront, such in cases where LKG calls or
            /// request refresh calls on background thread are counted. Configuration source is irrelevant in these cases.
            /// </remarks>
            public const string ConfigurationSourceUnknown = "Unknown";
        }

        /// <summary>
        /// Algorithm family constants for telemetry. Raw algorithm strings are mapped to these
        /// bounded values to prevent cardinality explosion in metric dimensions.
        /// </summary>
        public static class AlgorithmFamilies
        {
            public const string RSA = "RSA";
            public const string RSAPSS = "RSA-PSS";
            public const string ECDSA = "ECDSA";
            public const string HMAC = "HMAC";
            public const string None = "none";
            public const string Other = "other";
        }

        /// <summary>
        /// Signature validation error constants. Kept limited to prevent cardinality explosion.
        /// </summary>
        public static class SignatureValidationErrors
        {
            /// <summary>
            /// Signature validation succeeded.
            /// </summary>
            public const string None = "None";

            /// <summary>
            /// Signature validation failed after the signature provider was successfully created.
            /// This is the primary failure case we want to track - when the key is present,
            /// crypto provider is resolved, but signature verification fails.
            /// </summary>
            public const string SignatureVerificationFailed = "SignatureVerificationFailed";

            /// <summary>
            /// Algorithm not supported by the key or crypto provider.
            /// </summary>
            public const string AlgorithmNotSupported = "AlgorithmNotSupported";

            /// <summary>
            /// Signature provider could not be created by the crypto provider.
            /// </summary>
            public const string SignatureProviderCreationFailed = "SignatureProviderCreationFailed";

            /// <summary>
            /// No signing key was found or resolved.
            /// </summary>
            public const string SigningKeyNotFound = "SigningKeyNotFound";

            /// <summary>
            /// Other errors not covered by specific categories.
            /// </summary>
            public const string Other = "Other";
        }
    }
}
