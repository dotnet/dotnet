// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    internal sealed class RpmBuilder(string packageName, string version, string releaseVersion, Architecture architecture, OSPlatform os)
    {
        private readonly List<(string capability, string version)> _provides = [];
        private readonly List<string> _conflicts = [];

        private readonly List<(string name, string text)> _changelogLines = [];

        private readonly List<(string name, int flags, string version)> _requires = [
            ("rpmlib(CompressedFileNames)", 16777226, "3.0.4-1"),
            ("rpmlib(PayloadFilesHavePrefix)", 16777226, "4.0-1"),
            ("rpmlib(FileDigests)", 16777226, "4.6.0-1")
        ];

        private RpmLead Lead { get; } = new()
        {
            Major = 3,
            Minor = 0,
            Type = 0, // Binary package
            Architecture = GetRpmLeadArchitecture(architecture),
            OperatingSystem = GetRpmOS(os),
            SignatureType = 5 // Signature in a Header Structure
        };

        private List<RpmHeader<RpmHeaderTag>.Entry> PackageEntries { get; } = [
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.I18nTable, RpmHeaderEntryType.StringArray, new[] { "C" }),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.PackageName, RpmHeaderEntryType.String, packageName),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.PackageVersion, RpmHeaderEntryType.String, version),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.PackageRelease, RpmHeaderEntryType.String, releaseVersion),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.PayloadCompressor, RpmHeaderEntryType.String, "gzip"),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.PayloadCompressorLevel, RpmHeaderEntryType.String, "9"),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.PayloadFormat, RpmHeaderEntryType.String, "cpio"),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.OperatingSystem, RpmHeaderEntryType.String, os.ToString()),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.Architecture, RpmHeaderEntryType.String, GetRpmHeaderArchitecture(architecture)),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.Encoding, RpmHeaderEntryType.String, "utf-8"),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.RpmVersion, RpmHeaderEntryType.String, "4.18.2"), // Report that the package was built with the RPM version from the last version of rpmbuild we built packages with.
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.Platform, RpmHeaderEntryType.String, "x86_64-azl-linux"), // Report that the package was built on Azure Linux 3.0.
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.BuildHost, RpmHeaderEntryType.String, Dns.GetHostName()),
            new RpmHeader<RpmHeaderTag>.Entry(RpmHeaderTag.Group, RpmHeaderEntryType.String, "default"),
        ];

        private static short GetRpmLeadArchitecture(Architecture architecture)
        {
            // See /usr/lib/rpm/rpmrc for the canonical architecture mapping
            return architecture switch
            {
                Architecture.X86 => 1,
                Architecture.X64 => 1,
                Architecture.Arm => 12,
                Architecture.Arm64 => 19,
#if NET
                Architecture.Armv6 => 12,
                Architecture.S390x => 15,
                Architecture.Ppc64le => 16,
                Architecture.RiscV64 => 22,
                Architecture.LoongArch64 => 23,
#endif
                _ => throw new ArgumentException("Unsupported architecture", nameof(architecture))
            };
        }

        public static string GetRpmHeaderArchitecture(Architecture architecture)
        {
            // See /usr/lib/rpm/rpmrc for valida architecture values
            return architecture switch
            {
                Architecture.X86 => "i686",
                Architecture.X64 => "x86_64",
                Architecture.Arm => "armv7hl",
                Architecture.Arm64 => "aarch64",
#if NET
                Architecture.Armv6 => "armv6hl",
                Architecture.S390x => "s390x",
                Architecture.Ppc64le => "ppc64le",
                Architecture.RiscV64 => "riscv64",
                Architecture.LoongArch64 => "loongarch64",
#endif
                _ => throw new ArgumentException("Unsupported architecture", nameof(architecture))
            };
        }

        public static string GetDotNetArchitectureFromRpmHeaderArchitecture(string rpmPackageArchitecture)
        {
            return rpmPackageArchitecture switch
            {
                "noarch" => "any",
                "i386" => "x86",
                "i486" => "x86",
                "i586" => "x86",
                "i686" => "x86",
                "x86_64" => "x64",
                "armv6hl" => "arm",
                "armv7hl" => "arm",
                "aarch64" => "arm64",
                _ => rpmPackageArchitecture
            };
        }

        private static short GetRpmOS(OSPlatform os)
        {
            // See /usr/lib/rpm/rpmrc for the canonical OS mapping
            if (os.Equals(OSPlatform.Linux))
            {
                return 1;
            }
            else if (os.Equals(OSPlatform.Create("FREEBSD")))
            {
                return 8;
            }
            else
            {
                throw new ArgumentException("Unsupported OS", nameof(os));
            }
        }

        public void AddProvidedCapability(string capability, string version)
        {
            _provides.Add((capability, version));
        }

        public void AddConflict(string name)
        {
            _conflicts.Add(name);
        }

        public void AddRequiredCapability(string capability, string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                _requires.Add((capability, 0x0, ""));
            }
            else
            {
                _requires.Add((capability, 0xC, version));
            }
        }

        public void AddChangelogLine(string name, string text)
        {
            _changelogLines.Add((name, text));
        }

        private readonly List<(CpioEntry file, string fileKind)> _files = [];

        public void AddFile(CpioEntry file, string fileKind)
        {
            _files.Add((file, fileKind));
        }

        private readonly Dictionary<string, string> _scripts = [];

        public void AddScript(string kind, string script)
        {
            _scripts.Add(kind, script);
        }

        public string Url { get; set; } = "";
        public string Vendor { get; set; } = "";
        public string License { get; set; } = "";
        public string Packager { get; set; } = "";

        public string Summary { get; set; } = "";
        public string Description { get; set; } = "";

        private static readonly DateTimeOffset UnixEpoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        internal static readonly int[] Sha256DigestAlgorithmValue = new[] { 8 };

        public RpmPackage Build()
        {
            // Build the header first.
            List<RpmHeader<RpmHeaderTag>.Entry> entries = [..PackageEntries];
            if (_provides.Count != 0)
            {
                entries.Add(new(RpmHeaderTag.ProvideName, RpmHeaderEntryType.StringArray, _provides.Select(p => p.capability).ToArray()));
                entries.Add(new(RpmHeaderTag.ProvideVersion, RpmHeaderEntryType.StringArray, _provides.Select(p => p.version).ToArray()));
                entries.Add(new(RpmHeaderTag.ProvideFlags, RpmHeaderEntryType.Int32, _provides.Select(_ => 0).ToArray()));
            }
            if (_conflicts.Count != 0)
            {
                entries.Add(new(RpmHeaderTag.ConflictName, RpmHeaderEntryType.StringArray, _conflicts.ToArray()));
                entries.Add(new(RpmHeaderTag.ConflictFlags, RpmHeaderEntryType.Int32, _conflicts.Select(_ => 0).ToArray()));
                entries.Add(new(RpmHeaderTag.ConflictVersion, RpmHeaderEntryType.StringArray, _conflicts.Select(_ => "").ToArray()));
            }
            if (_requires.Count != 0)
            {
                entries.Add(new(RpmHeaderTag.RequireName, RpmHeaderEntryType.StringArray, _requires.Select(r => r.name).ToArray()));
                entries.Add(new(RpmHeaderTag.RequireVersion, RpmHeaderEntryType.StringArray, _requires.Select(r => r.version).ToArray()));
                entries.Add(new(RpmHeaderTag.RequireFlags, RpmHeaderEntryType.Int32, _requires.Select(r => r.flags).ToArray()));
            }
            if (_changelogLines.Count != 0)
            {
                entries.Add(new(RpmHeaderTag.ChangelogName, RpmHeaderEntryType.StringArray, _changelogLines.Select(l => l.name).ToArray()));
                entries.Add(new(RpmHeaderTag.ChangelogText, RpmHeaderEntryType.StringArray, _changelogLines.Select(l => l.text).ToArray()));
                entries.Add(new(RpmHeaderTag.ChangelogText, RpmHeaderEntryType.StringArray, _changelogLines.Select(l => l.text).ToArray()));
                entries.Add(new(RpmHeaderTag.ChangelogTimestamp, RpmHeaderEntryType.Int32, _changelogLines.Select(_ => (int)(DateTimeOffset.UtcNow - UnixEpoch).TotalSeconds).ToArray()));
            }
            entries.Add(new(RpmHeaderTag.BuildTime, RpmHeaderEntryType.Int32, new[] { (int)(DateTimeOffset.UtcNow - UnixEpoch).TotalSeconds }));
            entries.Add(new(RpmHeaderTag.Prefixes, RpmHeaderEntryType.StringArray, new[] { "/" }));
            entries.Add(new(RpmHeaderTag.Vendor, RpmHeaderEntryType.String, Vendor));
            entries.Add(new(RpmHeaderTag.License, RpmHeaderEntryType.String, License));
            entries.Add(new(RpmHeaderTag.Packager, RpmHeaderEntryType.String, Packager));
            entries.Add(new(RpmHeaderTag.Url, RpmHeaderEntryType.String, Url));
            entries.Add(new(RpmHeaderTag.Summary, RpmHeaderEntryType.I18NString, Summary));
            entries.Add(new(RpmHeaderTag.Description, RpmHeaderEntryType.I18NString, Description));

            foreach (var script in _scripts)
            {
                entries.Add(new((RpmHeaderTag)Enum.Parse(typeof(RpmHeaderTag), script.Key), RpmHeaderEntryType.String, script.Value));
                entries.Add(new((RpmHeaderTag)Enum.Parse(typeof(RpmHeaderTag), $"{script.Key}prog"), RpmHeaderEntryType.String, "/bin/sh"));
            }

            MemoryStream cpioArchive = new();
            using (CpioWriter writer = new(cpioArchive, leaveOpen: true))
            using (SHA256 sha256 = SHA256.Create())
            {
                List<string> fileDigests = [];
                List<string> baseNames = [];
                List<int> directoryNameIndices = [];
                List<string> directories = [];
                List<int> fileClassIndices = [];
                List<string> fileClassDictionary = [];
                List<int> inodes = [];
                List<int> fileSizes = [];
                List<string> fileUserAndGroupNames = [];
                List<short> fileModes = [];
                List<short> deviceFileIds = [];
                List<int> fileTimestamps = [];
                List<int> fileVerifyFlags = [];
                List<int> fileDevices = [];
                List<string> fileLangs = [];
                List<int> fileColors = [];
                List<int> fileFlags = [];
                List<string> fileLinkTos = [];
                int installedSize = 0;
                entries.Add(new(RpmHeaderTag.FileDigestAlgorithm, RpmHeaderEntryType.Int32, Sha256DigestAlgorithmValue));
                foreach ((CpioEntry file, string fileKind) in _files)
                {
                    writer.WriteNextEntry(file);
                    file.DataStream.Position = 0;

                    // If the entry is a regular file, compute its digest.
                    if ((file.Mode & CpioEntry.FileKindMask) == CpioEntry.RegularFile)
                    {
                        fileDigests.Add(HexConverter.ToHexStringLower(sha256.ComputeHash(file.DataStream)));
                        file.DataStream.Position = 0;
                    }
                    else
                    {
                        // Otherwise the digest is an empty string.
                        fileDigests.Add("");
                    }

                    if ((file.Mode & CpioEntry.FileKindMask) == CpioEntry.SymbolicLink)
                    {
                        // For symbolic links, the contents of the file is the link target.
                        using StreamReader reader = new(file.DataStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen: true);
                        fileLinkTos.Add(reader.ReadToEnd().TrimEnd());
                        file.DataStream.Position = 0;
                    }
                    else
                    {
                        fileLinkTos.Add("");
                    }
                    baseNames.Add(Path.GetFileName(file.Name));
                    string dirName = Path.GetDirectoryName(file.Name)!;
                    if (dirName.StartsWith("./"))
                    {
                        // The cpio entries must have './', but the RPM header entries must have
                        // the actual (non-relative) pathing.
                        dirName = dirName.Substring(1);
                    }

                    // RPM requires the directory names to end with the directory separator.
                    dirName += '/';

                    int directoryIndex = directories.IndexOf(dirName);
                    if (directoryIndex == -1)
                    {
                        directoryIndex = directories.Count;
                        directories.Add(dirName);
                    }
                    directoryNameIndices.Add(directoryIndex);

                    int fileClassIndex = fileClassDictionary.IndexOf(fileKind);
                    if (fileClassIndex == -1)
                    {
                        fileClassIndex = fileClassDictionary.Count;
                        fileClassDictionary.Add(fileKind);
                    }
                    fileClassIndices.Add(fileClassIndex);

                    inodes.Add((int)file.Inode);

                    installedSize += (int)file.DataStream.Length;
                    fileSizes.Add((int)file.DataStream.Length);

                    fileUserAndGroupNames.Add("root");

                    fileModes.Add((short)file.Mode);

                    deviceFileIds.Add(0);

                    fileTimestamps.Add((int)file.Timestamp);

                    fileVerifyFlags.Add(-1);

                    fileDevices.Add(1);

                    fileLangs.Add("");

                    if (fileKind.Contains("ELF 64-bit LSB"))
                    {
                        fileColors.Add(2);
                    }
                    else if (fileKind.Contains("ELF 32-bit LSB"))
                    {
                        fileColors.Add(1);
                    }
                    else
                    {
                        fileColors.Add(0);
                    }

                    if (file.Name.StartsWith("usr/share/doc") && Path.GetFileName(file.Name) == "copyright")
                    {
                        // Treat the copyright file as though it came from the %%doc section in an RPM spec file.
                        fileFlags.Add(2);
                    }
                    else
                    {
                        fileFlags.Add(0);
                    }
                }
                entries.Add(new(RpmHeaderTag.FileDigests, RpmHeaderEntryType.StringArray, fileDigests.ToArray()));
                entries.Add(new(RpmHeaderTag.BaseNames, RpmHeaderEntryType.StringArray, baseNames.ToArray()));
                entries.Add(new(RpmHeaderTag.DirectoryNameIndices, RpmHeaderEntryType.Int32, directoryNameIndices.ToArray()));
                entries.Add(new(RpmHeaderTag.DirectoryNames, RpmHeaderEntryType.StringArray, directories.ToArray()));
                entries.Add(new(RpmHeaderTag.FileClass, RpmHeaderEntryType.Int32, fileClassIndices.ToArray()));
                entries.Add(new(RpmHeaderTag.FileClassDictionary, RpmHeaderEntryType.StringArray, fileClassDictionary.ToArray()));
                entries.Add(new(RpmHeaderTag.FileInode, RpmHeaderEntryType.Int32, inodes.ToArray()));
                entries.Add(new(RpmHeaderTag.FileSizes, RpmHeaderEntryType.Int32, fileSizes.ToArray()));
                entries.Add(new(RpmHeaderTag.FileUserName, RpmHeaderEntryType.StringArray, fileUserAndGroupNames.ToArray()));
                entries.Add(new(RpmHeaderTag.FileGroupName, RpmHeaderEntryType.StringArray, fileUserAndGroupNames.ToArray()));
                entries.Add(new(RpmHeaderTag.FileModes, RpmHeaderEntryType.Int16, fileModes.ToArray()));
                entries.Add(new(RpmHeaderTag.DeviceFileIds, RpmHeaderEntryType.Int16, deviceFileIds.ToArray()));
                entries.Add(new(RpmHeaderTag.FileModificationTimestamp, RpmHeaderEntryType.Int32, fileTimestamps.ToArray()));
                entries.Add(new(RpmHeaderTag.FileVerifyFlags, RpmHeaderEntryType.Int32, fileVerifyFlags.ToArray()));
                entries.Add(new(RpmHeaderTag.FileDevices, RpmHeaderEntryType.Int32, fileDevices.ToArray()));
                entries.Add(new(RpmHeaderTag.FileLang, RpmHeaderEntryType.StringArray, fileLangs.ToArray()));
                entries.Add(new(RpmHeaderTag.FileColors, RpmHeaderEntryType.Int32, fileColors.ToArray()));
                entries.Add(new(RpmHeaderTag.InstalledSize, RpmHeaderEntryType.Int32, new[] { installedSize }));
                entries.Add(new(RpmHeaderTag.FileFlags, RpmHeaderEntryType.Int32, fileFlags.ToArray()));
                entries.Add(new(RpmHeaderTag.FileLinkTos, RpmHeaderEntryType.StringArray, fileLinkTos.ToArray()));
            }
            cpioArchive.Seek(0, SeekOrigin.Begin);

            // TODO: Add more package-level header entries.
            MemoryStream compressedPayload = new();
            using (GZipStream gzipStream = new(compressedPayload, CompressionLevel.Optimal, leaveOpen: true))
            {
                cpioArchive.CopyTo(gzipStream);
            }

            cpioArchive.Seek(0, SeekOrigin.Begin);
            compressedPayload.Seek(0, SeekOrigin.Begin);

            using (SHA256 sha256 = SHA256.Create())
            {
                entries.Add(new(RpmHeaderTag.PayloadDigestAlgorithm, RpmHeaderEntryType.Int32, Sha256DigestAlgorithmValue));
                entries.Add(new(RpmHeaderTag.CompressedPayloadDigest, RpmHeaderEntryType.StringArray, new string[] { HexConverter.ToHexStringLower(sha256.ComputeHash(compressedPayload)) }));
                entries.Add(new(RpmHeaderTag.UncompressedPayloadDigest, RpmHeaderEntryType.StringArray, new string[] { HexConverter.ToHexStringLower(sha256.ComputeHash(cpioArchive)) }));

                cpioArchive.Seek(0, SeekOrigin.Begin);

                CpioReader reader = new(cpioArchive, leaveOpen: true);
            }
            
            MemoryStream headerStream = new();
            RpmHeader<RpmHeaderTag> header = new(entries);
            header.WriteTo(headerStream, RpmHeaderTag.Immutable);
            headerStream.Seek(0, SeekOrigin.Begin);
            cpioArchive.Seek(0, SeekOrigin.Begin);

            List<RpmHeader<RpmSignatureTag>.Entry> signatureEntries = [
                new(RpmSignatureTag.UncompressedPayloadSize, RpmHeaderEntryType.Int32, new[] { (int)cpioArchive.Length }),
                new(RpmSignatureTag.HeaderAndPayloadSize, RpmHeaderEntryType.Int32, new[] { (int)headerStream.Length + (int)compressedPayload.Length }),
            ];

            // Only include the "header" signature tags.
            // RPM has removed the header+payload legacy tags in favor of the header-only tags + payload digests in the header in newer versions.
            using (SHA1 sha1 = SHA1.Create())
            {
                signatureEntries.Add(new(RpmSignatureTag.Sha1Header, RpmHeaderEntryType.String, HexConverter.ToHexStringLower(sha1.ComputeHash(headerStream))));
                headerStream.Seek(0, SeekOrigin.Begin);
            }
            using (SHA256 sha256 = SHA256.Create())
            {
                signatureEntries.Add(new(RpmSignatureTag.Sha256Header, RpmHeaderEntryType.String, HexConverter.ToHexStringLower(sha256.ComputeHash(headerStream))));
            }

            signatureEntries.Add(new(RpmSignatureTag.ReservedSpace, RpmHeaderEntryType.Binary, new ArraySegment<byte>(new byte[4128])));
            RpmHeader<RpmSignatureTag> signature = new(signatureEntries);
            return new RpmPackage(Lead with { Name = packageName }, signature, header, cpioArchive);
        }
    }
}
