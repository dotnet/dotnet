// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Represents a builder object for .nuspec file cotents. For testing purposes
    /// </summary>
    public class NuspecBuilder
    {
        public List<Tuple<string, string>> FileEntries { get; private set; }
        public string IconEntry { get; private set; }
        public string IconUrlEntry { get; private set; }
        public string ReadmeEntry { get; private set; }
        public string PackageIdEntry { get; private set; } = "testPackage";
        public string PackageVersionEntry { get; private set; } = "0.0.1";

        private NuspecBuilder()
        {
            FileEntries = new List<Tuple<string, string>>();
        }

        /// <summary>
        /// Factory method
        /// </summary>
        /// <returns>A <c>NuspecBuilder</c> factory object</returns>
        public static NuspecBuilder Create()
        {
            var builder = new NuspecBuilder();
            return builder;
        }

        public NuspecBuilder WithIcon(string icon)
        {
            IconEntry = icon;
            return this;
        }

        public NuspecBuilder WithReadme(string readme)
        {
            ReadmeEntry = readme;
            return this;
        }

        public NuspecBuilder WithIconUrl(string iconUrl)
        {
            IconUrlEntry = iconUrl;
            return this;
        }

        public NuspecBuilder WithPackageId(string packageId)
        {
            PackageIdEntry = packageId;
            return this;
        }

        public NuspecBuilder WithPackageVersion(string packageVersion)
        {
            PackageVersionEntry = packageVersion;
            return this;
        }

        public NuspecBuilder WithFile(string source)
        {
            return WithFile(source, string.Empty);
        }

        public NuspecBuilder WithFile(string source, string target)
        {
            FileEntries.Add(Tuple.Create(source, target));
            return this;
        }

        /// <summary>
        /// Creates the nuspec
        /// </summary>
        /// <returns>A <c>StringBuilder</c> object with the .nuspec content</returns>
        public StringBuilder Build()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            Write(sw);

            return sb;
        }

        public void Write(TextWriter sb)
        {
            sb.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.WriteLine("<package>");
            sb.WriteLine("  <metadata>");
            sb.Write("    <id>");
            sb.Write(PackageIdEntry);
            sb.WriteLine("</id>");
            sb.Write("    <version>");
            sb.Write(PackageVersionEntry);
            sb.WriteLine("</version>");
            sb.WriteLine("    <authors>Author1, author2</authors>");
            sb.WriteLine("    <description>A short sample description</description>");

            if (IconEntry != null)
            {
                sb.Write("    <icon>");
                sb.Write(IconEntry);
                sb.WriteLine("</icon>");
            }

            if (IconUrlEntry != null)
            {
                sb.Write("    <iconUrl>");
                sb.Write(IconUrlEntry);
                sb.WriteLine("</iconUrl>");
            }

            if (ReadmeEntry != null)
            {
                sb.Write("    <readme>");
                sb.Write(ReadmeEntry);
                sb.WriteLine("</readme>");
            }

            sb.WriteLine("  </metadata>");
            sb.WriteLine("  <files>");
            foreach (var fe in FileEntries)
            {
                sb.Write($"    <file src=\"{fe.Item1}\"");
                if (!string.Empty.Equals(fe.Item2))
                {
                    sb.Write($" target=\"{fe.Item2}\"");
                }
                sb.WriteLine(" />");
            }
            sb.WriteLine("  </files>");
            sb.WriteLine("</package>");
        }
    }
}
