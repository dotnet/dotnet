// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// A security token backed by a SAML assertion.
    /// </summary>
    public class SamlSecurityToken : SecurityToken
    {
        /// <summary>
        /// Initializes an instance of <see cref="SamlSecurityToken"/>.
        /// </summary>
        protected SamlSecurityToken()
        {
            Assertion = new SamlAssertion("_" + Guid.NewGuid().ToString(), ClaimsIdentity.DefaultIssuer, DateTime.UtcNow, new SamlConditions(), new SamlAdvice(), new List<SamlStatement>());
        }

        /// <summary>
        /// Initializes an instance of <see cref="SamlSecurityToken"/>.
        /// </summary>
        /// <param name="assertion">A <see cref="SamlAssertion"/> to initialize from.</param>
        public SamlSecurityToken(SamlAssertion assertion)
        {
            Assertion = assertion ?? throw LogArgumentNullException(nameof(assertion));
        }

        /// <summary>
        /// Gets the <see cref="SamlAssertion"/> for this token.
        /// </summary>
        public SamlAssertion Assertion
        {
            get;
        }

        /// <summary>
        /// Gets the SecurityToken id.
        /// </summary>
        public override string Id
        {
            get { return Assertion.AssertionId; }
        }

        /// <summary>
        /// Gets the issuer of this token
        /// </summary>
        public override string Issuer
        {
            get { return Assertion.Issuer; }
        }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> for this instance.
        /// </summary>
        public override SecurityKey SecurityKey
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SecurityKey"/> that was used to Sign this assertion.
        /// </summary>
        public override SecurityKey SigningKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the time the token is valid from. This value is always in UTC.
        /// </summary>
        public override DateTime ValidFrom
        {
            get
            {
                if (Assertion.Conditions != null)
                {
                    return Assertion.Conditions.NotBefore;
                }

                return DateTimeUtil.GetMinValue(DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Gets the time the token is valid to. This value is always in UTC.
        /// </summary>
        public override DateTime ValidTo
        {
            get
            {
                if (Assertion.Conditions != null)
                {
                    return Assertion.Conditions.NotOnOrAfter;
                }

                return DateTimeUtil.GetMaxValue(DateTimeKind.Utc);
            }
        }
    }
}
