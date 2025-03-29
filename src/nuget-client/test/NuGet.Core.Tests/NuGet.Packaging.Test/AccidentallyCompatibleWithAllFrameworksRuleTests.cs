using System.Collections.Generic;
using NuGet.Common;
using NuGet.Packaging.Rules;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class AccidentallyCompatibleWithAllFrameworksRuleTests
    {
        [Theory]
        [MemberData("WarningRaisedData", MemberType = typeof(FileSource))]
        public void WarningRaisedWhenLibOrRefFolderWithTFMDataIsNotFoundAndBuildFolderIsFound(string[] files)
        {
            //Act
            var rule = new AccidentallyCompatibleWithAllFrameworksRule();
            var issues = rule.Validate(files);

            // Assert
            Assert.Contains(issues, p => p.Code == NuGetLogCode.NU5127);
        }

        [Theory]
        [MemberData("WarningNotRaisedData", MemberType = typeof(FileSource))]
        public void WarningNotRaisedWhenLibOrRefFolderWithTFMDataIsFound(string[] files)
        {
            //Act
            var rule = new AccidentallyCompatibleWithAllFrameworksRule();
            var issues = rule.Validate(files);

            // Assert
            Assert.DoesNotContain(issues, p => p.Code == NuGetLogCode.NU5127);
        }

        public static class FileSource
        {
            public static readonly List<object[]> WarningNotRaisedData = new List<object[]>
            {
                // Packages with ref/ and lib/ files should not warn
                new object[] { new string[] {"build/net45/test.props", "ref/net45/test.dll"} },
                new object[] { new string[] {"build/net45/test.props", "lib/net45/test.dll"} },
                new object[] { new string[] {"ref/net45/test.dll"} },
                new object[] { new string[] {"lib/net45/test.dll"} },
                // build/any/* and build/ without TFM are by design compatible with all frameworks
                new object[] { new string[] {"build/any/test.props"} },
                new object[] { new string[] {"build/test.props"} },
                // build/??/* where ?? is not a valid TFM is commonly used, so we should not warn
                new object[] { new string[] {"build/bin/test.targets"} },
                new object[] { new string[] {"build/lib/test.targets"} },
                // build/native/* is excluded, as they never use lib/ or ref/ assets.
                new object[] { new string[] {"build/native/test.props"} },
                // Normally build/{tfm}/* should warn, but if the package already contains build files that makes it compatible with all frameworks, it should not warn
                new object[] { new string[] {"build/net45/test.props", "build/any/test.props", "build/native/test.props"} },
                new object[] { new string[] {"build/net45/test.targets", "build/any/test.targets", "build/native/test.targets"} },
                // buildCrossTargeting/* and buildMultiTargeting/* also force the package to be compatible with all TFMs
                new object[] { new string[] {"build/net45/test.props", "buildMultiTargeting/test.props"} },
                new object[] { new string[] {"build/net45/test.props", "buildCrossTargeting/test.props" } },
                new object[] { new string[] {"buildTransitive/net45/test.targets", "buildMultiTargeting/test.targets"} },
            };

            public static readonly List<object[]> WarningRaisedData = new List<object[]>
            {
                new object[] { new string[] {"build/netstandard1.3/test.props"} },
                new object[] { new string[] {"build/net45/test.props", "content/net45/test.props"} },
                new object[] { new string[] {"build/net45/test.targets"} },
                new object[] { new string[] {"build/net45/test.targets", "content/net45/test.targets"} },
                new object[] { new string[] {"build/net45/test.targets", "test.targets"} },
                new object[] { new string[] {"build/net45/test.targets", "build/netstandard1.3/test.targets" } },
                new object[] { new string[] {"buildTransitive/net48/test.props"} },
            };
        }
    }
}
