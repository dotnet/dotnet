// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    public enum RpmSignatureTag
    {
        HeaderSignatures = 62,
        RsaHeader = 268,
        Sha1Header = 269,
        Sha256Header = 273,
        HeaderAndPayloadSize = 1000,
        PgpHeaderAndPayload = 1002,
        Md5HeaderAndPayload = 1004,
        UncompressedPayloadSize = 1007,
        ReservedSpace = 1008,
    }
}