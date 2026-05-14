// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Licenses
{
    /// <summary>
    /// The valid token types in a license expression. These are ordered by priority, be aware when changing them. See <seealso cref="LicenseTokenTypeExtensions"/>
    /// </summary>
    internal enum LicenseTokenType
    {
        WITH,
        AND,
        OR,
        OPENING_BRACKET,
        CLOSING_BRACKET,
        IDENTIFIER,
    }
}
