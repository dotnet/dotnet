// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    /// <summary>
    /// Write a changelog file in the debian format and compressed using gzip.
    /// </summary>
    /// <remarks>
    /// The format is specified at https://manpages.debian.org/bookworm/dpkg-dev/deb-changelog.5.en.html
    /// </remarks>
    public sealed class CreateChangelogFile : BuildTask
    {
        [Required]
        public string ChangelogOutputPath { get; set; }

        [Required]
        public string MaintainerEmail { get; set; }

        [Required]
        public string MaintainerName { get; set; }

        [Required]
        public string PackageName { get; set; }

        [Required]
        public string PackageVersion { get; set; }

        [Required]
        public string ReleaseNotes { get; set; }

        public override bool Execute()
        {
            using GZipStream stream = new(File.Create(ChangelogOutputPath), CompressionLevel.Optimal);
            using StreamWriter writer = new(stream, Encoding.ASCII);
            writer.WriteLine($"{PackageName} ({PackageVersion}) unstable; urgency=low");
            writer.WriteLine();
            writer.WriteLine($"  * {ReleaseNotes}");
            writer.WriteLine();
            writer.WriteLine($" -- {MaintainerName} <{MaintainerEmail}>  {DateTime.Now:ddd, dd MMM yyyy HH:mm:ss zzz}");

            return true;
        }
    }
}
