// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.Commands
{
    public class TrustedSignersArgs
    {
        public enum TrustedSignersAction
        {
            Add,
            List,
            Remove,
            Sync
        }

        /// <summary>
        /// Action to be performed by the trusted signers command.
        /// </summary>
        public TrustedSignersAction Action { get; set; }

        /// <summary>
        /// Name of the trusted signer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Service index of the trusted repository.
        /// </summary>
        public string ServiceIndex { get; set; }

        /// <summary>
        /// Fingerprint of certificate added to a trusted signer
        /// </summary>
        public string CertificateFingerprint { get; set; }

        /// <summary>
        /// Algorithm used to calculate fingerprint. <see cref="CertificateFingerprint"/>
        /// </summary>
        public string FingerprintAlgorithm { get; set; }

        /// <summary>
        /// Specifies if the certificate to be added to a trusted signer
        /// should allow to chain to an untrusted root.
        /// </summary>
        public bool AllowUntrustedRoot { get; set; }

        /// <summary>
        /// Specifies if the signature to be trusted is the author signature.
        /// Only valid when specifying a packge path <see cref="PackagePath"/>
        /// </summary>
        public bool Author { get; set; }

        /// <summary>
        /// Specifies if the signature to be trusted is the repository signature or countersignature.
        /// Only valid when specifying a packge path <see cref="PackagePath"/>
        /// </summary>
        public bool Repository { get; set; }

        /// <summary>
        /// Owners to be trusted from a specific repository.
        /// Only valid when adding trust for a repository.
        /// </summary>
        public IEnumerable<string> Owners { get; set; }

        /// <summary>
        /// Path to the signed package to be trusted.
        /// </summary>
        public string PackagePath { get; set; }

        /// <summary>
        /// Logger to be used to display the logs during the execution of sign command.
        /// </summary>
        public ILogger Logger { get; set; }
    }
}
