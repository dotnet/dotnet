// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// This class is used to hold metadata about the signed package archive being verified.
    /// </summary>
    public sealed class SignedPackageArchiveMetadata
    {
        /// <summary>
        /// List of central directory metadata ordered by the same order the central directory headers are in the archive
        /// </summary>
        public List<CentralDirectoryHeaderMetadata> CentralDirectoryHeaders { get; set; }

        /// <summary>
        /// Offset, in bytes, to the first file header relative to the start of the archive. Should typically be 0.
        /// </summary>
        public long StartOfLocalFileHeaders { get; set; }

        /// <summary>
        /// Offset, in bytes, to the end of central directory headers relative to the start of the archive.
        /// </summary>
        public long EndOfCentralDirectory { get; set; }

        /// <summary>
        /// Index of the signature central directory header in CentralDirectoryHeaders.
        /// If the CentralDirectoryHeaders list is ordered by IndexInHeaders this index indicates the position on the list for the signature.
        /// </summary>
        public int SignatureCentralDirectoryHeaderIndex { get; set; }

        public CentralDirectoryHeaderMetadata GetPackageSignatureFileCentralDirectoryHeaderMetadata()
        {
            return CentralDirectoryHeaders[SignatureCentralDirectoryHeaderIndex];
        }
    }
}
