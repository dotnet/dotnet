// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
#if !NET462 && !NET472 && !NETSTANDARD2_0
using System.Security.Cryptography;
#endif
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.TestUtils.Telemetry;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Telemetry.Tests;

/// <summary>
/// Tests for JWT signature validation and decryption telemetry.
/// These tests must run sequentially to avoid telemetry cross-contamination from parallel test execution.
/// </summary>
[Collection("Telemetry Tests")]
public class SignatureValidationTelemetryTests
{
    const string ExpectedIssuer = "Default.Issuer.com";
    public SignatureValidationTelemetryTests()
    {
        CryptoTelemetry.EnableSignatureValidationTelemetry(true, new[] { ExpectedIssuer });
    }

    [Fact]
    public async Task ValidateToken_SignatureValidationSuccess_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        var result = await handler.ValidateTokenAsync(token, validationParameters);

        // Assert
        Assert.True(result.IsValid);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.None },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "RSA-2048" }
            });
    }

    [Fact]
    public async Task ValidateToken_ExperimentalAPI_SignatureValidationSuccess_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var validationParameters = new ValidationParameters();
        validationParameters.SigningKeys.Add(KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key);

        // Skip validations that require additional setup
        validationParameters.AudienceValidator = SkipValidationDelegates.SkipAudienceValidation;
        validationParameters.SignatureKeyValidator = SkipValidationDelegates.SkipIssuerSigningKeyValidation;
        validationParameters.IssuerValidatorAsync = SkipValidationDelegates.SkipIssuerValidation;
        validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;
        validationParameters.TokenReplayValidator = SkipValidationDelegates.SkipTokenReplayValidation;
        validationParameters.TokenTypeValidator = SkipValidationDelegates.SkipTokenTypeValidation;

        // Act
        var result = await ((IResultBasedValidation)handler).ValidateTokenAsync(token, validationParameters, new CallContext());

        // Assert
        Assert.True(result.Succeeded);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.None },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "RSA-2048" }
            });
    }

    [Fact]
    public void ValidateToken_LegacyJwtSecurityTokenHandler_SignatureValidationSuccess_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var jsonHandler = new JsonWebTokenHandler();
        var token = jsonHandler.CreateToken(tokenDescriptor);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        handler.ValidateToken(token, validationParameters, out _);

        // Assert
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.None },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "RSA-2048" }
            });
    }

    [Fact]
    public void ValidateToken_LegacyJwtSecurityTokenHandler_SignatureProviderCreationFails_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var jsonHandler = new JsonWebTokenHandler();
        var token = jsonHandler.CreateToken(tokenDescriptor);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.SymmetricSigningKey256,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        try
        {
            handler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            // Expected to throw
        }

        // Assert
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.SignatureProviderCreationFailed },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "SYM-256" }
            });
    }

    [Fact]
    public async Task ValidateToken_SymmetricSignature_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = Default.SymmetricSigningCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        var result = await handler.ValidateTokenAsync(token, validationParameters);

        // Assert
        Assert.True(result.IsValid);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.None },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.HMAC },
                { TelemetryConstants.KeyAlgorithmTag, "SYM-256" }
            });
    }

#if !NET462 && !NET472 && !NETSTANDARD2_0
    [Fact]
    public async Task ValidateToken_ES256Signature_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();

        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var ecdsaKey = new ECDsaSecurityKey(ecdsa) { KeyId = "ES256Key" };
        var signingCredentials = new SigningCredentials(ecdsaKey, SecurityAlgorithms.EcdsaSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = signingCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = ecdsaKey,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        var result = await handler.ValidateTokenAsync(token, validationParameters);

        // Assert
        Assert.True(result.IsValid);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.None },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.ECDSA },
                { TelemetryConstants.KeyAlgorithmTag, "ECDSA-P256" }
            });
    }

    [Fact]
    public async Task ValidateToken_ES384Signature_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();

        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP384);
        var ecdsaKey = new ECDsaSecurityKey(ecdsa) { KeyId = "ES384Key" };
        var signingCredentials = new SigningCredentials(ecdsaKey, SecurityAlgorithms.EcdsaSha384);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = signingCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = ecdsaKey,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        var result = await handler.ValidateTokenAsync(token, validationParameters);

        // Assert
        Assert.True(result.IsValid);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.None },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.ECDSA },
                { TelemetryConstants.KeyAlgorithmTag, "ECDSA-P384" }
            });
    }

    [Fact]
    public async Task ValidateToken_ES512Signature_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();

        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP521);
        var ecdsaKey = new ECDsaSecurityKey(ecdsa) { KeyId = "ES512Key" };
        var signingCredentials = new SigningCredentials(ecdsaKey, SecurityAlgorithms.EcdsaSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = signingCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = ecdsaKey,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        var result = await handler.ValidateTokenAsync(token, validationParameters);

        // Assert
        Assert.True(result.IsValid);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.None },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.ECDSA },
                { TelemetryConstants.KeyAlgorithmTag, "ECDSA-P521" }
            });
    }
