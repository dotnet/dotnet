// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

internal static partial class Interop
{
    internal static partial class Advapi32
    {
        [Flags]
        internal enum TOKEN_ACCESS_LEVELS : uint
        {
            AssignPrimary = 0x00000001,
            Duplicate = 0x00000002,
            Impersonate = 0x00000004,
            Query = 0x00000008,
            QuerySource = 0x00000010,
            AdjustPrivileges = 0x00000020,
            AdjustGroups = 0x00000040,
            AdjustDefault = 0x00000080,
            AdjustSessionId = 0x00000100,

            Read = 0x00020000 | Query,

            Write = 0x00020000 | AdjustPrivileges | AdjustGroups | AdjustDefault,

            AllAccess = 0x000F0000 |
                                AssignPrimary |
                                Duplicate |
                                Impersonate |
                                Query |
                                QuerySource |
                                AdjustPrivileges |
                                AdjustGroups |
                                AdjustDefault |
                                AdjustSessionId,

            MaximumAllowed = 0x02000000
        }
    }
}
