// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.SignCheck.Interop
{
    public enum Provider
    {
        WTD_USE_IE4_TRUST_FLAG = 0x00000001,
        WTD_NO_IE4_CHAIN_FLAG = 0x00000002,
        WTD_NO_POLICY_USAGE_FLAG = 0x00000004,
        WTD_REVOCATION_CHECK_NONE = 0x00000010,
        WTD_REVOCATION_CHECK_END_CERT = 0x00000020,
        WTD_REVOCATION_CHECK_CHAIN = 0x00000040,
        WTD_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 0x00000080,
        WTD_SAFER_FLAG = 0x00000100,
        WTD_HASH_ONLY_FLAG = 0x00000200,
        WTD_USE_DEFAULT_OSVER_CHECK = 0x00000400,
        WTD_LIFETIME_SIGNING_FLAG = 0x00000800,
        WTD_CACHE_ONLY_URL_RETRIEVAL = 0x00001000
    }
}
