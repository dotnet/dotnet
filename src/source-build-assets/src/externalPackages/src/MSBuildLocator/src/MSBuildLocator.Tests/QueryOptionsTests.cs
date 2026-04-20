// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Build.Locator.Tests
{
    public class QueryOptionsTests
    {
        [Fact]
        public void CombinationTest()
        {
            VisualStudioInstance[] instances =
            {
                new VisualStudioInstance("A7D13212839F4997AF65F7F74618EBAB", "none", new Version(1, 0), DiscoveryType.DeveloperConsole),
                new VisualStudioInstance("DBF404629ED2408182263033F0358A1E", "none", new Version(1, 0), DiscoveryType.VisualStudioSetup),
                new VisualStudioInstance("98B38291074547D89A86758A26621EF3", "none", new Version(1, 0), DiscoveryType.DotNetSdk),
            };

            VerifyQueryResults(instances, DiscoveryType.DeveloperConsole, "A7D13212839F4997AF65F7F74618EBAB");
            VerifyQueryResults(instances, DiscoveryType.VisualStudioSetup, "DBF404629ED2408182263033F0358A1E");
            VerifyQueryResults(instances, DiscoveryType.DotNetSdk, "98B38291074547D89A86758A26621EF3");
            VerifyQueryResults(instances, DiscoveryType.DeveloperConsole | DiscoveryType.DotNetSdk, "A7D13212839F4997AF65F7F74618EBAB", "98B38291074547D89A86758A26621EF3");
            VerifyQueryResults(instances, DiscoveryType.DeveloperConsole | DiscoveryType.VisualStudioSetup | DiscoveryType.DotNetSdk, "A7D13212839F4997AF65F7F74618EBAB", "DBF404629ED2408182263033F0358A1E", "98B38291074547D89A86758A26621EF3");
            VerifyQueryResults(instances, DiscoveryType.VisualStudioSetup | DiscoveryType.DotNetSdk, "DBF404629ED2408182263033F0358A1E", "98B38291074547D89A86758A26621EF3");
        }

        [Fact]
        public void MultipleResultsTest()
        {
            VisualStudioInstance[] instances =
            {
                new VisualStudioInstance("24B4CD0C7A954C3DAF02FE2ED7B14B2D", "none", new Version(1, 0), DiscoveryType.DeveloperConsole),
                new VisualStudioInstance("EFE7BA53882F4214BBD8447EC0683FC8", "none", new Version(1, 0), DiscoveryType.VisualStudioSetup),
                new VisualStudioInstance("5F932E55D1B84DCB82EE97B47EB531EB", "none", new Version(1, 0), DiscoveryType.VisualStudioSetup),
                new VisualStudioInstance("78C4AEBB58AE44ACA0FA1E78B8306E2A", "none", new Version(1, 0), DiscoveryType.VisualStudioSetup),
            };

            VerifyQueryResults(instances, DiscoveryType.VisualStudioSetup, "EFE7BA53882F4214BBD8447EC0683FC8", "5F932E55D1B84DCB82EE97B47EB531EB", "78C4AEBB58AE44ACA0FA1E78B8306E2A");
        }

        [Fact]
        public void NoResultsTest()
        {
            VisualStudioInstance[] instances =
            {
                new VisualStudioInstance("2F592DDF8E1744FEA7488AE2D49412C6", "none", new Version(1, 0), DiscoveryType.DeveloperConsole),
                new VisualStudioInstance("74F6297B9E304C608D37655EBEA5F90E", "none", new Version(1, 0), DiscoveryType.VisualStudioSetup),
            };

            VerifyQueryResults(instances, DiscoveryType.DotNetSdk);
        }

        private void VerifyQueryResults(IEnumerable<VisualStudioInstance> instances, DiscoveryType discoveryTypes, params string[] expectedInstanceNames)
        {
            IEnumerable<VisualStudioInstance> actual = MSBuildLocator.QueryVisualStudioInstances(instances, new VisualStudioInstanceQueryOptions
            {
                DiscoveryTypes = discoveryTypes
            });

            if (expectedInstanceNames != null && expectedInstanceNames.Any())
            {
                List<string> names = actual
                    .Select(i => i.Name)
                    .ToList();

                names.ShouldBe(expectedInstanceNames, ignoreOrder: true);
            }
            else
            {
                actual.ShouldBeEmpty();
            }
        }
    }
}