// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the StatementAbstractType specified in [Saml, 2.4].
    /// </summary>
    /// <remarks>
    /// This abstract class provides no operations; however, this type is used
    /// to declare collections of statements, for example SamlAssertion.Statements.
    /// </remarks>
    public abstract class SamlStatement
    {
    }
}
