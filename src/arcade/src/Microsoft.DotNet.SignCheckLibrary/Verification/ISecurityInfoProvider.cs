// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.Pkcs;

namespace Microsoft.SignCheck.Verification
{
    public interface ISecurityInfoProvider
    {
        /// <summary>
        /// Reads the security information from the specified path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>A SignedCms object containing the security information.</returns>
        SignedCms ReadSecurityInfo(string path);
    }
}
