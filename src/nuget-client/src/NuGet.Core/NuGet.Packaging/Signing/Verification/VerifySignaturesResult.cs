// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Collection of signature verification results.
    /// </summary>
    public sealed class VerifySignaturesResult
    {
        /// <summary>
        /// True if signature is valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// True if the package is signed.
        /// </summary>
        public bool IsSigned { get; }

        /// <summary>
        /// Individual trust results.
        /// </summary>
        public IReadOnlyList<PackageVerificationResult> Results { get; }

        public VerifySignaturesResult(bool isValid, bool isSigned)
            : this(isValid, isSigned, results: Enumerable.Empty<PackageVerificationResult>())
        {
        }

        public VerifySignaturesResult(bool isValid, bool isSigned, IEnumerable<PackageVerificationResult> results)
        {
            IsValid = isValid;
            IsSigned = isSigned;
            Results = results?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(results));
        }
    }
}
