using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class CustomJsonWebToken : JsonWebToken
    {
        private const string CustomClaimName = "CustomClaim";

        public CustomJsonWebToken(string jwtEncodedString) : base(jwtEncodedString) { }

        public CustomJsonWebToken(ReadOnlyMemory<char> encodedTokenMemory) : base(encodedTokenMemory) { }

        public CustomJsonWebToken(string header, string payload) : base(header, payload) { }

        private protected override void ReadPayloadValue(ref Utf8JsonReader reader, IDictionary<string, object> claims)
        {
            if (reader.ValueTextEquals(CustomClaimName))
            {
                _customClaim = JsonSerializerPrimitives.ReadString(ref reader, CustomClaimName, ClassName, true);
                claims[CustomClaimName] = _customClaim;
            }
            else
            {
                base.ReadPayloadValue(ref reader, claims);
            }
        }

        private string _customClaim;

        public string CustomClaim
        {
            get
            {
                _customClaim ??= Payload.GetStringValue(CustomClaimName);
                return _customClaim;
            }
        }
    }
}
