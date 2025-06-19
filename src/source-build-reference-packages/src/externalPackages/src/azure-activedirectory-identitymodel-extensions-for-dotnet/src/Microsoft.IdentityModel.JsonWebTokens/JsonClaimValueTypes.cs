// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Claims;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// Constants that indicate how the <see cref="Claim.Value"/> should be evaluated.
    /// </summary>
    public static class JsonClaimValueTypes
    {
        /// <summary>
        /// A value that indicates the <see cref="Claim.Value"/> is a Json object.
        /// </summary>
        /// <remarks>When creating a <see cref="Claim"/> from Json if the value was not a simple type {String, Null, True, False, Number}
        /// then <see cref="Claim.Value"/> will contain the Json value. If the Json was a JsonObject, the <see cref="Claim.ValueType"/> will be set to "JSON".</remarks>
        public const string Json = "JSON";

        /// <summary>
        /// A value that indicates the <see cref="Claim.Value"/> is a Json object.
        /// </summary>
        /// <remarks>When creating a <see cref="Claim"/> from Json if the value was not a simple type {String, Null, True, False, Number}
        /// then <see cref="Claim.Value"/> will contain the Json value. If the Json was a JsonArray, the <see cref="Claim.ValueType"/> will be set to "JSON_ARRAY".</remarks>
        public const string JsonArray = "JSON_ARRAY";

        /// <summary>
        /// A value that indicates the <see cref="Claim.Value"/> is Json null.
        /// </summary>
        /// <remarks>When creating a <see cref="Claim"/> the <see cref="Claim.Value"/> cannot be null. If the Json value was null, then the <see cref="Claim.Value"/>
        /// will be set to <see cref="string.Empty"/> and the <see cref="Claim.ValueType"/> will be set to "JSON_NULL".</remarks>
        public const string JsonNull = "JSON_NULL";
    }
}
