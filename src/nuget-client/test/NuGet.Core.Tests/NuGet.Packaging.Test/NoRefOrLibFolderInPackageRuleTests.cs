using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Rules;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class NoRefOrLibFolderInPackageRuleTests
    {
        [Theory]
        [MemberData("WarningRaisedData", MemberType = typeof(FileSource))]
        public void WarningRaisedWhenLibOrRefFolderWithTFMDataIsNotFoundAndBuildFolderIsFound(string[] files)
        {
            //Act
            var rule = new NoRefOrLibFolderInPackageRule(AnalysisResources.NoRefOrLibFolderInPackage);
            var issues = rule.Validate(files);

            // Assert
            Assert.Contains(issues, p => p.Code == NuGetLogCode.NU5127);
        }

        [Theory]
        [MemberData("WarningNotRaisedData", MemberType = typeof(FileSource))]
        public void WarningNotRaisedWhenLibOrRefFolderWithTFMDataIsFound(string[] files)
        {
            //Act
            var rule = new NoRefOrLibFolderInPackageRule(AnalysisResources.NoRefOrLibFolderInPackage);
            var issues = rule.Validate(files);

            // Assert
            Assert.DoesNotContain(issues, p => p.Code == NuGetLogCode.NU5127);
        }

        public static class FileSource
        {
            public static readonly List<object[]> WarningNotRaisedData
                    = new List<object[]>
                    {
                    new object[] { new string[] {"build/net45/test.props", "ref/net45/test.dll"} },
                    new object[] { new string[] {"build/net45/test.props", "lib/net45/test.dll"} },
                    new object[] { new string[] {"ref/net45/test.dll"} },
                    new object[] { new string[] {"lib/net45/test.dll"} },
                    new object[] { new string[] {"build/any/test.props"} },
                    new object[] { new string[] {"build/native/test.props"} },
                    new object[] { new string[] {"build/test.props"} },
                    new object[] { new string[] {"build/bin/test.targets"} },
                    new object[] { new string[] {"build/lib/test.targets"} }

                    };

            public static readonly List<object[]> WarningRaisedData
                = new List<object[]>
                {
                new object[] { new string[] {"build/netstandard1.3/test.props"} },
                new object[] { new string[] {"build/net45/test.props", "build/any/test.props", "build/native/test.props"} },
                new object[] { new string[] {"build/net45/test.props", "content/net45/test.props"} },
                new object[] { new string[] {"build/net45/test.targets"} },
                new object[] { new string[] {"build/net45/test.targets", "build/any/test.targets", "build/native/test.targets"} },
                new object[] { new string[] {"build/net45/test.targets", "content/net45/test.targets"} },
                new object[] { new string[] {"build/net45/test.targets", "test.targets"} },
                new object[] { new string[] {"build/net45/test.targets", "build/netstandard1.3/test.targets" } }
                };
        }
    }
}
