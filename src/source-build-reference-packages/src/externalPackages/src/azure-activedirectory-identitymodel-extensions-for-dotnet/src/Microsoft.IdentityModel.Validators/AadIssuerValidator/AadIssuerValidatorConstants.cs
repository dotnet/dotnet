// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Validators
{
    /// <summary>
    /// General constants for AAD Issuer Validator.
    /// </summary>
    internal class AadIssuerValidatorConstants
    {
        public const string Organizations = "organizations";
        public const string Common = "common";
        public const string OidcEndpoint = "/.well-known/openid-configuration";
        public const string FallbackAuthority = "https://login.microsoftonline.com/";

        /// <summary>
        /// Old TenantId claim: "http://schemas.microsoft.com/identity/claims/tenantid".
        /// </summary>
        public const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

        /// <summary>
        /// New Tenant Id claim: "tid".
        /// </summary>
        public const string Tid = "tid";

        /// <summary>
        /// Tfp claim: "tfp".
        /// </summary>
        public const string Tfp = "tfp";
    }
}
