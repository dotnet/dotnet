// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Deployment.Compression.Cab;
using Microsoft.SignCheck.Interop;
using Microsoft.SignCheck.Interop.PortableExecutable;

namespace Microsoft.SignCheck.Verification.BurnBundle
{
    /// <summary>
    /// Minimal managed reader for WiX Burn bundles. Locates the UX and (optional)
    /// attached containers (CAB files appended to the stub EXE) using the metadata
    /// in the bundle's <c>.wixburn</c> PE section, extracts them, and renames the
    /// hash-named cab entries back to their logical filenames using the bundle's
    /// <c>BurnManifest.xml</c>.
    ///
    /// This replaces the previous dependency on WiX's native <c>Unbinder</c>, which
    /// P/Invokes into the x86-only <c>winterop.dll</c> and therefore cannot be
    /// loaded into a 64-bit process.
    /// </summary>
    /// <remarks>
    /// Layout of the <c>.wixburn</c> section (see WiX 3 <c>burn/engine/section.h</c>,
    /// <c>BURN_SECTION</c>):
    /// <code>
    ///   DWORD dwMagic;                    // 0x00f14300
    ///   DWORD dwVersion;
    ///   GUID  guidBundleId;               // 16 bytes
    ///   DWORD dwStubSize;                 // file offset of the UX container
    ///   DWORD dwOriginalChecksum;
    ///   DWORD dwOriginalSignatureOffset;  // Authenticode signature on the engine
    ///   DWORD dwOriginalSignatureSize;
    ///   DWORD dwFormat;                   // 1 = CAB
    ///   DWORD cContainers;
    ///   DWORD rgcbContainers[cContainers];// [0] = UX cab size, [1] = attached cab size
    /// </code>
    /// The attached container (if present) immediately follows the engine's
    /// Authenticode signature; if the engine isn't signed it follows the UX cab.
    /// </remarks>
    internal static class BurnReader
    {
        private const uint BurnSectionMagic = 0x00f14300;
        private const uint BurnSectionFormatCab = 1;
        private const string BurnNamespace = "http://schemas.microsoft.com/wix/2008/Burn";

        /// <summary>
        /// Extracts the UX container, and any attached container, of a Burn bundle into
        /// <paramref name="destinationDirectory"/>. Hash-named cab entries are renamed
        /// to their logical filenames as recorded in the bundle's manifest, so that
        /// downstream signature verification can dispatch on file extension.
        /// </summary>
        /// <returns><c>true</c> if a Burn bundle was found and extracted; <c>false</c> otherwise.</returns>
        public static bool ExtractContainers(string bundlePath, PortableExecutableHeader peHeader, string destinationDirectory)
        {
            if (!TryGetWixburnSection(peHeader, out IMAGE_SECTION_HEADER wixburnSection))
            {
                return false;
            }

            using var fileStream = new FileStream(bundlePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new BinaryReader(fileStream);

            fileStream.Position = wixburnSection.PointerToRawData;

            uint magic = reader.ReadUInt32();
            if (magic != BurnSectionMagic)
            {
                throw new InvalidDataException(
                    $"Unexpected magic value 0x{magic:x8} in .wixburn section (expected 0x{BurnSectionMagic:x8}).");
            }

            reader.ReadUInt32();                              // dwVersion
            reader.ReadBytes(16);                             // guidBundleId
            uint stubSize = reader.ReadUInt32();              // file offset of UX container
            reader.ReadUInt32();                              // dwOriginalChecksum
            uint originalSignatureOffset = reader.ReadUInt32();
            uint originalSignatureSize = reader.ReadUInt32();
            uint format = reader.ReadUInt32();
            uint containerCount = reader.ReadUInt32();

            if (format != BurnSectionFormatCab)
            {
                throw new InvalidDataException($"Unsupported Burn container format {format}; only CAB (1) is supported.");
            }
            if (containerCount == 0)
            {
                return false;
            }

            uint uxContainerSize = reader.ReadUInt32();
            uint attachedContainerSize = containerCount > 1 ? reader.ReadUInt32() : 0;

            if (uxContainerSize == 0 || stubSize == 0 || (long)stubSize + uxContainerSize > fileStream.Length)
            {
                throw new InvalidDataException("Burn UX container offset/size are invalid for this file.");
            }

            Directory.CreateDirectory(destinationDirectory);

            string uxOutputDirectory = Path.Combine(destinationDirectory, "UX");
            ExtractCabSlice(fileStream, stubSize, uxContainerSize, uxOutputDirectory);

            // The first stream extracted from the UX cab is the BurnManifest.
            string manifestOriginal = Path.Combine(uxOutputDirectory, "0");
            string manifestPath = Path.Combine(uxOutputDirectory, "manifest.xml");
            if (File.Exists(manifestOriginal))
            {
                if (File.Exists(manifestPath)) File.Delete(manifestPath);
                File.Move(manifestOriginal, manifestPath);
            }

            BurnManifest manifest = File.Exists(manifestPath) ? BurnManifest.Load(manifestPath) : BurnManifest.Empty;

            RenamePayloads(uxOutputDirectory, manifest.UxPayloads);

            if (containerCount > 1 && attachedContainerSize > 0)
            {
                long attachedAddress = originalSignatureOffset > 0
                    ? (long)originalSignatureOffset + originalSignatureSize
                    : (long)stubSize + uxContainerSize;

                if (attachedAddress + attachedContainerSize > fileStream.Length)
                {
                    throw new InvalidDataException("Burn attached container offset/size are invalid for this file.");
                }

                string attachedOutputDirectory = Path.Combine(destinationDirectory, "AttachedContainer");
                ExtractCabSlice(fileStream, attachedAddress, attachedContainerSize, attachedOutputDirectory);
                RenamePayloads(attachedOutputDirectory, manifest.AttachedPayloads);
            }

            return true;
        }

        private static bool TryGetWixburnSection(PortableExecutableHeader peHeader, out IMAGE_SECTION_HEADER section)
        {
            foreach (var s in peHeader.ImageSectionHeaders)
            {
                if (s.Name != null && s.SectionName == ".wixburn")
                {
                    section = s;
                    return true;
                }
            }
            section = default;
            return false;
        }

        private static void ExtractCabSlice(FileStream source, long offset, uint size, string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory);
            string tempCabPath = Path.Combine(outputDirectory, Path.GetRandomFileName() + ".cab");

            source.Position = offset;
            byte[] cabBytes = new byte[size];
            source.ReadExactly(cabBytes);
            File.WriteAllBytes(tempCabPath, cabBytes);

            try
            {
                new CabInfo(tempCabPath).Unpack(outputDirectory);
            }
            finally
            {
                try { File.Delete(tempCabPath); } catch { }
            }
        }

