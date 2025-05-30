// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security;
using System.Text;
using System.Threading;
using Xunit;

namespace TestLibrary
{
    public static partial class Utilities
    {

        static volatile bool verbose;
        static volatile bool verboseSet = false;
        const char HIGH_SURROGATE_START = '\ud800';
        const char HIGH_SURROGATE_END = '\udbff';
        const char LOW_SURROGATE_START = '\udc00';
        const char LOW_SURROGATE_END = '\udfff';

        static string sTestDirectory = string.Empty;

        public static string TestDirectory
        {
            get
            {
                return sTestDirectory;
            }
            set
            {
                sTestDirectory = value;
            }
        }

        public static bool Verbose
        {
            get
            {
                if (!verboseSet)
                {
                    verboseSet = true;
                    verbose = false;
                }
                return (bool)verbose;
            }
            set
            {
                verbose = value;
            }
        }

        public static bool IsX86 => (RuntimeInformation.ProcessArchitecture == Architecture.X86);
        public static bool IsNotX86 => !IsX86;
        public static bool IsX64 => (RuntimeInformation.ProcessArchitecture == Architecture.X64);
        public static bool IsArm => (RuntimeInformation.ProcessArchitecture == Architecture.Arm);
        public static bool IsArm64 => (RuntimeInformation.ProcessArchitecture == Architecture.Arm64);
        public static bool IsXArch => IsX86 || IsX64;

        public static bool IsWindows => OperatingSystem.IsWindows();
        public static bool IsLinux => OperatingSystem.IsLinux();
        public static bool IsFreeBSD => OperatingSystem.IsFreeBSD();
        public static bool IsMacOSX => OperatingSystem.IsMacOS();
        public static bool IsWindows7 => IsWindows && Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1;
        public static bool IsWindowsNanoServer => (!IsWindowsIoTCore && GetInstallationType().Equals("Nano Server", StringComparison.OrdinalIgnoreCase));

        // Windows 10 October 2018 Update
        public static bool IsWindows10Version1809OrGreater =>
            IsWindows && GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 17763;

        public static bool IsWindowsIoTCore
        {
            get
            {
                if (IsWindows)
                {
                    int productType = GetWindowsProductType();
                    return productType == Kernel32.PRODUCT_IOTUAPCOMMERCIAL
                        || productType == Kernel32.PRODUCT_IOTUAP;
                }

                return false;
            }
        }

        // return whether or not the OS is a 64 bit OS
        public static bool Is64 => (IntPtr.Size == 8);

        public static bool IsMonoRuntime => Type.GetType("Mono.RuntimeStructs") != null;
        public static bool IsNotMonoRuntime => !IsMonoRuntime;
        public static bool IsNativeAot => IsNotMonoRuntime && !IsReflectionEmitSupported;
        public static bool IsNotNativeAot => !IsNativeAot;

        public static bool HasAssemblyFiles => !string.IsNullOrEmpty(typeof(Utilities).Assembly.Location);
        public static bool IsSingleFile => !HasAssemblyFiles;

#if NET
        public static bool IsReflectionEmitSupported => RuntimeFeature.IsDynamicCodeSupported;
        public static bool IsNotReflectionEmitSupported => !IsReflectionEmitSupported;
#else
        public static bool IsReflectionEmitSupported => true;
#endif
        public static bool SupportsExceptionInterop => IsWindows && IsNotMonoRuntime && !IsNativeAot; // matches definitions in clr.featuredefines.props
        public static bool IsGCStress => (Environment.GetEnvironmentVariable("DOTNET_GCStress") != null);

        public static string ByteArrayToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
                sb.Append(", ");
            }
            if (bytes.Length > 0)
                sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

        public static bool CompareBytes(byte[] arr1, byte[] arr2)
        {
            if (arr1 == null) return (arr2 == null);
            if (arr2 == null) return false;

            if (arr1.Length != arr2.Length) return false;

            for (int i = 0; i < arr1.Length; i++) if (arr1[i] != arr2[i]) return false;

            return true;
        }

