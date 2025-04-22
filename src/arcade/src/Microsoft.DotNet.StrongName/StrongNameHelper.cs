// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.DotNet.StrongName
{
    public static class StrongNameHelper
    {
        /// <summary>
        /// Returns true if the file has a valid strong name signature.
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="snPath">Path to sn.exe, if available and desired.</param>
        /// <returns>True if the file has a valid strong name signature, false otherwise.</returns>
        public static bool IsSigned(string file, string snPath = null) =>
            Verification.IsSigned(file, snPath);

        /// <summary>
        /// Determine whether the file is strong named, using sn.exe instead
        /// of the custom implementation
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="snPath">sn.exe path</param>
        /// <returns>True if the file is strong named, false otherwise.</returns>
        public static bool IsSigned_Legacy(string file, string snPath) =>
            Verification.IsSigned_Legacy(file, snPath);

        /// <summary>
        /// Unset the strong name signing bit from a file. This is required for sn
        /// </summary>
        /// <param name="file"></param>
        public static void ClearStrongNameSignedBit(string file) =>
            Signing.ClearStrongNameSignedBit(file);

        /// <summary>
        /// Gets the public key token from a strong named file.
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <returns>Public key token</returns>
        public static int GetStrongNameTokenFromAssembly(string file, out string tokenStr) =>
            Signing.GetStrongNameTokenFromAssembly(file, out tokenStr);

        /// <summary>
        /// Strong names an existing previously signed or delay-signed binary with keyfile.
        /// Fall back to legacy signing if available and new signing fails.
        /// </summary>
        /// <param name="file">Path to file to sign</param>
        /// <param name="keyFile">Path to key pair.</param>
        /// <param name="snPath">Optional path to sn.exe</param>
        /// <returns>True if the file was signed successfully, false otherwise</returns>
        public static bool Sign(string file, string keyFile, string snPath = null) =>
            Signing.Sign(file, keyFile, snPath);

        /// <summary>
        /// Strong names an existing previously signed or delay-signed binary with keyfile
        //  using sn.exe instead of the custom implementation.
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="snPath">sn.exe path</param>
        /// <returns>True if the file is strong named, false otherwise.</returns>
        public static bool Sign_Legacy(string file, string keyfile,  string snPath) =>
            Signing.Sign_Legacy(file, keyfile, snPath);

        /// <summary>
        /// Given a key file, sets the strong name in the managed binary
        /// </summary>
        /// <param name="peStream"></param>
        /// <param name="keyFile"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        public static void Sign(Stream peStream, string keyFile) =>
            Signing.Sign(peStream, keyFile);

        // Internal for testing to avoid having to write a file to disk.
        internal static bool IsSigned(Stream peStream) =>
            Verification.IsSigned(peStream);
    }
}