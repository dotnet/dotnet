// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using NuGet.Common;

namespace NuGet.Packaging
{
    internal static class NuGetExtractionFileIO
    {
        private static int _unixPermissions = Convert.ToInt32("766", 8);
        private static Lazy<Func<string, FileStream>> _createFileMethod =
            new Lazy<Func<string, FileStream>>(CreateFileMethodSelector);

        internal static FileStream CreateFile(string path)
        {
            return _createFileMethod.Value(path);
        }

        private static Func<string, FileStream> CreateFileMethodSelector()
        {
            // Entry permissions are not restored to maintain backwards compatibility with .NET Core 1.x.
            // (https://github.com/NuGet/Home/issues/4424)
            // On .NET Core 1.x, all extracted files had default permissions of 766.
            // The default on .NET Core 2.x has changed to 666.
            // To avoid breaking executable files in existing packages (which don't have the x-bit set)
            // we force the .NET Core 1.x default permissions.
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                // Windows doesn't use POSIX permission bits.
                return File.Create;
            }
            else if (RuntimeEnvironmentHelper.IsMono)
            {
                // Mono doesn't work with the DotnetCoreCreateFile method below, so we'll chmod each file.
                // But since the OS only applies the umask on creation, we'll need to figure out what the
                // umask is so that we can apply the correct permissions bits to chmod.
                ApplyUMaskToUnixPermissions();
                return MonoPosixCreateFile;
            }
            else
            {
                return DotnetCoreCreateFile;
            }
        }

        private static FileStream DotnetCoreCreateFile(string path)
        {
            // .NET APIs don't expose UNIX file permissions, so P/Invoke the POSIX create file API with our
            // preferred permissions, and wrap the file handle/descriptor in a SafeFileHandle.
            int fd;
            try
            {
                fd = PosixCreate(path, _unixPermissions);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error trying to create file {path}: {exception.Message}", exception);
            }

            if (fd == -1)
            {
                using (File.Create(path))
                {
                    // File.Create() should have thrown an exception with an appropriate error message
                }
                File.Delete(path);
                throw new InvalidOperationException("libc creat failed, but File.Create did not");
            }

            var sfh = new SafeFileHandle((IntPtr)fd, ownsHandle: true);

            try
            {
                return new FileStream(sfh, FileAccess.ReadWrite);
            }
            catch
            {
                sfh.Dispose();
                throw;
            }
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


        [DllImport("libc", EntryPoint = "creat")]
        private static extern int PosixCreate([MarshalAs(UnmanagedType.LPStr)] string pathname, int mode);

        [DllImport("libc", EntryPoint = "chmod")]
        private static extern int PosixChmod([MarshalAs(UnmanagedType.LPStr)] string pathname, int mode);

        [DllImport("libc", EntryPoint = "umask")]
        private static extern int PosixUMask(int mask);
    }
}
