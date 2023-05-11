// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.Test
{
    public class CompatibilityTableTests
    {
        [Fact]
        public void CompatibilityTable_PCL()
        {
            var fw1 = NuGetFramework.Parse("portable-net45+win8");
            var fw2 = NuGetFramework.Parse("portable-net45+win8+wp8");
            var win81 = NuGetFramework.Parse("win81");

            var all = new NuGetFramework[] { win81, fw1, fw2 };

            CompatibilityTable table = new CompatibilityTable(all);

            table.TryGetCompatible(win81, out IEnumerable<NuGetFramework>? compatible);

            var results = compatible.ToArray();

            Assert.Equal(3, results.Count());
            Assert.Equal(win81, results[0]);
            Assert.Equal(fw1, results[1]);
            Assert.Equal(fw2, results[2]);
        }

        [Fact]
        public void CompatibilityTable_Alias()
        {
            var win7 = NuGetFramework.Parse("win7");
            var netcore45 = NuGetFramework.Parse("netcore45");
            var win81 = NuGetFramework.Parse("win81");
            var win9 = NuGetFramework.Parse("win9");

            var all = new NuGetFramework[] { win7, win81, win9, netcore45 };

            CompatibilityTable table = new CompatibilityTable(all);

            table.TryGetCompatible(win9, out IEnumerable<NuGetFramework>? compatible);

            var results = compatible.ToArray();

            Assert.Equal(4, results.Count());
            Assert.Equal(win7, results[0]);
            Assert.Equal(win81, results[1]);
            Assert.Equal(win9, results[2]);
            Assert.Equal(netcore45, results[3]);
        }

        [Fact]
        public void CompatibilityTable_Basic2()
        {
            var net45 = NuGetFramework.Parse("net45");
            var net40 = NuGetFramework.Parse("net40");
            var net35 = NuGetFramework.Parse("net35");
            var wp8 = NuGetFramework.Parse("wp8");

            var all = new NuGetFramework[] { net35, net40, net45, wp8 };

            CompatibilityTable table = new CompatibilityTable(all);

            table.TryGetCompatible(net40, out IEnumerable<NuGetFramework>? compatible);

            Assert.Equal(2, compatible.Count());
            Assert.Equal(net35, compatible.First());
            Assert.Equal(net40, compatible.Skip(1).First());
        }

        [Fact]
        public void CompatibilityTable_Basic()
        {
            var net45 = NuGetFramework.Parse("net45");
            var net40 = NuGetFramework.Parse("net40");
            var net35 = NuGetFramework.Parse("net35");
            var wp8 = NuGetFramework.Parse("wp8");

            var all = new NuGetFramework[] { net35, net40, net45, wp8 };

            CompatibilityTable table = new CompatibilityTable(all);

            table.TryGetCompatible(wp8, out IEnumerable<NuGetFramework>? compatible);

            Assert.Equal(wp8, compatible.Single());
        }

        [Fact]
        public void CompatibilityTable_NearestNotFound()
        {
            var net45 = NuGetFramework.Parse("net45");
            var net40 = NuGetFramework.Parse("net40");
            var wp8 = NuGetFramework.Parse("wp8");

            var all = new NuGetFramework[] { net45, net40 };

            CompatibilityTable table = new CompatibilityTable(all);

            Assert.Null(table.GetNearest(wp8).SingleOrDefault());
        }

        [Fact]
        public void CompatibilityTable_NearestFound()
        {
            var net45 = NuGetFramework.Parse("net45");
            var net40 = NuGetFramework.Parse("net40");

            var all = new NuGetFramework[] { net45, net40 };

            CompatibilityTable table = new CompatibilityTable(all);

            Assert.Equal(net45, table.GetNearest(net45).Single());
            Assert.Equal(net40, table.GetNearest(net40).Single());
        }

        [Fact]
        public void CompatibilityTable_NearestSingle()
        {
            var net49 = NuGetFramework.Parse("net49");
            var net35 = NuGetFramework.Parse("net35");
            var net45 = NuGetFramework.Parse("net45");
            var net40 = NuGetFramework.Parse("net40");

            var all = new NuGetFramework[] { net45, net40 };

            CompatibilityTable table = new CompatibilityTable(all);

            Assert.Equal(net45, table.GetNearest(net49).Single());
            Assert.Null(table.GetNearest(net35).SingleOrDefault());
        }
    }
}
