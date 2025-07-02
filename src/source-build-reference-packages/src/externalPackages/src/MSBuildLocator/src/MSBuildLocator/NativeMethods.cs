// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Build.Locator
{
    internal class NativeMethods
    {
        internal const string HostFxrName = "hostfxr";

        internal enum hostfxr_resolve_sdk2_flags_t
        {
            disallow_prerelease = 0x1,
        };

        internal enum hostfxr_resolve_sdk2_result_key_t
        {
            resolved_sdk_dir = 0,
            global_json_path = 1,
        };

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal delegate void hostfxr_resolve_sdk2_result_fn(
                hostfxr_resolve_sdk2_result_key_t key,
                string value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal delegate void hostfxr_get_available_sdks_result_fn(
                hostfxr_resolve_sdk2_result_key_t key,
                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
                string[] value);

        [DllImport(HostFxrName, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int hostfxr_resolve_sdk2(
            string exe_dir,
            string working_dir,
            hostfxr_resolve_sdk2_flags_t flags,
            hostfxr_resolve_sdk2_result_fn result);

        [DllImport(HostFxrName, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int hostfxr_get_available_sdks(string exe_dir, hostfxr_get_available_sdks_result_fn result);

        [DllImport("libc", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr realpath(string path, IntPtr buffer);

        [DllImport("libc", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void free(IntPtr ptr);
    }
}
