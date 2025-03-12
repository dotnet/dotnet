// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.MacOsPkg.Core
{
    internal static class Package
    {
        internal static void Unpack(string srcPath, string dstPath)
        {
            ExpandPackage(srcPath, dstPath);

            string? distribution = Utilities.FindInPath("Distribution", dstPath, isDirectory: false, searchOption: SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrEmpty(distribution))
            {
                UnpackInstallerPackage(dstPath, distribution!);
                return;
            }

            string? packageInfo = Utilities.FindInPath("PackageInfo", dstPath, isDirectory: false, searchOption: SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrEmpty(packageInfo))
            {
                UnpackComponentPackage(dstPath);
                return;
            }

            throw new Exception("Cannot unpack: no 'Distribution' or 'PackageInfo' file found in package");
        }

        internal static void Pack(string srcPath, string dstPath)
        {
            string? distribution = Utilities.FindInPath("Distribution", srcPath, isDirectory: false, searchOption: SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrEmpty(distribution))
            {
                PackInstallerPackage(srcPath, dstPath, distribution!);
                return;
            }

            string? packageInfo = Utilities.FindInPath("PackageInfo", srcPath, isDirectory: false, searchOption: SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrEmpty(packageInfo))
            {
                PackComponentPackage(srcPath, dstPath, packageInfo!);
                return;
            }

            throw new Exception("Cannot pack: no 'Distribution' or 'PackageInfo' file found in package");
        }

        private static void UnpackInstallerPackage(string dstPath, string distribution)
        {
            var xml = XElement.Load(distribution);
            List<XElement> componentPackages = xml.Elements("pkg-ref").Where(e => e.Value.Trim() != "").ToList();
            foreach (var package in componentPackages)
            {
                // Expanding the installer will unpack the nested component packages to a directory with a .pkg extension
                // so we repack the component packages to a temporary file and then rename the file with the .pkg extension.
                // Repacking is needed so that the signtool can properly identify and sign the nested component packages.
                string packageName = Path.Combine(dstPath, package.Value.Substring(1));
                string tempDest = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                FlattenComponentPackage(packageName, tempDest);

                Directory.Delete(packageName, true);
                File.Move(tempDest, packageName);
            }
        }

        /// <summary>
        /// Unpack the payload of the .pkg, and package up .app directories as .zips
        /// </summary>
        /// <param name="dstPath"></param>
        /// <remarks>
        /// In a pkg, the .app bundles are represented by directories with a standard file structure.
        /// The directory is typically marked with a .app extension, though apparently this is not required.
        /// The important part is that the .app bundle is viewed by the system as a single entity and is signed as such.
        /// This means that signtool needs to view the .app as a signable file and not as a directory. To achieve this,
        /// After unpacking the payload, we zip the .app directories so that signtool can properly track them, unpack them recursively again,
        /// and sign them.
        /// 
        /// There's one other important element to this. We need to recognize whether the .app directory is in fact
        /// a .app bundle and not, say "Microsoft.NetCore.App". To do this, we apply some heuristics.
        /// If:
        /// - Extension should be lower case ".app"
        /// - A directory named "Contents" under the .app directory.
        /// - An Info.plist in this "Contents" directory.
        /// 
        /// If these conditions are met, the file is re-zipped and treated like a bundle.
        /// 
        /// NOTE: I believe that there are very no circumstances where the first condition is met but the other two
        /// will NOT be met. For now, this method will throw an exception in this case so that further examination can be
        /// made.
        /// </remarks>
        private static void UnpackComponentPackage(string dstPath)
        {
            UnpackPayload(dstPath);

            // Zip the nested app bundles
            IEnumerable<string> nestedApps = Directory.GetDirectories(dstPath, "*.app", SearchOption.AllDirectories).Where(app => AppBundle.IsBundle(app));
            foreach (string app in nestedApps)
            {
                string tempDest = $"{app}.zip";
                AppBundle.Pack(app, tempDest);
                Directory.Delete(app, true);

                // Rename the zipped file to .app
                // This is needed so that the signtool
                // can properly identify and sign app bundles
                File.Move(tempDest, app);
            }
        }

        private static void PackInstallerPackage(string srcPath, string dstPath, string distribution)
        {
            string args = $"--distribution {distribution}";

            if (Directory.GetFiles(srcPath, "*.pkg", SearchOption.TopDirectoryOnly).Any())
            {
                args += $" --package-path {srcPath}";
            }

            string? resources = Utilities.FindInPath("Resources", srcPath, isDirectory: true, searchOption: SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrEmpty(resources))
            {
                args += $" --resources {resources}";
            }

            string? scripts = Utilities.FindInPath("Scripts", srcPath, isDirectory: true, searchOption: SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrEmpty(scripts))
            {
                args += $" --scripts {scripts}";
            }

            args += $" {dstPath}";

            ExecuteHelper.Run("productbuild", args);
        }

        private static void PackComponentPackage(string srcPath, string dstPath, string packageInfo)
        {
            // Unzip the nested app bundles
            IEnumerable<string> zippedNestedApps = Directory.GetFiles(srcPath, "*.app", SearchOption.AllDirectories).Where(app => AppBundle.IsBundle(app));
            foreach (string appZip in zippedNestedApps)
            {
                // Unzip the .app directory
                string tempDest = appZip + ".unzipped";
                AppBundle.Unpack(appZip, tempDest);
                File.Delete(appZip);

                // Rename the unzipped directory back to .app
                // so that it can be packed properly
                Directory.Move(tempDest, appZip);
            }

            XElement pkgInfo = XElement.Load(packageInfo);
            
            string payloadDirectoryPath = GetPayloadPath(srcPath, isDirectory: true);
            string identifier = GetPackageInfoAttribute(pkgInfo, "identifier");
            string version = GetPackageInfoAttribute(pkgInfo, "version");
            string installLocation = GetPackageInfoAttribute(pkgInfo, "install-location");

            string args = $"--root {payloadDirectoryPath} --identifier {identifier} --version {version} --install-location {installLocation}";
            string? script = Utilities.FindInPath("Scripts", srcPath, isDirectory: true, searchOption: SearchOption.TopDirectoryOnly);
            if (!string.IsNullOrEmpty(script))
            {
                args += $" --scripts {script}";
            }
            args += $" {dstPath}";

            ExecuteHelper.Run("pkgbuild", args);
        }

        private static void FlattenComponentPackage(string sourcePath, string destinationPath)
            => ExecuteHelper.Run("pkgutil", $"--flatten {sourcePath} {destinationPath}");

        private static void UnpackPayload(string dstPath)
        {
            string payloadFilePath = GetPayloadPath(dstPath, isDirectory: false);

            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            ExecuteHelper.Run("cat", $"{payloadFilePath} | gzip -d | cpio -id", tempDir);

            // Remove the payload file and replace it with
            // a directory of the same name containing the unpacked contents
            File.Delete(payloadFilePath);
            Directory.Move(tempDir, payloadFilePath);
        }

        private static string GetPayloadPath(string searchPath, bool isDirectory) =>
            Path.GetFullPath(Utilities.FindInPath("Payload", searchPath, isDirectory, searchOption: SearchOption.TopDirectoryOnly)
            ?? throw new Exception("Payload was not found"));

        private static void ExpandPackage(string srcPath, string dstPath) =>
            ExecuteHelper.Run("pkgutil", $"--expand {srcPath} {dstPath}");

        private static string GetPackageInfoAttribute(XElement pkgInfo, string elementName) =>
            pkgInfo.Attribute(elementName)?.Value ?? throw new Exception($"{elementName} is required in PackageInfo");

        internal static void VerifySignature(string inputPath)
        {
            Console.WriteLine($"Verifying signature of {inputPath}");
            string full_path = Path.GetFullPath(inputPath);
            string output = ExecuteHelper.Run("pkgutil", $"--check-signature {full_path}");
            Console.WriteLine(output);
            if (output.Contains("Status: no signature"))
            {
                throw new Exception("No signature found in package");
            }
        }
    }
}
