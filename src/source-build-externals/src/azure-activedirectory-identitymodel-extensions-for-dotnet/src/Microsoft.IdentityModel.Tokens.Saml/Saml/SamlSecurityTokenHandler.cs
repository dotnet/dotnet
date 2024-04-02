// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using static Microsoft.IdentityModel.Logging.LogHelper;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Saml Tokens,
    /// which supports validating tokens passed as strings using <see cref="TokenValidationParameters"/>.
    /// </summary>
    ///
    public class SamlSecurityTokenHandler : SecurityTokenHandler
    {
        internal const string Actor = "Actor";
        private const string _className = "Microsoft.IdentityModel.Tokens.Saml.SamlSecurityTokenHandler";

        private IEqualityComparer<SamlSubject> _samlSubjectEqualityComparer = new SamlSubjectEqualityComparer();
        private SamlSerializer _serializer = new SamlSerializer();

#region fields
        /// <summary>
        /// Gets a value indicating whether this handler supports validation of tokens
        /// handled by this instance.
        /// </summary>v
        /// <returns>'true' if the instance is capable of SecurityToken
        /// validation.</returns>
        public override bool CanValidateToken
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the class provides serialization functionality to serialize securityToken handled
        /// by this instance.
        /// </summary>
        /// <returns>true if the WriteToken method can serialize this securityToken.</returns>
        public override bool CanWriteToken
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the SamlSubject comparer.
        /// </summary>
        public IEqualityComparer<SamlSubject> SamlSubjectEqualityComparer
        {
            get
            {
                return _samlSubjectEqualityComparer;
            }
            set
            {
                _samlSubjectEqualityComparer = value ?? throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11514));
            }
        }

        /// <summary>
        /// Gets or set the <see cref="SamlSerializer"/> that will be used to read and write a <see cref="SamlSecurityToken"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">'value' is null.</exception> 
        public SamlSerializer Serializer
        {
            get { return _serializer; }
            set { _serializer = value ?? throw LogHelper.LogArgumentNullException(nameof(value)); }
        } 

        /// <summary>
        /// Gets the securityToken type supported by this handler.
        /// </summary>
        public override Type TokenType
        {
            get { return typeof(SamlSecurityToken); }
        }

        #endregion fields

