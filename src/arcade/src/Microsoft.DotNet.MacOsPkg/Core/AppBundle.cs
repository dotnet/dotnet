// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.DotNet.MacOsPkg.Core
{
    internal static class AppBundle
    {
        /// <summary>
        /// Determine whether a path is an app bundle.
        /// 
        /// A path an app bundle if:
        /// - It's a directory and ends in .app and contains a Contents directory and that contains an Info.plist file.
        /// - It's a file and ends in .app.
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if this is an app bundle, false otherwise</returns>
        internal static bool IsBundle(string path)
        {
            if (Directory.Exists(path))
            {
                // Case sensitive check for .app extension is intentional.
                if (Path.GetExtension(path) == ".app")
                {
                    bool containsAppBundleContents = Directory.Exists(Path.Combine(path, "Contents")) &&
                        File.Exists(Path.Combine(path, "Contents", "Info.plist"));
                    if (!containsAppBundleContents)
                    {
                        // If we hit this, and it is a valid app bundle, then we should adjust the logic above.
                        // If we never hit this (likely), then remove.
                        throw new Exception("Unexpected .app directory structure. Please contact dnceng.");
                    }
                    return containsAppBundleContents;
                }
            }
            else
            {
                return Path.GetExtension(path) == ".app";
            }

            return false;
        }

        internal static void Unpack(string inputPath, string outputPath)
        {
            string args = $"-V -xk {inputPath} {outputPath}";
            ExecuteHelper.Run("ditto", args);
        }

        internal static void Pack(string inputPath, string outputPath)
        {
            string args = $"-c -k --sequesterRsrc {inputPath} {outputPath}";
            ExecuteHelper.Run("ditto", args);
        }

        internal static void VerifySignature(string inputPath)
        {
            string output = ExecuteHelper.Run("codesign", $"--verify --verbose {inputPath}");
            if (output.Contains("is not signed at all"))
            {
                throw new Exception("No signature found in app bundle");
            }
        }
    }
}
