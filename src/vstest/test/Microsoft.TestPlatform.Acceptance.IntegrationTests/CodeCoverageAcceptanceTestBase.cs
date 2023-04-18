// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;

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

    protected static string GetCodeCoverageExePath()
    {
        return Path.Combine(GetNetStandardAdapterPath(), "CodeCoverage", "CodeCoverage.exe");
    }

    protected static XmlNode? GetModuleNode(XmlNode node, string name)
    {
        var moduleNode = GetNode(node, "module", name);

        if (moduleNode == null)
        {
            moduleNode = GetNode(node, "package", name);

            moduleNode ??= GetNode(node, "package", Path.GetFileNameWithoutExtension(name));
        }

        return moduleNode;
    }

    protected static XmlNode? GetNode(XmlNode node, string type, string name)
    {
        return node.SelectSingleNode($"//{type}[@name='{name}']") ?? node.SelectSingleNode($"//{type}[@name='{name.ToLower(CultureInfo.CurrentCulture)}']");
    }

    protected static XmlDocument GetXmlCoverage(string coverageResult, TempDirectory tempDirectory)
    {
        var coverage = new XmlDocument();

        if (coverageResult.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            coverage.Load(coverageResult);
            return coverage;
        }

        var codeCoverageExe = GetCodeCoverageExePath();
        var output = Path.Combine(tempDirectory.Path, Guid.NewGuid().ToString() + ".xml");

        var watch = new Stopwatch();

        Console.WriteLine($"Starting {codeCoverageExe}");
        watch.Start();
        var analyze = Process.Start(new ProcessStartInfo
        {
            FileName = codeCoverageExe,
            Arguments = $"analyze /include_skipped_functions /include_skipped_modules /output:\"{output}\" \"{coverageResult}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false
        });

        string analysisOutput = analyze!.StandardOutput.ReadToEnd();

        analyze.WaitForExit();
        watch.Stop();
        Console.WriteLine($"Total execution time: {watch.Elapsed.Duration()}");

        Assert.IsTrue(0 == analyze.ExitCode, $"Code Coverage analyze failed: {analysisOutput}");

        coverage.Load(output);
        return coverage;
    }

    protected static void AssertCoverage(XmlNode node, double expectedCoverage)
    {
        var coverage = node.Attributes!["block_coverage"] != null
            ? double.Parse(node.Attributes!["block_coverage"]!.Value, CultureInfo.InvariantCulture)
            : double.Parse(node.Attributes!["line-rate"]!.Value, CultureInfo.InvariantCulture) * 100;
        Console.WriteLine($"Checking coverage for {node.Name} {node.Attributes!["name"]!.Value}. Expected at least: {expectedCoverage}. Result: {coverage}");
        Assert.IsTrue(coverage > expectedCoverage, $"Coverage check failed for {node.Name} {node.Attributes!["name"]!.Value}. Expected at least: {expectedCoverage}. Found: {coverage}");
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
}
