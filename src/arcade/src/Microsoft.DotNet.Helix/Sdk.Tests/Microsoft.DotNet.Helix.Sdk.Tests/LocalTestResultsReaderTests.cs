// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class LocalTestResultsReaderTests
    {
        [Fact]
        public async Task LocalTestResultsReader_ReadsXunitFileFromDownloadedResults()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string workItemDirectory = Path.Combine(tempDirectory, "work-item");
            Directory.CreateDirectory(workItemDirectory);

            try
            {
                File.WriteAllText(
                    Path.Combine(workItemDirectory, "testResults.xml"),
                    """
                    <assemblies>
                      <assembly name="Sample.Tests.dll" total="1" passed="1" failed="0" skipped="0">
                        <collection total="1" passed="1" failed="0" skipped="0">
                          <test name="Sample.Tests.Passes" type="Sample.Tests" method="Passes" time="0.125" result="Pass" />
                        </collection>
                      </assembly>
                    </assemblies>
                    """);

                var reader = new LocalTestResultsReader(NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>());
                string filePath = Path.Combine(workItemDirectory, "testResults.xml");
                IReadOnlyList<TestResult> resultSets = await reader.ReadResultFileAsync(filePath);
                IReadOnlyList<AggregatedResult> aggregate = new ResultAggregator().Aggregate([resultSets]);
                AggregatedResult result = Assert.Single(aggregate);

                Assert.Equal("Sample.Tests.Passes", result.Name);
                Assert.Equal("Passed", result.Result);
            }
            finally
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }

        [Fact]
        public async Task LocalTestResultsReader_CombinesPackedAndXmlResultsAcrossWorkItems()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string packedDirectory = Path.Combine(tempDirectory, "packed-item");
            string xmlDirectory = Path.Combine(tempDirectory, "xml-item");
            Directory.CreateDirectory(packedDirectory);
            Directory.CreateDirectory(xmlDirectory);
            string originalDirectory = Environment.CurrentDirectory;

            try
            {
                Environment.CurrentDirectory = packedDirectory;
                string filePath = Path.Combine(xmlDirectory, "testResults.xml");

                File.WriteAllText(
                    filePath,
                    """
                    <assemblies>
                      <assembly name="Xml.Tests.dll" total="1" passed="1" failed="0" skipped="0">
                        <collection total="1" passed="1" failed="0" skipped="0">
                          <test name="Xml.Tests.Passes" type="Xml.Tests" method="Passes" time="0.250" result="Pass" />
                        </collection>
                      </assembly>
                    </assemblies>
                    """);

                IReadOnlyList<TestResult> resultSets = await new LocalTestResultsReader(NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>()).ReadResultFileAsync(filePath);
                IReadOnlyList<AggregatedResult> aggregate = new ResultAggregator().Aggregate([resultSets]);

                Assert.Single(aggregate);
                Assert.Contains(aggregate, static x => x.Name == "Xml.Tests.Passes");
            }
            finally
            {
                Environment.CurrentDirectory = originalDirectory;
                Directory.Delete(tempDirectory, recursive: true);
            }
        }
    }
}
