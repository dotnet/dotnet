// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    /// <summary>
    /// Create a .deb package from a control file and a data file.
    /// </summary>
    /// <remarks>
    /// Implements the format specified in https://manpages.debian.org/bookworm/dpkg-dev/deb.5.en.html
    /// </remarks>
    public sealed class CreateRpmPackage : BuildTask
    {
        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Required]
        public string OutputRpmPackagePath { get; set; } = "";
        [Required]
        public string Vendor { get; set; } = "";
        [Required]
        public string Packager { get; set; } = "";
        [Required]
        public string PackageName { get; set; } = "";
        [Required]
        public string PackageVersion { get; set; } = "";
        [Required]
        public string PackageRelease { get; set; } = "";
        [Required]
        public string PackageOS { get; set; } = "";
        [Required]
        public string PackageArchitecture { get; set; } = "";
        [Required]
        public string Payload { get; set; } = "";
        /// <summary>
        ///  A list of file kinds that are included in the payload. Each item is the output from the 'file'
        ///  tool on the path of the file in the payload.
        /// </summary>
        [Required]
        public ITaskItem[] RawPayloadFileKinds { get; set; } = [];
        [Required]
        public ITaskItem[] Requires { get; set; } = [];
        [Required]
        public ITaskItem[] Conflicts { get; set; } = [];
        [Required]
        public ITaskItem[] OwnedDirectories { get; set; } = [];
        [Required]
        public ITaskItem[] ChangelogLines { get; set; } = [];
        [Required]
        public string License { get; set; } = "";
        [Required]
        public string Summary { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        [Required]
        public string PackageUrl { get; set; } = "";
        [Required]
        public ITaskItem[] Scripts { get; set; } = [];

        /// <summary>
        /// Paths that the package owns as "ghost" files: they are recorded in the RPM file list
        /// (with the RPMFILE_GHOST flag) but are not shipped in the payload. Each item's ItemSpec is the
        /// installed path (e.g. <c>/usr/bin/dnx</c>). The file's mode/type/link target are harvested from the
        /// matching entry in the payload, which is then omitted from the CPIO archive.
        /// </summary>
        public ITaskItem[] GhostFiles { get; set; } = [];

        /// <summary>
        /// File trigger scriptlets. Each item's ItemSpec is a path to the script file. Supported metadata:
        /// <c>Kind</c> (one of <c>FileTriggerIn</c>, <c>FileTriggerUn</c>, <c>FileTriggerPostUn</c>,
        /// <c>TransFileTriggerIn</c>, <c>TransFileTriggerUn</c>, or <c>TransFileTriggerPostUn</c>; defaults to
        /// <c>FileTriggerIn</c>) and <c>Paths</c> (a semicolon-separated list of path prefixes that arm the trigger).
        /// </summary>
        public ITaskItem[] FileTriggers { get; set; } = [];

        public override bool Execute()
        {
            var arch = PackageArchitecture switch
            {
                "x86" => Architecture.X86,
                "x64" => Architecture.X64,
                "arm" => Architecture.Arm,
                "arm64" => Architecture.Arm64,
                "armv6" => Architecture.Armv6,
                "s390x" => Architecture.S390x,
                "ppc64le" => Architecture.Ppc64le,
                "riscv64" => Architecture.RiscV64,
                "loongarch64"  => Architecture.LoongArch64,
                _ => throw new ArgumentException($"Unknown architecture: {PackageArchitecture}")
            };
            RpmBuilder builder = new(PackageName, PackageVersion, PackageRelease, arch, OSPlatform.Create(PackageOS))
            {
                Vendor = Vendor,
                Packager = Packager,
                License = License,
                Summary = Summary,
                Description = Description,
                Url = PackageUrl
            };

            foreach (ITaskItem require in Requires)
            {
                builder.AddRequiredCapability(require.ItemSpec, require.GetMetadata("Version"));
            }

            foreach (ITaskItem conflict in Conflicts)
            {
                builder.AddConflict(conflict.ItemSpec);
            }

            foreach (ITaskItem changelogLine in ChangelogLines)
            {
                builder.AddChangelogLine(Packager, changelogLine.ItemSpec);
            }

            builder.AddProvidedCapability(PackageName, PackageVersion);
            builder.AddProvidedCapability($"{PackageName}({RpmBuilder.GetRpmHeaderArchitecture(arch)})", PackageVersion);

            HashSet<string> ownedDirectories = new(OwnedDirectories.Select(d => d.ItemSpec));

            foreach (ITaskItem script in Scripts)
            {
                builder.AddScript(script.GetMetadata("Kind"), File.ReadAllText(script.ItemSpec));
            }

            foreach (ITaskItem trigger in FileTriggers)
            {
                string kind = trigger.GetMetadata("Kind");
                if (string.IsNullOrEmpty(kind))
                {
                    kind = "FileTriggerIn";
                }

                string[] paths = trigger.GetMetadata("Paths")
                    .Split([';'], StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => p.Length > 0)
                    .ToArray();
                if (paths.Length == 0)
                {
                    Log.LogError($"File trigger '{trigger.ItemSpec}' does not specify any paths in its 'Paths' metadata.");
                    return false;
                }
                if (!File.Exists(trigger.ItemSpec))
                {
                    Log.LogError($"File trigger script '{trigger.ItemSpec}' does not exist.");
                    return false;
                }

                builder.AddFileTrigger(kind, File.ReadAllText(trigger.ItemSpec), paths);
            }

            // Normalize ghost paths (e.g. "/usr/bin/dnx") to the CPIO-relative form used for payload entries ("./usr/bin/dnx").
            Dictionary<string, ITaskItem> ghostFiles = [];
            foreach (ITaskItem ghostFile in GhostFiles)
            {
                string normalizedPath = $"./{ghostFile.ItemSpec.TrimStart('/')}";
                if (!ghostFiles.TryAdd(normalizedPath, ghostFile))
                {
                    Log.LogError(
                        $"Duplicate ghost file path '{ghostFile.ItemSpec}'. Multiple RpmGhostFile items normalize to " +
                        $"the installed path '/{normalizedPath.Substring("./".Length)}'.");
                    return false;
                }
            }
            HashSet<string> ghostFilesFound = new();

            using (CpioReader reader = new(File.OpenRead(Payload), leaveOpen: false))
            {
                Dictionary<string, string> filePathToKind = RawPayloadFileKinds.Select(k => k.ItemSpec.Split(':')).ToDictionary(k => k[0], k => k[1].Trim());
                for (CpioEntry entry = reader.GetNextEntry(); entry is not null; entry = reader.GetNextEntry())
                {
                    if ((entry.Mode & CpioEntry.FileKindMask) == CpioEntry.Directory)
                    {
                        // Only include directories we own.
                        if (!ownedDirectories.Contains(entry.Name))
                        {
                            continue;
                        }
                    }
                    // RPM requires the CPIO entries to be rooted in a relative root of './'.
                    // The cpio tool doesn't want to do this, so we do it here.
                    entry = entry.WithName($"./{entry.Name}");
                    string kind = filePathToKind[entry.Name];
                    // Symlinks may be broken when we are assembling the package, but will not be when we install the package.
                    // Update the file kinds to not say "broken symlink".
                    kind = kind.Replace("broken symlink", "symlink");

                    // A ghost file is owned by the package but not shipped in the payload. We harvest the
                    // real layout entry's mode/type/link target, record it as a ghost, and omit it from the CPIO.
                    if (ghostFiles.ContainsKey(entry.Name))
                    {
                        builder.AddGhostFile(entry, kind);
                        ghostFilesFound.Add(entry.Name);
                    }
                    else
                    {
                        builder.AddFile(entry, kind);
                    }
                }
            }

            string[] missingGhostFiles = ghostFiles.Keys.Where(k => !ghostFilesFound.Contains(k)).ToArray();
            if (missingGhostFiles.Length > 0)
            {
                Log.LogError(
                    "The following ghost file paths were not found in the package payload and cannot be harvested: " +
                    string.Join(", ", missingGhostFiles.Select(k => "/" + k.Substring("./".Length))) +
                    ". A ghost file must be laid out in the package layout so its mode/type/link target can be harvested.");
                return false;
            }

            RpmPackage package = builder.Build();

            using FileStream rpmPackageStream = new(OutputRpmPackagePath, FileMode.Create, FileAccess.Write);

            package.WriteTo(rpmPackageStream);
            return true;
        }
    }
}
