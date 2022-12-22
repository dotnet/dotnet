// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Verification
{
    public static class StrongName
    {
        // See https://docs.microsoft.com/en-us/dotnet/framework/unmanaged-api/hosting/iclrstrongname-interface
        // See metahost.h for GUIDs
        public const string CLSID_CLRStrongName = "B79B0ACD-F5CD-409b-B5A5-A16244610B92";
        public const string IID_ICLRStrongName = "9FD93CCF-3280-4391-B3A9-96E1CDE77C8D";

        public const int S_OK = 0;

        internal static ICLRStrongName ClrStrongName = (ICLRStrongName)RuntimeEnvironment.GetRuntimeInterfaceAsObject(new Guid(CLSID_CLRStrongName), new Guid(IID_ICLRStrongName));

        /// <summary>
        /// Retrieve the StrongName token from an assembly.
        /// </summary>
        /// <param name="path">The path of the assembly.</param>
        /// <returns>The strong name token of an assembly.</returns>
        public static int GetStrongNameTokenFromAssembly(string path, out string tokenStr)
        {
            int tokenSize = 0;
            byte[] token = null;
            tokenStr = String.Empty;
            IntPtr tokenPtr = IntPtr.Zero;
            int hresult = ClrStrongName.StrongNameTokenFromAssembly(path, out tokenPtr, out tokenSize);

            if (hresult == S_OK)
            {
                token = new byte[tokenSize];
                Marshal.Copy(tokenPtr, token, 0, tokenSize);
                tokenStr = BitConverter.ToString(token);
                tokenStr = tokenStr.Replace("-", "");
            }

            return hresult;
        }
    }
}
