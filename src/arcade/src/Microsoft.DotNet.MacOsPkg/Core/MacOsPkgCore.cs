// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.DotNet.MacOsPkg.Core
{
    public static class MacOsPkgCore
    {
        public static int VerifySignature(string? srcPath)
        {
            if (string.IsNullOrEmpty(srcPath))
            {
                throw new Exception("src must be non-empty");
            }

            try
            {
                if (!File.Exists(srcPath) || !Utilities.IsPkg(srcPath) && !Utilities.IsAppBundle(srcPath))
                {
                    throw new Exception("Input path must be a .pkg or .app (zipped) file.");
                }

                if (Utilities.IsPkg(srcPath))
                {
                    Package.VerifySignature(srcPath);
                }
                else if (Utilities.IsAppBundle(srcPath))
                {
                    AppBundle.VerifySignature(srcPath);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }

            return 0;
        }

        public static int Pack(string? srcPath, string? dstPath)
        {
            if (string.IsNullOrEmpty(srcPath))
            {
                throw new Exception("src must be non-empty");
            }

            if (string.IsNullOrEmpty(dstPath))
            {
                throw new Exception("dst must be non-empty");
            }

            try
            {
                if (!Directory.Exists(srcPath))
                {
                    throw new Exception("Input path must be a valid directory.");
                }

                if (!Utilities.IsPkg(dstPath) && !Utilities.IsAppBundle(dstPath))
                {
                    throw new Exception("Output path must be a .pkg or .app (zipped) file.");
                }

                Utilities.CleanupPath(dstPath);
                Utilities.CreateParentDirectory(dstPath);

                if (Utilities.IsPkg(dstPath))
                {
                    Package.Pack(srcPath, dstPath);
                }
                else if (Utilities.IsAppBundle(dstPath))
                {
                    AppBundle.Pack(srcPath, dstPath);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }

            return 0;
        }

        public static int Unpack(string? srcPath, string? dstPath)
        {
            if (string.IsNullOrEmpty(srcPath))
            {
                throw new Exception("src must be non-empty");
            }

            if (string.IsNullOrEmpty(dstPath))
            {
                throw new Exception("dst must be non-empty");
            }

            try
            {
                if (!File.Exists(srcPath) || (!Utilities.IsPkg(srcPath) && !Utilities.IsAppBundle(srcPath)))
                {
                    throw new Exception("Input path must be an existing .pkg or .app (zipped) file.");
                }

                Utilities.CleanupPath(dstPath);
                Utilities.CreateParentDirectory(dstPath);

                if (Utilities.IsPkg(srcPath))
                {
                    Package.Unpack(srcPath, dstPath);
                }
                else if (Utilities.IsAppBundle(srcPath))
                {
                    AppBundle.Unpack(srcPath, dstPath);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }

            return 0;
        }
    }
}
