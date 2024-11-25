// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.MacOsPkg
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

        private static void UnpackComponentPackage(string dstPath)
        {
            UnpackPayload(dstPath);

            // Zip the nested app bundles
            IEnumerable<string> nestedApps = Directory.GetDirectories(dstPath, "*.app", SearchOption.AllDirectories);
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
            IEnumerable<string> zippedNestedApps = Directory.GetFiles(srcPath, "*.app", SearchOption.AllDirectories);
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
    }
}
