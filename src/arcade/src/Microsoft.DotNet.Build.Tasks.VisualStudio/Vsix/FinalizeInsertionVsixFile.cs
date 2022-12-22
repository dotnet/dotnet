// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    /// <summary>
    /// Post-processing necessary to convert VSIXes produced by the build to VSIXes suitable for insertion into VS.
    /// 
    /// Replaces Experimental="true" attribute of the Installation element with SystemComponent="true" in the VSIX manifest file.
    /// </summary>
    public sealed class FinalizeInsertionVsixFile : Microsoft.Build.Utilities.Task
    {
        private const string VsixManifestPartName = "/extension.vsixmanifest";
        private const string VsixNamespace = "http://schemas.microsoft.com/developer/vsx-schema/2011";

        [Required]
        public string VsixFilePath { get; set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            using (var package = Package.Open(VsixFilePath))
            {
                UpdatePartHashInManifestJson(package, VsixManifestPartName, UpdateExtensionVsixManifest(package));
            }
        }

        private byte[] UpdateExtensionVsixManifest(Package package)
        {
            var part = package.GetPart(new Uri(VsixManifestPartName, UriKind.Relative));

            byte[] hash;
            using (var stream = part.GetStream(FileMode.Open))
            {
                var document = XDocument.Load(stream);
                UpdateInstallationElement(document);

                using (var newContent = new MemoryStream())
                {
                    document.Save(newContent);

                    // overwrite the content of the part in VSIX:
                    stream.Position = 0;
                    stream.SetLength(newContent.Length);
                    newContent.Position = 0;
                    newContent.CopyTo(stream);

                    // calculate new hash:
                    newContent.Seek(0, SeekOrigin.Begin);
                    using (var sha = SHA256.Create())
                    {
                        hash = sha.ComputeHash(newContent);
                    }
                }
            }

            return hash;
        }

        internal void UpdateInstallationElement(XDocument document)
        {
            var installationElement = document.Element(XName.Get("PackageManifest", VsixNamespace))?.Element(XName.Get("Installation", VsixNamespace));
            if (installationElement == null)
            {
                Log.LogError($"PackageManifest.Installation element not found in manifest of '{VsixFilePath}'");
                return;
            }

            var experimental = installationElement.Attribute("Experimental");
            if (experimental == null || experimental.Value != "true")
            {
                Log.LogWarning($"PackageManifest.Installation element of the manifest does not have Experimental=\"true\": '{VsixFilePath}'");
            }

            experimental?.Remove();

            var systemComponentName = XName.Get("SystemComponent");
            var systemComponent = installationElement.Attribute(systemComponentName);
            if (systemComponent != null)
            {
                Log.LogWarning($"PackageManifest.Installation element of the manifest specifies SystemComponent attribute: '{VsixFilePath}'");
                systemComponent.SetValue(true);
            }
            else
            {
                systemComponent = new XAttribute(systemComponentName, true);
                installationElement.Add(systemComponent);
            }
        }

        private static void UpdatePartHashInManifestJson(Package package, string partName, byte[] partHash)
        {
            var part = package.GetPart(new Uri("/manifest.json", UriKind.Relative));

            using (var stream = part.GetStream(FileMode.Open))
            {
                string jsonStr;
                using (var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 2048, leaveOpen: true))
                {
                    jsonStr = reader.ReadToEnd();
                }

                var json = JObject.Parse(jsonStr);

                var file = ((JArray)json["files"]).Where(f => (string)f["fileName"] == partName).Single();
                file["sha256"] = BitConverter.ToString(partHash).Replace("-", "");

                stream.Position = 0;
                stream.SetLength(0);

                using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 2048, leaveOpen: false))
                {
                    writer.Write(json.ToString(Formatting.None));
                }
            }
        }
    }
}
