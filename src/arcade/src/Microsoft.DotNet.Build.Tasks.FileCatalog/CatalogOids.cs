// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.FileCatalog
{
    /// <summary>
    /// Microsoft OIDs used in catalog (.cat) files. These mirror the szOID_* constants
    /// defined by the Windows SDK headers (wintrust.h, mscat.h).
    /// </summary>
    internal static class CatalogOids
    {
        // PKCS#7 signed-data.
        public const string SignedData = "1.2.840.113549.1.7.2";

        // Catalog content (CTL) — szOID_CTL.
        public const string Ctl = "1.3.6.1.4.1.311.10.1";

        // CTL subjectUsage — szOID_CATALOG_LIST.
        public const string CatalogList = "1.3.6.1.4.1.311.12.1.1";

        // CTL subjectAlgorithm — szOID_CATALOG_LIST_MEMBER_V2 (SHA-256 catalog).
        public const string CatalogListMemberV2 = "1.3.6.1.4.1.311.12.1.3";

        // SPC_INDIRECT_DATA_OBJID — carries the per-member file hash.
        public const string SpcIndirectData = "1.3.6.1.4.1.311.2.1.4";

        // The data-type OID makecat emits inside SPC_INDIRECT_DATA for catalog members.
        public const string SpcLinkData = "1.3.6.1.4.1.311.2.1.25";

        // CAT_MEMBERINFO2_OBJID — per-member marker attribute.
        public const string CatMemberInfo2 = "1.3.6.1.4.1.311.12.2.3";

        // Hash algorithm OIDs.
        public const string Sha1 = "1.3.14.3.2.26";
        public const string Sha256 = "2.16.840.1.101.3.4.2.1";
    }
}
