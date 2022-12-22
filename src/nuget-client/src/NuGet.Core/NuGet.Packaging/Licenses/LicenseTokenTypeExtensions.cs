// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Licenses
{
    internal static class LicenseTokenTypeExtensions
    {
        public static bool IsOperator(this LicenseTokenType tokenType)
        {
            return tokenType == LicenseTokenType.WITH || tokenType == LicenseTokenType.AND || tokenType == LicenseTokenType.OR;
        }

        public static bool IsValidPrecedingToken(this LicenseTokenType current, LicenseTokenType precedingToken)
        {
            switch (current)
            {
                case LicenseTokenType.OPENING_BRACKET: // Legal preceding tokens: None, Operator, OpeningBracket
                    return precedingToken.IsOperator() || current == precedingToken;
                case LicenseTokenType.CLOSING_BRACKET: // Legal preceding tokens: ClosingBracket, Identifier
                    return precedingToken == LicenseTokenType.IDENTIFIER || precedingToken == LicenseTokenType.CLOSING_BRACKET;
                case LicenseTokenType.IDENTIFIER: // Legal preceding tokens: None, Operator, OpeningBracket
                    return precedingToken.IsOperator() || precedingToken == LicenseTokenType.OPENING_BRACKET;
                case LicenseTokenType.AND: // Legal preceding tokens: Identifier, ClosingBracket
                case LicenseTokenType.WITH:
                case LicenseTokenType.OR:
                    return precedingToken == LicenseTokenType.IDENTIFIER || precedingToken == LicenseTokenType.CLOSING_BRACKET;
                default:
                    return false;
            }
        }
    }
}
