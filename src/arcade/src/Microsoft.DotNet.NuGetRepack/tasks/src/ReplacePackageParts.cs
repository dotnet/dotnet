// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;

namespace Microsoft.DotNet.Tools
{
    /// <summary>
    /// Replaces content of files in specified package with new content and updates version of the package.
    /// </summary>
#if NET472
    [LoadInSeparateAppDomain]
    public sealed class ReplacePackageParts : AppDomainIsolatedTask
    {
        static ReplacePackageParts() => AssemblyResolution.Initialize();
#else
    public sealed class ReplacePackageParts : Microsoft.Build.Utilities.Task
    {
#endif
        /// <summary>
        /// Full path to the package to process.
        /// </summary>
        [Required]
        public string SourcePackage { get; set; }

        /// <summary>
        /// Directory to store the processed package to.
        /// </summary>
        [Required]
        public string DestinationFolder { get; set; }

        /// <summary>
        /// New version of the package.
        /// </summary>
        public string NewVersion { get; set; }

        /// <summary>
        /// Suffix of the new version of the package. New version is composed of the current version base and <see cref="NewVersionSuffix"/>.
        /// </summary>
        public string NewVersionSuffix { get; set; }

        /// <summary>
        /// Relative paths to files within the package.
        /// </summary>
        public string[] Parts { get; set; }

        /// <summary>
        /// Full paths to files whose content should replace the files in the package.
        /// Each item of <see cref="ReplacementFiles"/> corresponds to an item of <see cref="Parts"/> array.
        /// </summary>
        public string[] ReplacementFiles { get; set; }

        /// <summary>
        /// Full path the the processed package.
        /// </summary>
        [Output]
        public string NewPackage { get; private set; }

        public override bool Execute()
        {
#if NET472
            AssemblyResolution.Log = Log;
#endif
            try
            {
                ExecuteImpl();
                return !Log.HasLoggedErrors;
            }
            finally
            {
#if NET472
                AssemblyResolution.Log = null;
#endif
            }
        }

        private Dictionary<string, string> GetPartReplacementMap()
        {
            int partCount = Parts?.Length ?? 0;

            if (partCount != (ReplacementFiles?.Length ?? 0))
            {
                Log.LogError($"{nameof(Parts)} and {nameof(ReplacementFiles)} lists must have the same length.");
                return null;
            }

            var map = new Dictionary<string, string>(partCount);

            for (int i = 0; i < partCount; i++)
            {
                var partUri = Parts[i].Replace('\\', '/');

                if (!partUri.StartsWith("/"))
                {
                    partUri = "/" + partUri;
                }

                map[partUri] = ReplacementFiles[i];
            }

            return map;
        }

