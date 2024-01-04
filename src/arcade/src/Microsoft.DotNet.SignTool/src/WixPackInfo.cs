// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.SignTool
{
    internal struct WixPackInfo
    {
        internal string Moniker { get; private set; }

        internal string FullPath { get; private set; }

        private const string WixPackExtension = ".wixpack.zip";

        internal WixPackInfo(string fullPath)
        {
            Moniker = GetMoniker(fullPath);
            FullPath = fullPath;
        }

        internal static string GetMoniker(string path)
        {
            string moniker = null;

            if (IsWixPack(path))
            {
                string filename = Path.GetFileName(path);
                int trimLength = WixPackExtension.Length;
                moniker = filename.Remove(filename.Length - trimLength, trimLength);
            }
            return moniker;
        }

        internal static bool IsWixPack(string path)
        {
            return Path.GetFileName(path).EndsWith(WixPackExtension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
