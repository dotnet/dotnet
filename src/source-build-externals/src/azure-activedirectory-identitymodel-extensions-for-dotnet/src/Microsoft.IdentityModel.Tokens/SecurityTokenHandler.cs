// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Defines the interface for a Security Token Handler.
    /// </summary>
    public abstract class SecurityTokenHandler : TokenHandler, ISecurityTokenValidator
    {

        /// <summary>
        /// Creates an instance of <see cref="SecurityTokenHandler"/>
        /// </summary>
        protected SecurityTokenHandler()
        {
        }

        /// <summary>
        /// Returns <see cref="SecurityKeyIdentifierClause"/>.
        /// </summary>
        /// <param name="token"><see cref="SecurityToken"/></param>
        /// <param name="attached">true if attached; otherwise, false.</param>
        public virtual SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="tokenDescriptor"><see cref="SecurityTokenDescriptor"/></param>
        public virtual SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether this handler supports validation of tokens 
        /// handled by this instance.
        /// </summary>v
        /// <returns>'True' if the instance is capable of SecurityToken
        /// validation.</returns>
        public virtual bool CanValidateToken
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the class provides serialization functionality to serialize token handled 
        /// by this instance.
        /// </summary>
        /// <returns>true if the WriteToken method can serialize this token.</returns>
        public virtual bool CanWriteToken
        {
            get { return false; }
        }

        /// <summary>
        /// This must be overridden to get the System.Type of the SecurityToken this instance handles.
        /// </summary>
        public abstract Type TokenType
        {
            get;
        }

        /// <summary>
        /// Indicates whether the <see cref="XmlReader"/> is positioned at an element that can be read.
        /// </summary>
        /// <param name="reader">An <see cref="XmlReader"/> reader positioned at a start element. The reader should not be advanced.</param>
        /// <returns>'true' if the token can be read.</returns>
        public virtual bool CanReadToken(XmlReader reader)
        {
            return false;
        }

        /// <summary>
        /// Indicates whether the current token string can be read as a token 
        /// of the type handled by this instance.
        /// </summary>
        /// <param name="tokenString">The token string thats needs to be read.</param>
        /// <returns>'True' if the ReadToken method can parse the token string.</returns>
        public virtual bool CanReadToken(string tokenString)
        {
            return false;
        }

        /// <summary>
        /// Gets security token.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/>.</param>
        /// <remarks>SecurityToken instance which represents the serialized token.</remarks>
        public virtual SecurityToken ReadToken(XmlReader reader)
        {
            return null;
        }

        /// <summary>
        /// Serializes to string a token of the type handled by this instance.
        /// </summary>
        /// <param name="token">A token of type TokenType.</param>
        /// <returns>The serialized token.</returns>
        public virtual string WriteToken(SecurityToken token)
        {
            return null;
        }

        /// <summary>
        /// This must be overridden to serialize to XML a token of the type handled by this instance.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="token">A token of type <see cref="TokenType"/>.</param>
        public abstract void WriteToken(XmlWriter writer, SecurityToken token);

        /// <summary>
        /// This must be overridden to deserialize token with the provided <see cref="TokenValidationParameters"/>.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/>.</param>
        /// <param name="validationParameters">the current <see cref="TokenValidationParameters"/>.</param>
        /// <remarks>SecurityToken instance which represents the serialized token.</remarks>
        public abstract SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters);

        /// <summary>
        /// This must be overridden to validate a token passed as a string using <see cref="TokenValidationParameters"/>
        /// </summary>
        /// <param name="securityToken">A token of type <see cref="TokenType"/>.</param>
        /// <param name="validationParameters">the current <see cref="TokenValidationParameters"/>.</param>
        /// <param name="validatedToken">The token of type <see cref="TokenType"/> that was validated.</param>
        public virtual ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads and validates a token using a xmlReader and <see cref="TokenValidationParameters"/>
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> pointing at the start element of the token.</param>
        /// <param name="validationParameters">Contains data and information needed for validation.</param>
        /// <param name="validatedToken">The <see cref="SecurityToken"/> that was validated.</param>
        public virtual ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            throw new NotImplementedException();
        }
    }
}
