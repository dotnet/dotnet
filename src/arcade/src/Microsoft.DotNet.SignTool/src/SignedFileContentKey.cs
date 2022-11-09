// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// Key that uniquely identifies a file that could be signed. The
    /// key is the a combination of the file name and its content key.
    /// Two files that have the same key must have the same content once they are signed.
    /// 
    /// This contrasts with <seealso cref="PathWithHash"/>, which is a helper data structure
    /// designed to convey the full path to a file and its associated file.
    /// </summary>
    internal struct SignedFileContentKey : IEquatable<SignedFileContentKey>
    {
        /// <summary>
        ///     Hash of the file.
        /// </summary>
        /// <remarks>
        ///     Note that we use the string rather than the immutable array version of the hash.
        ///     This is largely because GetHashCode for two different byte arrays with the same
        ///     content would return two different hash codes.
        /// </remarks>
        public readonly string StringHash;
        public readonly string FileName;

        public SignedFileContentKey(ImmutableArray<byte> contentHash, string fileName)
        {
            Debug.Assert(!contentHash.IsDefault);
            Debug.Assert(fileName != null);

            StringHash = ContentUtil.HashToString(contentHash);
            FileName = fileName;
        }

        public override bool Equals(object obj)
            => obj is SignedFileContentKey key && Equals(key);

        public override int GetHashCode()
            => Hash.Combine(FileName, StringHash.GetHashCode());

        bool IEquatable<SignedFileContentKey>.Equals(SignedFileContentKey other)
            => FileName == other.FileName && StringHash == other.StringHash;

        public static bool operator ==(SignedFileContentKey key1, SignedFileContentKey key2)
            => key1.Equals(key2);

        public static bool operator !=(SignedFileContentKey key1, SignedFileContentKey key2)
            => !(key1 == key2);
    }
}
