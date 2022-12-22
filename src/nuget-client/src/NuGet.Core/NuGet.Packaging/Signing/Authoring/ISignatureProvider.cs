// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Creates Signatures that can be added to packages.
    /// </summary>
    public interface ISignatureProvider
    {
        /// <summary>
        /// Create a signature.
        /// </summary>
        /// <param name="request">Signing request with all the information needed to create signature.</param>
        /// <param name="signatureContent">SignatureContent containing the Hash of the package and the signature version.</param>
        /// <param name="logger">Logger</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A signature for the package.</returns>
        Task<PrimarySignature> CreatePrimarySignatureAsync(SignPackageRequest request, SignatureContent signatureContent, ILogger logger, CancellationToken token);

        /// <summary>
        /// Create a repository countersignature.
        /// </summary>
        /// <param name="request">Signing request with all the information needed to create signature.</param>
        /// <param name="primarySignature">Primary signature to be countersigned.</param>
        /// <param name="logger">Logger</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A signature for the package.</returns>
        Task<PrimarySignature> CreateRepositoryCountersignatureAsync(RepositorySignPackageRequest request, PrimarySignature primarySignature, ILogger logger, CancellationToken token);
    }
}
