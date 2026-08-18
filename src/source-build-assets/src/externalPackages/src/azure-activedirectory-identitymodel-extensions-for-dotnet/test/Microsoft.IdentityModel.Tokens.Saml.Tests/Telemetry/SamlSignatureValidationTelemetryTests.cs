// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.TestUtils.Telemetry;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests.Telemetry;

/// <summary>
/// Tests for SAML signature validation telemetry.
/// These tests must run sequentially to avoid telemetry cross-contamination from parallel test execution.
/// </summary>
[Collection("Telemetry Tests")]
public class SamlSignatureValidationTelemetryTests
{
    const string ExpectedIssuer = "Default.Issuer.com";

    public SamlSignatureValidationTelemetryTests()
    {
        CryptoTelemetry.EnableSignatureValidationTelemetry(true, new[] { ExpectedIssuer });
    }

    [Fact]
    public void ValidateToken_SamlSignatureValidationSuccess_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        ITestingTokenHandler handler = new SamlSecurityTestingTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = Default.ClaimsIdentityLongNames,
            SigningCredentials = Default.AsymmetricSigningCredentials,
            Audience = Default.Audience,
            Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
            Issuer = Default.Issuer
        };

        var tokenString = handler.CreateStringToken(tokenDescriptor);

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.AsymmetricSigningCredentials.Key,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
        };

        // Act
        handler.ValidateTokenAsync(tokenString, validationParameters);

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
    public void ValidateToken_SamlSignatureValidationFailure_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        ITestingTokenHandler handler = new SamlSecurityTestingTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = Default.ClaimsIdentityLongNames,
            SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
            Audience = Default.Audience,
            Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
            Issuer = Default.Issuer
        };

        var tokenString = handler.CreateStringToken(tokenDescriptor);

        // Use a different key for validation (should fail)
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = KeyingMaterial.RsaSecurityKey2,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
        };

        // Act
        try
        {
            handler.ValidateTokenAsync(tokenString, validationParameters);
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
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.SignatureVerificationFailed },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "RSA-2048" }
            });
    }

    [Fact]
    public void ValidateToken_Saml2SignatureValidationSuccess_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        ITestingTokenHandler handler = new Saml2SecurityTestingTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = Default.ClaimsIdentityLongNames,
            SigningCredentials = Default.AsymmetricSigningCredentials,
            Audience = Default.Audience,
            Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
            Issuer = Default.Issuer
        };

        var tokenString = handler.CreateStringToken(tokenDescriptor);

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.AsymmetricSigningCredentials.Key,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
        };

        // Act
        handler.ValidateTokenAsync(tokenString, validationParameters);

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
    public void ValidateToken_Saml2SignatureValidationFailure_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        ITestingTokenHandler handler = new Saml2SecurityTestingTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = Default.ClaimsIdentityLongNames,
            SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
            Audience = Default.Audience,
            Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
            Issuer = Default.Issuer
        };

        var tokenString = handler.CreateStringToken(tokenDescriptor);

        // Use a different key for validation (should fail)
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = KeyingMaterial.RsaSecurityKey2,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
        };

        // Act
        try
        {
            handler.ValidateTokenAsync(tokenString, validationParameters);
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
                { TelemetryConstants.ErrorTag, TelemetryConstants.SignatureValidationErrors.SignatureVerificationFailed },
                { TelemetryConstants.IssuerTag, ExpectedIssuer },
                { TelemetryConstants.AlgorithmTag, TelemetryConstants.AlgorithmFamilies.RSA },
                { TelemetryConstants.KeyAlgorithmTag, "RSA-2048" }
            });
    }

    [Fact]
    public void ValidateToken_SamlSymmetricSignature_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        ITestingTokenHandler handler = new SamlSecurityTestingTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = Default.ClaimsIdentityLongNames,
            SigningCredentials = Default.SymmetricSigningCredentials,
            Audience = Default.Audience,
            Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
            Issuer = Default.Issuer
        };

        var tokenString = handler.CreateStringToken(tokenDescriptor);

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
        };

        // Act
        handler.ValidateTokenAsync(tokenString, validationParameters);

        // Assert
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

    [Fact]
    public void ValidateToken_Saml2SymmetricSignature_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        ITestingTokenHandler handler = new Saml2SecurityTestingTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = Default.ClaimsIdentityLongNames,
            SigningCredentials = Default.SymmetricSigningCredentials,
            Audience = Default.Audience,
            Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
            Issuer = Default.Issuer
        };

        var tokenString = handler.CreateStringToken(tokenDescriptor);

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
        };

        // Act
        handler.ValidateTokenAsync(tokenString, validationParameters);

        // Assert
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

    [Fact]
    public void ValidateToken_SamlRsa2048Signature_RecordsTelemetry()
    {
        // Arrange
        using var listener = new TestMeterListener();
        ITestingTokenHandler handler = new SamlSecurityTestingTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = Default.ClaimsIdentityLongNames,
            SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
            Audience = Default.Audience,
            Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
            Issuer = Default.Issuer
        };

        var tokenString = handler.CreateStringToken(tokenDescriptor);

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
        };

        // Act
        handler.ValidateTokenAsync(tokenString, validationParameters);

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
}
