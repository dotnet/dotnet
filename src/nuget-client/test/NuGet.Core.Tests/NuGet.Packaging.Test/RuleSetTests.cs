using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.Rules;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class RuleSetTests
    {
        [Fact]
        public void PathTooLongWarning_PackageWithLongPath_Warn()
        {
            using (var packageFile = TestPackagesCore.GetPackageCoreReaderLongPathTestPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);
                var ruleSet = RuleSet.PackageCreationRuleSet;

                using (var reader = new PackageArchiveReader(zip))
                {
                    var issues = new List<PackagingLogMessage>();

                    foreach (var rule in ruleSet)
                    {
                        issues.AddRange(rule.Validate(reader).OrderBy(p => p.Code.ToString(), StringComparer.CurrentCulture));
                    }

                    Assert.Contains(issues, p => p.Code == NuGetLogCode.NU5123);
                }
            }
        }

        [Fact]
        public void PathTooLongWarning_PackageWithOutLongPath_NoWarn()
        {
            using (var packageFile = TestPackagesCore.GetPackageCoreReaderTestPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);
                var ruleSet = RuleSet.PackageCreationRuleSet;

                using (var reader = new PackageArchiveReader(zip))
                {
                    var issues = new List<PackagingLogMessage>();

                    foreach (var rule in ruleSet)
                    {
                        issues.AddRange(rule.Validate(reader).OrderBy(p => p.Code.ToString(), StringComparer.CurrentCulture));
                    }

                    Assert.DoesNotContain(issues, p => p.Code == NuGetLogCode.NU5123);
                }
            }
        }
    }
}
