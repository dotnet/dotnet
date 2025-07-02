// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Constants for JsonWebKeyUse (sec 4.2)
    /// https://datatracker.ietf.org/doc/html/rfc7517#section-4.2
    /// </summary>
    public static class JsonWebKeyUseNames
    {
#pragma warning disable 1591
        public const string Sig = "sig";
        public const string Enc = "enc";
#pragma warning restore 1591
    }
}
