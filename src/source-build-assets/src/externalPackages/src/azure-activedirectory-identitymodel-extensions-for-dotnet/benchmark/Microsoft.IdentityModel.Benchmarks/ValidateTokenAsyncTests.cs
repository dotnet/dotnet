// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.ValidateTokenAsyncTests*

    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class ValidateTokenAsyncTests
    {
        private CallContext _callContext;
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;
        private SecurityTokenDescriptor _tokenDescriptorExtendedClaims;
        private string _jws;
        private string _jwsExtendedClaims;
        private TokenValidationParameters _tokenValidationParameters;
        private TokenValidationParameters _invalidTokenValidationParameters;
        private ValidationParameters _validationParameters;
        private ValidationParameters _invalidValidationParameters;

        [GlobalSetup]
        public void Setup()
        {
            _tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = BenchmarkUtils.Claims,
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
            };

            _tokenDescriptorExtendedClaims = new SecurityTokenDescriptor
            {
                Claims = BenchmarkUtils.ClaimsExtendedExample,
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
            };

            _jsonWebTokenHandler = new JsonWebTokenHandler();
            _jws = _jsonWebTokenHandler.CreateToken(_tokenDescriptor);
            _jwsExtendedClaims = _jsonWebTokenHandler.CreateToken(_tokenDescriptorExtendedClaims);

            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _jwtSecurityTokenHandler.SetDefaultTimesOnTokenCreation = false;

            _tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = BenchmarkUtils.Audience,
                ValidateLifetime = true,
                ValidIssuer = BenchmarkUtils.Issuer,
                IssuerSigningKey = BenchmarkUtils.SigningCredentialsRsaSha256.Key,
            };

            _validationParameters = new ValidationParameters();
            _validationParameters.ValidAudiences.Add(BenchmarkUtils.Audience);
            _validationParameters.ValidIssuers.Add(BenchmarkUtils.Issuer);
            _validationParameters.SigningKeys.Add(BenchmarkUtils.SigningCredentialsRsaSha256.Key);

            _invalidTokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = BenchmarkUtils.Audience,
                ValidateLifetime = true,
                ValidIssuer = BenchmarkUtils.Issuer,
                ValidateIssuerSigningKey = true,
                ValidateTokenReplay = true,
                ValidateSignatureLast = true
            };

            _invalidValidationParameters = new ValidationParameters();
            _invalidValidationParameters.ValidAudiences.Add(BenchmarkUtils.Audience);
            _invalidValidationParameters.ValidIssuers.Add(BenchmarkUtils.Issuer);

            _callContext = new CallContext();
        }

        [BenchmarkCategory("ValidateTokenAsync_Success"), Benchmark]
        public async Task<TokenValidationResult> JwtSecurityTokenHandler_ValidateTokenAsync() => await _jwtSecurityTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _tokenValidationParameters).ConfigureAwait(false);

        [BenchmarkCategory("ValidateTokenAsync_Success"), Benchmark(Baseline = true)]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsyncWithTVP() => await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _tokenValidationParameters).ConfigureAwait(false);

        [BenchmarkCategory("ValidateTokenAsync_Success"), Benchmark]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsyncWithTVPUsingModifiedClone()
        {
            var tokenValidationParameters = _tokenValidationParameters.Clone();
            tokenValidationParameters.ValidIssuer = "different-issuer";
            tokenValidationParameters.ValidAudience = "different-audience";
            tokenValidationParameters.ValidateLifetime = false;
            return await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, tokenValidationParameters).ConfigureAwait(false);
        }

        [BenchmarkCategory("ValidateTokenAsync_Success"), Benchmark]
        public async Task<bool> JsonWebTokenHandler_ValidateTokenAsyncWithVP()
        {
            // Because ValidationResult is an internal type, we cannot return it in the benchmark.
            // We return a boolean instead until the type is made public.
            ValidationResult<ValidatedToken, ValidationError> validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _validationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            return validationResult.Succeeded;
        }

        [BenchmarkCategory("ValidateTokenAsync_FailTwiceBeforeSuccess"), Benchmark(Baseline = true)]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsyncWithTVP_SucceedOnThirdAttempt()
        {
            TokenValidationResult result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _tokenValidationParameters).ConfigureAwait(false);

            return result;
        }

        [BenchmarkCategory("ValidateTokenAsync_FailTwiceBeforeSuccess"), Benchmark]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsyncWithTVPUsingClone_SucceedOnThirdAttempt()
        {
            TokenValidationResult result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters.Clone()).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters.Clone()).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _tokenValidationParameters.Clone()).ConfigureAwait(false);

            return result;
        }

        [BenchmarkCategory("ValidateTokenAsync_FailTwiceBeforeSuccess"), Benchmark]
        public async Task<bool> JsonWebTokenHandler_ValidateTokenAsyncWithVP_SucceedOnThirdAttempt()
        {
            ValidationResult<ValidatedToken, ValidationError> validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidValidationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidValidationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _validationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);

            return validationResult.Succeeded;
        }

        [BenchmarkCategory("ValidateTokenAsync_FailFourTimesBeforeSuccess"), Benchmark(Baseline = true)]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsyncWithTVP_SucceedOnFifthAttempt()
        {
            TokenValidationResult result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _tokenValidationParameters).ConfigureAwait(false);

            return result;
        }

        [BenchmarkCategory("ValidateTokenAsync_FailFourTimesBeforeSuccess"), Benchmark]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsyncWithTVPUsingClone_SucceedOnFifthAttempt()
        {
            TokenValidationResult result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters.Clone()).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters.Clone()).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters.Clone()).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidTokenValidationParameters.Clone()).ConfigureAwait(false);
            result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _tokenValidationParameters.Clone()).ConfigureAwait(false);

            return result;
        }

        [BenchmarkCategory("ValidateTokenAsync_FailFourTimesBeforeSuccess"), Benchmark]
        public async Task<bool> JsonWebTokenHandler_ValidateTokenAsyncWithVP_SucceedOnFifthAttempt()
        {
            ValidationResult<ValidatedToken, ValidationError> validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidValidationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidValidationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidValidationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _invalidValidationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _validationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);

            return validationResult.Succeeded;
        }

        [BenchmarkCategory("ValidateTokenAsyncClaimAccess"), Benchmark(Baseline = true)]
        public async Task<List<Claim>> JsonWebTokenHandler_ValidateTokenAsyncWithTVP_CreateClaims()
        {
            var result = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _tokenValidationParameters).ConfigureAwait(false);
            var claimsIdentity = result.ClaimsIdentity;
            var claims = claimsIdentity.Claims;
            return claims.ToList();
        }

        [BenchmarkCategory("ValidateTokenAsyncClaimAccess"), Benchmark]
        public async Task<List<Claim>> JsonWebTokenHandler_ValidateTokenAsyncWithVP_CreateClaims()
        {
            ValidationResult<ValidatedToken, ValidationError> validationResult = await _jsonWebTokenHandler.ValidateTokenAsync(_jwsExtendedClaims, _validationParameters, _callContext, CancellationToken.None).ConfigureAwait(false);
            var claimsIdentity = validationResult.Result.ClaimsIdentity;
            var claims = claimsIdentity.Claims;
            return claims.ToList();
        }
    }

    // ===== Telemetry Impact Benchmarks =====
    // "Tracking" in this context refers to tracking signature validation for specific issuer hosts.
    // When enabled, telemetry collects data for the configured hosts (e.g., "contoso.com").
    // Every other host is reported as "other" to avoid excessive cardinality in telemetry.
    // These benchmarks measure the performance impact of enabling telemetry overall, and of tracking specific hosts.
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class ValidateTokenAsyncTests_TelemetryImpact
    {
        private const int IterationCount = 10000;
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private string _jwsClaims;
        private TokenValidationParameters _tokenValidationParameters;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var tokenDescriptorClaims = new SecurityTokenDescriptor
            {
                Claims = BenchmarkUtils.Claims,
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
            };

            _jsonWebTokenHandler = new JsonWebTokenHandler();
            _jwsClaims = _jsonWebTokenHandler.CreateToken(tokenDescriptorClaims);

            _tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = BenchmarkUtils.Audience,
                ValidateLifetime = true,
                ValidIssuer = BenchmarkUtils.Issuer,
                IssuerSigningKey = BenchmarkUtils.SigningCredentialsRsaSha256.Key,
            };
        }

        [IterationSetup(Target = nameof(JsonWebTokenHandler_ValidateTokenAsync_TelemetryDisabled))]
        public void Setup_TelemetryDisabled()
        {
            CryptoTelemetry.EnableSignatureValidationTelemetry(false, null);
        }

        [BenchmarkCategory("ValidateTokenAsync_TelemetryImpact"), Benchmark(Baseline = true, OperationsPerInvoke = IterationCount)]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsync_TelemetryDisabled()
        {
            return await _jsonWebTokenHandler.ValidateTokenAsync(_jwsClaims, _tokenValidationParameters).ConfigureAwait(false);
        }

        [IterationSetup(Target = nameof(JsonWebTokenHandler_ValidateTokenAsync_TelemetryEnabledNoTracking))]
        public void Setup_TelemetryEnabledNoTracking()
        {
            CryptoTelemetry.EnableSignatureValidationTelemetry(true, null);
        }

        [BenchmarkCategory("ValidateTokenAsync_TelemetryImpact"), Benchmark(OperationsPerInvoke = IterationCount)]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsync_TelemetryEnabledNoTracking()
        {
            return await _jsonWebTokenHandler.ValidateTokenAsync(_jwsClaims, _tokenValidationParameters).ConfigureAwait(false);
        }

        [IterationSetup(Target = nameof(JsonWebTokenHandler_ValidateTokenAsync_TelemetryEnabledWithTracking))]
        public void Setup_TelemetryEnabledWithTracking()
        {
            CryptoTelemetry.EnableSignatureValidationTelemetry(true, new[] { "contoso.com" });
        }

        [BenchmarkCategory("ValidateTokenAsync_TelemetryImpact"), Benchmark(OperationsPerInvoke = IterationCount)]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsync_TelemetryEnabledWithTracking()
        {
            return await _jsonWebTokenHandler.ValidateTokenAsync(_jwsClaims, _tokenValidationParameters).ConfigureAwait(false);
        }
    }
}
