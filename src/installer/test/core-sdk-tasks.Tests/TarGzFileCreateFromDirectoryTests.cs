using System.Reflection;
using FluentAssertions;
using Microsoft.DotNet.Build.Tasks;
using Xunit;

namespace EndToEnd
{
    public class TarGzFileCreateFromDirectoryTests
    {
        [Fact]
        public void ItPreservesTheDefaultTarArguments()
        {
            var task = new TarGzFileCreateFromDirectory
            {
                DestinationArchive = "archive.tar.gz",
                SourceDirectory = "source",
            };

            GetCommandLine(task).Should().StartWith("-czf archive.tar.gz ");
        }

        [Fact]
        public void ItNormalizesOwnershipForSourceBuild()
        {
            var task = new TarGzFileCreateFromDirectory
            {
                DestinationArchive = "archive.tar.gz",
                NormalizeSourceBuildOwnership = true,
                SourceDirectory = "source",
            };

            GetCommandLine(task).Should().StartWith("--numeric-owner --owner=2001 --group=2001 -czf archive.tar.gz ");
        }

        private static string GetCommandLine(TarGzFileCreateFromDirectory task)
        {
            MethodInfo method = typeof(TarGzFileCreateFromDirectory).GetMethod(
                "GenerateCommandLineCommands",
                BindingFlags.Instance | BindingFlags.NonPublic);

            return (string)method.Invoke(task, null);
        }
    }
}
