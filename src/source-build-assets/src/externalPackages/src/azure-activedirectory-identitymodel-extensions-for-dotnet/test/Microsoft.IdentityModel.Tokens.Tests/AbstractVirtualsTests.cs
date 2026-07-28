// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// This class tests abstract virtual methods that were added to abstract classes as virtuals and throw NotImplementedException to ensure the exception message makes sense.
    /// </summary>
    public class AbstractVirtualsTests
    {
        #region BaseConfigurationManager
        [Fact]
        public async Task BaseConfigurationManager_GetBaseConfigurationAsync()
        {
            TestUtilities.WriteHeader($"{this}.BaseConfigurationManager_GetBaseConfigurationAsync");

            try
            {
                await new DerivedBaseConfigurationManager().GetBaseConfigurationAsync(default);
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'public virtual Task<BaseConfiguration> GetBaseConfigurationAsync(CancellationToken cancel)'", ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }
        #endregion

        #region SignatureProvider
        [Fact]
        public void SignatureProvider_Sign()
        {
            TestUtilities.WriteHeader($"{this}.SignatureProvider_Sign");

            try
            {
                new DerivedSignatureProvider(KeyingMaterial.RsaSecurityKey1, "RS256").Sign(new byte[1], 0, 0);
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'public virtual byte[] Sign(byte[] input, int offset, int count)'", ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void SignatureProvider_Sign_Offset()
        {
            TestUtilities.WriteHeader($"{this}.SignatureProvider_Sign_Offset");

            try
            {
                new DerivedSignatureProvider(KeyingMaterial.RsaSecurityKey1, "RS256").Sign((new byte[1]).AsSpan(), (new byte[1]).AsSpan(), out int bytesOut);
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'public virtual bool Sign(ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten)'", ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }
#endif

        [Fact]
        public void SignatureProvider_Verify_Offset()
        {
            TestUtilities.WriteHeader($"{this}.SignatureProvider_Verify_Offset");

            try
            {
                new DerivedSignatureProvider(KeyingMaterial.RsaSecurityKey1, "RS256").Verify(new byte[1], 0, 0, new byte[1], 0, 0);
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'public virtual bool " +
                    "Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength)'",
                    ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }
        #endregion

        #region TokenHandler
        [Fact]
        public void TokenHandler_ReadToken()
        {
            TestUtilities.WriteHeader($"{this}.TokenHandler_ReadToken");

            try
            {
                new DerivedTokenHandler().ReadToken("");
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'public virtual SecurityToken ReadToken(string token)'", ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }

        [Fact]
        public void TokenHandler_CreateClaimsIdentityInternal()
        {
            TestUtilities.WriteHeader($"{this}.TokenHandler_CreateClaimsIdentityInternal");

            try
            {
                new DerivedTokenHandler().CreateClaimsIdentityInternal(new DerivedSecurityToken(), new TokenValidationParameters(), "");
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'internal virtual ClaimsIdentity " +
                    "CreateClaimsIdentityInternal(SecurityToken securityToken, TokenValidationParameters tokenValidationParameters, string issuer)'",
                    ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }
        [Fact]
        public async Task TokenHandler_ValidateTokenAsyncString()
        {
            TestUtilities.WriteHeader($"{this}.TokenHandler_ValidateTokenAsyncString");

            try
            {
                await new DerivedTokenHandler().ValidateTokenAsync("token", new TokenValidationParameters());
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'public virtual Task<TokenValidationResult> " +
                    "ValidateTokenAsync(string token, TokenValidationParameters validationParameters)'",
                    ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }

        [Fact]
        public async Task TokenHandler_ValidateTokenAsyncToken()
        {
            TestUtilities.WriteHeader($"{this}.TokenHandler_ValidateTokenAsyncToken");

            try
            {
                await new DerivedTokenHandler().ValidateTokenAsync(new DerivedSecurityToken(), new TokenValidationParameters());
            }
            catch (Exception ex)
            {
                Assert.Contains("IDX10267: 'public virtual Task<TokenValidationResult> " +
                    "ValidateTokenAsync(SecurityToken token, TokenValidationParameters validationParameters)'",
                    ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }

        [Fact]
        public async Task TokenHandler_ValidationParameters_ValidateTokenAsyncString()
        {
            TestUtilities.WriteHeader($"{this}.TokenHandler_ValidationParameters_ValidateTokenAsyncString");

            try
            {
                await new DerivedTokenHandler().ValidateTokenAsync(
                    "token",
                    new ValidationParameters(),
                    new CallContext(),
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                Assert.Contains("internal virtual Task<OperationResult<ValidatedToken, ValidationError>> " +
                        "ValidateTokenAsync(string token, ValidationParameters validationParameters, CallContext callContext, CancellationToken cancellationToken)",
                        ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }

        [Fact]
        public async Task TokenHandler_ValidationParameters_ValidateTokenAsyncToken()
        {
            TestUtilities.WriteHeader($"{this}.TokenHandler_ValidationParameters_ValidateTokenAsyncToken");

            try
            {
                await new DerivedTokenHandler().ValidateTokenAsync(
                    new DerivedSecurityToken(),
                    new ValidationParameters(),
                    new CallContext(),
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                Assert.Contains("internal virtual Task<OperationResult<ValidatedToken, ValidationError>> " +
                        "ValidateTokenAsync(SecurityToken token, ValidationParameters validationParameters, CallContext callContext, CancellationToken cancellationToken)",
                        ex.Message);

                Assert.IsAssignableFrom<NotImplementedException>(ex);
            }
        }
        #endregion
    }

    public class DerivedBaseConfigurationManager : BaseConfigurationManager
    {
        public override void RequestRefresh() => throw new NotImplementedException();
    }

    public class DerivedSignatureProvider : SignatureProvider
    {
        public DerivedSignatureProvider(SecurityKey key, string algorithm) : base(key, algorithm)
        {
        }

        protected override void Dispose(bool disposing) => throw new NotImplementedException();

        public override byte[] Sign(byte[] input) => throw new NotImplementedException();

        public override bool Verify(byte[] input, byte[] signature) => throw new NotImplementedException();
    }

    public class DerivedTokenHandler : TokenHandler
    {
    }
}
