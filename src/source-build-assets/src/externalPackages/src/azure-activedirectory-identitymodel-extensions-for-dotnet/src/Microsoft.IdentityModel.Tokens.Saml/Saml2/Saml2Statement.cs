// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the StatementAbstractType specified in [Saml2Core, 2.7.1].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    /// <remarks>
    /// This abstract class provides no operations; however, this type is used
    /// to declare collections of statements, for example Saml2Assertion.Statements.
    /// </remarks>
    public abstract class Saml2Statement
    {
    }
}
