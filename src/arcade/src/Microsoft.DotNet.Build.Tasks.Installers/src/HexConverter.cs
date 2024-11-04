// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET
global using HexConverter = System.Convert;
#else
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
   internal static class HexConverter
    {
        public static string ToHexStringLower(byte[] byteArray)
        {
            StringBuilder hexString = new(byteArray.Length * 2);
            foreach (byte b in byteArray)
            {
                hexString.AppendFormat("{0:x2}", b);
            }
            return hexString.ToString();
        }
    }
}

#endif