#endif

    [Fact]
    public async Task ValidateToken_AlgorithmNotSupported_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);

        // Use a symmetric key for an asymmetric algorithm (algorithm not supported)
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.SymmetricSigningKey256,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        var result = await handler.ValidateTokenAsync(token, validationParameters);

        // Assert
        Assert.False(result.IsValid);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.AlgorithmNotSupported },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "SYM-256" }
            });
    }

    [Fact]
    public async Task ValidateToken_LegacyHandler_SignatureProviderCreationFails_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var jsonHandler = new JsonWebTokenHandler();
        var token = jsonHandler.CreateToken(tokenDescriptor);

        // Use a symmetric key for an asymmetric algorithm (algorithm not supported)
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.SymmetricSigningKey256,
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
        };

        // Act
        try
        {
            handler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            // Expected to throw
        }

        // Assert
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.SignatureProviderCreationFailed },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "SYM-256" }
            });
    }

    [Fact]
    public async Task ValidateToken_SigningKeyNotFound_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);

        // Don't provide any signing keys
        var validationParameters = new TokenValidationParameters
        {
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
        };

        // Act
        var result = await handler.ValidateTokenAsync(token, validationParameters);

        // Assert
        Assert.False(result.IsValid);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.SigningKeyNotFound },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "NO-KEY" }
            });
    }

    [Fact]
    public async Task ValidateToken_ExperimentalAPI_SigningKeyNotFound_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var token = handler.CreateToken(tokenDescriptor);
        var validationParameters = new ValidationParameters();
        // Don't provide any signing keys

        // Skip validations that require additional setup
        validationParameters.AudienceValidator = SkipValidationDelegates.SkipAudienceValidation;
        validationParameters.SignatureKeyValidator = SkipValidationDelegates.SkipIssuerSigningKeyValidation;
        validationParameters.IssuerValidatorAsync = SkipValidationDelegates.SkipIssuerValidation;
        validationParameters.LifetimeValidator = SkipValidationDelegates.SkipLifetimeValidation;
        validationParameters.TokenReplayValidator = SkipValidationDelegates.SkipTokenReplayValidation;
        validationParameters.TokenTypeValidator = SkipValidationDelegates.SkipTokenTypeValidation;

        // Act
        var result = await ((IResultBasedValidation)handler).ValidateTokenAsync(token, validationParameters, new CallContext());

        // Assert
        Assert.False(result.Succeeded);
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.SigningKeyNotFound },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "NO-KEY" }
            });
    }

    [Fact]
    public void ValidateToken_LegacyHandler_SigningKeyNotFound_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        var handler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            Issuer = Default.Issuer,
        };

        var jsonHandler = new JsonWebTokenHandler();
        var token = jsonHandler.CreateToken(tokenDescriptor);

        // Don't provide any signing keys
        var validationParameters = new TokenValidationParameters
        {
            ValidAudience = Default.Audience,
            ValidIssuer = Default.Issuer,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
        };

        // Act
        try
        {
            handler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            // Expected to throw
        }

        // Assert
        TelemetryAssertionHelpers.AssertTelemetryRecorded(listener, TelemetryDataRecorder.SignatureValidationCounterName,
            new Dictionary<string, object>
            {
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.SigningKeyNotFound },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "NO-KEY" }
            });
    }
}
