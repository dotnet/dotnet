// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Validators.Tests
{
    internal class ValidatorConstants
    {
        // AadIssuerValidation
        public const string AadAuthority = "aadAuthority";
        public const string InvalidAuthorityFormat = "login.microsoft.com";
        public const string Issuer = "issuer";
        public const string SecurityToken = "securityToken";
        public const string ValidationParameters = "validationParameters";
        public const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

        public const string TenantIdAsGuid = "f645ad92-e38d-4d1a-b510-d1b09a74a8ca";
        public const string ProductionPrefNetworkUSEnvironment = "login.microsoftonline.us";
        public const string AadInstance = "https://login.microsoftonline.com";
        public const string AuthorityV1 = AadInstance + "/common";
        public const string AuthorityCommonTenant = AadInstance + "/common/";
        public const string AuthorityOrganizationsTenant = AadInstance + "/organizations/";
        public const string AuthorityOrganizationsUSTenant = "https://" + ProductionPrefNetworkUSEnvironment + "/organizations";
        public const string Organizations = "organizations";

        public const string AuthorityWithTenantSpecified = AadInstance + "/" + TenantIdAsGuid;
        public const string AuthorityCommonTenantWithV2 = AadInstance + "/common/v2.0";
        public const string AuthorityOrganizationsWithV2 = AadInstance + "/organizations/v2.0";
        public const string AuthorityOrganizationsUSWithV2 = AuthorityOrganizationsUSTenant + "/v2.0";
        public const string AuthorityWithTenantSpecifiedWithV2 = AadInstance + "/" + TenantIdAsGuid + "/v2.0";
        public const string AadIssuer = AadInstance + "/" + TenantIdAsGuid + "/v2.0";
        public const string UsGovIssuer = "https://login.microsoftonline.us/" + UsGovTenantId + "/v2.0";
        public const string UsGovTenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47";
        public const string V1Issuer = "https://sts.windows.net/f645ad92-e38d-4d1a-b510-d1b09a74a8ca/";
        public const string AadIssuerV2CommonAuthority = AadInstance + "/{tenantid}/v2.0";

        public const string B2CSignUpSignInUserFlow = "b2c_1_susi";
        public const string B2CInstance = "https://fabrikamb2c.b2clogin.com";
        public const string B2CInstance2 = "https://catb2c.b2clogin.com";
        public const string B2CTenantAsGuid = "775527ff-9a37-4307-8b3d-cc311f58d925";
        public const string B2CCustomDomainInstance = "https://public.msidlabb2c.com";
        public const string B2CCustomDomainTenant = "cpimtestpartners.onmicrosoft.com";
        public const string B2CTenant = "fabrikamb2c.onmicrosoft.com";
        public const string Tfp = "tfp";
        public const string B2CCustomDomainUserFlow = "B2C_1_signupsignin_userflow";
        public const string B2CCustomDomainIssuer = B2CCustomDomainInstance + "/" + B2CCustomDomainTenant + "/v2.0/";
        public const string B2CCustomDomainAuthority = B2CCustomDomainInstance + "/" + B2CCustomDomainTenant + "/" + B2CCustomDomainUserFlow;
        public const string B2CCustomDomainAuthorityWithV2 = B2CCustomDomainAuthority + "/v2.0";
        public const string B2CIssuer = B2CInstance + "/" + B2CTenantAsGuid + "/v2.0/";
        public const string B2CIssuer2 = B2CInstance2 + "/" + B2CTenantAsGuid + "/v2.0/";
        public const string B2CAuthority = B2CInstance + "/" + B2CTenant + "/" + B2CSignUpSignInUserFlow;
        public const string B2CAuthorityWithV2 = B2CAuthority + "/v2.0";
        public const string B2CIssuerTfp = B2CInstance + "/" + Tfp + "/" + B2CTenantAsGuid + "/" + B2CSignUpSignInUserFlow + "/v2.0";

        // Claims
        public const string ClaimNameTid = "tid";
        public const string ClaimNameIss = "iss";
        public const string ClaimNameTfp = "tfp"; // Trust Framework Policy for B2C (aka userflow/policy)
    }
}
