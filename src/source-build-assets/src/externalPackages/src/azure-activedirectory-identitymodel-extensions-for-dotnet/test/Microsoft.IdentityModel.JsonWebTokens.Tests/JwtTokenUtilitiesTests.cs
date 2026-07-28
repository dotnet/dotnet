// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Moq;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JwtTokenUtilitiesTests
    {
        // Used for formatting a message for testing with one parameter.
        private const string TestMessageOneParam = "This is the parameter: '{0}'.";
        private static readonly SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricSecurityKey_128.Key)
        {
            KeyId = KeyingMaterial.DefaultSymmetricSecurityKey_256.KeyId,
        };

        [Fact]
        public void LogSecurityArtifactTest()
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Error;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Error);

            var jweTokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                Claims = Default.PayloadDictionary
            };

            var jwsTokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims)
            };

            string stringJwe = new JsonWebTokenHandler().CreateToken(jweTokenDescriptor);
            JsonWebToken jwe = new JsonWebToken(stringJwe);
            string stringJws = new JsonWebTokenHandler().CreateToken(jwsTokenDescriptor);
            JsonWebToken jws = new JsonWebToken(stringJws);

            // LogExceptionMessage should not log the jwe since ShowPII is false.
            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, jwe)));
            Assert.DoesNotContain(jwe.EncodedToken, listener.TraceBuffer);

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, jwe)));
            Assert.DoesNotContain(jwe.EncodedToken, listener.TraceBuffer);

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, LogHelper.MarkAsSecurityArtifact(stringJwe, JwtTokenUtilities.SafeLogJwtToken))));
            Assert.DoesNotContain(stringJws, listener.TraceBuffer);

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, LogHelper.MarkAsSecurityArtifact(stringJwe, JwtTokenUtilities.SafeLogJwtToken))));
            Assert.DoesNotContain(stringJws, listener.TraceBuffer);

            // LogExceptionMessage should log the masked jwe since ShowPII is true but LogCompleteSecurityArtifact is false.
            IdentityModelEventSource.ShowPII = true;

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, jwe)));
            Assert.DoesNotContain(jwe.EncodedToken.Substring(0, jwe.EncodedToken.LastIndexOf(".")), listener.TraceBuffer);
            Assert.DoesNotContain(jwe.AuthenticationTag, listener.TraceBuffer);
            Assert.Contains(
                string.Format(IdentityModelEventSource.HiddenSecurityArtifactString, jwe.GetType().ToString()),
                listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, jws)));
            Assert.DoesNotContain(jws.EncodedToken.Substring(0, jws.EncodedToken.LastIndexOf(".")), listener.TraceBuffer);
            Assert.DoesNotContain(jws.EncodedSignature, listener.TraceBuffer);
            Assert.Contains(
                string.Format(IdentityModelEventSource.HiddenSecurityArtifactString, jws.GetType().ToString()),
                listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;

            var sa = LogHelper.MarkAsSecurityArtifact(stringJwe, JwtTokenUtilities.SafeLogJwtToken);
            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, sa)));
            Assert.DoesNotContain(stringJwe.Substring(0, stringJwe.LastIndexOf(".")), listener.TraceBuffer);
            Assert.DoesNotContain(stringJwe.Substring(stringJwe.LastIndexOf(".")), listener.TraceBuffer);
            Assert.Contains(
                string.Format(IdentityModelEventSource.HiddenSecurityArtifactString, sa.GetType().ToString()),
                listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;

            sa = LogHelper.MarkAsSecurityArtifact(stringJws, JwtTokenUtilities.SafeLogJwtToken);
            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, sa)));
            Assert.DoesNotContain(stringJws.Substring(0, stringJws.LastIndexOf(".")), listener.TraceBuffer);
            Assert.DoesNotContain(stringJws.Substring(stringJws.LastIndexOf(".")), listener.TraceBuffer);
            Assert.Contains(
                string.Format(IdentityModelEventSource.HiddenSecurityArtifactString, sa.GetType().ToString()),
                listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;

            // LogExceptionMessage should log the jwe since CompleteSecurityArtifact is true.
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, jwe)));
            Assert.Contains(jwe.EncodedToken, listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, jws)));
            Assert.Contains(jws.EncodedToken, listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, LogHelper.MarkAsSecurityArtifact(
                stringJwe,
                JwtTokenUtilities.SafeLogJwtToken,
                t => t.ToString()))));
            Assert.Contains(stringJwe, listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;

            LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TestMessageOneParam, LogHelper.MarkAsSecurityArtifact(
                stringJws,
                JwtTokenUtilities.SafeLogJwtToken,
                t => t.ToString()))));
            Assert.Contains(stringJws, listener.TraceBuffer);
            listener.TraceBuffer = string.Empty;
        }

        [Fact]
        public void ResolveTokenSigningKey()
        {
            var testKeyId = Guid.NewGuid().ToString();
            var tvp = new TokenValidationParameters();

            // null configuration
            var resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, null, tvp, null);
            Assert.Null(resolvedKey);

            // null tvp
            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, null, null, null);
            Assert.Null(resolvedKey);

            var signingKey = new X509SecurityKey(KeyingMaterial.CertSelfSigned1024_SHA256);
            signingKey.KeyId = testKeyId;
            tvp.IssuerSigningKey = signingKey;

            #region KeyId

            // signingKey.KeyId matches TVP.IssuerSigningKey
            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, null, tvp, null);
            Assert.NotNull(resolvedKey);
            Assert.Same(resolvedKey, tvp.IssuerSigningKey);

            // signingKey.KeyId matched, TVP.IssuerSigningKeys
            tvp.IssuerSigningKey = null;
            tvp.IssuerSigningKeys = new List<SecurityKey>() { signingKey };

            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, Base64UrlEncoder.Encode(testKeyId), tvp, null);
            Assert.NotNull(resolvedKey);
            Assert.Same(resolvedKey, tvp.IssuerSigningKeys.First());

            // signingKey.KeyId matches configuration.SigningKeys.First()
            tvp.IssuerSigningKey = null;
            tvp.IssuerSigningKeys = null;
            var configuration = GetConfigurationMock();
            var testSigningKey = configuration.SigningKeys.First();

            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testSigningKey.KeyId, string.Empty, tvp, configuration);
            Assert.Same(resolvedKey, testSigningKey);

            #endregion

            #region X5t

            // signingKey.X5t matches TVP.IssuerSigningKey
            signingKey.KeyId = Guid.NewGuid().ToString();
            tvp.IssuerSigningKey = signingKey;
            tvp.IssuerSigningKeys = null;

            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, signingKey.X5t, tvp, null);
            Assert.Same(resolvedKey, tvp.IssuerSigningKey);

            // signingKey.X5t matches tvp.IssuerSigningKey since X509SecurityKey comparison is case-insensitive
            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, signingKey.X5t.ToUpper(), tvp, null);
            Assert.Same(resolvedKey, tvp.IssuerSigningKey);

            // signingKey.X5t matches TVP.IssuerSigningKeys.First()
            signingKey.KeyId = Guid.NewGuid().ToString();
            tvp.IssuerSigningKey = null;
            tvp.IssuerSigningKeys = new List<SecurityKey>() { signingKey };

            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, signingKey.X5t, tvp, null);
            Assert.Same(resolvedKey, tvp.IssuerSigningKeys.First());

            // signingKey.X5t matches configuration.SigningKeys.First()
            signingKey.KeyId = Guid.NewGuid().ToString();
            tvp.IssuerSigningKey = null;
            tvp.IssuerSigningKeys = null;
            configuration = GetConfigurationMock();

            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(testKeyId, signingKey.X5t, tvp, configuration);
            Assert.Same(resolvedKey, configuration.SigningKeys.First());

            #endregion

            // no signing key resolved
            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), tvp, null);
            Assert.Null(resolvedKey);

            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(null, null, tvp, null);
            Assert.Null(resolvedKey);

            resolvedKey = JwtTokenUtilities.ResolveTokenSigningKey(null, null, tvp, GetConfigurationNoMatchingKeyMock());
            Assert.Null(resolvedKey);
        }

        #region DecryptJwtToken Tests
        [Fact]
        public void DecryptJwtToken_WhenValidationParametersIsNull_ThrowsException()
        {
            // Arrange
            var expectedExceptionParamName = "validationParameters";
            TokenValidationParameters validationParameters = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                JwtTokenUtilities.DecryptJwtToken(
                    securityToken: null,
                    validationParameters: validationParameters,
                    decryptionParameters: null));

            Assert.Equal(expectedExceptionParamName, exception.ParamName);
        }

        [Fact]
        public void DecryptJwtToken_WhenJwtTokenDecryptionParametersIsNull_ThrowsException()
        {
            // Arrange
            var expectedExceptionParamName = "decryptionParameters";
            var validationParameters = new TokenValidationParameters();
            JwtTokenDecryptionParameters decryptionParameters = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                JwtTokenUtilities.DecryptJwtToken(
                    securityToken: null,
                    validationParameters: validationParameters,
                    decryptionParameters: decryptionParameters));

            Assert.Equal(expectedExceptionParamName, exception.ParamName);
        }

        [Fact]
        public void DecryptJwtToken_WhenCryptoProviderFactoryIsNull_ThrowsException()
        {
            // Arrange
            var securityKey = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricSecurityKey_128.Key)
            {
                KeyId = KeyingMaterial.DefaultSymmetricSecurityKey_256.KeyId,
            };

            // Get the private _cryptoProviderFactory field and set it to null.
            var fieldInfo = typeof(SecurityKey).GetField("_cryptoProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(securityKey, null);

            using var listener = new SampleListener();
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Warning;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Warning);

            var keys = new List<SecurityKey>()
            {
                securityKey,
            };

            var validationParameters = new TokenValidationParameters()
            {
                CryptoProviderFactory = null,
            };
            var decryptionParameters = new JwtTokenDecryptionParameters
            {
                Keys = keys,
            };

            var expectedWarning = LogHelper.FormatInvariant(
                Tokens.LogMessages.IDX10607,
                LogHelper.MarkAsNonPII(securityKey.KeyId));
            var expectedExceptionMessage = new MessageDetail(
                Tokens.LogMessages.IDX10609,
                LogHelper.MarkAsSecurityArtifact(decryptionParameters.EncodedToken,
                JwtTokenUtilities.SafeLogJwtToken)).Message;

            // Act & Assert
            var exception = Assert.Throws<SecurityTokenDecryptionFailedException>(() =>
                JwtTokenUtilities.DecryptJwtToken(
                    securityToken: null,
                    validationParameters: validationParameters,
                    decryptionParameters: decryptionParameters));

            Assert.Contains(expectedWarning, listener.TraceBuffer);
            Assert.Contains(expectedExceptionMessage, listener.TraceBuffer);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void DecryptJwtToken_WhenEncIsNotSupported_ThrowsException()
        {
            // Arrange
            var securityKey = symmetricSecurityKey;
            using var listener = new SampleListener();
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Warning;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Warning);

            var keys = new List<SecurityKey>()
            {
                securityKey,
            };
            var securityToken = new JwtSecurityToken();
            var decryptionParameters = new JwtTokenDecryptionParameters
            {
                Alg = "an algorithm",
                Enc = "unsupported-algorithm",
                Keys = keys,
            };

            var mockCryptoProviderFactory = new Mock<CryptoProviderFactory>();
            mockCryptoProviderFactory
                .Setup(factory => factory.IsSupportedAlgorithm(It.IsAny<string>(), It.IsAny<SecurityKey>()))
                .Returns(false);

            var validationParameters = new TokenValidationParameters()
            {
                CryptoProviderFactory = mockCryptoProviderFactory.Object,
            };

            var expectedWarning = LogHelper.FormatInvariant(
                Tokens.LogMessages.IDX10611,
                LogHelper.MarkAsNonPII(decryptionParameters.Enc),
                LogHelper.MarkAsNonPII(securityKey.KeyId));
            var expectedExceptionMessage = new MessageDetail(
                        Tokens.LogMessages.IDX10619,
                        LogHelper.MarkAsNonPII(decryptionParameters.Alg),
                        LogHelper.MarkAsNonPII(decryptionParameters.Enc)).Message;

            // Act & Assert
            var exception = Assert.Throws<SecurityTokenDecryptionFailedException>(() =>
                JwtTokenUtilities.DecryptJwtToken(securityToken, validationParameters, decryptionParameters));

            Assert.Contains(expectedWarning, listener.TraceBuffer);
            Assert.Contains(expectedExceptionMessage, listener.TraceBuffer);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void DecryptJwtToken_WhenNoKeys_ThrowsException()
        {
            // Arrange
            using var listener = new SampleListener();
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Warning;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Warning);

            var securityToken = new JwtSecurityToken();
            var decryptionParameters = new JwtTokenDecryptionParameters()
            {
                Keys = new List<SecurityKey>(),
            };

            var mockCryptoProviderFactory = new Mock<CryptoProviderFactory>();
            mockCryptoProviderFactory
                .Setup(factory => factory.IsSupportedAlgorithm(It.IsAny<string>(), It.IsAny<SecurityKey>()))
                .Returns(false);

            var validationParameters = new TokenValidationParameters()
            {
                CryptoProviderFactory = mockCryptoProviderFactory.Object,
            };

            var expectedExceptionMessage = new MessageDetail(
                        Tokens.LogMessages.IDX10609,
                        LogHelper.MarkAsSecurityArtifact(decryptionParameters.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)).Message;

            // Act & Assert
            var exception = Assert.Throws<SecurityTokenDecryptionFailedException>(() =>
                JwtTokenUtilities.DecryptJwtToken(securityToken, validationParameters, decryptionParameters));

            Assert.Contains(expectedExceptionMessage, listener.TraceBuffer);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void DecryptJwtToken_WhenDecryptionFails_ThrowsException()
        {
            // Arrange
            var securityKey = NotDefault.SymmetricSigningKey256;
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jweCreatedInMemory = jsonWebTokenHandler.CreateToken(
                Default.PayloadString,
                Default.SymmetricSigningCredentials,
                Default.SymmetricEncryptingCredentials);
            var securityToken = new JsonWebToken(jweCreatedInMemory);

            using var listener = new SampleListener();
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Warning;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Warning);

            var keys = new List<SecurityKey>()
            {
                securityKey,
            };
            var decryptionParameters = new JwtTokenDecryptionParameters
            {
                Alg = securityToken.Alg,
                AuthenticationTagBytes = securityToken.AuthenticationTagBytes,
                CipherTextBytes = securityToken.CipherTextBytes,
                DecompressionFunction = JwtTokenUtilities.DecompressToken,
                Enc = securityToken.Enc,
                EncodedToken = securityToken.EncodedToken,
                HeaderAsciiBytes = securityToken.HeaderAsciiBytes,
                InitializationVectorBytes = securityToken.InitializationVectorBytes,
                MaximumDeflateSize = jsonWebTokenHandler.MaximumTokenSizeInBytes,
                Keys = keys,
                Zip = securityToken.Zip,
            };

            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = Default.SymmetricSigningKey256,
                TokenDecryptionKey = securityKey,
            };

            var keysAttempted = new StringBuilder().AppendLine(validationParameters.TokenDecryptionKey.KeyId);
            var incompleteExceptionMessage = new MessageDetail(
                        Tokens.LogMessages.IDX10603,
                        LogHelper.MarkAsNonPII(keysAttempted.ToString()),
                        string.Empty,   // Using empty since actual exception contains file paths, which are machine specific.
                        LogHelper.MarkAsSecurityArtifact(decryptionParameters.EncodedToken, JwtTokenUtilities.SafeLogJwtToken)).Message;
            // Get partial messages as the actual exception message contains file paths.
            var partialExceptionMessage = incompleteExceptionMessage.Substring(
                0,
                incompleteExceptionMessage.IndexOf(securityKey.KeyId) + securityKey.KeyId.Length);

            // Act & Assert
            var exception = Assert.Throws<SecurityTokenDecryptionFailedException>(() =>
                JwtTokenUtilities.DecryptJwtToken(securityToken, validationParameters, decryptionParameters));

            Assert.Contains(partialExceptionMessage, listener.TraceBuffer);
            Assert.Contains(partialExceptionMessage, exception.Message);
        }

        [Theory, MemberData(nameof(DecompressionFailTheoryData), DisableDiscoveryEnumeration = true)]
        public void DecryptJwtToken_WhenDecompressionFails_ThrowsException(DecompressionFailureTheoryData theoryData)
        {
            // Arrange
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
            var securityToken = new JsonWebToken(theoryData.JwtEncodedString);

            using var listener = new SampleListener();
            IdentityModelEventSource.ShowPII = false;
            IdentityModelEventSource.Logger.LogLevel = EventLevel.Warning;
            listener.EnableEvents(IdentityModelEventSource.Logger, EventLevel.Warning);

            var kwp = Default.JWECompressionTokenValidationParameters.TokenDecryptionKey.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(Default.JWECompressionTokenValidationParameters.TokenDecryptionKey, securityToken.Alg);
            var unwrappedKey = kwp.UnwrapKey(securityToken.EncryptedKeyBytes);

            var keys = new List<SecurityKey>()
            {
                new SymmetricSecurityKey(unwrappedKey),
            };
            var decryptionParameters = new JwtTokenDecryptionParameters
            {
                Alg = securityToken.Alg,
                AuthenticationTagBytes = securityToken.AuthenticationTagBytes,
                CipherTextBytes = securityToken.CipherTextBytes,
                DecompressionFunction = JwtTokenUtilities.DecompressToken,
                Enc = securityToken.Enc,
                EncodedToken = securityToken.EncodedToken,
                HeaderAsciiBytes = securityToken.HeaderAsciiBytes,
                InitializationVectorBytes = securityToken.InitializationVectorBytes,
                MaximumDeflateSize = jsonWebTokenHandler.MaximumTokenSizeInBytes,
                Keys = keys,
                Zip = securityToken.Zip,
            };
            var validationParameters = Default.JWECompressionTokenValidationParameters;

            var exceptionMessage = LogHelper.FormatInvariant(Tokens.LogMessages.IDX10679, LogHelper.MarkAsNonPII(decryptionParameters.Zip));
            string innerExceptionMessage = string.Empty;
            if (theoryData.ValidateInnerExceptionMessage)
                innerExceptionMessage = LogHelper.FormatInvariant(theoryData.InnerExceptionMessageId, LogHelper.MarkAsNonPII(decryptionParameters.Zip));

            // Act & Assert
            var exception = Assert.Throws<SecurityTokenDecompressionFailedException>(() =>
                JwtTokenUtilities.DecryptJwtToken(securityToken, validationParameters, decryptionParameters));

            Assert.Contains(exceptionMessage, listener.TraceBuffer);
            Assert.Contains(exceptionMessage, exception.Message);
            Assert.Equal(theoryData.InnerExceptionType, exception.InnerException.GetType());
            if (theoryData.ValidateInnerExceptionMessage)
            {
                Assert.Contains(innerExceptionMessage, listener.TraceBuffer);
                Assert.Contains(innerExceptionMessage, exception.InnerException.Message);
            }
        }

        public static TheoryData<DecompressionFailureTheoryData> DecompressionFailTheoryData()
        {
            var compressionProviderFactoryForCustom2 = new CompressionProviderFactory()
            {
                CustomCompressionProvider = new SampleCustomCompressionProviderDecompressAndCompressAlwaysFail("MyAlgorithm")
            };

            return new TheoryData<DecompressionFailureTheoryData>() {
                new DecompressionFailureTheoryData
                {
                    TestId = "NotSupportedAlgorithm",
                    JwtEncodedString = ReferenceTokens.JWECompressionTokenWithUnsupportedAlgorithm,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    InnerExceptionType = typeof(NotSupportedException),
                    ValidateInnerExceptionMessage = true,
                    InnerExceptionMessageId = Tokens.LogMessages.IDX10682,
                },
                new DecompressionFailureTheoryData
                {
                    TestId = "InvalidToken",
                    JwtEncodedString = ReferenceTokens.JWEInvalidCompressionTokenWithDEF,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    InnerExceptionType = typeof(InvalidDataException),
                    ValidateInnerExceptionMessage = false,
                },
                new DecompressionFailureTheoryData
                {
                    TestId = "NotSupportedAlgorithmFromCustomCompressionProvider",
                    JwtEncodedString = ReferenceTokens.JWECompressionTokenWithDEF,
                    CompressionProviderFactory = compressionProviderFactoryForCustom2,
                    InnerExceptionType = typeof(SecurityTokenDecompressionFailedException),
                    ValidateInnerExceptionMessage = true,
                    InnerExceptionMessageId = Tokens.LogMessages.IDX10679,
                }
            };
        }

        public class DecompressionFailureTheoryData : TheoryDataBase
        {
            public string JwtEncodedString;
            public CompressionProviderFactory CompressionProviderFactory;
            public Type InnerExceptionType;
            public bool ValidateInnerExceptionMessage;
            public string InnerExceptionMessageId;
        }
        #endregion

        private BaseConfiguration GetConfigurationMock()
        {
            var config = new OpenIdConnectConfiguration();
            config.SigningKeys.Add(KeyingMaterial.X509SecurityKeySelfSigned1024_SHA256_Public);
            config.SigningKeys.Add(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA384_Public);

            return config;
        }

        private BaseConfiguration GetConfigurationNoMatchingKeyMock()
        {
            var config = new OpenIdConnectConfiguration();
            config.SigningKeys.Add(KeyingMaterial.DefaultRsaSecurityKey1);
            config.SigningKeys.Add(KeyingMaterial.DefaultRsaSecurityKey2);

            return config;
        }
    }
}
