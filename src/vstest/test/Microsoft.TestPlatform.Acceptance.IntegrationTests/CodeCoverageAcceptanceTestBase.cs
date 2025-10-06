// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.CodeCoverage.Core;
using Microsoft.CodeCoverage.Core.Reports.Coverage;
using Microsoft.TestPlatform.TestUtilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

public class CodeCoverageAcceptanceTestBase : AcceptanceTestBase
{
    /*
     * Below value is just safe coverage result for which all tests are passing.
     * Inspecting this value gives us confidence that there is no big drop in overall coverage.
     */
    protected const double ExpectedMinimalModuleCoverage = 30.0;

    protected static string GetNetStandardAdapterPath()
    {
        return Path.Combine(IntegrationTestEnvironment.PublishDirectory,
            $"Microsoft.CodeCoverage.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg", "build", "netstandard2.0");
    }

    protected static ModuleData? GetModule(CoverageReport coverageReport, string name)
    {
        var module = coverageReport.Modules.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
        module ??= coverageReport.Modules.FirstOrDefault(m => string.Equals(m.Name, Path.GetFileNameWithoutExtension(name), StringComparison.OrdinalIgnoreCase));

        return module;
    }

    protected static XmlNode? GetNode(XmlNode node, string type, string name)
    {
        return node.SelectSingleNode($"//{type}[@name='{name}']") ?? node.SelectSingleNode($"//{type}[@name='{name.ToLower(CultureInfo.CurrentCulture)}']");
    }

    protected static CoverageReport GetCoverageReport(string coverageResult)
    {
        CoverageFileUtilityV2 utility = new(new TpCoverageFileConfiguration());
        return utility.ReadCoverageReportAsync(coverageResult, default).Result;
    }

    protected static void AssertCoverage(ModuleData module, double expectedCoverage)
    {
        string coverageData = module.CoverageBuffer.Length == 0 ? module.LineCoverage : module.BlockCoverage;
        var coverage = double.Parse(coverageData, CultureInfo.InvariantCulture);
        Console.WriteLine($"Checking coverage for {module.Name}. Expected at least: {expectedCoverage}. Result: {coverage}");
        Assert.IsTrue(coverage > expectedCoverage, $"Coverage check failed for {module.Name}. Expected at least: {expectedCoverage}. Found: {coverage}");
    }

    protected static string GetCoverageFileNameFromTrx(string trxFilePath, string resultsDirectory)
    {
        Assert.IsTrue(File.Exists(trxFilePath), "Trx file not found: {0}", trxFilePath);
        XmlDocument doc = new();
        using var trxStream = new FileStream(trxFilePath, FileMode.Open, FileAccess.Read);
        doc.Load(trxStream);
        var deploymentElements = doc.GetElementsByTagName("Deployment");
        Assert.IsTrue(deploymentElements.Count == 1,
            "None or more than one Deployment tags found in trx file:{0}", trxFilePath);
        var deploymentDir = deploymentElements[0]!.Attributes!.GetNamedItem("runDeploymentRoot")?.Value;
        Assert.IsTrue(string.IsNullOrEmpty(deploymentDir) == false,
            "runDeploymentRoot attribute not found in trx file:{0}", trxFilePath);
        var collectors = doc.GetElementsByTagName("Collector");

        string? fileName = string.Empty;
        for (int i = 0; i < collectors.Count; i++)
        {
            if (string.Equals(collectors[i]!.Attributes!.GetNamedItem("collectorDisplayName")!.Value,
                    "Code Coverage", StringComparison.OrdinalIgnoreCase))
            {
                fileName = collectors[i]?.FirstChild?.FirstChild?.FirstChild?.Attributes?.GetNamedItem("href")
                    ?.Value;
            }
        }

        Assert.IsTrue(string.IsNullOrEmpty(fileName) == false, "Coverage file name not found in trx file: {0}",
            trxFilePath);
        return Path.Combine(resultsDirectory, deploymentDir, "In", fileName);
    }

    internal class TpCoverageFileConfiguration : ICoverageFileConfiguration
    {
        public bool ReadModules => true;

        public bool ReadSkippedModules => true;

        public bool ReadSkippedFunctions => true;

        public bool ReadSnapshotsData => true;

        public bool GenerateCoverageBufferFiles => false;

        public bool FixCoverageBuffersMismatch => false;

        public int MaxDegreeOfParallelism => 2;

        public bool SkipInvalidData => true;

        public CoverageMergeOperation MergeOperation => CoverageMergeOperation.MergeToCobertura;
    }
}
