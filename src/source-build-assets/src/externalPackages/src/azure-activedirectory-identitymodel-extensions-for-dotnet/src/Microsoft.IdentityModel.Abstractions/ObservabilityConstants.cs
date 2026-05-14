// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// Common class containing observability constants to be used as well known metric keys.
    /// </summary>
    public static class ObservabilityConstants
    {
        /// <summary>
        /// String used for the name of the property indicating if the call was successful.
        /// </summary>
        public const string Succeeded = "Succeeded";

        /// <summary>
        /// String used for the name of the property indicating the call in Duration (ms).
        /// </summary>
        public const string Duration = "Duration";

        /// <summary>
        /// String used for the name of the property indicating the call's Activity Id/Correlation Id.
        /// </summary>
        public const string ActivityId = "ActivityId";

        /// <summary>
        /// String used for the name of the property indicating the caller's ClientId.
        /// </summary>
        public const string ClientId = "ClientId";
    }
}
