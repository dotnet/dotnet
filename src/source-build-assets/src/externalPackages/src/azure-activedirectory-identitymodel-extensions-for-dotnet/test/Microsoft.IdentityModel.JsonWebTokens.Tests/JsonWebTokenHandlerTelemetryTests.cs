// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.Telemetry.Tests;
using Microsoft.IdentityModel.Validators;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenHandlerTelemetryTests
    {
        [Fact]
        public async Task ValidateJwsWithConfigAsync_ExpectedTagsExist()
        {
            var invalidIssuerConfig = new OpenIdConnectConfiguration()
            {
                TokenEndpoint = Default.Issuer + "oauth/token",
                Issuer = Default.Issuer + "2"
            };
            invalidIssuerConfig.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);

            var validationParameters = new TokenValidationParameters
            {
                ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(invalidIssuerConfig),
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var testTelemetryClient = new MockTelemetryClient();
            try
            {
                var handler = new JsonWebTokenHandler()
                {
                    TelemetryClient = testTelemetryClient
                };
                var jwt = handler.ReadJsonWebToken(Default.AsymmetricJws);
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = validationParameters.ConfigurationManager;
                var validationResult = await handler.ValidateTokenAsync(jwt, validationParameters);
                var rawTokenValidationResult = await handler.ValidateTokenAsync(Default.AsymmetricJws, validationParameters);
            }
            catch (Exception)
            {
                // ignore exceptions
            }

            var expectedCounterTagList = new Dictionary<string, object>
            {
                // metadata address is null because the configuration manager is made using an invalid config to trigger an exception
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.MetadataAddressTag, null },
                { TelemetryConstants.OperationStatusTag, TelemetryConstants.Protocols.Lkg },
                { TelemetryConstants.ConfigurationSourceTag, TelemetryConstants.Protocols.ConfigurationSourceUnknown }
            };

            Assert.Equal(expectedCounterTagList, testTelemetryClient.ExportedItems);
        }
    }
}
