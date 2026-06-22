// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.SignCheck.Interop
{
    public class STGM
    {
        // Access 
        public const uint STGM_READ = 0x00000000;
        public const uint STGM_WRITE = 0x00000001;
        public const uint STGM_READWRITE = 0x00000002;

        // Sharing 
        public const uint STGM_SHARE_DENY_NONE = 0x00000040;
        public const uint STGM_SHARE_DENY_READ = 0x00000030;
        public const uint STGM_SHARE_DENY_WRITE = 0x00000020;
        public const uint STGM_SHARE_EXCLUSIVE = 0x00000010;
        public const uint STGM_PRIORITY = 0x00040000;

        // Creation
        public const uint STGM_CREATE = 0x00001000;
        public const uint STGM_CONVERT = 0x00020000;
        public const uint STGM_FAILIFTHERE = 0x00000000;

        // Transactioning 
        public const uint STGM_DIRECT = 0x00000000;
        public const uint STGM_TRANSACTED = 0x00010000;

        // Transactioning Performance
        public const uint STGM_NOSCRATCH = 0x00100000;
        public const uint STGM_NOSNAPSHOT = 0x00200000;

        // Direct SWMR and Simple 
        public const uint STGM_SIMPLE = 0x08000000;
        public const uint STGM_DIRECT_SWMR = 0x00400000;

        // Delete On Release 
        public const uint STGM_DELETEONRELEASE = 0x04000000;
    }
}
