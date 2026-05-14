// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.LibraryModel
{
    internal class EnumUtility
    {
        public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum =>
#if NET
       Enum.GetValues<TEnum>();
#else
        (TEnum[])Enum.GetValues(typeof(TEnum));
#endif
    }
}
