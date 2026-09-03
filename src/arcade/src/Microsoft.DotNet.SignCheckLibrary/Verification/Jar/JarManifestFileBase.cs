// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

namespace Microsoft.SignCheck.Verification.Jar
{
    /// <summary>
    /// Base class for manifest files.
    /// </summary>
    public class JarManifestFileBase
    {
        // Backing fields
        private JarAttributes _mainAttributes;
        private JarIndividualSection _individualSection;

        public string ArchivePath
        {
            get;
            private set;
        }

        public JarAttributes MainAttributes
        {
            get
            {
                if (_mainAttributes == null)
                {
                    _mainAttributes = JarAttributes.From(MainSectionText);
                }

                return _mainAttributes;
            }
        }

        public JarIndividualSection IndividualSection
        {
            get
            {
                if (_individualSection == null)
                {
                    _individualSection = new JarIndividualSection(IndividualSectionText);
                }

                return _individualSection;
            }
        }

        public string ManifestPath
        {
            get;
            private set;
        }

        /// <summary>
        /// The raw text comprising the individual-section of the manifest.
        /// </summary>
        protected string IndividualSectionText
        {
            get;
            private set;
        }

        /// <summary>
        /// The raw text comprising the main-section of the manifest.
        /// </summary>
        protected string MainSectionText
        {
            get;
            private set;
        }

        /// <summary>
        /// The raw text comprising the complete manifest.
        /// </summary>
        protected string ManifestText
        {
            get;
            private set;
        }

        public JarManifestFileBase(string archivePath, string manifestPath)
        {
            ArchivePath = archivePath;
            ManifestPath = manifestPath;

            using (ZipArchive archive = ZipFile.Open(ArchivePath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry manifestArchiveEntry = archive.Entries.First(a => String.Equals(a.FullName, ManifestPath, StringComparison.OrdinalIgnoreCase));

                if (manifestArchiveEntry != null)
                {
                    // Parse the manifest file by splitting the Main and Individual sections. The actual attributes
                    // will be parsed on demand as they're accessed.
                    using (Stream manifestStream = manifestArchiveEntry.Open())
                    using (var manifestStreamReader = new StreamReader(manifestStream, Encoding.UTF8))
                    {
                        // It's important to use ReadToEnd instead of something like ReadLines
                        // as the CR/LF bytes need to be preserved when calculating digests
                        // for the different sections.
                        ManifestText = manifestStreamReader.ReadToEnd();

                        // The start of the individual section (first "Name: xxxx" entry)
                        // indicates the end of the Main section. Avoid x-Name: attributes.
                        // The first individual Name: attribute will be preceded by either CR+LF | CR | LF
                        int crNameIndex = ManifestText.IndexOf("\rName:");
                        int lfNameIndex = ManifestText.IndexOf("\nName:");

                        int mainSectionLength = crNameIndex > 0 ? crNameIndex + 1 : lfNameIndex + 1;
                        //int mainSectionLength = ManifestText.IndexOf("\rName:");
                        MainSectionText = ManifestText.Substring(0, mainSectionLength);

                        // Anything remaining should be the individual section
                        IndividualSectionText = ManifestText.Substring(mainSectionLength);
                    }
                }
            }
        }

        /// <summary>
        /// Computes the hash digest (encoded as Base64) of the full manifest text using the specified hash algorithm.
        /// </summary>
        /// <param name="algorithmName">The name of the hash algorithm to use.</param>
        /// <returns>A base64 encoded string of the manifest hash.</returns>
        public string GetManifestDigest(string algorithmName)
        {
            using (HashAlgorithm hashAlgorithm = Utils.CreateHashAlgorithm(algorithmName))
            {
                byte[] hashValue = hashAlgorithm.ComputeHash(new UTF8Encoding().GetBytes(ManifestText.ToCharArray()));
                return Convert.ToBase64String(hashValue);
            }
        }
    }
}
