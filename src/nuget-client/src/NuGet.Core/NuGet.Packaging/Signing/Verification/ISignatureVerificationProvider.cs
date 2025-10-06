// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Providers signature trust information.
    /// </summary>
    public interface ISignatureVerificationProvider
    {
        /// <summary>
        /// Check if <paramref name="signature" /> is trusted by the provider.
        /// </summary>
        Task<PackageVerificationResult> GetTrustResultAsync(ISignedPackageReader package, PrimarySignature signature, SignedPackageVerifierSettings settings, CancellationToken token);
    }
}
