// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Packaging.Signing
{
    public class SignedPackageVerificationResult : PackageVerificationResult
    {
        /// <summary>
        /// Signature
        /// </summary>
        public PrimarySignature Signature { get; }

        public SignedPackageVerificationResult(SignatureVerificationStatus trust, PrimarySignature signature, IEnumerable<SignatureLog> issues) :
            base(trust, issues)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }
    }
}
