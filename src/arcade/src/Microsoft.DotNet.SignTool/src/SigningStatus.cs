// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.SignTool
{
    internal enum SigningStatus
    {
        /// <summary>
        /// The file is signed.
        /// </summary>
        Signed,
        /// <summary>
        /// The file is not signed.
        /// </summary>
        NotSigned,
        /// <summary>
        /// The status of the file could not be determined.
        /// </summary>
        Unknown
    }
}
