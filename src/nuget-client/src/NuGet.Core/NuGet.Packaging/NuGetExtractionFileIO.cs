// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETFRAMEWORK
using System;
using System.Diagnostics;
#endif
using System.IO;
#if NETFRAMEWORK
using System.Runtime.InteropServices;
using NuGet.Common;
#endif

namespace NuGet.Packaging
{
    internal static class NuGetExtractionFileIO
    {
#if NETFRAMEWORK
        private static int _unixPermissions = Convert.ToInt32("766", 8);
        private static Lazy<Func<string, FileStream>> _createFileMethod =
            new Lazy<Func<string, FileStream>>(CreateFileMethodSelector);
#endif

        internal static FileStream CreateFile(string path)
        {
#if NETFRAMEWORK
            return _createFileMethod.Value(path);
#else
            // Entry permissions are not restored to maintain backwards compatibility with .NET Core 1.x.
            // (https://github.com/NuGet/Home/issues/4424)
            // On .NET Core 1.x, all extracted files had default permissions of 766.
            // The default on .NET Core 2.x has changed to 666.
            // To avoid breaking executable files in existing packages (which don't have the x-bit set)
            // we force the .NET Core 1.x default permissions.
            if (!System.OperatingSystem.IsWindows())
            {
                // Match creat's write-only descriptor without restricting other processes from opening the file.
                return new FileStream(
                    path,
                    new FileStreamOptions
                    {
                        Mode = FileMode.Create,
                        Access = FileAccess.Write,
                        Share = FileShare.ReadWrite | FileShare.Delete,
                        UnixCreateMode =
                            UnixFileMode.UserRead |
                            UnixFileMode.UserWrite |
                            UnixFileMode.UserExecute |
                            UnixFileMode.GroupRead |
                            UnixFileMode.GroupWrite |
                            UnixFileMode.OtherRead |
                            UnixFileMode.OtherWrite
                    });
            }
            return File.Create(path);
#endif
        }

#if NETFRAMEWORK
        private static Func<string, FileStream> CreateFileMethodSelector()
        {
            // Entry permissions are not restored to maintain backwards compatibility with .NET Core 1.x.
            // (https://github.com/NuGet/Home/issues/4424)
            // On .NET Core 1.x, all extracted files had default permissions of 766.
            // The default on .NET Core 2.x has changed to 666.
            // To avoid breaking executable files in existing packages (which don't have the x-bit set)
            // we force the .NET Core 1.x default permissions.
            if (RuntimeEnvironmentHelper.IsMono)
            {
                // Since the OS only applies the umask on creation, figure out what the
                // umask is so that we can apply the correct permission bits to chmod.
                ApplyUMaskToUnixPermissions();
                return MonoPosixCreateFile;
            }

            // Windows doesn't use POSIX permission bits.
            return File.Create;
        }

        private static FileStream MonoPosixCreateFile(string path)
        {
            var fileStream = File.Create(path);
            _ = PosixChmod(path, _unixPermissions);
            return fileStream;
        }

        private static void ApplyUMaskToUnixPermissions()
        {
            if (!ApplyUMaskToUnixPermissionsFromProcess())
            {
                ApplyUMaskToUnixPermissionsFromLibc();
            }
        }

        private static bool ApplyUMaskToUnixPermissionsFromProcess()
        {
            try
            {
                string output;
                using (var process = new Process())
                {
                    // Unfortunately typing "umask" in a shell doesn't run a program, instead "umask" is a built-in
                    // function in the shell. The shell named "sh" is almost always available, since many scripts
                    // expect it to be there, so it's a fairly safe assumption.
                    // We're intentionally not using the full path to "sh" because the POSIX spec says this:
                    // http://pubs.opengroup.org/onlinepubs/9699919799/utilities/sh.html#tag_20_117_16
                    // Applications should note that the standard PATH to the shell cannot be assumed to be either
                    // /bin/sh or /usr/bin/sh, and should be determined by interrogation of the PATH
                    process.StartInfo.FileName = "sh";
                    process.StartInfo.Arguments = "-c umask";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;

                    process.Start();
                    if (!process.WaitForExit(1000) || process.ExitCode != 0)
                    {
                        return false;
                    }

                    output = process.StandardOutput.ReadToEnd();
                }

                var mask = Convert.ToInt32(output.Substring(0, 4), 8);
                _unixPermissions = _unixPermissions & ~mask;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void ApplyUMaskToUnixPermissionsFromLibc()
        {
            // POSIX umask API doesn't have a get-only version. So, we must change the mask to get the current value,
            // then change it back again. There's a potential timing issue if another thread creates a file or
            // directory after our first call to umask and before the second, so we'll set it to a safe, restrictive
            // permission, since that's better than accidentally writing files that are too permissive.

            // Ideally this method would be called before the program creates any threads or async tasks. However,
            // this class is in a class library, meaning we can't control is the calling assembly has already started
            // threading or not. We could create a public initialization method, but to enforce it being called we
            // would have to break backwards compatability. Since this method is only called when the umask couldn't be
            // read from running "sh -c umask", we're extremely unlikely to ever get here, so best-effort is good enough.
            var mask = Convert.ToInt32("700", 8);
            mask = PosixUMask(mask);
            _ = PosixUMask(mask);

            _unixPermissions = _unixPermissions & ~mask;
        }

        [DllImport("libc", EntryPoint = "chmod")]
        private static extern int PosixChmod([MarshalAs(UnmanagedType.LPStr)] string pathname, int mode);

        [DllImport("libc", EntryPoint = "umask")]
        private static extern int PosixUMask(int mask);
#endif
    }
}
