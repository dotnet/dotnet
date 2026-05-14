// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio.UnitTests
{
    public class GenerateTrainingPropsFileTests
    {
        [Fact]
        public void Execute()
        {
            var temp = Path.GetTempPath();
            var dir = Path.Combine(temp, Guid.NewGuid().ToString());

            var task = new GenerateTrainingPropsFile()
            {
                ProductDropName = "Products/DevDiv/dotnet/roslyn/12345",
                RepositoryName = "dotnet/roslyn",
                OutputDirectory = dir,
            };

            bool result = task.Execute();

            var actual = File.ReadAllText(Path.Combine(dir, "dotnet.roslyn.props"));
            Assert.Equal(@"<?xml version=""1.0""?>
<Project>
  <ItemGroup>
    <TestStore Include=""vstsdrop:ProfilingInputs/DevDiv/dotnet/roslyn/12345"" />
  </ItemGroup>
</Project>", actual);

            Assert.True(result);

            Directory.Delete(dir, recursive: true);
        }

        [Fact]
        public void EmptyArgs()
        {
            var temp = Path.GetTempPath();
            var dir = Path.Combine(temp, Guid.NewGuid().ToString());

            var task = new GenerateTrainingPropsFile()
            {
                ProductDropName = null,
                RepositoryName = null,
                OutputDirectory = dir,
            };

            bool result = task.Execute();

            var actual = File.ReadAllText(Path.Combine(dir, "ProfilingInputs.props"));
            Assert.Equal(@"<?xml version=""1.0""?>
<Project>
  <ItemGroup>
    <TestStore Include=""vstsdrop:ProfilingInputs/dummy"" />
  </ItemGroup>
</Project>", actual);

            Assert.True(result);

            Directory.Delete(dir, recursive: true);
        }
    }
}
