// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.Packaging.Signing
{
    public class CertificateHashAllowListEntry : VerificationAllowListEntry
    {
        public string Fingerprint { get; }

        public HashAlgorithmName FingerprintAlgorithm { get; }

        public CertificateHashAllowListEntry(VerificationTarget target, SignaturePlacement placement, string fingerprint, HashAlgorithmName algorithm)
            : base(target, placement)
        {
            if (!Enum.IsDefined(typeof(SignaturePlacement), placement))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.UnrecognizedEnumValue,
                        placement),
                    nameof(placement));
            }

            if (!Enum.IsDefined(typeof(HashAlgorithmName), algorithm))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.UnrecognizedEnumValue,
                        algorithm),
                    nameof(algorithm));
            }

            if ((placement.HasFlag(SignaturePlacement.Countersignature) && !target.HasFlag(VerificationTarget.Repository)) ||
                (placement == SignaturePlacement.Countersignature && target != VerificationTarget.Repository))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidArgumentCombination,
                        nameof(target),
                        nameof(placement)),
                    nameof(placement));
            }

            Fingerprint = fingerprint ?? throw new ArgumentNullException(nameof(fingerprint));
            FingerprintAlgorithm = algorithm;
        }

        public override bool Equals(object obj)
        {
            if (obj is CertificateHashAllowListEntry hashEntry)
            {
                return Placement == hashEntry.Placement &&
                    Target == hashEntry.Target &&
                    string.Equals(Fingerprint, hashEntry.Fingerprint, StringComparison.Ordinal) &&
                    FingerprintAlgorithm == hashEntry.FingerprintAlgorithm;
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

            return combiner.CombinedHash;
        }
    }
}
