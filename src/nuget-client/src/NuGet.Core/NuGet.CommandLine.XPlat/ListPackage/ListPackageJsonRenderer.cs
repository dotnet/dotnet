// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.ListPackage
{
    /// <summary>
    /// json format renderer for dotnet list package command
    /// </summary>
    internal class ListPackageJsonRenderer : IReportRenderer
    {
        private const int ReportOutputVersion = 1;
        private const string VersionProperty = "version";
        private const string ParametersProperty = "parameters";
        private const string ProblemsProperty = "problems";
        private const string SourcesProperty = "sources";
        private const string ProjectProperty = "project";
        private const string ProjectsProperty = "projects";
        private const string FrameworksProperty = "frameworks";
        private const string FrameworkProperty = "framework";
        private const string PathProperty = "path";
        private const string TopLevelPackagesProperty = "topLevelPackages";
        private const string TransitivePackagesProperty = "transitivePackages";
        private const string IdProperty = "id";
        private const string RequestedVersionProperty = "requestedVersion";
        private const string ResolvedVersionProperty = "resolvedVersion";
        private const string AutoReferencedProperty = "autoReferenced";
        private const string SeverityProperty = "severity";
        private const string AdvisoryUrlProperty = "advisoryurl";
        private const string VulnerabilitiesProperty = "vulnerabilities";
        private const string LatestVersionProperty = "latestVersion";
        private const string DeprecationReasonsProperty = "deprecationReasons";
        private const string AlternativePackageProperty = "alternativePackage";
        private const string VersionRangeProperty = "versionRange";
        private const string LevelProperty = "level";
        private const string WarningProperty = "warning";
        private const string TextProperty = "text";
        private const string ErrorProperty = "error";

        protected readonly List<ReportProblem> _problems = new();
        protected TextWriter _writer;

        private ListPackageJsonRenderer()
        { }

        public ListPackageJsonRenderer(TextWriter textWriter = null)
        {
            _writer = textWriter ?? Console.Out;
        }

        public void AddProblem(ProblemType problemType, string text)
        {
            _problems.Add(new ReportProblem(problemType, string.Empty, text));
        }

        public IEnumerable<ReportProblem> GetProblems()
        {
            return _problems;
        }

        public void Render(ListPackageReportModel listPackageReportModel)
        {
            // Aggregate problems from projects.
            _problems.AddRange(listPackageReportModel.Projects.Where(p => p.ProjectProblems != null).SelectMany(p => p.ProjectProblems));
            var jsonRenderedOutput = WriteJson(listPackageReportModel);
            _writer.WriteLine(jsonRenderedOutput);
        }

        internal string WriteJson(ListPackageReportModel listPackageReportModel)
        {
            using (var writer = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    WriteJson(jsonWriter, listPackageReportModel);
                }

                return writer.ToString();
            }
        }

        private void WriteJson(JsonWriter writer, ListPackageReportModel listPackageReportModel)
        {
            ListPackageArgs listPackageArgs = listPackageReportModel.ListPackageArgs;
            writer.WriteStartObject();

            writer.WritePropertyName(VersionProperty);
            writer.WriteValue(ReportOutputVersion);

            writer.WritePropertyName(ParametersProperty);
            writer.WriteValue(PathUtility.GetPathWithForwardSlashes(listPackageArgs.ArgumentText));

            if (listPackageReportModel.Projects.Any(p => p.AutoReferenceFound))
            {
                _problems.Add(new ReportProblem(ProblemType.Warning, string.Empty, Strings.ListPkg_AutoReferenceDescription));
            }

            if (_problems?.Count > 0)
            {
                WriteProblems(writer, _problems);
            }

            WriteSources(writer, listPackageReportModel);
            WriteProjects(writer, listPackageReportModel.Projects, listPackageReportModel.ListPackageArgs);
            writer.WriteEndObject();
        }

        private static void WriteProblems(JsonWriter writer, IEnumerable<ReportProblem> reportProblems)
        {
            writer.WritePropertyName(ProblemsProperty);
            writer.WriteStartArray();

            foreach (ReportProblem reportProblem in reportProblems)
            {
                writer.WriteStartObject();

                if (!string.IsNullOrEmpty(reportProblem.Project))
                {
                    writer.WritePropertyName(ProjectProperty);
                    writer.WriteValue(PathUtility.GetPathWithForwardSlashes(reportProblem.Project));
                }

                writer.WritePropertyName(LevelProperty);
                writer.WriteValue(reportProblem.ProblemType == ProblemType.Error ? ErrorProperty : WarningProperty);
                writer.WritePropertyName(TextProperty);
                writer.WriteValue(PathUtility.GetPathWithForwardSlashes(reportProblem.Text));
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        private static void WriteSources(JsonWriter writer, ListPackageReportModel listPackageReportModel)
        {
            if (listPackageReportModel.ListPackageArgs.ReportType == ReportType.Default)
            {
                // generic list is offline.
                return;
            }

            writer.WritePropertyName(SourcesProperty);
            writer.WriteStartArray();

            if (listPackageReportModel.ListPackageArgs.ReportType == ReportType.Vulnerable && listPackageReportModel.AuditSourcesUsed.Count > 0)
            {
                foreach (PackageSource packageSource in listPackageReportModel.AuditSourcesUsed)
                {
                    writer.WriteValue(PathUtility.GetPathWithForwardSlashes(packageSource.Source));
                }
            }
            else
            {
                foreach (PackageSource packageSource in listPackageReportModel.ListPackageArgs.PackageSources)
                {
                    writer.WriteValue(PathUtility.GetPathWithForwardSlashes(packageSource.Source));
                }
            }

            writer.WriteEndArray();
        }

        private static void WriteProjects(JsonWriter writer, List<ListPackageProjectModel> reportProjects, ListPackageArgs listPackageArgs)
        {
            writer.WritePropertyName(ProjectsProperty);
            writer.WriteStartArray();

            foreach (ListPackageProjectModel reportProject in reportProjects)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(PathProperty);
                writer.WriteValue(PathUtility.GetPathWithForwardSlashes(reportProject.ProjectPath));

                if (reportProject.TargetFrameworkPackages?.Count > 0)
                {
                    writer.WritePropertyName(FrameworksProperty);

                    WriteFrameworkPackage(writer, reportProject.TargetFrameworkPackages, listPackageArgs);

                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        private static void WriteFrameworkPackage(JsonWriter writer, List<ListPackageReportFrameworkPackage> reportFrameworkPackages, ListPackageArgs listPackageArgs)
        {
            writer.WriteStartArray();

            foreach (ListPackageReportFrameworkPackage reportFrameworkPackage in reportFrameworkPackages)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(FrameworkProperty);
                writer.WriteValue(reportFrameworkPackage.Framework);
                WriteTopLevelPackages(writer, TopLevelPackagesProperty, reportFrameworkPackage.TopLevelPackages, listPackageArgs);
                WriteTransitivePackages(writer, TransitivePackagesProperty, reportFrameworkPackage.TransitivePackages, listPackageArgs);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        private static void WriteTopLevelPackages(JsonWriter writer, string property, List<ListReportPackage> topLevelPackages, ListPackageArgs listPackageArgs)
        {
            if (topLevelPackages != null)
            {
                writer.WritePropertyName(property);

                writer.WriteStartArray();
                foreach (ListReportPackage topLevelPackage in topLevelPackages)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName(IdProperty);
                    writer.WriteValue(topLevelPackage.PackageId);

                    writer.WritePropertyName(RequestedVersionProperty);
                    writer.WriteValue(topLevelPackage.RequestedVersion);

                    writer.WritePropertyName(ResolvedVersionProperty);
                    writer.WriteValue(topLevelPackage.ResolvedVersion);

                    if (topLevelPackage.AutoReference)
                    {
                        writer.WritePropertyName(AutoReferencedProperty);
                        writer.WriteValue("true");
                    }

                    switch (listPackageArgs.ReportType)
                    {
                        case ReportType.Outdated:
                            writer.WritePropertyName(LatestVersionProperty);
                            writer.WriteValue(topLevelPackage.LatestVersion);
                            break;
                        case ReportType.Deprecated:
                            WriteDeprecations(writer, topLevelPackage);
                            break;
                        case ReportType.Vulnerable:
                            WriteVulnerabilities(writer, topLevelPackage.Vulnerabilities);
                            break;
                        default:
                            break;
                    }

                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
        }

        private static void WriteTransitivePackages(JsonWriter writer, string property, List<ListReportPackage> transitivePackages, ListPackageArgs listPackageArgs)
        {
            if (!listPackageArgs.IncludeTransitive)
            {
                return;
            }

            if (transitivePackages?.Count > 0)
            {
                writer.WritePropertyName(property);

                writer.WriteStartArray();
                foreach (ListReportPackage transitivePackage in transitivePackages)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName(IdProperty);
                    writer.WriteValue(transitivePackage.PackageId);

                    writer.WritePropertyName(ResolvedVersionProperty);
                    writer.WriteValue(transitivePackage.ResolvedVersion);

                    switch (listPackageArgs.ReportType)
                    {
                        case ReportType.Outdated:
                            writer.WritePropertyName(LatestVersionProperty);
                            writer.WriteValue(transitivePackage.LatestVersion);
                            break;
                        case ReportType.Deprecated:
                            WriteDeprecations(writer, transitivePackage);
                            break;
                        case ReportType.Vulnerable:
                            WriteVulnerabilities(writer, transitivePackage.Vulnerabilities);
                            break;
                        default:
                            break;
                    }


                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
        }

        private static void WriteDeprecations(JsonWriter writer, ListReportPackage listPackage)
        {
            if (listPackage.DeprecationReasons != null)
            {
                writer.WritePropertyName(DeprecationReasonsProperty);
                writer.WriteStartArray();

                foreach (var deprecationReason in listPackage.DeprecationReasons?.Reasons)
                {
                    writer.WriteValue(deprecationReason);
                }

                writer.WriteEndArray();
            }

            if (listPackage.AlternativePackage != null)
            {
                writer.WritePropertyName(AlternativePackageProperty);
                writer.WriteStartObject();
                writer.WritePropertyName(IdProperty);
                writer.WriteValue(listPackage.AlternativePackage.PackageId);
                writer.WritePropertyName(VersionRangeProperty);

                var versionRangeString = VersionRangeFormatter.Instance.Format(
                    "p",
                    listPackage.AlternativePackage.Range,
                    VersionRangeFormatter.Instance);

                writer.WriteValue(versionRangeString);
                writer.WriteEndObject();
            }
        }

        private static void WriteVulnerabilities(JsonWriter writer, List<PackageVulnerabilityMetadata> vulnerabilities)
        {
            if (vulnerabilities == null)
            {
                return;
            }

            writer.WritePropertyName(VulnerabilitiesProperty);
            writer.WriteStartArray();

            foreach (PackageVulnerabilityMetadata vulnerability in vulnerabilities)
            {
                writer.WriteStartObject();
                var severity = (vulnerability?.Severity ?? -1) switch
                {
                    0 => "Low",
                    1 => "Moderate",
                    2 => "High",
                    3 => "Critical",
                    _ => string.Empty,
                };

                writer.WritePropertyName(SeverityProperty);
                writer.WriteValue(severity);

                writer.WritePropertyName(AdvisoryUrlProperty);
                writer.WriteValue(vulnerability.AdvisoryUrl);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