        // Given a string, display the unicode characters in hex format, optionally displaying each
        // characters unicode category
        public static string FormatHexStringFromUnicodeString(string string1, bool includeUnicodeCategory)
        {
            string returnString = "";
            if (null == string1)
            {
                return null;
            }

            foreach (char x in string1)
            {
                string tempString = FormatHexStringFromUnicodeChar(x, includeUnicodeCategory);
                if (!returnString.Equals("") && !includeUnicodeCategory)
                {
                    returnString = returnString + " " + tempString;
                }
                else
                {
                    returnString += tempString;
                }
            }

            return returnString;
        }

        // Given a character, display its unicode value in hex format.
        public static string FormatHexStringFromUnicodeChar(char char1, bool includeUnicodeCategory)
        {
            if (includeUnicodeCategory)
                throw new Exception("Win8P does not support Char.UnicodeCategory");

            return ((int)char1).ToString("X4");
        }

        public static CultureInfo CurrentCulture
        {
            get { return System.Globalization.CultureInfo.CurrentCulture; }
            set
            {
                System.Globalization.CultureInfo.DefaultThreadCurrentCulture = value;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetWindowsProductType()
        {
            if (!Kernel32.GetProductInfo(Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor, 0, 0, out int productType))
            {
                return Kernel32.PRODUCT_UNDEFINED;
            }

            return productType;
        }

        private static string GetInstallationType()
        {
            if (IsWindows)
            {
                return GetInstallationTypeForWindows();
            }

            return string.Empty;
        }

        private static string GetInstallationTypeForWindows()
        {
            try
            {
                string key = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                string value = "InstallationType";
                return GetRegistryValueString(key, value);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{nameof(GetInstallationTypeForWindows)} exception: {e}");
                return string.Empty;
            }
        }

        internal static uint GetWindowsVersion()
        {
            if (!IsWindows)
            {
                return 0;
            }

            Assert.Equal(0, Ntdll.RtlGetVersionEx(out Ntdll.RTL_OSVERSIONINFOEX osvi));
            return osvi.dwMajorVersion;
        }
        internal static uint GetWindowsMinorVersion()
        {
            if (!IsWindows)
            {
                return 0;
            }

            Assert.Equal(0, Ntdll.RtlGetVersionEx(out Ntdll.RTL_OSVERSIONINFOEX osvi));
            return osvi.dwMinorVersion;
        }
        internal static uint GetWindowsBuildNumber()
        {
            if (!IsWindows)
            {
                return 0;
            }

            Assert.Equal(0, Ntdll.RtlGetVersionEx(out Ntdll.RTL_OSVERSIONINFOEX osvi));
            return osvi.dwBuildNumber;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetRegistryValueString(string key, string value)
        {
            int dataSize = 0;
            Advapi32.RType type;
            int result = Advapi32.RegGetValueW(
                Advapi32.HKEY_LOCAL_MACHINE,
                key,
                value,
                Advapi32.RFlags.RRF_RT_REG_SZ,
                out type,
                IntPtr.Zero,
                ref dataSize);
            if (result != 0 || type != Advapi32.RType.RegSz)
            {
                throw new Exception($"Invalid {nameof(Advapi32.RegGetValueW)} result: 0x{result:x} type: {type}");
            }

            IntPtr data = Marshal.AllocCoTaskMem(dataSize + 1);
            result = Advapi32.RegGetValueW(
                Advapi32.HKEY_LOCAL_MACHINE,
                key,
                value,
                Advapi32.RFlags.RRF_RT_REG_SZ,
                out type,
                data,
                ref dataSize);
            if (result != 0 || type != Advapi32.RType.RegSz)
            {
                throw new Exception($"Invalid {nameof(Advapi32.RegGetValueW)} result: 0x{result:x} type: {type}");
            }

            string stringValue = Marshal.PtrToStringUni(data);
            Marshal.FreeCoTaskMem(data);

            return stringValue;
        }

        private static class Ntdll
        {
            [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
            internal unsafe struct RTL_OSVERSIONINFOEX
            {
                internal uint dwOSVersionInfoSize;
                internal uint dwMajorVersion;
                internal uint dwMinorVersion;
                internal uint dwBuildNumber;
                internal uint dwPlatformId;
                internal fixed char szCSDVersion[128];
            }

            [DllImport(nameof(Ntdll), ExactSpelling=true)]
            private static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);

            internal static unsafe int RtlGetVersionEx(out RTL_OSVERSIONINFOEX osvi)
            {
                osvi = new RTL_OSVERSIONINFOEX();
                osvi.dwOSVersionInfoSize = (uint)sizeof(RTL_OSVERSIONINFOEX);
                return RtlGetVersion(ref osvi);
            }

            internal static unsafe string RtlGetVersion()
            {
                const string Version = "Microsoft Windows";
                if (RtlGetVersionEx(out RTL_OSVERSIONINFOEX osvi) == 0)
                {
                    return osvi.szCSDVersion[0] != '\0' ?
                        string.Format("{0} {1}.{2}.{3} {4}", Version, osvi.dwMajorVersion, osvi.dwMinorVersion, osvi.dwBuildNumber, new string(&(osvi.szCSDVersion[0]))) :
                        string.Format("{0} {1}.{2}.{3}", Version, osvi.dwMajorVersion, osvi.dwMinorVersion, osvi.dwBuildNumber);
                }
                else
                {
                    return Version;
                }
            }
        }

        private sealed class Kernel32
        {
            public const int PRODUCT_UNDEFINED = 0;
            public const int PRODUCT_IOTUAP = 0x0000007B;
            public const int PRODUCT_IOTUAPCOMMERCIAL = 0x00000083;
            public const int PRODUCT_CORE = 0x00000065;
            public const int PRODUCT_CORE_COUNTRYSPECIFIC = 0x00000063;
            public const int PRODUCT_CORE_N = 0x00000062;
            public const int PRODUCT_CORE_SINGLELANGUAGE = 0x00000064;
            public const int PRODUCT_HOME_BASIC = 0x00000002;
            public const int PRODUCT_HOME_BASIC_N = 0x00000005;
            public const int PRODUCT_HOME_PREMIUM = 0x00000003;
            public const int PRODUCT_HOME_PREMIUM_N = 0x0000001A;

            /// <summary>
            /// https://learn.microsoft.com/windows/desktop/api/sysinfoapi/nf-sysinfoapi-getproductinfo
            /// </summary>
            [DllImport(nameof(Kernel32), SetLastError = false)]
            public static extern bool GetProductInfo(
                int dwOSMajorVersion,
                int dwOSMinorVersion,
                int dwSpMajorVersion,
                int dwSpMinorVersion,
                out int pdwReturnedProductType);
        }

        private sealed class Advapi32
        {
            /// <summary>
            /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms724884(v=vs.85).aspx
            /// </summary>
            public enum RFlags
            {
                /// <summary>
                /// Any
                /// </summary>
                Any = 0xffff,

                /// <summary>
                /// A null-terminated string.
                /// This will be either a Unicode or an ANSI string, depending on whether you use the Unicode or ANSI function.
                /// </summary>
                RRF_RT_REG_SZ = 2,
            }

            /// <summary>
            /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms724884(v=vs.85).aspx
            /// </summary>
            public enum RType
            {
                /// <summary>
                /// No defined value type
                /// </summary>
                RegNone = 0,

                /// <summary>
                /// A null-terminated string.
                /// This will be either a Unicode or an ANSI string, depending on whether you use the Unicode or ANSI function.
                /// </summary>
                RegSz = 1,
            }

            [DllImport(nameof(Advapi32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern int RegGetValueW(
                IntPtr hkey,
                string lpSubKey,
                string lpValue,
                RFlags dwFlags,
                out RType pdwType,
                IntPtr pvData,
                ref int pcbData);

            public static IntPtr HKEY_LOCAL_MACHINE => new IntPtr(unchecked((int)0x80000002));
        }

        class TestAssemblyLoadContext : AssemblyLoadContext
        {
            public TestAssemblyLoadContext() : base(isCollectible: true)
            {

            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                return null;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int ExecuteAndUnloadInternal(string assemblyPath, string[] args, Action<AssemblyLoadContext> unloadingCallback, out WeakReference alcWeakRef)
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext();
            if (unloadingCallback != null)
            {
                alc.Unloading += unloadingCallback;
            }
            alcWeakRef = new WeakReference(alc);

            Assembly a = alc.LoadFromAssemblyPath(assemblyPath);

            object[] argsObjArray = (a.EntryPoint.GetParameters().Length != 0) ? new object[] { args } : null;
            object res = a.EntryPoint.Invoke(null, argsObjArray);

            alc.Unload();

            return (a.EntryPoint.ReturnType == typeof(void)) ? Environment.ExitCode : Convert.ToInt32(res);
        }

        public static int ExecuteAndUnload(string assemblyPath, string[] args, Action<AssemblyLoadContext> unloadingCallback = null)
        {
            WeakReference alcWeakRef;
            int exitCode;

            exitCode = ExecuteAndUnloadInternal(assemblyPath, args, unloadingCallback, out alcWeakRef);

            // Run the GC and finalizer a few times to ensure that any complicated
            // object trees and runtime data structures that may keep the ALC alive are freed.
            for (int i = 0; i < 8 && alcWeakRef.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            if (alcWeakRef.IsAlive)
            {
                exitCode += 100;
                Console.WriteLine($"Unload failed - {assemblyPath}");
            }
            else
            {
                Console.WriteLine($"Unload succeeded - {assemblyPath}");
            }

            return exitCode;
        }

        private static void ExecuteAndUnloadInternal(string assemblyPath, string typeName, string methodName, object[] args, out WeakReference alcWeakRef)
        {
            AssemblyLoadContext alc = new AssemblyLoadContext($"[{assemblyPath}]{typeName}.{methodName}", true);
            alcWeakRef = new WeakReference(alc);

            Assembly asm = alc.LoadFromAssemblyPath(assemblyPath);
            Type testType = asm.GetType(typeName);
            testType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static).Invoke(null, args);
            alc.Unload();
        }

        public static void ExecuteAndUnload(string assemblyPath, string typeName, string methodName, params object[] args)
        {
            WeakReference alcWeakRef;
            ExecuteAndUnloadInternal(assemblyPath, typeName, methodName, args, out alcWeakRef);

            // Run the GC and finalizer a few times to ensure that any complicated
            // object trees and runtime data structures that may keep the ALC alive are freed.
            for (int i = 0; i < 8 && alcWeakRef.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Assert.False(alcWeakRef.IsAlive);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RLimit
        {
            public ulong rlim_cur; // Soft limit
            public ulong rlim_max; // Hard limit
        }

        public const int RLIMIT_CORE = 4; // Core file size

        [DllImport("libc", SetLastError = true)]
        public static extern int setrlimit(int resource, ref RLimit rlim);

        // Ensure that the OS doesn't generate core dump for the current process
        public static void DisableOSCoreDump()
        {
            // At present, RLimit is defined in a way where the fields are always 64-bit.
            // Before adding support for a new platform, its definition of rlimit should be confirmed.
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                RLimit rlimit = new RLimit
                {
                    rlim_cur = 0,
                    rlim_max = 0
                };

                if (setrlimit(RLIMIT_CORE, ref rlimit) != 0)
                {
                    throw new Exception($"Failed to disable core dump, error {Marshal.GetLastPInvokeError()} - {Marshal.GetLastPInvokeErrorMessage()}.");
                }
            }
        }
    }
}
