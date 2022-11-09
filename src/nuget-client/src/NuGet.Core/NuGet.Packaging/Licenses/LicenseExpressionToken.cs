// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Packaging.Licenses
{
    /// <summary>
    /// Represents a token of a parsed license expression. The tokens are either operators, parentheses or values. 
    /// </summary>
    internal class LicenseExpressionToken
    {
        /// <summary>
        /// The token type
        /// </summary>
        internal LicenseTokenType TokenType { get; }

        /// <summary>
        /// The value of this token.
        /// </summary>
        internal string Value { get; }

        internal LicenseExpressionToken(string value, LicenseTokenType tokenType)
        {
            Value = value ?? throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeNullOrEmpty, nameof(value)));
            TokenType = tokenType;
        }

        public override string ToString()
        {
            return $"Value: {Value}, Type: {TokenType}";
        }
    }
}
