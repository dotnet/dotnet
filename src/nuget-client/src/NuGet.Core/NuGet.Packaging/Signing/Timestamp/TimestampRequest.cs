// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Request for timestamping a signature
    /// </summary>
    public class TimestampRequest
    {
        /// <summary>
        /// Signing Specification for this timestamp request.
        /// </summary>
        public SigningSpecifications SigningSpecifications { get; }

        /// <summary>
        /// Gets the hashed message to be timestamped.
        /// </summary>
        public byte[] HashedMessage { get; }

        /// <summary>
        /// Gets the hash algorithm used to generate <see cref="HashedMessage" />.
        /// </summary>
        public HashAlgorithmName HashAlgorithm { get; }

        /// <summary>
        /// Gets the target signature for the timestamp
        /// </summary>
        public SignaturePlacement Target { get; }

        public TimestampRequest(SigningSpecifications signingSpecifications, byte[] hashedMessage, HashAlgorithmName hashAlgorithm, SignaturePlacement target)
        {
            SigningSpecifications = signingSpecifications ?? throw new ArgumentNullException(nameof(signingSpecifications));
            HashedMessage = hashedMessage ?? throw new ArgumentNullException(nameof(hashedMessage));
            HashAlgorithm = hashAlgorithm;
            Target = target;
        }
    }
}
