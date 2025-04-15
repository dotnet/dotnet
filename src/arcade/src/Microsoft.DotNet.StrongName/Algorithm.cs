// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.StrongName
{
    internal static class Algorithm
    {
        /// <summary>
        /// Adapted from roslyn's CryptoBlobParser
        /// </summary>
        internal enum AlgorithmClass
        {
            Signature = 1,
            Hash = 4,
        }

        /// <summary>
        /// Adapted from roslyn's CryptoBlobParser
        /// </summary>
        internal enum AlgorithmSubId
        {
            Sha1Hash = 4,
            // Other possible values ommitted
        }

        /// <summary>
        /// Adapted from roslyn's CryptoBlobParser
        /// </summary>
        internal struct AlgorithmId
        {
            // From wincrypt.h
            private const int AlgorithmClassOffset = 13;
            private const int AlgorithmClassMask = 0x7;
            private const int AlgorithmSubIdOffset = 0;
            private const int AlgorithmSubIdMask = 0x1ff;

            private readonly uint _flags;

            internal const int RsaSign = 0x00002400;
            internal const int Sha = 0x00008004;

            internal bool IsSet
            {
                get { return _flags != 0; }
            }

            internal AlgorithmClass Class
            {
                get { return (AlgorithmClass)((_flags >> AlgorithmClassOffset) & AlgorithmClassMask); }
            }

            internal AlgorithmSubId SubId
            {
                get { return (AlgorithmSubId)((_flags >> AlgorithmSubIdOffset) & AlgorithmSubIdMask); }
            }

            internal AlgorithmId(uint flags)
            {
                _flags = flags;
            }
        }
    }
}
