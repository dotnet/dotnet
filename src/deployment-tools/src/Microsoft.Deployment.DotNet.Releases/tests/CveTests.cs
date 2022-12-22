// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class CveTests
    {
        [Fact]
        public void ItImplementsIEquatable()
        {
            List<Cve> cves = new List<Cve>();
            var cve1 = new Cve("cve-1", "https://cve.com");
            var cve2 = new Cve("cve-1", "https://cve.com");

            cves.Add(new Cve("cve-2", "https://cve.com"));
            cves.Add(cve1);
            cves.Add(cve2);

            Assert.Equal(cve1, cve2);
            Assert.Equal(2, cves.Distinct().Count());
        }

        [Fact]
        public void GetHashCodeReturnsTheSameValueIfObjectsAreEqual()
        {
            var a = new Cve("cve-1", "https://cve.com");
            var b = new Cve("cve-1", "https://cve.com");

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
    }
}
