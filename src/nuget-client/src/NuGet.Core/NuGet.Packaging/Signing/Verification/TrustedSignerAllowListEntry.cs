// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.Packaging.Signing
{
    public sealed class TrustedSignerAllowListEntry : CertificateHashAllowListEntry
    {
        /// <summary>
        /// List of allowed owners for a repository signature
        /// </summary>
        public IReadOnlyList<string> Owners { get; }

        /// <summary>
        /// Describe if the certificate should be allowed to chain to an untrusted certificate
        /// </summary>
        public bool AllowUntrustedRoot { get; }

        public TrustedSignerAllowListEntry(
            VerificationTarget target,
            SignaturePlacement placement,
            string fingerprint,
            HashAlgorithmName algorithm,
            bool allowUntrustedRoot = false,
            IReadOnlyList<string> owners = null)
            : base(target, placement, fingerprint, algorithm)
        {
            AllowUntrustedRoot = allowUntrustedRoot;
            Owners = owners;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && obj is TrustedSignerAllowListEntry trustedSigner)
            {
                var ownersEquals = (Owners == null || !Owners.Any()) &&
                    (trustedSigner.Owners == null || !trustedSigner.Owners.Any());

                if (Owners != null && trustedSigner.Owners != null)
                {
                    ownersEquals = Owners.OrderBy(o => o).SequenceEqual(trustedSigner.Owners.OrderBy(o => o));
                }

                return AllowUntrustedRoot == trustedSigner.AllowUntrustedRoot && ownersEquals;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStruct(Placement);
            combiner.AddStruct(Target);
            combiner.AddObject(Fingerprint);
            combiner.AddStruct(FingerprintAlgorithm);
            combiner.AddSequence(Owners);

            return combiner.CombinedHash;
        }
    }
}
