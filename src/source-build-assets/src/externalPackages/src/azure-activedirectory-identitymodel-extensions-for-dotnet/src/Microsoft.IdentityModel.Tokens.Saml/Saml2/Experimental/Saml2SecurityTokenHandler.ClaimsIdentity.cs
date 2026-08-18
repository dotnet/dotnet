// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Saml2 Tokens. See: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public partial class Saml2SecurityTokenHandler : SecurityTokenHandler
    {
        internal override ClaimsIdentity CreateClaimsIdentityInternal(SecurityToken securityToken, ValidationParameters validationParameters, string issuer)
        {
            return CreateClaimsIdentity((Saml2SecurityToken)securityToken, validationParameters, issuer);
        }

        internal ClaimsIdentity CreateClaimsIdentity(Saml2SecurityToken samlToken, ValidationParameters validationParameters, string issuer)
        {
            if (samlToken == null)
                throw LogHelper.LogArgumentNullException(nameof(samlToken));

            if (samlToken.Assertion == null)
                throw LogHelper.LogArgumentNullException(LogMessages.IDX13110);

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            string actualIssuer = issuer;
            if (string.IsNullOrWhiteSpace(issuer))
                actualIssuer = ClaimsIdentity.DefaultIssuer;

            ClaimsIdentity identity = validationParameters.CreateClaimsIdentity(samlToken, actualIssuer);

            ProcessSubject(samlToken.Assertion.Subject, identity, actualIssuer);
            ProcessStatements(samlToken.Assertion.Statements, identity, actualIssuer);

            return identity;
        }
    }
}
#nullable restore
