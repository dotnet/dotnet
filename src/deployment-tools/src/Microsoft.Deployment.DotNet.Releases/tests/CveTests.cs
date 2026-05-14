// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class CveTests
    {
        [Fact]
        public void ItImplementsIEquatable()
        {
            List<Cve> cves = new List<Cve>();
            var cve1 = Cve.Create("cve-1", "https://cve.com");
            var cve2 = Cve.Create("cve-1", "https://cve.com");

            cves.Add(Cve.Create("cve-2", "https://cve.com"));
            cves.Add(cve1);
            cves.Add(cve2);

            Assert.Equal(cve1, cve2);
            Assert.Equal(2, cves.Distinct().Count());
        }

        [Fact]
        public void GetHashCodeReturnsTheSameValueIfObjectsAreEqual()
        {
            var a = Cve.Create("cve-1", "https://cve.com");
            var b = Cve.Create("cve-1", "https://cve.com");

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void ItCanDeserializeACveEntry()
        {
            Cve cve = new(JsonDocument.Parse(@"{""cve-id"": ""CVE-2020-1147"", ""cve-url"": ""https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2020-1147""}").RootElement);

            Assert.Equal("CVE-2020-1147", cve.Id);
            Assert.Equal("https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2020-1147", cve.DescriptionLink.ToString());
        }
    }
}
