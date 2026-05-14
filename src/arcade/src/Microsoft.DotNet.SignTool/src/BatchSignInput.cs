// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// Represents all of the input to the batch signing process.
    /// </summary>
    internal sealed class BatchSignInput
    {
        /// <summary>
        /// The ordered names of the files to be signed.
        /// </summary>
        internal ImmutableArray<FileSignInfo> FilesToSign { get; }

        /// <summary>
        /// Holds information about each of the containers that will be signed.
        /// The key is the content hash of the file.
        /// </summary>
        internal ImmutableDictionary<SignedFileContentKey, ZipData> ZipDataMap;

        /// <summary>
        /// A list of files whose content needs to be overwritten by signed content from a different file.
        /// Copy the content of file with full path specified in Key to file with full path specified in Value.
        /// </summary>
        internal ImmutableArray<KeyValuePair<string, string>> FilesToCopy;

        internal BatchSignInput(ImmutableArray<FileSignInfo> filesToSign, ImmutableDictionary<SignedFileContentKey, ZipData> zipDataMap, ImmutableArray<KeyValuePair<string, string>> filesToCopy)
        {
            Debug.Assert(!filesToSign.IsDefault);
            Debug.Assert(zipDataMap != null);
            Debug.Assert(!filesToCopy.IsDefault);

            FilesToSign = filesToSign;
            ZipDataMap = zipDataMap;
            FilesToCopy = filesToCopy;
        }
    }
}