#region methods
        /// <summary>
        /// Adds all Actors.
        /// </summary>
        /// <param name="subject"><see cref="ICollection{SamlAttribute}"/>.</param>
        /// <param name="attributes">Attribute collection to which the Actor added.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="attributes"/> is null.</exception>
        protected virtual void AddActorToAttributes(ICollection<SamlAttribute> attributes, ClaimsIdentity subject)
        {
            if (attributes == null)
                throw LogArgumentNullException(nameof(attributes));

            if (subject == null)
                return;

            var actorAttributes = new Collection<SamlAttribute>();
            foreach (var claim in subject.Claims)
            {
                if (claim != null)
                    actorAttributes.Add(CreateAttribute(claim));
            }

            // perform depth first recursion
            AddActorToAttributes(attributes, subject.Actor);

            var collectedAttributes = ConsolidateAttributes(actorAttributes);
            attributes.Add(CreateAttribute(new Claim(ClaimTypes.Actor, CreateXmlStringFromAttributes(collectedAttributes))));
        }

        /// <summary>
        /// Determines if the string is a valid Saml token by examining the xml for the correct start element.
        /// </summary>
        /// <param name="securityToken">A Saml token as a string.</param>
        /// <returns>'true' if the string has a start element equal <see cref="SamlConstants.Elements.Assertion"/>.</returns>
        public override bool CanReadToken(string securityToken)
        {
            if (string.IsNullOrWhiteSpace(securityToken) || securityToken.Length > MaximumTokenSizeInBytes)
                return false;

            try
            {
                using (var sr = new StringReader(securityToken))
                {
                    var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit };
                    using (var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(sr, settings))) 
                    {
                        return CanReadToken(reader);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates whether the current reader is positioned at a Saml assertion.
        /// </summary>
        /// <param name="reader">An <see cref="XmlReader"/> reader positioned at a start element. The reader should not be advanced.</param>
        /// <returns>'true' if a token can be read.</returns>
        public override bool CanReadToken(XmlReader reader)
        {
            if (reader == null)
                return false;

            return reader.IsStartElement(SamlConstants.Elements.Assertion, SamlConstants.Namespace);
        }

        /// <summary>
        /// Collects attributes with a common claim type, claim value type, and original issuer into a single attribute with multiple values.
        /// </summary>
        /// <param name="attributes"><see cref="ICollection{SamlAttribute}"/> to consolidate.</param>
        /// <returns><see cref="ICollection{SamlAttribute}"/>common attributes collected into value lists.</returns>
        protected virtual ICollection<SamlAttribute> ConsolidateAttributes(ICollection<SamlAttribute> attributes)
        {
            if (attributes == null)
                throw LogArgumentNullException(nameof(attributes));

            var distinctAttributes = new Dictionary<SamlAttributeKeyComparer.AttributeKey, SamlAttribute>(attributes.Count, new SamlAttributeKeyComparer());
            foreach (var attribute in attributes)
            {
                // Use unique attribute if name, value type, or issuer differ
                var attributeKey = new SamlAttributeKeyComparer.AttributeKey(attribute);
                if (distinctAttributes.TryGetValue(attributeKey, out var attr))
                {
                    foreach (var attributeValue in attribute.Values)
                        attr.Values.Add(attributeValue);
                }
                else
                {
                    distinctAttributes.Add(attributeKey, attribute);
                }
            }

            return distinctAttributes.Values;
        }

        /// <summary>
        /// Override this method to provide a SamlAdvice to place in the Samltoken. 
        /// </summary>
        /// <param name="tokenDescriptor">Contains information about the token.</param>
        /// <returns>SamlAdvice, default is null.</returns>
        protected virtual SamlAdvice CreateAdvice(SecurityTokenDescriptor tokenDescriptor)
        {
            return null;
        }

        /// <summary>
        /// Generates a SamlAttribute from a claim.
        /// </summary>
        /// <param name="claim">Claim from which to generate a SamlAttribute.</param>
        /// <returns><see cref="SamlAttribute"/></returns>
        /// <exception cref="ArgumentNullException">if the <paramref name="claim"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenException">if the type of <paramref name="claim"/> doesn't have "/" or only has "/" at the beginning or doesn't have any character(s) after the last "/".</exception>
        protected virtual SamlAttribute CreateAttribute(Claim claim)
        {
            if (claim == null)
                throw LogArgumentNullException(nameof(claim));

            // A SamlAttribute 1.0 is required to have the attributeNamespace and attributeName be non-null and non-empty.
            string claimType = claim.Type;

            int lastSlashIndex = claimType.LastIndexOf('/');
            if ((lastSlashIndex == 0) || (lastSlashIndex == -1))
                throw LogExceptionMessage(new SamlSecurityTokenException(FormatInvariant(LogMessages.IDX11523, MarkAsNonPII(claimType))));

            if (lastSlashIndex == claim.Type.Length - 1)
                throw LogExceptionMessage(new SamlSecurityTokenException(FormatInvariant(LogMessages.IDX11523, MarkAsNonPII(claimType))));

            return new SamlAttribute(
                claimType.Substring(0, lastSlashIndex),
                claimType.Substring(lastSlashIndex + 1, claimType.Length - (lastSlashIndex + 1)),
                new string[] { claim.Value })
            {
                OriginalIssuer = claim.OriginalIssuer,
                AttributeValueXsiType = claim.ValueType
            };
        }

        /// <summary>
        /// Creates SamlAttributeStatements and adds them to a collection.
        /// Override this method to provide a custom implementation.
        /// <para>
        /// Default behavior is to create a new SamlAttributeStatement for each Subject in the tokenDescriptor.Subjects collection.
        /// </para>
        /// </summary>
        /// <param name="subject">The SamlSubject to use in the SamlAttributeStatement that are created.</param>
        /// <param name="tokenDescriptor">Contains all the other information that is used in token issuance.</param>
        /// <returns>SamlAttributeStatement</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="subject"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenDescriptor"/> is null.</exception>
        protected virtual SamlAttributeStatement CreateAttributeStatement(SamlSubject subject, SecurityTokenDescriptor tokenDescriptor)
        {

            if (subject == null)
                throw LogArgumentNullException(nameof(subject));

            if (tokenDescriptor == null)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            IEnumerable<Claim> claims = SamlTokenUtilities.GetAllClaims(tokenDescriptor.Claims, tokenDescriptor.Subject != null ? tokenDescriptor.Subject.Claims : null);

            if (claims != null && claims.Any())
            {
                var attributes = new List<SamlAttribute>();
                foreach (var claim in claims)
                {
                    if (claim != null && claim.Type != ClaimTypes.NameIdentifier)
                    {
                        //
                        // NameIdentifier claim is already processed while creating the samlsubject
                        // AuthenticationInstant and AuthenticationType are not converted to Claims
                        //
                        switch (claim.Type)
                        {
                            case ClaimTypes.AuthenticationInstant:
                            case ClaimTypes.AuthenticationMethod:
                                break;
                            default:
                                attributes.Add(CreateAttribute(claim));
                                break;
                        }
                    }
                }

                AddActorToAttributes(attributes, tokenDescriptor.Subject?.Actor);

                var consolidatedAttributes = ConsolidateAttributes(attributes);
                if (consolidatedAttributes.Count > 0)
                {
                    return new SamlAttributeStatement(subject, consolidatedAttributes);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a SamlAuthenticationStatement for each AuthenticationInformation found in AuthenticationInformation. 
        /// Override this method to provide a custom implementation.
        /// </summary>
        /// <param name="subject">The SamlSubject of the Statement.</param>
        /// <param name="authenticationInformation">Contains all the other information that is used in token issuance.</param>
        /// <returns>SamlAuthenticationStatement</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="subject"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenException">if Assertion has one or more AuthenticationStatement, and one of AuthenticationMethod and authenticationInstant is null.</exception>
        protected virtual SamlAuthenticationStatement CreateAuthenticationStatement(SamlSubject subject, AuthenticationInformation authenticationInformation)
        {
            if (subject == null)
                throw LogArgumentNullException(nameof(subject));

            if (authenticationInformation == null)
                return null;

            return new SamlAuthenticationStatement(subject, authenticationInformation.AuthenticationMethod.OriginalString, authenticationInformation.AuthenticationInstant, authenticationInformation.DnsName, authenticationInformation.IPAddress, authenticationInformation.AuthorityBindings);
        }

        /// <summary>
        /// Creates a <see cref="SamlAuthorizationDecisionStatement"/> from a <see cref="SecurityTokenDescriptor"/>.
        /// </summary>
        /// <param name="tokenDescriptor">The token descriptor.</param>
        /// <returns>A <see cref="SamlAuthorizationDecisionStatement"/>.</returns>
        /// <remarks>By default a null statement is returned. Override to return a <see cref="SamlAuthorizationDecisionStatement"/> to be added to a <see cref="SamlSecurityToken"/>.</remarks>
        public virtual SamlAuthorizationDecisionStatement CreateAuthorizationDecisionStatement(SecurityTokenDescriptor tokenDescriptor)
        {
            return null;
        }

        /// <summary>
        /// Creates claims from a Saml securityToken.
        /// </summary>
        /// <param name="samlToken">A <see cref="SamlSecurityToken"/> that will be used to create the claims.</param>
        /// <param name="issuer">The value to set <see cref="Claim.Issuer"/></param>
        /// <param name="validationParameters"> Contains parameters for validating the securityToken.</param>
        /// <returns>A <see cref="IEnumerable{ClaimsIdentity}"/> containing the claims from the <see cref="SamlSecurityToken"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="samlToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <see cref="SamlSecurityToken.Assertion"/> is null.</exception>
        protected virtual IEnumerable<ClaimsIdentity> CreateClaimsIdentities(SamlSecurityToken samlToken, string issuer, TokenValidationParameters validationParameters)
        {
            if (samlToken == null)
                throw LogArgumentNullException(nameof(samlToken));

            if (samlToken.Assertion == null)
                throw LogArgumentNullException(LogMessages.IDX11110);

            var actualIssuer = issuer;
            if (string.IsNullOrWhiteSpace(issuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(TokenLogMessages.IDX10244, LogHelper.MarkAsNonPII(ClaimsIdentity.DefaultIssuer));

                actualIssuer = ClaimsIdentity.DefaultIssuer;
            }

            return ProcessStatements(samlToken, actualIssuer, validationParameters);
        }

        /// <summary>
        /// Generates all the conditions for saml
        /// </summary>
        /// <param name="tokenDescriptor">information that is used in token issuance.</param>
        /// <returns>SamlConditions</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenDescriptor"/> is null.</exception>
        protected virtual SamlConditions CreateConditions(SecurityTokenDescriptor tokenDescriptor)
        {
            if (null == tokenDescriptor)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            var conditions = new SamlConditions();
            if (tokenDescriptor.NotBefore.HasValue)
                conditions.NotBefore = tokenDescriptor.NotBefore.Value;
            else if (SetDefaultTimesOnTokenCreation)
                conditions.NotBefore = DateTime.UtcNow;

            if (tokenDescriptor.Expires.HasValue)
                conditions.NotOnOrAfter = tokenDescriptor.Expires.Value;
            else if (SetDefaultTimesOnTokenCreation)
                conditions.NotOnOrAfter = DateTime.UtcNow + TimeSpan.FromMinutes(TokenLifetimeInMinutes);

            if (!string.IsNullOrEmpty(tokenDescriptor.Audience))
                conditions.Conditions.Add(new SamlAudienceRestrictionCondition(new Uri(tokenDescriptor.Audience)));

            return conditions;
        }

        /// <summary>
        /// Generates an enumeration of SamlStatements from a SecurityTokenDescriptor.
        /// Only SamlAttributeStatements and SamlAuthenticationStatements are generated.
        /// Overwrite this method to customize the creation of statements.
        /// <para>
        /// Calls in order (all are virtual):
        /// 1. CreateSamlSubject
        /// 2. CreateAttributeStatements
        /// 3. CreateAuthenticationStatements
        /// 4. CreateAuthorizationDecisionStatement
        /// </para>
        /// </summary>
        /// <param name="tokenDescriptor">The SecurityTokenDescriptor to use to build the statements.</param>
        /// <param name="authenticationInformation">additional information for creating a <see cref="SamlAuthenticationStatement"/>.</param>
        /// <returns>An enumeration of SamlStatement.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenDescriptor"/> is null.</exception>
        protected virtual ICollection<SamlStatement> CreateStatements(SecurityTokenDescriptor tokenDescriptor, AuthenticationInformation authenticationInformation)
        {
            if (null == tokenDescriptor)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            var statements = new Collection<SamlStatement>();
            var subject = CreateSubject(tokenDescriptor);
            var attributeStatement = CreateAttributeStatement(subject, tokenDescriptor);
            if (attributeStatement != null)
                statements.Add(attributeStatement);

            var authnStatement = CreateAuthenticationStatement(subject, authenticationInformation);
            if (authnStatement != null)
                statements.Add(authnStatement);

            var authzStatement = CreateAuthorizationDecisionStatement(tokenDescriptor);
            if (authzStatement != null)
                statements.Add(authzStatement);

            return statements;
        }

        /// <summary>
        /// Returns the SamlSubject to use for all the statements that will be created.
        /// Overwrite this method to customize the creation of the SamlSubject.
        /// </summary>
        /// <param name="tokenDescriptor">Contains all the information that is used in token issuance.</param>
        /// <returns>A SamlSubject created from the first subject found in the tokenDescriptor as follows:
        /// <para>
        /// 1. Claim of Type NameIdentifier is searched. If found, SamlSubject.Name is set to claim.Value.
        /// 2. If a non-null tokenDescriptor.proof is found then SamlSubject.KeyIdentifier = tokenDescriptor.Proof.KeyIdentifier AND SamlSubject.ConfirmationMethod is set to 'HolderOfKey'.
        /// 3. If a null tokenDescriptor.proof is found then SamlSubject.ConfirmationMethod is set to 'BearerKey'.
        /// </para>
        /// </returns>
        /// <exception cref="ArgumentNullException">if <paramref name="tokenDescriptor"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenException">if the tokenDescriptor has more than one name identifier claim.</exception>
        protected virtual SamlSubject CreateSubject(SecurityTokenDescriptor tokenDescriptor)
        {
            if (null == tokenDescriptor)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            var samlSubject = new SamlSubject();
            Claim identityClaim = null;

            IEnumerable<Claim> claims = SamlTokenUtilities.GetAllClaims(tokenDescriptor.Claims, tokenDescriptor.Subject != null ? tokenDescriptor.Subject.Claims : null);

            if (claims != null && claims.Any())
            {
                foreach (var claim in claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                    {
                        // Do not allow multiple name identifier claim.
                        if (null != identityClaim)
                            throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11521));

                        identityClaim = claim;
                    }
                }
            }

            if (identityClaim != null)
            {
                samlSubject.Name = identityClaim.Value;
            }

            samlSubject.ConfirmationMethods.Add(SamlConstants.BearerConfirmationMethod);

            return samlSubject;
        }

        /// <summary>
        /// Creates a <see cref="SamlSecurityToken"/> based on a information contained in the <see cref="SecurityTokenDescriptor"/>.
        /// </summary>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> that has creation information.</param>
        /// <returns>A <see cref="SecurityToken"/> instance.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="tokenDescriptor"/> is null.</exception>
        public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            return CreateToken(tokenDescriptor, null);
        }

        /// <summary>
        /// Creates a <see cref="SamlSecurityToken"/> based on a information contained in the <see cref="SecurityTokenDescriptor"/>.
        /// </summary>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> that has creation information.</param>
        /// <param name="authenticationInformation">additional information for creating the <see cref="SamlAuthenticationStatement"/>.</param>
        /// <returns>A <see cref="SecurityToken"/> instance.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="tokenDescriptor"/> is null.</exception>
        public virtual SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor, AuthenticationInformation authenticationInformation)
        {
            if (null == tokenDescriptor)
                throw LogArgumentNullException(nameof(tokenDescriptor));

            var statements = CreateStatements(tokenDescriptor, authenticationInformation);
            var conditions = CreateConditions(tokenDescriptor);
            var advice = CreateAdvice(tokenDescriptor);

            var issuedAt = tokenDescriptor.IssuedAt.HasValue ? tokenDescriptor.IssuedAt.Value : DateTime.UtcNow;
            return new SamlSecurityToken(new SamlAssertion("_" + Guid.NewGuid().ToString(), tokenDescriptor.Issuer, issuedAt, conditions, advice, statements)
            {
                SigningCredentials = tokenDescriptor.SigningCredentials
            });

        }

        /// <summary>
        /// Builds an XML formated string from a collection of saml attributes that represent an Actor. 
        /// </summary>
        /// <param name="attributes"><see cref="ICollection{SamlAttribute}"/>.</param>
        /// <returns>A well formed XML string.</returns>
        /// <remarks>The string is of the form "&lt;Actor&gt;&lt;SamlAttribute name, ns&gt;&lt;SamlAttributeValue&gt;...&lt;/SamlAttributeValue&gt;, ...&lt;/SamlAttribute&gt;...&lt;/Actor&gt;"</remarks>        
        protected virtual string CreateXmlStringFromAttributes(ICollection<SamlAttribute> attributes)
        {
            if (attributes == null)
                throw LogArgumentNullException(nameof(attributes));

            bool actorElementWritten = false;

            using (var ms = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                {
                    foreach (var samlAttribute in attributes)
                    {
                        if (samlAttribute != null)
                        {
                            if (!actorElementWritten)
                            {
                                writer.WriteStartElement(Actor);
                                actorElementWritten = true;
                            }
                       //     Serializer.WriteAttribute(writer, samlAttribute);
                        }
                    }

                    if (actorElementWritten)
                        writer.WriteEndElement();

                    writer.Flush();
                }

                return Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }

        /// <summary>
        /// Creates claims from a <see cref="SamlAttributeStatement"/>.
        /// </summary>
        /// <param name="statement">The <see cref="SamlAttributeStatement"/>.</param>
        /// <param name="identity">A <see cref="ClaimsIdentity"/>.</param>
        /// <param name="issuer">The issuer.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="statement"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="identity"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenException">if Actor existing in both identity and attributes of statement.</exception>
        protected virtual void ProcessAttributeStatement(SamlAttributeStatement statement, ClaimsIdentity identity, string issuer)
        {
            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (identity == null)
                throw LogArgumentNullException(nameof(identity));

            foreach (var attribute in statement.Attributes)
            {
                if (StringComparer.Ordinal.Equals(attribute.Name, ClaimTypes.Actor))
                {
                    if (identity.Actor != null)
                        throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11312));

                    SetDelegateFromAttribute(attribute, identity, issuer);
                }
                else
                {
                    // each value has same issuer
                    string originalIssuer = attribute.OriginalIssuer ?? issuer;
                    foreach (var value in attribute.Values)
                    {
                        if (value != null)
                        {
                            var claim = new Claim(attribute.ClaimType, value, attribute.AttributeValueXsiType, issuer, originalIssuer);
                            identity.AddClaim(claim);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates claims from a <see cref="SamlAuthenticationStatement"/>.
        /// </summary>
        /// <param name="statement">The <see cref="SamlAuthenticationStatement"/>.</param>
        /// <param name="identity">A <see cref="ClaimsIdentity"/>.</param>
        /// <param name="issuer">The issuer.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="statement"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="identity"/> is null.</exception>
        protected virtual void ProcessAuthenticationStatement(SamlAuthenticationStatement statement, ClaimsIdentity identity, string issuer)
        {
            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (identity == null)
                throw LogArgumentNullException(nameof(identity));

            identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, statement.AuthenticationMethod, ClaimValueTypes.String, issuer));
            identity.AddClaim(new Claim(ClaimTypes.AuthenticationInstant, XmlConvert.ToString(statement.AuthenticationInstant.ToUniversalTime(), SamlConstants.GeneratedDateTimeFormat), ClaimValueTypes.DateTime, issuer));
        }

        /// <summary>
        /// Creates claims from a <see cref="SamlAuthorizationDecisionStatement"/>.
        /// </summary>
        /// <param name="statement">The <see cref="SamlAuthorizationDecisionStatement"/>.</param>
        /// <param name="identity">A <see cref="ClaimsIdentity"/>.</param>
        /// <param name="issuer">The issuer.</param>
        /// <remarks>Provided for extensibility. By default no claims are created.</remarks>
        protected virtual void ProcessAuthorizationDecisionStatement(SamlAuthorizationDecisionStatement statement, ClaimsIdentity identity, string issuer)
        {
        }

        /// <summary>
        /// Creates claims from a unknow statements.
        /// </summary>
        /// <param name="statement">The <see cref="SamlStatement"/>.</param>
        /// <param name="identity">A <see cref="ClaimsIdentity"/></param>
        /// <param name="issuer">The issuer.</param>
        protected virtual void ProcessCustomSubjectStatement(SamlStatement statement, ClaimsIdentity identity, string issuer)
        {
            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (LogHelper.IsEnabled(EventLogLevel.Warning))
                LogHelper.LogWarning(LogMessages.IDX11516, LogHelper.MarkAsNonPII(statement.GetType()));
        }

        /// <summary>
        /// Processes all statements to generate claims.
        /// </summary>
        /// <param name="samlToken">A <see cref="SamlSecurityToken"/> that will be used to create the claims.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="validationParameters"> Contains parameters for validating the securityToken.</param>
        /// <returns>A <see cref="IEnumerable{ClaimsIdentity}"/> containing the claims from the <see cref="SamlSecurityToken"/>.</returns>
        /// <exception cref="SamlSecurityTokenException">if the statement is not a <see cref="SamlSubjectStatement"/>.</exception>
        protected virtual IEnumerable<ClaimsIdentity> ProcessStatements(SamlSecurityToken samlToken, string issuer, TokenValidationParameters validationParameters)
        {
            if (samlToken == null)
                throw LogArgumentNullException(nameof(samlToken));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            var identityDict = new Dictionary<SamlSubject, ClaimsIdentity>(SamlSubjectEqualityComparer);
            foreach (var item in samlToken.Assertion.Statements)
            {
                var statement = item as SamlSubjectStatement;
                if (statement == null)
                    throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11515));

                if (!identityDict.TryGetValue(statement.Subject, out ClaimsIdentity identity))
                {
                    identity = validationParameters.CreateClaimsIdentity(samlToken, issuer);
                    ProcessSubject(statement.Subject, identity, issuer);
                    identityDict.Add(statement.Subject, identity);
                }

                if (statement is SamlAttributeStatement attrStatement)
                    ProcessAttributeStatement(attrStatement, identity, issuer);
                else if (statement is SamlAuthenticationStatement authnStatement)
                    ProcessAuthenticationStatement(authnStatement, identity, issuer);
                else if (statement is SamlAuthorizationDecisionStatement authzStatement)
                    ProcessAuthorizationDecisionStatement(authzStatement, identity, issuer);
                else
                    ProcessCustomSubjectStatement(statement, identity, issuer);
            }

            return identityDict.Values;
        }

        /// <summary>
        /// Creates subject claims from the <see cref="SamlSubject"/>.
        /// </summary>
        /// <param name="subject">The <see cref="SamlSubject"/>.</param>
        /// <param name="identity">The <see cref="ClaimsIdentity"/> subject.</param>
        /// <param name="issuer">The issuer.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="subject"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="identity"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenException">if the <see cref="SamlSubject"/> doesn't have the name or confirmation methonds.</exception>
        protected virtual void ProcessSubject(SamlSubject subject, ClaimsIdentity identity, string issuer)
        {
            if (subject == null)
                throw LogArgumentNullException(nameof(subject));

            if (identity == null)
                throw LogArgumentNullException(nameof(identity));

            if (string.IsNullOrEmpty(subject.Name) && subject.ConfirmationMethods.Count == 0)
                throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11313));

            if (!string.IsNullOrEmpty(subject.Name))
            {
                var claim = new Claim(ClaimTypes.NameIdentifier, subject.Name, ClaimValueTypes.String, issuer);
                if (!string.IsNullOrEmpty(subject.NameFormat))
                    claim.Properties[ClaimProperties.SamlNameIdentifierFormat] = subject.NameFormat;

                if (!string.IsNullOrEmpty(subject.NameQualifier))
                    claim.Properties[ClaimProperties.SamlNameIdentifierNameQualifier] = subject.NameQualifier;

                identity.AddClaim(claim);
            }
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="SamlSecurityToken"/>.
        /// </summary>
        /// <param name="token">a Saml token as a string.</param>
        /// <returns>A <see cref="SamlSecurityToken"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        public override SecurityToken ReadToken(string token)
        {
            return ReadSamlToken(token);
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="SamlSecurityToken"/>.
        /// </summary>
        /// <param name="token">a Saml token as a string.</param>
        /// <returns>A <see cref="SamlSecurityToken"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        public virtual SamlSecurityToken ReadSamlToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw LogArgumentNullException(nameof(token));

            if (token.Length > MaximumTokenSizeInBytes)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

            using (var reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(token), XmlDictionaryReaderQuotas.Max))
            {
                return ReadSamlToken(reader);
            }
        }

        /// <summary>
        /// Reads a <see cref="SamlSecurityToken"/> where the XmlReader is positioned the beginning of a Saml assertion.
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/> positioned at a 'saml:assertion' element.</param>
        /// <returns>An instance of <see cref="SamlSecurityToken"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        public override SecurityToken ReadToken(XmlReader reader)
        {
            return ReadSamlToken(reader);
        }

        /// <summary>
        /// Reads a <see cref="SamlSecurityToken"/> where the XmlReader is positioned the beginning of a Saml assertion.
        /// </summary>
        /// <param name="reader">a <see cref="XmlReader"/> positioned at a 'saml:assertion' element.</param>
        /// <returns>A <see cref="SamlSecurityToken"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenReadException">If <see cref="SamlSerializer.ReadAssertion(XmlReader)"/> returns null.</exception>
        public virtual SamlSecurityToken ReadSamlToken(XmlReader reader)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            var assertion = Serializer.ReadAssertion(reader);
            if (assertion == null)
                throw LogExceptionMessage(
                    new SamlSecurityTokenReadException(FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ReadSamlToken"), LogHelper.MarkAsNonPII(Serializer.GetType()), LogHelper.MarkAsNonPII("ReadAssertion"), LogHelper.MarkAsNonPII(typeof(SamlAssertion)))));

            return new SamlSecurityToken(assertion);
        }

        /// <summary>
        /// Deserializes from XML a token of the type handled by this instance.
        /// </summary>
        /// <param name="reader">An XML reader positioned at the token's start 
        /// element.</param>
        /// <param name="validationParameters"> validation parameters for the <see cref="SamlSecurityToken"/>.</param>
        /// <returns>An instance of <see cref="SamlSecurityToken"/>.</returns>
        public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters)
        {
            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogMessages.IDX11950));
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use for validating the signature of a token.
        /// </summary>
        /// <param name="token">The <see cref="string"/> representation of the token that is being validated.</param>
        /// <param name="securityToken">The <see cref="SamlSecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> that will be used during validation.</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="securityToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <see cref="SamlSecurityToken.Assertion"/> is null.</exception>
        /// <remarks>If key fails to resolve, then null is returned</remarks>
        protected virtual SecurityKey ResolveIssuerSigningKey(string token, SamlSecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (securityToken == null)
                throw LogArgumentNullException(nameof(securityToken));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            if (securityToken.Assertion == null)
                throw LogArgumentNullException(nameof(securityToken.Assertion));

            return SamlTokenUtilities.ResolveTokenSigningKey(securityToken.Assertion.Signature.KeyInfo, validationParameters);
        }

        /// <summary>
        /// This method gets called when a special type of SamlAttribute is detected. The SamlAttribute passed in wraps a SamlAttribute 
        /// that contains a collection of AttributeValues, each of which are mapped to a claim.  All of the claims will be returned
        /// in an ClaimsIdentity with the specified issuer.
        /// </summary>
        /// <param name="attribute">The SamlAttribute to be processed.</param>
        /// <param name="subject">The identity that should be modified to reflect the SamlAttribute.</param>
        /// <param name="issuer">Issuer Identity.</param>
        /// <exception cref="SamlSecurityTokenException">if we have two delegates acting as an identity, we do not allow this.</exception>
        protected virtual void SetDelegateFromAttribute(SamlAttribute attribute, ClaimsIdentity subject, string issuer)
        {
            // bail here nothing to add.
            if (subject == null || attribute == null || attribute.Values == null || attribute.Values.Count < 1)
                return;

            var claims = new Collection<Claim>();
            SamlAttribute actingAsAttribute = null;
            foreach (string attributeValue in attribute.Values)
            {
                if (attributeValue != null && attributeValue.Length > 0)
                {
                    using (var xmlReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(attributeValue), XmlDictionaryReaderQuotas.Max))
                    {
                        xmlReader.MoveToContent();
                        xmlReader.ReadStartElement(Actor);
                        while (xmlReader.IsStartElement(SamlConstants.Elements.Attribute))
                        {
                            var innerAttribute = Serializer.ReadAttribute(xmlReader);
                            if (innerAttribute != null)
                            {
                                string claimType = string.IsNullOrEmpty(innerAttribute.Namespace) ? innerAttribute.Name : innerAttribute.Namespace + "/" + innerAttribute.Name;
                                if (claimType == ClaimTypes.Actor)
                                {
                                    // In this case we have two delegates acting as an identity, we do not allow this
                                    if (actingAsAttribute != null)
                                        throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11522));

                                    actingAsAttribute = innerAttribute;
                                }
                                else
                                {
                                    string claimValueType = ClaimValueTypes.String;
                                    string originalIssuer = null;

                                    if (innerAttribute is SamlAttribute SamlAttribute)
                                    {
                                        claimValueType = SamlAttribute.AttributeValueXsiType;
                                        originalIssuer = SamlAttribute.OriginalIssuer;
                                    }

                                    foreach (var value in innerAttribute.Values)
                                    {
                                        Claim claim = null;
                                        if (string.IsNullOrEmpty(originalIssuer))
                                            claim = new Claim(claimType, value, claimValueType, issuer);
                                        else
                                            claim = new Claim(claimType, value, claimValueType, issuer, originalIssuer);

                                        claims.Add(claim);
                                    }
                                }
                            }
                        }

                        xmlReader.ReadEndElement(); // Actor
                    }
                }
            }

            subject.Actor = new ClaimsIdentity(claims, "Federation");
            SetDelegateFromAttribute(actingAsAttribute, subject.Actor, issuer);
        }

        /// <summary>
        /// Determines if the audiences found in a <see cref="SamlSecurityToken"/> are valid.
        /// </summary>
        /// <param name="audiences"><see cref="IEnumerable{String}"/>.</param>
        /// <param name="securityToken">The <see cref="SamlSecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <remarks>see <see cref="Validators.ValidateAudience"/> for additional details.</remarks>
        protected virtual void ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateAudience(audiences, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates the Lifetime and Audience conditions.
        /// </summary>
        /// <param name="securityToken">a <see cref="SamlSecurityToken"/> that contains the <see cref="SamlConditions"/>.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="securityToken"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If <see cref="SamlSecurityToken.Assertion"/> is null.</exception>
        /// <exception cref="SecurityTokenValidationException">if the Condition 'OneTimeUse' is specified. Requires overriding.</exception>
        /// <exception cref="SecurityTokenValidationException">if the Condition 'ProxyRestriction' is specified. Requires overriding.</exception>
        protected virtual void ValidateConditions(SamlSecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (securityToken == null)
                throw LogArgumentNullException(nameof(securityToken));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            if (securityToken.Assertion == null)
                throw LogArgumentNullException(nameof(securityToken.Assertion));

            if (securityToken.Assertion.Conditions == null)
            {
                if (validationParameters.RequireAudience)
                    throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11401));
               
                return;
            }

            ValidateLifetime(securityToken.Assertion.Conditions.NotBefore, securityToken.Assertion.Conditions.NotOnOrAfter, securityToken, validationParameters);

            var foundAudienceRestriction = false;
            foreach (var condition in securityToken.Assertion.Conditions.Conditions)
            {
                if (condition is SamlAudienceRestrictionCondition audienceRestriction)
                {
                    if (!foundAudienceRestriction)
                        foundAudienceRestriction = true;

                    ValidateAudience(audienceRestriction.Audiences.ToDictionary(x => x.OriginalString).Keys, securityToken, validationParameters);
                }
            }

            if (validationParameters.RequireAudience && !foundAudienceRestriction)
                throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11401));
        }

        /// <summary>
        /// Determines if an issuer found in a <see cref="SamlSecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="SamlSecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>The issuer to use when creating the <see cref="Claim"/>(s) in the <see cref="ClaimsIdentity"/>.</returns>
        /// <remarks><see cref="Validators.ValidateIssuer(string, SecurityToken, TokenValidationParameters)"/> for additional details.</remarks>
        protected virtual string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return Validators.ValidateIssuer(issuer, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates the <see cref="SecurityToken"/> was signed by a valid <see cref="SecurityKey"/>.
        /// </summary>
        /// <param name="securityKey">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">The current <see cref="TokenValidationParameters"/>.</param>
        protected virtual void ValidateIssuerSecurityKey(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
           Validators.ValidateIssuerSecurityKey(securityKey, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates the lifetime of a <see cref="SamlSecurityToken"/>.
        /// </summary>
        /// <param name="notBefore">The <see cref="DateTime"/> value found in the <see cref="SamlSecurityToken"/>.</param>
        /// <param name="expires">The <see cref="DateTime"/> value found in the <see cref="SamlSecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SamlSecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <remarks><see cref="Validators.ValidateLifetime"/> for additional details.</remarks>
        protected virtual void ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateLifetime(notBefore, expires, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates that the signature, if found is valid.
        /// </summary>
        /// <param name="token">A Saml token.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> that will be used during validation.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="SecurityTokenValidationException">If <see cref="ReadSamlToken(string)"/> returns null"/>.</exception>
        /// <exception cref="SecurityTokenValidationException">If <see cref="TokenValidationParameters.SignatureValidator"/> returns null OR an object other than a <see cref="SamlSecurityToken"/>.</exception>
        /// <exception cref="SecurityTokenValidationException">If a signature is not found and <see cref="TokenValidationParameters.RequireSignedTokens"/> is true.</exception>
        /// <exception cref="SecurityTokenSignatureKeyNotFoundException">If the 'token' has a key identifier and none of the <see cref="SecurityKey"/>(s) provided result in a validated signature. 
        /// This can indicate that a key refresh is required.</exception>
        /// <exception cref="SecurityTokenInvalidSignatureException">If after trying all the <see cref="SecurityKey"/>(s), none result in a validated signture AND the 'token' does not have a key identifier.</exception>
        /// <returns>A <see cref="SamlSecurityToken"/> that has had the signature validated if token was signed.</returns>
        /// <remarks><para>If the 'token' is signed, the signature is validated even if <see cref="TokenValidationParameters.RequireSignedTokens"/> is false.</para>
        /// <para>If the 'token' signature is validated, then the <see cref="SamlSecurityToken.SigningKey"/> will be set to the key that signed the 'token'. It is the responsibility of <see cref="TokenValidationParameters.SignatureValidator"/> to set the <see cref="SamlSecurityToken.SigningKey"/></para></remarks>
        protected virtual SamlSecurityToken ValidateSignature(string token, TokenValidationParameters validationParameters)
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

                if (!(validatedSamlToken is SamlSecurityToken validatedSaml))
                    throw LogExceptionMessage(new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10506, LogHelper.MarkAsNonPII(typeof(SamlSecurityToken)), LogHelper.MarkAsNonPII(validatedSamlToken.GetType()), token)));

                return validatedSaml;
            }

            var samlToken = ReadSamlToken(token);
            if (samlToken == null)
                throw LogExceptionMessage(
                    new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateSignature"), LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ReadSamlToken"), LogHelper.MarkAsNonPII(typeof(SamlSecurityToken)))));

            return ValidateSignature(samlToken, samlToken.Assertion.CanonicalString, validationParameters);
        }

        private SamlSecurityToken ValidateSignature(SamlSecurityToken samlToken, string token, TokenValidationParameters validationParameters)
        {
            if (samlToken.Assertion.Signature == null)
            {
                if (validationParameters.RequireSignedTokens)
                    throw LogExceptionMessage(new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10504, token)));
                else
                    return samlToken;
            }

            bool keyMatched = false;
            IEnumerable<SecurityKey> keys = null;
            if (validationParameters.IssuerSigningKeyResolver != null)
            {
                keys = validationParameters.IssuerSigningKeyResolver(token, samlToken, samlToken.Assertion.Signature.KeyInfo?.Id, validationParameters);
            }
            else
            {
                var securityKey = ResolveIssuerSigningKey(token, samlToken, validationParameters);
                if (securityKey != null)
                {
                    // remember that key was matched for throwing exception SecurityTokenSignatureKeyNotFoundException
                    keyMatched = true;
                    keys = new List<SecurityKey> { securityKey };
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
        /// Validates the <see cref="SamlSecurityToken.SigningKey"/> is an expected value.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="SamlSecurityToken"/> to validate.</param>
        /// <param name="validationParameters">The current <see cref="TokenValidationParameters"/>.</param>
        /// <remarks>If the <see cref="SamlSecurityToken.SigningKey"/> is a <see cref="X509SecurityKey"/> then the X509Certificate2 will be validated using the CertificateValidator.</remarks>
        protected virtual void ValidateIssuerSecurityKey(SecurityKey key, SamlSecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateIssuerSecurityKey(key, securityToken, validationParameters);
        }

        /// <summary>
        /// Validates the token replay.
        /// </summary>
        /// <param name="expiration">expiration time of the <see cref="SamlSecurityToken"/></param>
        /// <param name="token"><see cref="SamlSecurityToken"/> to validate</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> that will be used during validation</param>
        protected virtual void ValidateTokenReplay(DateTime? expiration, string token, TokenValidationParameters validationParameters)
        {
            Validators.ValidateTokenReplay(expiration, token, validationParameters);
        }

        /// <summary>
        /// Reads and validates a well formed <see cref="SamlSecurityToken"/>.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> reader positioned at a saml assertion element.</param>
        /// <param name="validationParameters">Contains data and information needed for validation.</param>
        /// <param name="validatedToken">The <see cref="SecurityToken"/> that was validated.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> generated from the claims in the saml assertion.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="SecurityTokenValidationException">if <see cref="ReadSamlToken(XmlReader)"/> returns null."</exception>
        public override ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            validationParameters = SamlTokenUtilities.PopulateValidationParametersWithCurrentConfigurationAsync(validationParameters).ConfigureAwait(false).GetAwaiter()
                .GetResult();

            var samlToken = ReadSamlToken(reader);
            if (samlToken == null)
                throw LogExceptionMessage(
                    new SecurityTokenValidationException(FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateToken"), LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ReadSamlToken"), LogHelper.MarkAsNonPII(typeof(SamlSecurityToken)))));

            ValidateSignature(samlToken, samlToken.Assertion.CanonicalString, validationParameters);

            return ValidateToken(samlToken, samlToken.Assertion.CanonicalString, validationParameters, out validatedToken);
        }

        /// <inheritdoc/>
        public override async Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw LogArgumentNullException(nameof(token));

                if (validationParameters == null)
                    throw LogArgumentNullException(nameof(validationParameters));

                if (token.Length > MaximumTokenSizeInBytes)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

                validationParameters = await SamlTokenUtilities.PopulateValidationParametersWithCurrentConfigurationAsync(validationParameters).ConfigureAwait(false);

                var samlToken = ValidateSignature(token, validationParameters);
                if (samlToken == null)
                    throw LogExceptionMessage(new SecurityTokenValidationException(
                        FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateToken"), LogHelper.MarkAsNonPII(GetType()), LogHelper.MarkAsNonPII("ValidateSignature"), LogHelper.MarkAsNonPII(typeof(SamlSecurityToken)))));

                var claimsPrincipal = ValidateToken(samlToken, token, validationParameters, out var validatedToken);
                return new TokenValidationResult
                {
                    SecurityToken = validatedToken,
                    ClaimsIdentity = claimsPrincipal?.Identities.First(),
                    IsValid = true,
                };
            }
            catch (Exception ex)
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Reads and validates a well formed <see cref="SamlSecurityToken"/>.
        /// </summary>
        /// <param name="token">A string containing a well formed securityToken.</param>
        /// <param name="validationParameters">Contains data and information needed for validation.</param>
        /// <param name="validatedToken">The <see cref="SecurityToken"/> that was validated.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> generated from the claims in the Saml securityToken.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="token"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentException">if 'securityToken.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw LogArgumentNullException(nameof(token));

            if (validationParameters == null)
                throw LogArgumentNullException(nameof(validationParameters));

            if (token.Length > MaximumTokenSizeInBytes)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

            validationParameters = SamlTokenUtilities.PopulateValidationParametersWithCurrentConfigurationAsync(validationParameters).ConfigureAwait(false).GetAwaiter()
                .GetResult();

            var samlToken = ValidateSignature(token, validationParameters);
            if (samlToken == null)
                throw LogExceptionMessage(new SecurityTokenValidationException(
                    FormatInvariant(TokenLogMessages.IDX10254, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("ValidateToken"), LogHelper.MarkAsNonPII(GetType()), LogHelper.MarkAsNonPII("ValidateSignature"), LogHelper.MarkAsNonPII(typeof(SamlSecurityToken)))));

            return ValidateToken(samlToken, token, validationParameters, out validatedToken);
        }

        private ClaimsPrincipal ValidateToken(SamlSecurityToken samlToken, string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ValidateConditions(samlToken, validationParameters);
            var issuer = ValidateIssuer(samlToken.Issuer, samlToken, validationParameters);

            if (samlToken.Assertion.Conditions != null)
                ValidateTokenReplay(samlToken.Assertion.Conditions.NotOnOrAfter, samlToken.Assertion.CanonicalString, validationParameters);

            ValidateIssuerSecurityKey(samlToken.SigningKey, samlToken, validationParameters);
            validatedToken = samlToken;
            var identities = CreateClaimsIdentities(samlToken, issuer, validationParameters);
            if (validationParameters.SaveSigninToken)
            {
                identities.ElementAt(0).BootstrapContext = samlToken.Assertion.CanonicalString;
            }

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(
                    TokenLogMessages.IDX10241,
                    LogHelper.MarkAsUnsafeSecurityArtifact(token, t => t.ToString()));

            return new ClaimsPrincipal(identities);
        }

        /// <summary>
        /// Serializes a <see cref="SamlSecurityToken"/> to a string.
        /// </summary>
        /// <param name="token">A <see cref="SamlSecurityToken"/>.</param>
        /// <exception cref="ArgumentNullException">if the <paramref name="token"/> is null.</exception>
        /// <exception cref="ArgumentException">if the token is not a <see cref="SamlSecurityToken"/>.</exception>
        public override string WriteToken(SecurityToken token)
        {
            if (token == null)
                throw LogArgumentNullException(nameof(token));

            var samlToken = token as SamlSecurityToken;
            if (samlToken == null)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX11400, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII(typeof(SamlSecurityToken)), LogHelper.MarkAsNonPII(token.GetType()))));

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
        /// Serializes to XML a securityToken of the type handled by this instance.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="token">A securityToken of type <see cref="TokenType"/>.</param>
        /// <exception cref="ArgumentNullException">if the <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if the <paramref name="token"/> is null.</exception>
        /// <exception cref="ArgumentException">if the token is not a <see cref="SamlSecurityToken"/>.</exception>
        /// <exception cref="ArgumentNullException">if <see cref="SamlSecurityToken.Assertion"/> is null.</exception>
        public override void WriteToken(XmlWriter writer, SecurityToken token)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (token == null)
                throw LogArgumentNullException(nameof(token));

            var samlToken = token as SamlSecurityToken;
            if (samlToken == null)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX11400, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII(typeof(SamlSecurityToken)), LogHelper.MarkAsNonPII(token.GetType()))));

            if (samlToken.Assertion == null)
                throw LogArgumentNullException(nameof(samlToken.Assertion));

            Serializer.WriteAssertion(writer, samlToken.Assertion);
        }

#endregion methods
    }
}
