// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Saml Tokens. See: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public partial class SamlSecurityTokenHandler : SecurityTokenHandler
    {
        internal override ClaimsIdentity CreateClaimsIdentityInternal(SecurityToken securityToken, ValidationParameters validationParameters, string issuer)
        {
            return CreateClaimsIdentity((SamlSecurityToken)securityToken, validationParameters, issuer);
        }

        internal ClaimsIdentity CreateClaimsIdentity(SamlSecurityToken samlToken, ValidationParameters validationParameters, string issuer)
        {
            if (samlToken == null)
                throw LogHelper.LogArgumentNullException(nameof(samlToken));

            if (samlToken.Assertion == null)
                throw LogHelper.LogArgumentNullException(LogMessages.IDX11110);

            var actualIssuer = issuer;
            if (string.IsNullOrWhiteSpace(issuer))
                actualIssuer = ClaimsIdentity.DefaultIssuer;

            IEnumerable<ClaimsIdentity> identities = ProcessStatements(
                samlToken,
                actualIssuer,
                validationParameters);

            return identities.First();
        }

        /// <summary>
        /// Processes all statements to generate claims.
        /// </summary>
        /// <param name="samlToken">A <see cref="SamlSecurityToken"/> that will be used to create the claims.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validating the token.</param>
        /// <returns>A <see cref="IEnumerable{ClaimsIdentity}"/> containing the claims from the <see cref="SamlSecurityToken"/>.</returns>
        /// <exception cref="SamlSecurityTokenException">if the statement is not a <see cref="SamlSubjectStatement"/>.</exception>
        internal virtual IEnumerable<ClaimsIdentity> ProcessStatements(SamlSecurityToken samlToken, string issuer, ValidationParameters validationParameters)
        {
            if (samlToken == null)
                throw LogHelper.LogArgumentNullException(nameof(samlToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            var identityDict = new Dictionary<SamlSubject, ClaimsIdentity>(SamlSubjectEqualityComparer);
            foreach (SamlStatement? item in samlToken.Assertion.Statements)
            {
                if (item is not SamlSubjectStatement statement)
                    throw LogHelper.LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11515));

                if (!identityDict.TryGetValue(statement.Subject, out ClaimsIdentity? identity))
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
    }
}
#nullable restore
