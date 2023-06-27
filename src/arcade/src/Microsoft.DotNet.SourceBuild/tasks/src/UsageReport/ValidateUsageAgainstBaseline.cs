// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Packaging.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Microsoft.DotNet.SourceBuild.Tasks.UsageReport
{
    public class ValidateUsageAgainstBaseline : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string DataFile { get; set; }

        /// <summary>
        /// The prebuilt baseline: an XML file that lists allowed prebuilt usage.
        /// </summary>
        public string BaselineDataFile { get; set; }

        /// <summary>
        /// A hint path used in error messages to tell the user where to update the prebuilt
        /// baseline data if a baseline validation error occurs. If there is no baseline data file
        /// at all yet, the error indicates that one should be created at this location if the
        /// prebuilt usage should be permitted.
        /// </summary>
        [Required]
        public string BaselineDataUpdateHintFile { get; set; }

        [Required]
        public string OutputBaselineFile { get; set; }

        [Required]
        public string OutputReportFile { get; set; }

        private readonly string _preBuiltDocMessage = "See https://aka.ms/dotnet/prebuilts " +
            "for guidance on what pre-builts are and how to eliminate them.";

        private readonly string _reviewRequestMessage = "Whenever altering this " +
            "or other Source Build files, please include @dotnet/source-build-internal " +
            "as a reviewer.";

        public override bool Execute()
        {
            string ReviewRequestComment = $"<!-- {_reviewRequestMessage} -->{Environment.NewLine}";
            string PreBuiltDocXmlComment = $"<!-- {_preBuiltDocMessage} -->{Environment.NewLine}";
            
            var used = UsageData.Parse(XElement.Parse(File.ReadAllText(DataFile)));

            string baselineText = string.IsNullOrEmpty(BaselineDataFile)
                ? "<UsageData />"
                : File.ReadAllText(BaselineDataFile);

            var baseline = UsageData.Parse(XElement.Parse(baselineText));

            UsageValidationData data = GetUsageValidationData(baseline, used);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputBaselineFile));
            File.WriteAllText(OutputBaselineFile, ReviewRequestComment + PreBuiltDocXmlComment + Environment.NewLine + data.ActualUsageData.ToXml().ToString());

            Directory.CreateDirectory(Path.GetDirectoryName(OutputReportFile));
            File.WriteAllText(OutputReportFile, PreBuiltDocXmlComment + Environment.NewLine + data.Report.ToString());

            return !Log.HasLoggedErrors;
        }

        public UsageValidationData GetUsageValidationData(UsageData baseline, UsageData used)
        {
            // Remove prebuilts from the used data if the baseline says to ignore them. Do this
            // first, so the new generated baseline doesn't list usages that are ignored by a
            // pattern anyway.
            ApplyBaselineIgnorePatterns(used, baseline);

            // Find new, removed, and unchanged usage after filtering patterns.
            Comparison<PackageIdentity> diff = Compare(
                used.Usages.Select(u => u.GetIdentityWithoutRid()).Distinct(),
                baseline.Usages.Select(u => u.GetIdentityWithoutRid()).Distinct());

            var report = new XElement("BaselineComparison");

            bool tellUserToUpdateBaseline = false;

            if (diff.Added.Any())
            {
                tellUserToUpdateBaseline = true;
                Log.LogError(
                    $"{diff.Added.Length} new pre-builts discovered! Detailed usage " +
                    $"report can be found at {OutputReportFile}.{Environment.NewLine}{_preBuiltDocMessage}{Environment.NewLine}" +
                    $"Package IDs are:{Environment.NewLine}" + string.Join(Environment.NewLine, diff.Added.Select(u => u.ToString())));

                // In the report, list full usage info, not only identity.
                report.Add(
                    new XElement(
                        "New",
                        used.Usages
                            .Where(u => diff.Added.Contains(u.GetIdentityWithoutRid()))
                            .Select(u => u.ToXml())));
            }

            if (diff.Removed.Any())
            {
                tellUserToUpdateBaseline = true;
                Log.LogMessage(
                    MessageImportance.High,
                    $"{diff.Removed.Length} packages in baseline weren't used!");

                report.Add(new XElement("Removed", diff.Removed.Select(id => id.ToXElement())));
            }

            if (diff.Unchanged.Any())
            {
                Log.LogMessage(
                    MessageImportance.High,
                    $"{diff.Unchanged.Length} packages used as expected in the baseline.");
            }

            // Simplify the used data to what is necessary for a baseline, to reduce file size.
            foreach (var usage in used.Usages)
            {
                usage.AssetsFile = null;
            }

            used.ProjectDirectories = null;
            used.Usages = used.Usages.Distinct().ToArray();

            if (tellUserToUpdateBaseline)
            {
                string baselineNotFoundWarning = "";
                if (string.IsNullOrEmpty(BaselineDataFile))
                {
                    baselineNotFoundWarning =
                        $"not expected, because no baseline file exists at '{BaselineDataUpdateHintFile}'";
                }
                else
                {
                    baselineNotFoundWarning =
                        $"different from the baseline found at '{BaselineDataFile}'";
                }

                Log.LogMessage(
                    MessageImportance.High,
                    $"Prebuilt usages are {baselineNotFoundWarning}. If it's acceptable to " +
                    "update the baseline, copy the contents of the automatically generated " +
                    $"baseline '{OutputBaselineFile}'.");
            }

            return new UsageValidationData
            {
                Report = report,
                ActualUsageData = used
            };
        }

        private static void ApplyBaselineIgnorePatterns(UsageData actual, UsageData baseline)
        {
            Regex[] ignoreUsageRegexes = baseline.IgnorePatterns.NullAsEmpty()
                .Select(p => p.CreateRegex())
                .ToArray();

            actual.IgnorePatterns = baseline.IgnorePatterns;

            var ignoredUsages = actual.Usages
                .Where(usage =>
                {
                    string id = $"{usage.PackageIdentity.Id}/{usage.PackageIdentity.Version}";
                    return ignoreUsageRegexes.Any(r => r.IsMatch(id));
                })
                .ToArray();

            actual.Usages = actual.Usages.Except(ignoredUsages).ToArray();
        }

        private static Comparison<T> Compare<T>(IEnumerable<T> actual, IEnumerable<T> baseline)
        {
            return new Comparison<T>(actual.ToArray(), baseline.ToArray());
        }

        private class Comparison<T>
        {
            public T[] Added { get; }
            public T[] Removed { get; }
            public T[] Unchanged { get; }

            public Comparison(
                IEnumerable<T> actual,
                IEnumerable<T> baseline)
            {
                Added = actual.Except(baseline).ToArray();
                Removed = baseline.Except(actual).ToArray();
                Unchanged = actual.Intersect(baseline).ToArray();
            }
        }
    }
}
