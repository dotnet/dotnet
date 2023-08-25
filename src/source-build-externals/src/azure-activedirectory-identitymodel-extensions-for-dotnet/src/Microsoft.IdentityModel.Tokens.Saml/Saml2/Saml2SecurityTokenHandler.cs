// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Saml;
using static Microsoft.IdentityModel.Logging.LogHelper;

using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Saml2 Tokens. See: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2SecurityTokenHandler : SecurityTokenHandler
    {
        private const string _actor = "Actor";
        private const string _className = "Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler";
        private Saml2Serializer _serializer = new Saml2Serializer();
        private string _actorClaimName = DefaultActorClaimName;

        /// <summary>
        /// Default value of the Actor Claim Name used when processing actor claims.
        /// </summary>
        public static string DefaultActorClaimName = ClaimTypes.Actor;

        /// <summary>
        /// Gets or set the <see cref="Saml2Serializer"/> that will be used to read and write a <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">'value' is null.</exception>
        public Saml2Serializer Serializer
        {
            get { return _serializer; }
            set { _serializer = value ?? throw LogHelper.LogArgumentNullException(nameof(value)); }
        }

        /// <summary>
        /// Gets or set the actor claim attribute name that will be used when processing actor claims.
        /// </summary>
        public string ActorClaimName
        {
            get { return _actorClaimName; }
            set { _actorClaimName = string.IsNullOrWhiteSpace(value) ? throw LogHelper.LogArgumentNullException(nameof(value)) : value; }
        }

        /// <summary>
        /// Returns a value that indicates if this handler can validate a <see cref="SecurityToken"/>.
        /// </summary>
        /// <returns>'true', indicating this instance can validate a <see cref="Saml2SecurityToken"/>.</returns>
        public override bool CanValidateToken
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the token type supported by this handler.
        /// </summary>
        public override Type TokenType
        {
            get { return typeof(Saml2SecurityToken); }
        }

        /// <summary>
        /// Gets the value that indicates if this instance can write a <see cref="SecurityToken"/>.
        /// </summary>
        /// <returns>'true', indicating this instance can write a <see cref="Saml2SecurityToken"/>.</returns>
        public override bool CanWriteToken
        {
            get { return true; }
        }

        /// <summary>
        /// Determines if the string is a valid Saml2 token by examining the xml for the correct start element.
        /// </summary>
        /// <param name="token">A Saml2 token as a string.</param>
        /// <returns>'true' if the string has a start element equal <see cref="Saml2Constants.Elements.Assertion"/>.</returns>
        public override bool CanReadToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (token.Length > MaximumTokenSizeInBytes)
                return false;

            try
            {
                using (var sr = new StringReader(token))
                {
                    var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit };
                    using (var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(sr, settings)))
                    {
                        return CanReadToken(reader);
                    }
                }
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates whether the current reader is positioned at a Saml2 assertion.
        /// </summary>
        /// <param name="reader">An <see cref="XmlReader"/> reader positioned at a start element. The reader should not be advanced.</param>
        /// <returns>'true' if a token can be read.</returns>
        public override bool CanReadToken(XmlReader reader)
        {
            if (reader == null)
                return false;

            return reader.IsStartElement(Saml2Constants.Elements.Assertion, Saml2Constants.Namespace);
        }

        /// <summary>
        /// Creates a <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> that has creation information.</param>
        /// <returns>A <see cref="SecurityToken"/> instance.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="tokenDescriptor"/> is null.</exception>
        public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            return CreateToken(tokenDescriptor, null);
        }

        /// <summary>
        /// Creates a <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> that has creation information.</param>
        /// <param name="authenticationInformation">additional information for creating a <see cref="Saml2AuthenticationStatement"/>.</param>
        /// <returns>A <see cref="SecurityToken"/> instance.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="tokenDescriptor"/> is null.</exception>
        public virtual SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor, AuthenticationInformation authenticationInformation)
        {
            if (tokenDescriptor == null)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            // Assertion/issuer
            var assertion = new Saml2Assertion(CreateIssuerNameIdentifier(tokenDescriptor))
            {
                Subject = CreateSubject(tokenDescriptor),
                SigningCredentials = tokenDescriptor.SigningCredentials,
                Conditions = CreateConditions(tokenDescriptor),
                Advice = CreateAdvice(tokenDescriptor)
            };

            if (tokenDescriptor.IssuedAt.HasValue)
                assertion.IssueInstant = tokenDescriptor.IssuedAt.Value;

            // Statements
            IEnumerable<Saml2Statement> statements = CreateStatements(tokenDescriptor, authenticationInformation);
            if (statements != null)
            {
                foreach (var statement in statements)
                {
                    assertion.Statements.Add(statement);
                }
            }

            return new Saml2SecurityToken(assertion);
        }

        /// <inheritdoc/>
        public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            try
            {
                var claimsPrincipal = ValidateToken(token, validationParameters, out var validatedToken);
                return Task.FromResult(new TokenValidationResult
                {
                    SecurityToken = validatedToken,
                    ClaimsIdentity = claimsPrincipal?.Identities.First(),
                    IsValid = true,
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new TokenValidationResult
                {
                    IsValid = false,
                    Exception = ex
                });
            }
        }

        /// <summary>
        /// Reads and validates a <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> reader positioned at a saml2 assertion element.</param>
        /// <param name="validationParameters">Contains validation parameters for the <see cref="Saml2SecurityToken"/>.</param>
        /// <param name="validatedToken">The <see cref="Saml2SecurityToken"/> that was validated.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenReadException">if the token is not well-formed.</exception>
        /// <exception cref="SecurityTokenValidationException">if <see cref="Saml2Serializer.ReadAssertion(XmlReader)"/> returns null.</exception>
        /// <returns>A <see cref="ClaimsPrincipal"/> representing the identity contained in the token.</returns>
        public override ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            var samlToken = ReadSaml2Token(reader);
            if (samlToken == null)
                throw LogExceptionMessage(
                    new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateToken"), LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ReadSaml2Token"), LogHelper.MarkAsNonPII(typeof(Saml2Assertion)))));

            ValidateSignature(samlToken, samlToken.Assertion.CanonicalString, validationParameters);

            return ValidateToken(samlToken, samlToken.Assertion.CanonicalString, validationParameters, out validatedToken);
        }

        /// <summary>
        /// Reads and validates a <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <param name="token">The Saml2 token.</param>
        /// <param name="validationParameters">Contains validation parameters for the <see cref="Saml2SecurityToken"/>.</param>
        /// <param name="validatedToken">The <see cref="Saml2SecurityToken"/> that was validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="token"/>.Length is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <exception cref="Saml2SecurityTokenReadException">if the <paramref name="token"/> is not well-formed.</exception>
        /// <exception cref="SecurityTokenValidationException">if <see cref="ValidateSignature(string, TokenValidationParameters)"/> returns null.</exception>
        /// <returns>A <see cref="ClaimsPrincipal"/> representing the identity contained in the token.</returns>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            if (string.IsNullOrEmpty(token))
                throw LogArgumentNullException(nameof(token));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            if (token.Length > MaximumTokenSizeInBytes)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

            var samlToken = ValidateSignature(token, validationParameters);
            if (samlToken == null)
                throw LogExceptionMessage(
                    new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateToken"), LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateSignature"), LogHelper.MarkAsNonPII(typeof(Saml2SecurityToken)))));

            return ValidateToken(samlToken, token, validationParameters, out validatedToken);
        }

        private ClaimsPrincipal ValidateToken(Saml2SecurityToken samlToken, string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ValidateConditions(samlToken, validationParameters);
            ValidateSubject(samlToken, validationParameters);
            var issuer = ValidateIssuer(samlToken.Issuer, samlToken, validationParameters);

            if (samlToken.Assertion.Conditions != null)
                ValidateTokenReplay(samlToken.Assertion.Conditions.NotOnOrAfter, samlToken.Assertion.CanonicalString, validationParameters);

            ValidateIssuerSecurityKey(samlToken.SigningKey, samlToken, validationParameters);
            validatedToken = samlToken;
            var identity = CreateClaimsIdentity(samlToken, issuer, validationParameters);
            if (validationParameters.SaveSigninToken)
                identity.BootstrapContext = samlToken.Assertion.CanonicalString;

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(TokenLogMessages.IDX10241, token);

            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Validates the first SubjectConfirmationData
        /// </summary>
        /// <param name="samlToken">the Saml2 token that is being validated.</param>
        /// <param name="validationParameters">validation parameters.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/>.Assertion is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenException">If <see cref="Saml2SecurityToken.Assertion"/>.Subject is null.</exception>
        protected virtual void ValidateSubject(Saml2SecurityToken samlToken, TokenValidationParameters validationParameters)
        {
            if (samlToken == null)
                throw LogArgumentNullException(nameof(samlToken));

            if (samlToken.Assertion == null)
                throw LogArgumentNullException(nameof(samlToken.Assertion));

            if (samlToken.Assertion.Subject == null)
                throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13509));

            foreach (var subjectConfirmation in samlToken.Assertion.Subject.SubjectConfirmations)
            {
                if (subjectConfirmation != null && subjectConfirmation.SubjectConfirmationData != null)
                    ValidateConfirmationData(samlToken, validationParameters, subjectConfirmation.SubjectConfirmationData);
            }
        }

        /// <summary>
        /// Validates the <see cref="Saml2SecurityToken.SigningKey"/> is an expected value.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="Saml2SecurityToken"/> to validate.</param>
        /// <param name="validationParameters">The current <see cref="TokenValidationParameters"/>.</param>
        /// <remarks>If the <see cref="Saml2SecurityToken.SigningKey"/> is a <see cref="X509SecurityKey"/> then the X509Certificate2 will be validated using the CertificateValidator.</remarks>
        protected virtual void ValidateIssuerSecurityKey(SecurityKey key, Saml2SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateIssuerSecurityKey(key, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates the lifetime of a <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <param name="notBefore">The <see cref="DateTime"/> value found in the <see cref="Saml2SecurityToken"/>.</param>
        /// <param name="expires">The <see cref="DateTime"/> value found in the <see cref="Saml2SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="Saml2SecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <remarks><see cref="Validators.ValidateLifetime"/> for additional details.</remarks>
        protected virtual void ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateLifetime(notBefore, expires, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates token for replay.
        /// </summary>
        /// <param name="expirationTime">expiration time.</param>
        /// <param name="securityToken">the Saml2 token that is being validated.</param>
        /// <param name="validationParameters">validation parameters.</param>
        /// <remarks>By default no validation is performed. Validation requires that <see cref="TokenValidationParameters.TokenReplayCache"/> has been set.</remarks>
        protected virtual void ValidateTokenReplay(DateTime? expirationTime, string securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateTokenReplay(expirationTime, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates <see cref="Saml2SubjectConfirmationData"/> object.
        /// </summary>
        /// <param name="samlToken">the <see cref="Saml2SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">the <see cref="TokenValidationParameters"/> that will be used during validation.</param>
        /// <param name="confirmationData">The <see cref="Saml2SubjectConfirmationData"/> to validate.</param>
        /// <remarks>Validation of confirmation data is currently not supported by default. To customize SubjectConfirmationData processing, extend Saml2SecurityTokenHandler and override ValidateConfirmationData.</remarks>
        protected virtual void ValidateConfirmationData(Saml2SecurityToken samlToken, TokenValidationParameters validationParameters, Saml2SubjectConfirmationData confirmationData)
        {
            LogHelper.LogInformation(LogMessages.IDX13951);
        }

        /// <summary>
        /// Validates that the signature.
        /// </summary>
        /// <param name="token">A Saml2 token.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> that will be used during validation.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="SecurityTokenValidationException">If <see cref="ReadSaml2Token(string)"/> return null.</exception>
        /// <exception cref="SecurityTokenValidationException">If <see cref="TokenValidationParameters.SignatureValidator"/> returns null OR an object other than a <see cref="Saml2SecurityToken"/>.</exception>
        /// <exception cref="SecurityTokenValidationException">If a signature is not found and <see cref="TokenValidationParameters.RequireSignedTokens"/> is true.</exception>
        /// <exception cref="SecurityTokenSignatureKeyNotFoundException">If the  <paramref name="token"/> has a key identifier and none of the <see cref="SecurityKey"/>(s) provided result in a validated signature. 
        /// This can indicate that a key refresh is required.</exception>
        /// <exception cref="SecurityTokenInvalidSignatureException">If after trying all the <see cref="SecurityKey"/>(s), none result in a validated signature AND the 'token' does not have a key identifier.</exception>
        /// <returns>A <see cref="Saml2SecurityToken"/> that has had the signature validated if token was signed.</returns>
        /// <remarks><para>If the 'token' is signed, the signature is validated even if <see cref="TokenValidationParameters.RequireSignedTokens"/> is false.</para>
        /// <para>If the 'token' signature is validated, then the <see cref="Saml2SecurityToken.SigningKey"/> will be set to the key that signed the 'token'. It is the responsibility of <see cref="TokenValidationParameters.SignatureValidator"/> to set the <see cref="Saml2SecurityToken.SigningKey"/></para></remarks>
        protected virtual Saml2SecurityToken ValidateSignature(string token, TokenValidationParameters validationParameters)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw LogArgumentNullException(nameof(token));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            if (validationParameters.SignatureValidator != null)
            {
                var validatedSamlToken = validationParameters.SignatureValidator(token, validationParameters);
                if (validatedSamlToken == null)
                    throw LogExceptionMessage(new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10505, token)));

                if (!(validatedSamlToken is Saml2SecurityToken validatedSaml))
                    throw LogExceptionMessage(new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10506, LogHelper.MarkAsNonPII(typeof(Saml2SecurityToken)), LogHelper.MarkAsNonPII(validatedSamlToken.GetType()), token)));

                return validatedSaml;
            }

            var samlToken = ReadSaml2Token(token);
            if (samlToken == null)
                throw LogExceptionMessage(
                    new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateSignature"), LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ReadSaml2Token"), LogHelper.MarkAsNonPII(typeof(Saml2SecurityToken)))));

            return ValidateSignature(samlToken, token, validationParameters);
        }

        private Saml2SecurityToken ValidateSignature(Saml2SecurityToken samlToken, string token, TokenValidationParameters validationParameters)
        {
            if (samlToken.Assertion.Signature == null)
                if (validationParameters.RequireSignedTokens)
                    throw LogExceptionMessage(new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10504, token)));
                else
                    return samlToken;

            bool keyMatched = false;
            IEnumerable<SecurityKey> keys = null;
            if (validationParameters.IssuerSigningKeyResolver != null)
            {
                keys = validationParameters.IssuerSigningKeyResolver(token, samlToken, samlToken.Assertion.Signature.KeyInfo?.Id, validationParameters);
            }
            else
            {
                var key = ResolveIssuerSigningKey(token, samlToken, validationParameters);
                if (key != null)
                {
                    // remember that key was matched for throwing exception SecurityTokenSignatureKeyNotFoundException
                    keyMatched = true;
                    keys = new List<SecurityKey> { key };
                }
            }

            if (keys == null && validationParameters.TryAllIssuerSigningKeys)
            {
                // control gets here if:
                // 1. User specified delegate: IssuerSigningKeyResolver returned null
                // 2. ResolveIssuerSigningKey returned null
                // Try all the keys. This is the degenerate case, not concerned about perf.
                keys = TokenUtilities.GetAllSigningKeys(validationParameters: validationParameters);
            }

            // keep track of exceptions thrown, keys that were tried
            var exceptionStrings = new StringBuilder();
            var keysAttempted = new StringBuilder();
            bool canMatchKey = samlToken.Assertion.Signature.KeyInfo != null;

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    try
                    {
                        Validators.ValidateAlgorithm(samlToken.Assertion.Signature.SignedInfo.SignatureMethod, key, samlToken, validationParameters);

                        samlToken.Assertion.Signature.Verify(key, validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory);

                        if (LogHelper.IsEnabled(EventLogLevel.Informational))
                            LogHelper.LogInformation(TokenLogMessages.IDX10242, token);

                        samlToken.SigningKey = key;
                        return samlToken;
                    }
                    catch (Exception ex)
                    {
                        exceptionStrings.AppendLine(ex.ToString());
                    }

                    if (key != null)
                    {
                        keysAttempted.Append(key.ToString()).Append(" , KeyId: ").AppendLine(key.KeyId);
                        if (canMatchKey && !keyMatched && key.KeyId != null)
                            keyMatched = samlToken.Assertion.Signature.KeyInfo.MatchesKey(key);
                    }
                }
            }

            if (canMatchKey)
            {
                if (keyMatched)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10514, keysAttempted, samlToken.Assertion.Signature.KeyInfo, exceptionStrings, samlToken)));

                ValidateIssuer(samlToken.Issuer, samlToken, validationParameters);
                ValidateConditions(samlToken, validationParameters);
            }

            if (keysAttempted.Length > 0)
                throw LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(FormatInvariant(TokenLogMessages.IDX10512, keysAttempted, exceptionStrings, samlToken)));

            throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(TokenLogMessages.IDX10500));
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use for validating the signature of a token.
        /// </summary>
        /// <param name="token">The <see cref="string"/> representation of the token that is being validated.</param>
        /// <param name="samlToken">The <see cref="Saml2SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> that will be used during validation.</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/>.Assertion' is null.</exception>
        /// <remarks>If key fails to resolve, then null is returned.</remarks>
        protected virtual SecurityKey ResolveIssuerSigningKey(string token, Saml2SecurityToken samlToken, TokenValidationParameters validationParameters)
        {
            if (samlToken == null)
                throw LogArgumentNullException(nameof(samlToken));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            if (samlToken.Assertion == null)
                throw LogArgumentNullException(nameof(samlToken.Assertion));

            return SamlTokenUtilities.ResolveTokenSigningKey(samlToken.Assertion.Signature.KeyInfo, validationParameters);
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <param name="token">a Saml2 token as a string.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">If <paramref name="token"/>.Length is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <returns>A <see cref="Saml2SecurityToken"/></returns>
        public override SecurityToken ReadToken(string token)
        {
            return ReadSaml2Token(token);
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="Saml2SecurityToken"/>.
        /// </summary>
        /// <param name="token">a Saml2 token as a string.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">If <paramref name="token"/>.Length is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <returns>A <see cref="Saml2SecurityToken"/></returns>
        public virtual Saml2SecurityToken ReadSaml2Token(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw LogArgumentNullException(nameof(token));

            if (token.Length > MaximumTokenSizeInBytes)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

            using (var reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(token), XmlDictionaryReaderQuotas.Max))
            {
                return ReadSaml2Token(reader);
            }
        }

        /// <summary>
        /// Reads a <see cref="Saml2SecurityToken"/> where the XmlReader is positioned the beginning of a Saml2 assertion.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> reader positioned at a saml2 assertion element.</param>
        /// <returns>A <see cref="Saml2SecurityToken"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/> is null.</exception>
        public override SecurityToken ReadToken(XmlReader reader)
        {
            return ReadSaml2Token(reader);
        }

        /// <summary>
        /// Reads a <see cref="Saml2SecurityToken"/> where the XmlReader is positioned the beginning of a Saml2 assertion.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> reader positioned at a saml2 assertion element.</param>
        /// <returns>A <see cref="Saml2SecurityToken"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenReadException">If <see cref="Saml2Serializer.ReadAssertion(XmlReader)"/> returns null.</exception>
        public virtual Saml2SecurityToken ReadSaml2Token(XmlReader reader)
        {
            if (reader == null)
                LogHelper.LogArgumentNullException(nameof(reader));

            var assertion = Serializer.ReadAssertion(reader);
            if (assertion == null)
                throw LogExceptionMessage(
                    new Saml2SecurityTokenReadException(FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ReadSaml2Token"), LogHelper.MarkAsNonPII(Serializer.GetType()), LogHelper.MarkAsNonPII("ReadAssertion"), LogHelper.MarkAsNonPII(typeof(Saml2Assertion)))));

            return new Saml2SecurityToken(assertion);
        }

        /// <summary>
        /// Reads and validates a SAML 2.0 token using the XmlReader.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> reader positioned at a <see cref="Saml2Assertion"/> element.</param>
        /// <param name="validationParameters"> validation parameters for the <see cref="Saml2SecurityToken"/>.</param>
        /// <returns>An instance of <see cref="Saml2SecurityToken"/>.</returns>
        /// <exception cref="NotSupportedException">Currently not supported.</exception>
        public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters)
        {
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX13950));
        }

        /// <summary>
        /// Indicates if the current XML element is pointing to a Saml2Assertion.
        /// </summary>
        /// <param name="reader">A reader that may contain a <see cref="Saml2Assertion"/>.</param>
        /// <returns>'true' if reader contains a <see cref="Saml2Assertion"/>. 'false' otherwise.</returns>
        internal static bool IsSaml2Assertion(XmlReader reader)
        {
            return reader.IsStartElement(Saml2Constants.Elements.Assertion, Saml2Constants.Namespace);
        }

        /// <summary>
        /// Creates the conditions for the assertion.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Generally, conditions should be included in assertions to limit the 
        /// impact of misuse of the assertion. Specifying the NotBefore and 
        /// NotOnOrAfter conditions can limit the period of vulnerability in 
        /// the case of a compromised assertion. The AudienceRestrictionCondition
        /// can be used to explicitly state the intended relying party or parties
        /// of the assertion, which coupled with appropriate audience restriction
        /// enforcement at relying parties can help to mitigate spoofing attacks
        /// between relying parties.
        /// </para>
        /// <para>
        /// The default implementation creates NotBefore and NotOnOrAfter conditions
        /// based on the tokenDescriptor.Lifetime. It will also generate an 
        /// AudienceRestrictionCondition limiting consumption of the assertion to 
        /// tokenDescriptor.Scope.Address.
        /// </para>
        /// </remarks>
        /// <param name="tokenDescriptor">contains the details of the conditions.</param>
        /// <returns>A Saml2Conditions object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="tokenDescriptor"/> is null.</exception>
        protected virtual Saml2Conditions CreateConditions(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            var conditions = new Saml2Conditions();
            if (tokenDescriptor.NotBefore.HasValue)
                conditions.NotBefore = tokenDescriptor.NotBefore.Value;
            else if (SetDefaultTimesOnTokenCreation)
                conditions.NotBefore = DateTime.UtcNow;

            if (tokenDescriptor.Expires.HasValue)
                conditions.NotOnOrAfter = tokenDescriptor.Expires.Value;
            else if (SetDefaultTimesOnTokenCreation)
                conditions.NotOnOrAfter = DateTime.UtcNow + TimeSpan.FromMinutes(TokenLifetimeInMinutes);

            if (!string.IsNullOrEmpty(tokenDescriptor.Audience))
                conditions.AudienceRestrictions.Add(new Saml2AudienceRestriction(tokenDescriptor.Audience));

            return conditions;
        }

        /// <summary>
        /// Creates the advice for the assertion.
        /// </summary>
        /// <remarks>
        /// By default, this method returns null.
        /// </remarks>
        /// <param name="tokenDescriptor">The token descriptor.</param>
        /// <returns>A <see cref="Saml2Advice"/> object, default is null.</returns>
        protected virtual Saml2Advice CreateAdvice(SecurityTokenDescriptor tokenDescriptor)
        {
            return null;
        }

        /// <summary>
        /// Creates a name identifier that identifies the assertion issuer.
        /// </summary>
        /// <param name="tokenDescriptor">The token descriptor.</param>
        /// <returns>A <see cref="Saml2NameIdentifier"/> using <paramref name="tokenDescriptor"/>.Issuer.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="tokenDescriptor"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenException">If <paramref name="tokenDescriptor"/>.Issuer is null or empty.</exception>
        /// <remarks>Uses tokenDescriptor.Issuer.</remarks>
        protected virtual Saml2NameIdentifier CreateIssuerNameIdentifier(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            return new Saml2NameIdentifier(tokenDescriptor.Issuer);
        }

        /// <summary>
        /// Creates a Saml2Attribute from a claim.
        /// </summary>
        /// <param name="claim">The <see cref="Claim"/> from which to generate a <see cref="Saml2Attribute"/>.</param>
        /// <returns>A <see cref="Saml2Attribute"/>created from the <paramref name="claim"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="claim"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenException">if the <paramref name="claim"/> has a property "ClaimsProperties.SamlAttributeNameFormat" and the value is not a valid absolute URI.</exception>
        protected virtual Saml2Attribute CreateAttribute(Claim claim)
        {
            if (claim == null)
                throw LogArgumentNullException(nameof(claim));

            var attribute = new Saml2Attribute(claim.Type, claim.Value);
            if (!StringComparer.Ordinal.Equals(claim.Issuer, claim.OriginalIssuer))
                attribute.OriginalIssuer = claim.OriginalIssuer;

            attribute.AttributeValueXsiType = claim.ValueType;
            if (claim.Properties.TryGetValue(ClaimProperties.SamlAttributeNameFormat, out string nameFormat))
            {
                if (!Saml2Serializer.CanCreateValidUri(nameFormat, UriKind.Absolute))
                    throw LogExceptionMessage(new Saml2SecurityTokenException(FormatInvariant(LogMessages.IDX13300, LogHelper.MarkAsNonPII(ClaimProperties.SamlAttributeNameFormat), nameFormat)));

                attribute.NameFormat = new Uri(nameFormat);
            }

            if (claim.Properties.TryGetValue(ClaimProperties.SamlAttributeFriendlyName, out string displayName))
                attribute.FriendlyName = displayName;

            return attribute;
        }

        /// <summary>
        /// Creates <see cref="Saml2AttributeStatement"/> from a <see cref="SecurityTokenDescriptor"/> and a <see cref="ClaimsIdentity"/>
        /// </summary>
        /// <remarks>This method may return null if the token descriptor does not contain any subject or the subject does not have any claims.
        /// </remarks>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> that contains information on building the <see cref="Saml2AttributeStatement"/>.</param>
        /// <returns>A Saml2AttributeStatement.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenDescriptor"/> is null.</exception>
        protected virtual Saml2AttributeStatement CreateAttributeStatement(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            if (tokenDescriptor.Subject == null)
                throw LogArgumentNullException(nameof(tokenDescriptor.Subject));

            var attributes = new List<Saml2Attribute>();

            IEnumerable<Claim> claims = SamlTokenUtilities.GetAllClaims(tokenDescriptor.Claims, tokenDescriptor.Subject != null ? tokenDescriptor.Subject.Claims : null);

            if (claims != null && claims.Any())
            {
                foreach (Claim claim in claims)
                {
                    if (claim != null)
                    {
                        switch (claim.Type)
                        {
                            // TODO - should these really be filtered?
                            case ClaimTypes.AuthenticationInstant:
                            case ClaimTypes.AuthenticationMethod:
                            case ClaimTypes.NameIdentifier:
                                break;
                            default:
                                attributes.Add(CreateAttribute(claim));
                                break;
                        }
                    }
                }
            }

            if (tokenDescriptor.Subject.Actor != null)
                attributes.Add(CreateAttribute(new Claim(ClaimTypes.Actor, CreateActorString(tokenDescriptor.Subject.Actor), ClaimValueTypes.String)));

            return new Saml2AttributeStatement(ConsolidateAttributes(attributes));
        }

        /// <summary>
        /// Consolidates attributes into a single attribute with multiple values.
        /// </summary>
        /// <param name="attributes">A <see cref="ICollection{T}"/> of <see cref="Saml2Attribute"/>.</param>
        /// <returns>A <see cref="ICollection{T}"/> of <see cref="Saml2Attribute"/> with common attributes consolidated into unique attributes with multiple values.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="attributes"/> is null.</exception>
        protected virtual ICollection<Saml2Attribute> ConsolidateAttributes(ICollection<Saml2Attribute> attributes)
        {
            if (attributes == null)
                throw LogArgumentNullException(nameof(attributes));

            var distinctAttributes = new Dictionary<Saml2AttributeKeyComparer.AttributeKey, Saml2Attribute>(attributes.Count, Saml2AttributeKeyComparer.Instance);
            foreach (var attribute in attributes)
            {
                if (attribute != null)
                {
                    var attributeKey = new Saml2AttributeKeyComparer.AttributeKey(attribute);
                    if (distinctAttributes.TryGetValue(attributeKey, out Saml2Attribute attr))
                    {
                        foreach (string value in attribute.Values)
                            attr.Values.Add(value);
                    }
                    else
                    {
                        distinctAttributes.Add(attributeKey, attribute);
                    }
                }
            }

            return distinctAttributes.Values;
        }

        /// <summary>
        /// Transforms a ClaimsIdentity into a string.
        /// </summary>
        /// <param name="actor">A <see cref="ClaimsIdentity"/> to be transformed.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="actor"/> is null.</exception>
        /// <returns>A well-formed XML string.</returns>
        /// <remarks>Normally this is called when creating a <see cref="Saml2Assertion"/> from a <see cref="ClaimsIdentity"/>. When <see cref="ClaimsIdentity.Actor"/> is not null, 
        /// this method is called to create an string representation to add as an attribute.
        /// <para>The string is formed: "&lt;Actor&gt;&lt;Attribute name, namespace&gt;&lt;AttributeValue&gt;...&lt;/AttributeValue&gt;, ...&lt;/Attribute&gt;...&lt;/Actor&gt;</para></remarks>
        protected string CreateActorString(ClaimsIdentity actor)
        {
            if (actor == null)
                throw LogArgumentNullException(nameof(actor));

            var attributes = new List<Saml2Attribute>();
            foreach (Claim claim in actor.Claims)
            {
                if (claim != null)
                    attributes.Add(CreateAttribute(claim));
            }

            return CreateXmlStringFromAttributes(ConsolidateAttributes(attributes));
        }

        /// <summary>
        /// Builds an XML formatted string from a collection of SAML attributes that represent the Actor. 
        /// </summary>
        /// <param name="attributes">An enumeration of Saml2Attributes.</param>
        /// <returns>A well-formed XML string.</returns>
        /// <remarks>The string is of the form "&lt;Actor&gt;&lt;Attribute name, namespace&gt;&lt;AttributeValue&gt;...&lt;/AttributeValue&gt;, ...&lt;/Attribute&gt;...&lt;/Actor&gt;"</remarks>
        private string CreateXmlStringFromAttributes(IEnumerable<Saml2Attribute> attributes)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var dictionaryWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false))
                {
                    dictionaryWriter.WriteStartElement(_actor);
                    foreach (var attribute in attributes)
                    {
                        if (attribute != null)
                            Serializer.WriteAttribute(dictionaryWriter, attribute);
                    }

                    dictionaryWriter.WriteEndElement();
                    dictionaryWriter.Flush();
                }

                return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        /// <summary>
        /// Creates an <see cref="IEnumerable{T}"/> of <see cref="Saml2Statement"/> to be included in the assertion.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Statements are not required in a SAML2 assertion. This method may
        /// return an empty collection.
        /// </para>
        /// </remarks>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> that contains information on creating the <see cref="Saml2Statement"/>.</param>
        /// <returns>An enumeration of Saml2Statements.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenDescriptor"/> is null.</exception>
        protected virtual IEnumerable<Saml2Statement> CreateStatements(SecurityTokenDescriptor tokenDescriptor)
        {
            return CreateStatements(tokenDescriptor, null);
        }

        /// <summary>
        /// Creates an <see cref="IEnumerable{T}"/> of <see cref="Saml2Statement"/> to be included in the assertion.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Statements are not required in a SAML2 assertion. This method may
        /// return an empty collection.
        /// </para>
        /// </remarks>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> that contains information on creating the <see cref="Saml2Statement"/>.</param>
        /// <param name="authenticationInformation">additional information used when creating a <see cref="Saml2AuthenticationStatement"/>.</param>
        /// <returns>An enumeration of Saml2Statements.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenDescriptor"/> is null.</exception>
        protected virtual IEnumerable<Saml2Statement> CreateStatements(SecurityTokenDescriptor tokenDescriptor, AuthenticationInformation authenticationInformation)
        {
            if (tokenDescriptor == null)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            var statements = new Collection<Saml2Statement>();

            var attributeStatement = CreateAttributeStatement(tokenDescriptor);
            if (attributeStatement != null)
                statements.Add(attributeStatement);

            var authenticationStatement = CreateAuthenticationStatement(authenticationInformation);
            if (authenticationStatement != null)
                statements.Add(authenticationStatement);

            var authorizationDecisionStatement = CreateAuthorizationDecisionStatement(tokenDescriptor);
            if (authorizationDecisionStatement != null)
                statements.Add(authorizationDecisionStatement);

            return statements;
        }

        /// <summary>
        /// Creates a Saml2AuthenticationStatement
        /// </summary>
        /// <param name="authenticationInformation">authenticationInformation object containing the state to be wrapped as a Saml2AuthenticationStatement object.</param>
        /// <returns>A <see cref="Saml2AuthenticationStatement"/></returns>
        /// <remarks>if <paramref name="authenticationInformation"/> is null, then null is returned.</remarks>
        protected virtual Saml2AuthenticationStatement CreateAuthenticationStatement(AuthenticationInformation authenticationInformation)
        {
            if (authenticationInformation == null)
                return null;

            var authContext = new Saml2AuthenticationContext(authenticationInformation.AuthenticationMethod);
            var authenticationStatement = new Saml2AuthenticationStatement(authContext, authenticationInformation.AuthenticationInstant);
            if (!string.IsNullOrEmpty(authenticationInformation.DnsName) || !string.IsNullOrEmpty(authenticationInformation.Address))
                authenticationStatement.SubjectLocality = new Saml2SubjectLocality(authenticationInformation.Address, authenticationInformation.DnsName);

            if (!string.IsNullOrEmpty(authenticationInformation.Session))
                authenticationStatement.SessionIndex = authenticationInformation.Session;

            authenticationStatement.SessionNotOnOrAfter = authenticationInformation.NotOnOrAfter;

            return authenticationStatement;
        }

        /// <summary>
        /// Creates a <see cref="Saml2AuthorizationDecisionStatement"/> from a <see cref="SecurityTokenDescriptor"/>.
        /// </summary>
        /// <param name="tokenDescriptor">The token descriptor.</param>
        /// <returns>A <see cref="Saml2AuthorizationDecisionStatement"/>.</returns>
        /// <remarks>By default a null statement is returned. Override to return a <see cref="Saml2AuthorizationDecisionStatement"/> to be added to a <see cref="Saml2SecurityToken"/>.</remarks>
        public virtual Saml2AuthorizationDecisionStatement CreateAuthorizationDecisionStatement(SecurityTokenDescriptor tokenDescriptor)
        {
            return null;
        }

        /// <summary>
        /// Creates a SAML2 subject of the assertion.
        /// </summary>
        /// <param name="tokenDescriptor">The security token descriptor to create the subject.</param>
        /// <exception cref="ArgumentNullException">Thrown when 'tokenDescriptor' is null.</exception>
        /// <returns>A <see cref="Saml2Subject"/>.</returns>
        protected virtual Saml2Subject CreateSubject(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            var saml2Subject = new Saml2Subject();

            // Look for name identifier claims
            string nameIdentifierClaim = null;
            string nameIdentifierFormat = null;
            string nameIdentifierNameQualifier = null;
            string nameIdentifierSpProviderId = null;
            string nameIdentifierSpNameQualifier = null;

            IEnumerable<Claim> claims = SamlTokenUtilities.GetAllClaims(tokenDescriptor.Claims, tokenDescriptor.Subject != null ? tokenDescriptor.Subject.Claims : null);

            if (claims != null && claims.Any())
            {
                foreach (var claim in claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                    {
                        // Do not allow multiple name identifier claim.
                        if (nameIdentifierClaim != null)
                            throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13306));

                        nameIdentifierClaim = claim.Value;
                        string propValue;
                        if (claim.Properties.TryGetValue(ClaimProperties.SamlNameIdentifierFormat, out propValue))
                            nameIdentifierFormat = propValue;

                        if (claim.Properties.TryGetValue(ClaimProperties.SamlNameIdentifierNameQualifier, out propValue))
                            nameIdentifierNameQualifier = propValue;

                        if (claim.Properties.TryGetValue(ClaimProperties.SamlNameIdentifierSPNameQualifier, out propValue))
                            nameIdentifierSpNameQualifier = propValue;

                        if (claim.Properties.TryGetValue(ClaimProperties.SamlNameIdentifierSPProvidedId, out propValue))
                            nameIdentifierSpProviderId = propValue;
                    }
                }
            }

            if (nameIdentifierClaim != null)
            {
                var nameIdentifier = new Saml2NameIdentifier(nameIdentifierClaim);
                if (nameIdentifierFormat != null && Saml2Serializer.CanCreateValidUri(nameIdentifierFormat, UriKind.Absolute))
                    nameIdentifier.Format = new Uri(nameIdentifierFormat);

                nameIdentifier.NameQualifier = nameIdentifierNameQualifier;
                nameIdentifier.SPNameQualifier = nameIdentifierSpNameQualifier;
                nameIdentifier.SPProvidedId = nameIdentifierSpProviderId;
                saml2Subject.NameId = nameIdentifier;
            }

            saml2Subject.SubjectConfirmations.Add(new Saml2SubjectConfirmation(Saml2Constants.ConfirmationMethods.Bearer));
            return saml2Subject;
        }

        /// <summary>
        /// Validates the Lifetime and Audience conditions.
        /// </summary>
        /// <param name="samlToken">a <see cref="Saml2SecurityToken"/> that contains the <see cref="Saml2Conditions"/>.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/>.Assertion' is null.</exception>
        /// <exception cref="SecurityTokenValidationException">if the Condition 'OneTimeUse' is specified. Requires overriding.</exception>
        /// <exception cref="SecurityTokenValidationException">if the Condition 'ProxyRestriction' is specified. Requires overriding.</exception>
        protected virtual void ValidateConditions(Saml2SecurityToken samlToken, TokenValidationParameters validationParameters)
        {
            if (samlToken == null)
                throw LogArgumentNullException(nameof(samlToken));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            if (samlToken.Assertion == null)
                throw LogArgumentNullException(nameof(samlToken.Assertion));

            if (samlToken.Assertion.Conditions == null)
            {
                if (validationParameters.RequireAudience)
                    throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13002));

                return;
            }

            ValidateLifetime(samlToken.Assertion.Conditions.NotBefore, samlToken.Assertion.Conditions.NotOnOrAfter, samlToken, validationParameters);

            if (samlToken.Assertion.Conditions.OneTimeUse)
                ValidateOneTimeUseCondition(samlToken, validationParameters);

            if (samlToken.Assertion.Conditions.ProxyRestriction != null)
                throw LogExceptionMessage(new SecurityTokenValidationException(LogMessages.IDX13511));

            var foundAudienceRestriction = false;
            foreach (var audienceRestriction in samlToken.Assertion.Conditions.AudienceRestrictions)
            {
                if (!foundAudienceRestriction)
                    foundAudienceRestriction = true;

                ValidateAudience(audienceRestriction.Audiences, samlToken, validationParameters);
            }
            
            if (validationParameters.RequireAudience && !foundAudienceRestriction)
                throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13002));
        }

        /// <summary>
        /// Validates the OneTimeUse condition.
        /// </summary>
        /// <param name="securityToken">The <see cref="Saml2SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        protected virtual void ValidateOneTimeUseCondition(Saml2SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            throw LogExceptionMessage(new SecurityTokenValidationException(LogMessages.IDX13510));
        }

        /// <summary>
        /// This method gets called when a special type of Saml2Attribute is detected. The Saml2Attribute passed in 
        /// wraps a Saml2Attribute that contains a collection of AttributeValues, each of which will get mapped to a 
        /// claim.  All of the claims will be returned in an ClaimsIdentity with the specified issuer.
        /// </summary>
        /// <param name="attribute">The <see cref="Saml2Attribute"/> to use.</param>
        /// <param name="identity">The <see cref="ClaimsIdentity"/> that is the subject of this token.</param>
        /// <param name="issuer">The issuer of the claim.</param>
        /// <exception cref="InvalidOperationException">Will be thrown if the Saml2Attribute does not contain any 
        /// valid Saml2AttributeValues.
        /// </exception>
        protected virtual void SetClaimsIdentityActorFromAttribute(Saml2Attribute attribute, ClaimsIdentity identity, string issuer)
        {
            // bail here; nothing to add.
            if (identity == null || attribute == null || (attribute.Name != ActorClaimName) || attribute.Values == null || attribute.Values.Count < 1)
                return;

            Saml2Attribute actorAttribute = null;
            var claims = new Collection<Claim>();
            
            // search through attribute values to see if the there is an embedded actor.
            foreach (string value in attribute.Values)
            {
                if (value != null)
                {
                    using (var dictionaryReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(value), XmlDictionaryReaderQuotas.Max))
                    {
                        dictionaryReader.MoveToContent();
                        dictionaryReader.ReadStartElement(_actor);
                        while (dictionaryReader.IsStartElement(Saml2Constants.Elements.Attribute))
                        {
                            var innerAttribute = Serializer.ReadAttribute(dictionaryReader);
                            if (innerAttribute != null)
                            {
                                if (innerAttribute.Name == ClaimTypes.Actor)
                                {
                                    // multiple actors at the same level is not supported
                                    if (actorAttribute != null)
                                        throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13142));

                                    actorAttribute = innerAttribute;
                                }
                                else
                                {
                                    string originalIssuer = innerAttribute.OriginalIssuer;
                                    foreach (var attr in innerAttribute.Values)
                                    {
                                        Claim claim = null;
                                        if (string.IsNullOrEmpty(originalIssuer))
                                            claim = new Claim(innerAttribute.Name, attr, innerAttribute.AttributeValueXsiType, issuer);
                                        else
                                            claim = new Claim(innerAttribute.Name, attr, innerAttribute.AttributeValueXsiType, issuer, originalIssuer);

                                        if (innerAttribute.NameFormat != null)
                                            claim.Properties[ClaimProperties.SamlAttributeNameFormat] = innerAttribute.NameFormat.OriginalString;

                                        if (innerAttribute.FriendlyName != null)
                                            claim.Properties[ClaimProperties.SamlAttributeFriendlyName] = innerAttribute.FriendlyName;

                                        claims.Add(claim);
                                    }
                                }
                            }
                        }

                        dictionaryReader.ReadEndElement(); // Actor
                    }
                }
            }

            identity.Actor = new ClaimsIdentity(claims);
            SetClaimsIdentityActorFromAttribute(actorAttribute, identity.Actor, issuer);
        }

        /// <summary>
        /// Processes all statements and adds claims to the identity.
        /// </summary>
        /// <param name="statements">A collection of Saml2Statement.</param>
        /// <param name="identity">The <see cref="ClaimsIdentity"/>.</param>
        /// <param name="issuer">The issuer.</param>
        protected virtual void ProcessStatements(ICollection<Saml2Statement> statements, ClaimsIdentity identity, string issuer)
        {
            if (statements == null)
                throw LogArgumentNullException(nameof(statements));

            foreach (var statement in statements)
            {
                if (statement is Saml2AttributeStatement attrStatement)
                    ProcessAttributeStatement(attrStatement, identity, issuer);
                else if (statement is Saml2AuthenticationStatement authnStatement)
                    ProcessAuthenticationStatement(authnStatement, identity, issuer);
                else if (statement is Saml2AuthorizationDecisionStatement authzStatement)
                    ProcessAuthorizationDecisionStatement(authzStatement, identity, issuer);
                else if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogWarning(LogMessages.IDX13516, LogHelper.MarkAsNonPII(statement.GetType()));
            }
        }

        /// <summary>
        /// Adds claims from the <see cref="Saml2Subject"/> into the <see cref="ClaimsIdentity"/>.
        /// </summary>
        /// <param name="subject">The <see cref="Saml2Subject"/>.</param>
        /// <param name="identity">The <see cref="ClaimsIdentity"/>.</param>
        /// <param name="issuer">The issuer.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="subject"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is null.</exception>
        protected virtual void ProcessSubject(Saml2Subject subject, ClaimsIdentity identity, string issuer)
        {
            if (subject == null)
                throw LogArgumentNullException(nameof(subject));

            if (identity == null)
                throw LogArgumentNullException(nameof(identity));

            var nameId = subject.NameId;
            if (nameId != null)
            {
                var claim = new Claim(ClaimTypes.NameIdentifier, nameId.Value, ClaimValueTypes.String, issuer);
                if (nameId.Format != null)
                    claim.Properties[ClaimProperties.SamlNameIdentifierFormat] = nameId.Format.OriginalString;

                if (nameId.NameQualifier != null)
                    claim.Properties[ClaimProperties.SamlNameIdentifierNameQualifier] = nameId.NameQualifier;

                if (nameId.SPNameQualifier != null)
                    claim.Properties[ClaimProperties.SamlNameIdentifierSPNameQualifier] = nameId.SPNameQualifier;

                if (nameId.SPProvidedId != null)
                    claim.Properties[ClaimProperties.SamlNameIdentifierSPProvidedId] = nameId.SPProvidedId;

                identity.AddClaim(claim);
            }
        }

        /// <summary>
        /// Creates claims from a <see cref="Saml2AttributeStatement"/>.
        /// </summary>
        /// <param name="statement">The <see cref="Saml2AttributeStatement"/>.</param>
        /// <param name="identity">The identity that will contain a <see cref="Claim"/> for each <see cref="Saml2Attribute.Values"/>.</param>
        /// <param name="issuer">The issuer for each <see cref="Claim"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="statement"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenException">if multiple 'Actor' claims are found.</exception>
        protected virtual void ProcessAttributeStatement(Saml2AttributeStatement statement, ClaimsIdentity identity, string issuer)
        {
            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (identity == null)
                throw LogArgumentNullException(nameof(identity));

            foreach (var attribute in statement.Attributes)
            {
                if (StringComparer.Ordinal.Equals(attribute.Name, ClaimTypes.Actor))
                {
                    // multiple actors at same level is not supported
                    if (identity.Actor != null)
                        throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13512));

                    SetClaimsIdentityActorFromAttribute(attribute, identity, issuer);
                }
                else
                {
                    // each value has same issuer
                    string originalIssuer = attribute.OriginalIssuer ?? issuer;
                    foreach (string value in attribute.Values)
                    {
                        if (value != null)
                        {
                            var claim = new Claim(attribute.Name, value, attribute.AttributeValueXsiType, issuer, originalIssuer);
                            if (attribute.NameFormat != null)
                                claim.Properties[ClaimProperties.SamlAttributeNameFormat] = attribute.NameFormat.OriginalString;

                            if (attribute.FriendlyName != null)
                                claim.Properties[ClaimProperties.SamlAttributeFriendlyName] = attribute.FriendlyName;

                            identity.AddClaim(claim);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates claims from a <see cref="Saml2AuthenticationStatement"/>.
        /// </summary>
        /// <param name="statement">The <see cref="Saml2AuthenticationStatement"/>.</param>
        /// <param name="identity">The identity that will contain the Authentication <see cref="Claim"/>s.</param>
        /// <param name="issuer">The issuer for each <see cref="Claim"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="statement"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenException">If <see cref="Saml2AuthenticationStatement.AuthenticationContext"/>.DeclarationReference is not null. Override if this is required.</exception>
        protected virtual void ProcessAuthenticationStatement(Saml2AuthenticationStatement statement, ClaimsIdentity identity, string issuer)
        {
            if (identity == null)
                throw LogArgumentNullException(nameof(identity));

            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (statement.AuthenticationContext.DeclarationReference != null)
                throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13001));

            if (statement.AuthenticationContext.ClassReference != null)
            {
                identity.AddClaim(
                    new Claim(ClaimTypes.AuthenticationMethod,
                              statement.AuthenticationContext.ClassReference.OriginalString,
                              ClaimValueTypes.String,
                              issuer));
            }

            identity.AddClaim(new Claim(ClaimTypes.AuthenticationInstant, XmlConvert.ToString(statement.AuthenticationInstant.ToUniversalTime(), Saml2Constants.GeneratedDateTimeFormat), ClaimValueTypes.DateTime, issuer));
        }

        /// <summary>
        /// Creates claims from a <see cref="Saml2AuthorizationDecisionStatement"/>.
        /// </summary>
        /// <param name="statement">The <see cref="Saml2AuthorizationDecisionStatement"/>.</param>
        /// <param name="identity">The identity that will contain the AuthorizationDecision <see cref="Claim"/>s.</param>
        /// <param name="issuer">The issuer for each <see cref="Claim"/>.</param>
        /// <remarks>Provided for extensibility. By default no claims are added.</remarks>
        protected virtual void ProcessAuthorizationDecisionStatement(Saml2AuthorizationDecisionStatement statement, ClaimsIdentity identity, string issuer)
        {
        }

        /// <summary>
        /// Creates claims from a Saml2 token.
        /// </summary>
        /// <param name="samlToken">The <see cref="Saml2SecurityToken"/>.</param>
        /// <param name="issuer">The value to set <see cref="Claim.Issuer"/></param>
        /// <param name="validationParameters">creates the <see cref="ClaimsIdentity"/> using <see cref="TokenValidationParameters.CreateClaimsIdentity(SecurityToken, string)"/>.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> with claims from the saml statements.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="samlToken"/>.Assertion is null.</exception>
        protected virtual ClaimsIdentity CreateClaimsIdentity(Saml2SecurityToken samlToken, string issuer, TokenValidationParameters validationParameters)
        {
            if (samlToken == null)
                throw LogArgumentNullException(nameof(samlToken));

            if (samlToken.Assertion == null)
                throw LogArgumentNullException(LogMessages.IDX13110);

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            var actualIssuer = issuer;
            if (string.IsNullOrWhiteSpace(issuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(TokenLogMessages.IDX10244, LogHelper.MarkAsNonPII(ClaimsIdentity.DefaultIssuer));

                actualIssuer = ClaimsIdentity.DefaultIssuer;
            }

            var identity = validationParameters.CreateClaimsIdentity(samlToken, actualIssuer);
            ProcessSubject(samlToken.Assertion.Subject, identity, actualIssuer);
            ProcessStatements(samlToken.Assertion.Statements, identity, actualIssuer);

            return identity;
        }

        /// <summary>
        /// Determines if the audience found in a <see cref="Saml2SecurityToken"/> is valid.
        /// </summary>
        /// <param name="audiences">The audiences found in the <see cref="Saml2SecurityToken"/></param>
        /// <param name="securityToken">The <see cref="Saml2SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <remarks><see cref="Validators.ValidateAudience(IEnumerable{string}, SecurityToken, TokenValidationParameters)"/> for additional details.</remarks>
        protected virtual void ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateAudience(audiences, securityToken, validationParameters);
        }

        /// <summary>
        /// Determines if the issuer found in a <see cref="Saml2SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="Saml2SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>The issuer to use when creating the <see cref="Claim"/>(s) in the <see cref="ClaimsIdentity"/>.</returns>
        /// <remarks><see cref="Validators.ValidateIssuer(string, SecurityToken, TokenValidationParameters)"/> for additional details.</remarks>
        protected virtual string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return Validators.ValidateIssuer(issuer, securityToken, validationParameters);
        }

        /// <summary>
        /// Serializes a <see cref="Saml2SecurityToken"/> to a string.
        /// </summary>
        /// <param name="securityToken">A <see cref="Saml2SecurityToken"/>.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="securityToken"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="securityToken"/> is not a <see cref="Saml2SecurityToken"/>.</exception>
        public override string WriteToken(SecurityToken securityToken)
        {
            if (securityToken == null)
                throw LogArgumentNullException(nameof(securityToken));

            var samlToken = securityToken as Saml2SecurityToken;
            if (samlToken == null)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13400, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII(typeof(Saml2SecurityToken)), LogHelper.MarkAsNonPII(securityToken.GetType()))));

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false))
                {
                    WriteToken(writer, samlToken);
                    writer.Flush();
                    return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="Saml2SecurityToken"/> using the XmlWriter.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="securityToken"/> is null.</exception>
        /// <exception cref="Saml2SecurityTokenWriteException">If <paramref name="securityToken"/> is not a <see cref="Saml2SecurityToken"/>.</exception>
        /// <exception cref="ArgumentNullException">If <see cref="Saml2SecurityToken.Assertion"/> is null.</exception>
        public override void WriteToken(XmlWriter writer, SecurityToken securityToken)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (securityToken == null)
                throw LogArgumentNullException(nameof(securityToken));

            var samlToken = securityToken as Saml2SecurityToken;
            if (samlToken == null)
                throw Saml2Serializer.LogWriteException(LogMessages.IDX13150, securityToken.GetType());

            if (samlToken.Assertion == null)
                throw LogArgumentNullException(nameof(samlToken.Assertion));

            Serializer.WriteAssertion(writer, samlToken.Assertion);
        }
    }
}
