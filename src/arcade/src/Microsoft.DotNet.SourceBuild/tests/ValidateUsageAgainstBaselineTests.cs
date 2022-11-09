// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.DotNet.SourceBuild.Tasks.UsageReport;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.DotNet.SourceBuild.Tasks.Tests
{
    public class ValidateUsageAgainstBaselineTests
    {
        public Usage SimpleUsage(string idVersion)
        {
            var parts = idVersion.Split('/');
            return new Usage
            {
                PackageIdentity = new PackageIdentity(parts[0], NuGetVersion.Parse(parts[1]))
            };
        }

        public UsageData SimpleUsageData(params string[] ids) => new UsageData
        {
            Usages = ids.Select(SimpleUsage).ToArray()
        };

        [Fact]
        public void NewUsageShowsInReport()
        {
            var mockEngine = new Mock<IBuildEngine>(MockBehavior.Loose);
            var task = new ValidateUsageAgainstBaseline { BuildEngine = mockEngine.Object };

            var baseline = SimpleUsageData("A/1.0.0");
            var used = SimpleUsageData("A/1.0.0", "B/1.0.0");
            var data = task.GetUsageValidationData(baseline, used);

            Assert.Equal(
                XElement
                    .Parse(@"<BaselineComparison> <New> <Usage Id=""B"" Version=""1.0.0"" /> </New> </BaselineComparison>")
                    .ToString(),
                data.Report.ToString());
        }

        [Fact]
        public void IgnoredNewUsagesAreNotOnGeneratedBaseline()
        {
            var mockEngine = new Mock<IBuildEngine>(MockBehavior.Loose);
            var task = new ValidateUsageAgainstBaseline { BuildEngine = mockEngine.Object };

            var baseline = SimpleUsageData();
            baseline.IgnorePatterns = new[]
            {
                new UsagePattern { IdentityGlob = "A/*" },
                new UsagePattern { IdentityGlob = "B/*" },
                new UsagePattern { IdentityRegex = "Ca[tr]/.*" },
            };

            var used = SimpleUsageData("A/1.0.0", "B/1.0.0", "Cat/1.2.3", "Car/1.4.5");

            var data = task.GetUsageValidationData(baseline, used);

            Assert.Empty(data.ActualUsageData.Usages);
        }

        [Fact]
        public void IgnoredBaselineUsagesAreRemoved()
        {
            var mockEngine = new Mock<IBuildEngine>(MockBehavior.Loose);
            var task = new ValidateUsageAgainstBaseline { BuildEngine = mockEngine.Object };

            var baseline = SimpleUsageData("A/1.0.0", "B/1.0.0");
            baseline.IgnorePatterns = new[]
            {
                new UsagePattern { IdentityGlob = "A/*" },
            };

            var used = SimpleUsageData("A/1.0.0", "B/1.0.0");

            var data = task.GetUsageValidationData(baseline, used);

            Assert.Equal(
                baseline.Usages.Skip(1).Select(u => u.PackageIdentity).ToArray(),
                data.ActualUsageData.Usages.Select(u => u.PackageIdentity).ToArray());
        }

        [Fact]
        public void IgnorePatternsPassThroughToGeneratedBaseline()
        {
            var mockEngine = new Mock<IBuildEngine>(MockBehavior.Loose);
            var task = new ValidateUsageAgainstBaseline { BuildEngine = mockEngine.Object };

            var baseline = SimpleUsageData();
            baseline.IgnorePatterns = new[]
            {
                new UsagePattern { IdentityGlob = "*/*" },
                new UsagePattern { IdentityGlob = "Pizza/42.0.0" },
                new UsagePattern { IdentityRegex = "[Az]*" },
            };

            var used = SimpleUsageData();

            var data = task.GetUsageValidationData(baseline, used);

            Assert.Equal(baseline.IgnorePatterns.Length, data.ActualUsageData.IgnorePatterns.Length);

            foreach (var (a, b) in baseline.IgnorePatterns.Zip(data.ActualUsageData.IgnorePatterns, (a, b) => (a, b)))
            {
                Assert.Equal(a.IdentityGlob, b.IdentityGlob);
                Assert.Equal(a.IdentityRegex, b.IdentityRegex);
            }
        }
    }
}
