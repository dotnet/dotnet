// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Microsoft.IdentityModel.Validators
// Range: 40000 - 40999

namespace Microsoft.IdentityModel.Validators
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
        // general
        // public const string IDX40000 = "IDX40000:";

        // Token validation
        public const string IDX40001 = "IDX40001: Issuer: '{0}', does not match any of the valid issuers provided for this application. ";
        public const string IDX40002 = "IDX40002: Microsoft.IdentityModel does not support a B2C issuer with 'tfp' in the URI. See https://aka.ms/ms-id-web/b2c-issuer for details. ";

        // Protocol
        public const string IDX40003 = "IDX40003: Neither `tid` nor `tenantId` claim is present in the token obtained from Microsoft identity platform. ";
    }
}