        private void ExecuteImpl()
        {
            var replacementMap = GetPartReplacementMap();
            if (replacementMap == null)
            {
                return;
            }

            string packageId = null;
            SemanticVersion packageVersion = null;
            string tempPackagePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                File.Copy(SourcePackage, tempPackagePath);

                using (var package = Package.Open(tempPackagePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    foreach (var part in package.GetParts())
                    {
                        string relativePath = part.Uri.OriginalString;
                        if (NuGetUtils.IsNuSpec(relativePath))
                        {
                            if (packageId != null)
                            {
                                Log.LogError($"'{SourcePackage}' has multiple .nuspec files in the root");
                                return;
                            }

                            using (var nuspecStream = part.GetStream(FileMode.Open, FileAccess.ReadWrite))
                            {
                                string nuspecXmlns = NuGetUtils.DefaultNuspecXmlns;
                                var nuspecXml = XDocument.Load(nuspecStream);

                                if (nuspecXml.Root.HasAttributes)
                                {
                                    var xmlNsAttribute = nuspecXml.Root.Attributes("xmlns").SingleOrDefault();
                                    if (xmlNsAttribute != null)
                                    {
                                        nuspecXmlns = xmlNsAttribute.Value;
                                    }
                                }

                                var metadata = nuspecXml.Element(XName.Get("package", nuspecXmlns))?.Element(XName.Get("metadata", nuspecXmlns));
                                if (metadata == null)
                                {
                                    Log.LogError($"'{SourcePackage}' has invalid nuspec: missing 'metadata' element");
                                    return;
                                }

                                packageId = metadata.Element(XName.Get("id", nuspecXmlns))?.Value;
                                if (packageId == null)
                                {
                                    Log.LogError($"'{SourcePackage}' has invalid nuspec: missing 'id' element");
                                    return;
                                }

                                var versionElement = metadata.Element(XName.Get("version", nuspecXmlns));
                                var versionStr = versionElement?.Value;
                                if (versionStr == null)
                                {
                                    Log.LogError($"'{SourcePackage}' has invalid nuspec: missing 'version' element");
                                    return;
                                }

                                if (!SemanticVersion.TryParse(versionStr, out packageVersion))
                                {
                                    Log.LogError($"Package NuSpec specifies an invalid package version: '{packageVersion}'");
                                }

                                packageVersion = GetNewVersion(packageVersion);
                                versionElement.SetValue(packageVersion);

                                nuspecStream.SetLength(0);
                                nuspecXml.Save(nuspecStream);
                            }
                        }
                        else if (replacementMap.TryGetValue(relativePath, out var replacementFilePath))
                        {
                            Stream replacementStream;
                            try
                            {
                                replacementStream = File.OpenRead(replacementFilePath);
                            }
                            catch (Exception e)
                            {
                                Log.LogError($"Failed to open replacement file '{replacementFilePath}': {e.Message}");
                                continue;
                            }

                            using (replacementStream)
                            using (var partStream = part.GetStream(FileMode.Open, FileAccess.ReadWrite))
                            {
                                partStream.SetLength(0);
                                replacementStream.CopyTo(partStream);
                            }

                            Log.LogMessage(MessageImportance.Low, $"Part '{relativePath}' of package '{SourcePackage}' replaced with '{replacementFilePath}'.");
                            replacementMap.Remove(relativePath);
                        }
                    }

                    if (packageId == null)
                    {
                        Log.LogError($"'{SourcePackage}' has no .nuspec file in the root");
                        return;
                    }

                    package.PackageProperties.Version = packageVersion.ToFullString();
                }

                if (replacementMap.Count > 0)
                {
                    foreach (var partName in replacementMap.Keys.OrderBy(k => k))
                    {
                        Log.LogWarning($"File '{partName}' not found in package '{SourcePackage}'");
                    }
                }

                // remove signature if present (the signature part is not accessible thru Package API):
                using (var archive = new ZipArchive(File.Open(tempPackagePath, FileMode.Open, FileAccess.ReadWrite), ZipArchiveMode.Update))
                {
                    archive.Entries.FirstOrDefault(e => e.FullName == NuGetUtils.SignaturePartUri)?.Delete();
                }

                NewPackage = Path.Combine(DestinationFolder, packageId + "." + packageVersion + ".nupkg");

                Directory.CreateDirectory(DestinationFolder);
                File.Copy(tempPackagePath, NewPackage, overwrite: true);
            }
            finally
            {
                File.Delete(tempPackagePath);
            }
        }

        private SemanticVersion GetNewVersion(SemanticVersion currentVersion)
        {
            if (NewVersion != null)
            {
                if (SemanticVersion.TryParse(NewVersion, out var newVersion))
                {
                    return newVersion;
                }

                Log.LogError($"Invalid package version specified in {nameof(NewVersion)} parameter: '{NewVersion}'");
            }
            else if (NewVersionSuffix != null)
            {
                try
                {
                    return new SemanticVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Patch, NewVersionSuffix);
                }
                catch (Exception)
                {
                    Log.LogError($"Invalid package version suffix specified in {nameof(NewVersionSuffix)} parameter: '{NewVersionSuffix}'");
                }
            }

            return currentVersion;
        }
    }
}