        private static void RenamePayloads(string baseDirectory, IEnumerable<(string SourcePath, string FilePath)> payloads)
        {
            foreach (var (sourcePath, filePath) in payloads)
            {
                string source = Path.Combine(baseDirectory, sourcePath);
                string destination = Path.Combine(baseDirectory, filePath);

                if (!File.Exists(source))
                {
                    continue;
                }

                string destinationDir = Path.GetDirectoryName(destination);
                if (!string.IsNullOrEmpty(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                if (File.Exists(destination)) File.Delete(destination);
                File.Move(source, destination);
            }
        }

        private sealed class BurnManifest
        {
            public static readonly BurnManifest Empty = new BurnManifest(
                Array.Empty<(string, string)>(), Array.Empty<(string, string)>());

            public BurnManifest(
                IReadOnlyList<(string SourcePath, string FilePath)> uxPayloads,
                IReadOnlyList<(string SourcePath, string FilePath)> attachedPayloads)
            {
                UxPayloads = uxPayloads;
                AttachedPayloads = attachedPayloads;
            }

            public IReadOnlyList<(string SourcePath, string FilePath)> UxPayloads { get; }
            public IReadOnlyList<(string SourcePath, string FilePath)> AttachedPayloads { get; }

            public static BurnManifest Load(string manifestPath)
            {
                var document = new XmlDocument();
                document.Load(manifestPath);
                var ns = new XmlNamespaceManager(document.NameTable);
                ns.AddNamespace("burn", BurnNamespace);

                var ux = new List<(string, string)>();
                foreach (XmlNode node in document.SelectNodes("/burn:BurnManifest/burn:UX/burn:Payload", ns))
                {
                    if (TryReadPayload(node, requirePackaging: null, out var pair))
                    {
                        ux.Add(pair);
                    }
                }

                var attached = new List<(string, string)>();
                foreach (XmlNode node in document.SelectNodes("/burn:BurnManifest/burn:Payload", ns))
                {
                    if (TryReadPayload(node, requirePackaging: "embedded", out var pair))
                    {
                        attached.Add(pair);
                    }
                }

                return new BurnManifest(ux, attached);
            }

            private static bool TryReadPayload(XmlNode node, string requirePackaging, out (string SourcePath, string FilePath) pair)
            {
                pair = default;
                string sourcePath = node.Attributes?["SourcePath"]?.Value;
                string filePath = node.Attributes?["FilePath"]?.Value;
                if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(filePath))
                {
                    return false;
                }
                if (requirePackaging != null)
                {
                    string packaging = node.Attributes?["Packaging"]?.Value;
                    if (!string.Equals(packaging, requirePackaging, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
                pair = (sourcePath, filePath);
                return true;
            }
        }
    }
}
