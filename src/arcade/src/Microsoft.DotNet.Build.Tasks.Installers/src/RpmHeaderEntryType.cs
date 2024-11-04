// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    internal enum RpmHeaderEntryType : uint
    {
        Null = 0,
        Char = 1,
        Int8 = 2,
        Int16 = 3,
        Int32 = 4,
        Int64 = 5,
        String = 6,
        Binary = 7,
        StringArray = 8,
        I18NString = 9,
    }
}
