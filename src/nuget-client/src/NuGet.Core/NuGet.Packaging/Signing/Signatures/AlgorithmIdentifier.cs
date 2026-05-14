// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 3280 (https://tools.ietf.org/html/rfc3280#section-4.1.1.2):

            AlgorithmIdentifier ::= SEQUENCE {
                algorithm               OBJECT IDENTIFIER,
                parameters              ANY DEFINED BY algorithm OPTIONAL
            }
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class AlgorithmIdentifier
    {
        public Oid Algorithm { get; }

        internal AlgorithmIdentifier(Oid algorithm)
        {
            Algorithm = algorithm;
        }

        public static AlgorithmIdentifier Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static AlgorithmIdentifier Read(DerSequenceReader reader)
        {
            var algIdReader = reader.ReadSequence();
            var algorithm = algIdReader.ReadOid();

            // For all algorithms we currently support, parameter must be null.
            // However, presence of a DER encoded NULL value is optional.
            if (algIdReader.HasData)
            {
                algIdReader.ReadNull();

                if (algIdReader.HasData)
                {
                    throw new SignatureException(Strings.SigningCertificateV2Invalid);
                }
            }

            return new AlgorithmIdentifier(algorithm);
        }

        internal byte[][] Encode()
        {
            return DerEncoder.ConstructSegmentedSequence(DerEncoder.SegmentedEncodeOid(Algorithm));
        }
    }
}
