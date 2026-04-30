// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5280 (https://tools.ietf.org/html/rfc5280#section-4.1):

            Extensions  ::=  SEQUENCE SIZE (1..MAX) OF Extension
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class Extensions
    {
        public IReadOnlyList<Extension> ExtensionsList { get; }

        private Extensions(IReadOnlyList<Extension> extensions)
        {
            ExtensionsList = extensions;
        }

        public static Extensions Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static Extensions Read(DerSequenceReader reader)
        {
            var extensionsReader = reader.ReadSequence();
            var extensions = new List<Extension>();

            while (extensionsReader.HasData)
            {
                var extension = Extension.Read(extensionsReader);

                extensions.Add(extension);
            }

            if (extensions.Count == 0)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            return new Extensions(extensions);
        }
    }
}
