// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        NuGetPackageOwners ::= SEQUENCE SIZE (1..MAX) OF NuGetPackageOwner

        NuGetPackageOwner ::= UTF8String (SIZE (1..MAX))
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class NuGetPackageOwners
    {
        public IReadOnlyList<string> PackageOwners { get; }

        public NuGetPackageOwners(IReadOnlyList<string> packageOwners)
        {
            if (packageOwners == null || packageOwners.Count == 0)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageOwners));
            }

            if (packageOwners.Any(packageOwner => string.IsNullOrWhiteSpace(packageOwner)))
            {
                throw new ArgumentException(Strings.NuGetPackageOwnersInvalidValue, nameof(packageOwners));
            }

            PackageOwners = packageOwners;
        }

        public static NuGetPackageOwners Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static NuGetPackageOwners Read(DerSequenceReader reader)
        {
            var packageOwners = new List<string>();
            var ownersReader = reader.ReadSequence();

            while (ownersReader.HasData)
            {
                var packageOwner = ownersReader.ReadUtf8String();

                if (string.IsNullOrWhiteSpace(packageOwner))
                {
                    throw new SignatureException(Strings.NuGetPackageOwnersInvalid);
                }

                packageOwners.Add(packageOwner);
            }

            if (packageOwners.Count == 0)
            {
                throw new SignatureException(Strings.NuGetPackageOwnersInvalid);
            }

            return new NuGetPackageOwners(packageOwners);
        }

        public byte[] Encode()
        {
            return DerEncoder.ConstructSequence(
                PackageOwners.Select(packageOwner => DerEncoder.SegmentedEncodeUtf8String(packageOwner.ToCharArray())));
        }
    }
}
