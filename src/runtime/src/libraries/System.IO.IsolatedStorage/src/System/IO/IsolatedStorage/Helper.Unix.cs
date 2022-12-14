// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.IO.IsolatedStorage
{
    internal static partial class Helper
    {
        internal static void CreateDirectory(string path, IsolatedStorageScope _ /*scope*/)
        {
            Directory.CreateDirectory(path);
        }
    }
}
