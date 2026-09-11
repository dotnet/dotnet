// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.DotNet.UnifiedBuild.Tasks.UsageReport
{
    public class WriteAnnotatedUsageReport : Task
    {
        private const string ProducedPrefix = "PackageVersions.";
        private const string ProducedSuffix = ".Produced.props";

        /// <summary>
        /// Source usage data JSON file.
        /// </summary>
        [Required]
        public string DataFile { get; set; }

        /// <summary>
        /// Suffix to use for the version property name.
        /// </summary>
        [Required]
        public string VersionPropertySuffix { get; set; }

        /// <summary>
        /// A set of "PackageVersions.{repo}.Produced.props" files. Each file lists the packages
        /// directly produced by a specific repo, providing accurate attribution without relying
        /// on snapshot diffs (which can misattribute packages when repos have independent
        /// dependency trees).
        /// </summary>
        [Required]
        public ITaskItem[] PackageVersionPropsProducedFiles { get; set; }

        /// <summary>
        /// File containing the results of poisoning the prebuilts. Example format:
        /// 
        /// MATCH: output built\dotnet-sdk-...\System.Collections.dll(hash 4b...31) matches one of:
        ///     intermediate\netstandard.library.2.0.1.nupkg\build\...\System.Collections.dll
        /// 
        /// The usage report reads this file, looking for 'intermediate\*.nupkg' to annotate.
        /// </summary>
        public string PoisonedReportFile { get; set; }

        /// <summary>
        /// Path to write the prebuilt annotated usage report file.
        /// </summary>
        [Required]
        public string PrebuiltAnnotatedUsageReportFile { get; set; }

        public override bool Execute()
        {
            UsageData data = UsageData.Parse(XElement.Parse(File.ReadAllText(DataFile)));
            IEnumerable<RepoOutput> sourceBuildRepoOutputs = GetSourceBuildRepoOutputs();

            var poisonNupkgFilenames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (File.Exists(PoisonedReportFile))
            {
                foreach (string line in File.ReadAllLines(PoisonedReportFile))
                {
                    string[] segments = line.Split('\\');
                    if (segments.Length > 2 &&
                        segments[0].Trim() == "intermediate" &&
                        segments[1].EndsWith(".nupkg"))
                    {
                        poisonNupkgFilenames.Add(Path.GetFileNameWithoutExtension(segments[1]));
                    }
                }
            }

            var report = new XElement("AnnotatedUsages");

            var annotatedUsages = data.Usages.NullAsEmpty()
                .Select(usage =>
                {
                    string id = usage.PackageIdentity.Id;
                    string version = usage.PackageIdentity.Version.OriginalVersion;

                    string pvpIdent = WritePackageVersionsProps.GetPropertyName(id, VersionPropertySuffix);

                    var sourceBuildCreator = new StringBuilder();
                    foreach (RepoOutput output in sourceBuildRepoOutputs)
                    {
                        foreach (PackageVersionPropsElement p in output.Built)
                        {
                            if (p.Name.Equals(pvpIdent, StringComparison.OrdinalIgnoreCase))
                            {
                                if (sourceBuildCreator.Length != 0)
                                {
                                    sourceBuildCreator.Append(" ");
                                }
                                sourceBuildCreator.Append(output.Repo);
                                sourceBuildCreator.Append(" ");
                                sourceBuildCreator.Append(p.Name);
                                sourceBuildCreator.Append("/");
                                sourceBuildCreator.Append(p.Version);
                            }
                        }
                    }

                    return new AnnotatedUsage
                    {
                        Usage = usage,

                        Project = data.ProjectDirectories
                            ?.FirstOrDefault(p => usage.AssetsFile?.StartsWith(p) ?? false),

                        SourceBuildPackageIdCreator = sourceBuildCreator.Length == 0
                            ? null
                            : sourceBuildCreator.ToString(),

                        TestProjectByHeuristic = IsTestUsageByHeuristic(usage),

                        EndsUpInOutput = poisonNupkgFilenames.Contains($"{id}.{version}")
                    };
                })
                .ToArray();

            foreach (var onlyTestProjectUsage in annotatedUsages
                .GroupBy(u => u.Usage.PackageIdentity)
                .Where(g => g.All(u => u.TestProjectByHeuristic))
                .SelectMany(g => g))
            {
                onlyTestProjectUsage.TestProjectOnlyByHeuristic = true;
            }

            report.Add(annotatedUsages.Select(u => u.ToXml()));

            Directory.CreateDirectory(Path.GetDirectoryName(PrebuiltAnnotatedUsageReportFile));
            File.WriteAllText(PrebuiltAnnotatedUsageReportFile, report.ToString());

            return !Log.HasLoggedErrors;
        }

        private RepoOutput[] GetSourceBuildRepoOutputs()
        {
            return PackageVersionPropsProducedFiles.NullAsEmpty()
                .Select(item =>
                {
                    string filename = Path.GetFileName(item.ItemSpec);
                    string repo = filename.Substring(
                        ProducedPrefix.Length,
                        filename.Length - ProducedPrefix.Length - ProducedSuffix.Length);

                    var xml = XElement.Parse(File.ReadAllText(item.ItemSpec));
                    return new RepoOutput
                    {
                        Repo = repo,
                        Built = PackageVersionPropsElement.Parse(xml)
                    };
                })
                .ToArray();
        }

        public static bool IsTestUsageByHeuristic(Usage usage)
        {
            string[] assetsFileParts = usage.AssetsFile?.Split('/', '\\');

            // If the dir name ends in Test(s), it's probably a test project.
            // Ignore the first two segments to avoid classifying everything in "src/vstest".
            // This also catches "test" dirs that contain many test projects.
            if (assetsFileParts?.Skip(2).Any(p =>
                p.EndsWith("Tests", StringComparison.OrdinalIgnoreCase) ||
                p.EndsWith("Test", StringComparison.OrdinalIgnoreCase)) == true)
            {
                return true;
            }

            // CoreFX restores test dependencies during this sync project.
            if (assetsFileParts?.Contains("XUnit.Runtime") == true)
            {
                return true;
            }

            return false;
        }

        private class RepoOutput
        {
            public string Repo { get; set; }
            public PackageVersionPropsElement[] Built { get; set; }
        }

        private struct PackageVersionPropsElement
        {
            public static PackageVersionPropsElement[] Parse(XElement xml)
            {
                return xml
                    // Get the first PropertyGroup. The second PropertyGroup is 'extra properties', and the third group is the creation time.
                    // Only select the first because the extra properties are not built packages.
                    .Elements()
                    .First()
                    // Get all *PackageVersion property elements.
                    .Elements()
                    .Select(x => new PackageVersionPropsElement(
                        x.Name.LocalName,
                        x.Nodes().OfType<XText>().First().Value))
                    .ToArray();
            }

            public string Name { get; }
            public string Version { get; }

            public PackageVersionPropsElement(string name, string version)
            {
                Name = name;
                Version = version;
            }
        }
    }
}
