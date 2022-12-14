// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Apple;
using Internal.Cryptography;

namespace System.Security.Cryptography
{
    internal sealed partial class EccSecurityTransforms
    {
#pragma warning disable IDE0060
        private static ECParameters ExportParametersFromLegacyKey(SecKeyPair keys, bool includePrivateParameters)
            => throw new CryptographicException();

        private static void ExtractPublicKeyFromPrivateKey(ref ECParameters ecParameters)
            => throw new PlatformNotSupportedException(SR.Cryptography_NotValidPublicOrPrivateKey);
#pragma warning restore IDE0060
    }
}
