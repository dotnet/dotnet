// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CompatTestExtension
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

    /// <summary>
    /// Test discoverer built with older ObjectModel assemblies for compatibility testing.
    /// </summary>
    public class TestDiscoverer : ITestDiscoverer
    {
        /// <inheritdoc/>
        public void DiscoverTests(
            IEnumerable<string> sources,
            IDiscoveryContext discoveryContext,
            IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            var sourceList = sources.ToList();
            foreach (var source in sourceList)
            {
                Debug.Assert(File.Exists(source), $"Source path doesn't exist: {source}");
            }

            // TODO Add validation for runsettings
            logger.SendMessage(TestMessageLevel.Informational, "Start discovery");
            logger.SendMessage(TestMessageLevel.Informational, $"COMPATEXTENSION:{discoveryContext.RunSettings.SettingsXml}");

            // Create a few dummy tests
            var test = new TestCase(
                           "Assembly.Class.Method",
                           new Uri("discoverer://compatTestDiscoverer"),
                           "/tmp/sampleTestFile.dll");
            test.DisplayName = "Sample test case";
            test.CodeFilePath = "/tmp/sampleTestFile.cs";
        }
    }
}
