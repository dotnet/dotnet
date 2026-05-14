// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    ///     A tuple representing a specific file on disk and its associated hash.
    ///     
    ///     This contrasts with <seealso cref="SignedFileContentKey"/>, which represents
    ///     a file name and its associated content key.
    /// </summary>
    public class PathWithHash
    {
        /// <summary>
        /// The hash of the file
        /// </summary>
        public readonly ImmutableArray<byte> ContentHash;
        /// <summary>
        /// Full path to the file.
        /// </summary>
        public readonly string FullPath;
        /// <summary>
        /// Name of the file
        /// </summary>
        public readonly string FileName;

        public PathWithHash(string fullPath, ImmutableArray<byte> contentHash)
        {
            Debug.Assert(!contentHash.IsDefault);
            Debug.Assert(fullPath != null);

            ContentHash = contentHash;
            FullPath = fullPath;
            FileName = Path.GetFileName(fullPath);
        }
    }
}
