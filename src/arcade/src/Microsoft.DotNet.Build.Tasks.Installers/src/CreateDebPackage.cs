// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
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
    public sealed class CreateDebPackage : BuildTask
    {
        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly uint Permissions = Convert.ToUInt32("100644", 8);

        [Required]
        public string OutputDebPackagePath { get; set; }

        [Required]
        public ITaskItem ControlFile { get; set; }

        [Required]
        public ITaskItem DataFile { get; set; }

        public override bool Execute()
        {
            ulong timestamp = (ulong)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
            using ArWriter arWriter = new(File.Open(OutputDebPackagePath, FileMode.Create), false);
            arWriter.AddEntry(new ArEntry("debian-binary", timestamp, 0, 0, Permissions, new MemoryStream(Encoding.ASCII.GetBytes("2.0\n"))));
            using Stream controlFile = File.OpenRead(ControlFile.ItemSpec);
            arWriter.AddEntry(new ArEntry("control.tar.gz", timestamp, 0, 0, Permissions, controlFile));
            using Stream dataFile = File.OpenRead(DataFile.ItemSpec);
            arWriter.AddEntry(new ArEntry("data.tar.gz", timestamp, 0, 0, Permissions, dataFile));
            return true;
        }
    }
}
