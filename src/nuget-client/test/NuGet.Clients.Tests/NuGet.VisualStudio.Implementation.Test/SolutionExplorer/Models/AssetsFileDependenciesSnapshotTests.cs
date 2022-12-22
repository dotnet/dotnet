// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGet.VisualStudio.SolutionExplorer.Models;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.SolutionExplorer.Models
{
    public class AssetsFileDependenciesSnapshotTests
    {
        [Fact]
        public void ParseLibraries_IgnoreCaseInDependenciesTree_Succeeds()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    ""net5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    ""net5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    }
  ]
}";
            var lockFileFormat = new LockFileFormat();
            var lockFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            var dependencies = AssetsFileDependenciesSnapshot.ParseLibraries(lockFile, lockFile.Targets.First());

            Assert.Equal(1, dependencies.Count);
            Assert.True(dependencies.ContainsKey("system.runtime"));
        }

        [Fact]
        public void ParseLibraries_LockFileTargetLibrariesWithDifferentCase_Throws()
        {
            // Arrange
            var lockFileTarget = new LockFileTarget();
            lockFileTarget.Libraries = new List<LockFileTargetLibrary>
            {
                new LockFileTargetLibrary()
                {
                    Name = "packageA",
                    Type = "package",
                    Version = NuGetVersion.Parse("1.0.0")
                },
                new LockFileTargetLibrary()
                {
                    Name = "PackageA",
                    Type = "package",
                    Version = NuGetVersion.Parse("1.0.0")
                }
            };

            var exception = Assert.Throws<ArgumentException>(() => AssetsFileDependenciesSnapshot.ParseLibraries(new LockFile(), lockFileTarget));

            Assert.Contains("PackageA", exception.Message);
        }

        [Fact]
        public void ParseLibraries_LockFileTargetLibrariesMatchesDependencies_Succeeds()
        {
            // Arrange
            var lockFileTarget = new LockFileTarget();
            lockFileTarget.Libraries = new List<LockFileTargetLibrary>
            {
                new LockFileTargetLibrary()
                {
                    Name = "packageA",
                    Type = "package",
                    Version = NuGetVersion.Parse("1.0.0")
                },
                new LockFileTargetLibrary()
                {
                    Name = "packageB",
                    Type = "package",
                    Version = NuGetVersion.Parse("1.0.0")
                },
                new LockFileTargetLibrary()
                {
                    Name = "projectA",
                    Type = "project",
                    Version = NuGetVersion.Parse("1.0.0")
                },
                new LockFileTargetLibrary()
                {
                    Name = "projectB",
                    Type = "project",
                    Version = NuGetVersion.Parse("1.0.0")
                }
            };

            ImmutableDictionary<string, AssetsFileTargetLibrary> dependencies = AssetsFileDependenciesSnapshot.ParseLibraries(new LockFile(), lockFileTarget);

            Assert.Equal(lockFileTarget.Libraries.Count, dependencies.Count);
            Assert.All<LockFileTargetLibrary>(lockFileTarget.Libraries,
                source =>
                {
                    Assert.True(dependencies.ContainsKey(source.Name));

                    AssetsFileTargetLibrary target = dependencies[source.Name];
                    Assert.Equal(source.Name, target.Name);
                    Assert.Equal(source.Version.ToNormalizedString(), target.Version);

                    AssetsFileLibraryType sourceType;
                    Assert.True(Enum.TryParse<AssetsFileLibraryType>(source.Type, ignoreCase: true, out sourceType));
                    Assert.Equal(sourceType, target.Type);
                });
        }
    }
}
