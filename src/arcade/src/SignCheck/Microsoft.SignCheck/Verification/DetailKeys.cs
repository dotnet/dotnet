// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.SignCheck.Verification
{
    public static class DetailKeys
    {
        public const string AuthentiCode = "AuthentiCode";
        public const string Error = "Error";
        public const string File = "File";
        public const string Misc = "Misc";

        // Classifier for signatures other than AuthentiCode/StrongName
        public const string Signature = "Signature";
        public const string StrongName = "StrongName";

        public static readonly string[] ResultKeysVerbose = { File, Error, AuthentiCode, StrongName, Signature, Misc };
        public static readonly string[] ResultKeysNormal = { File, Error };
        public static readonly string[] ResultKeysExcluded = { File };
    }
}
