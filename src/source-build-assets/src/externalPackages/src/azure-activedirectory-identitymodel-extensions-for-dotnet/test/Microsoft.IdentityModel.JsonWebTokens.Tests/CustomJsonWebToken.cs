// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class CustomJsonWebToken : JsonWebToken
    {
        // Represents claims known to this custom implementation and not to the IdentityModel.
        // The claim name is the same in the header and the payload, but the value is different.
        public const string CustomClaimName = "CustomClaimName";

        private CustomClaim _customHeaderClaim;
        private CustomClaim _customPayloadClaim;

        public CustomClaim CustomHeaderClaim
        {
            get
            {
                _customHeaderClaim ??= Header.GetValue<CustomClaim>(CustomClaimName);
                return _customHeaderClaim;
            }
        }

        public CustomClaim CustomPayloadClaim
        {
            get
            {
                _customPayloadClaim ??= Payload.GetValue<CustomClaim>(CustomClaimName);
                return _customPayloadClaim;
            }
        }

        public CustomJsonWebToken(string jwtEncodedString) : base(jwtEncodedString) { }

        public CustomJsonWebToken(ReadOnlyMemory<char> encodedTokenMemory) : base(encodedTokenMemory) { }

        public CustomJsonWebToken(string header, string payload) : base(header, payload) { }

        private protected override void ReadHeaderValue(ref Utf8JsonReader reader, IDictionary<string, object> claims)
        {
            // Handle custom claims.
            if (reader.ValueTextEquals(CustomClaimName))
            {
                // Deserialize the custom object claim in an appropriate way.
                reader.Read(); // Move to the value.
                _customHeaderClaim = JsonSerializer.Deserialize<CustomClaim>(reader.GetString());
                claims[CustomClaimName] = _customHeaderClaim;
                reader.Read();
            }
            else
            {
                // Call base implementation to handle other claims known to IdentityModel.
                base.ReadHeaderValue(ref reader, claims);
            }
        }

        private protected override void ReadPayloadValue(ref Utf8JsonReader reader, IDictionary<string, object> claims)
        {
            // Handle custom claims.
            if (reader.ValueTextEquals(CustomClaimName))
            {
                // Deserialize the custom object claim in an appropriate way.
                reader.Read(); // Move to the value.
                _customPayloadClaim = JsonSerializer.Deserialize<CustomClaim>(reader.GetString());
                claims[CustomClaimName] = _customPayloadClaim;
                reader.Read();
            }
            else
            {
                // Call base implementation to handle other claims known to IdentityModel.
                base.ReadPayloadValue(ref reader, claims);
            }
        }
    }

    public class CustomClaim
    {
        public CustomClaim() { }

        public CustomClaim(string value)
        {
            CustomClaimValue = value;
        }

        public string CustomClaimValue { get; set; }
    }
}
