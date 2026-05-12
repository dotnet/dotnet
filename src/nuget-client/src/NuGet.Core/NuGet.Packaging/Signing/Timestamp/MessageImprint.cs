// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 3161 (https://tools.ietf.org/html/rfc3161#section-2.4.2):

            MessageImprint ::= SEQUENCE  {
                hashAlgorithm                AlgorithmIdentifier,
                hashedMessage                OCTET STRING  }
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class MessageImprint
    {
        public AlgorithmIdentifier HashAlgorithm { get; }
        public byte[] HashedMessage { get; }

        private MessageImprint(
            AlgorithmIdentifier hashAlgorithm,
            byte[] hashedMessage)
        {
            HashAlgorithm = hashAlgorithm;
            HashedMessage = hashedMessage;
        }

        public static MessageImprint Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static MessageImprint Read(DerSequenceReader reader)
        {
            var imprintReader = reader.ReadSequence();
            var hashAlgorithm = AlgorithmIdentifier.Read(imprintReader);
            var hashedMessage = imprintReader.ReadOctetString();

            if (hashedMessage == null || hashedMessage.Length == 0)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            if (imprintReader.HasData)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            return new MessageImprint(hashAlgorithm, hashedMessage);
        }
    }
}
