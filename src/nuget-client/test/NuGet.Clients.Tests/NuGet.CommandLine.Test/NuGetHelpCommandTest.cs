using System;
using System.IO;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetHelpCommandTest
    {
        [Theory]
        [InlineData("add")]
        [InlineData("client-certs")]
        [InlineData("config")]
        [InlineData("delete")]
        [InlineData("help")]
        [InlineData("init")]
        [InlineData("install")]
        [InlineData("list")]
        [InlineData("locals")]
        [InlineData("pack")]
        [InlineData("push")]
        [InlineData("restore")]
        [InlineData("search")]
        [InlineData("setApiKey")]
        [InlineData("sign")]
        [InlineData("sources")]
        [InlineData("spec")]
        [InlineData("trusted-signers")]
        [InlineData("update")]
        [InlineData("verify")]
        public void HelpCommand_HelpMessage(string command)
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            // Act
            CommandRunnerResult r = CommandRunner.Run(
                nugetexe,
                Directory.GetCurrentDirectory(),
                "help " + command,
                waitForExit: true);

            // Assert
            Assert.True(0 == r.ExitCode, r.Output + Environment.NewLine + r.Errors);
        }

        // Tests that -ConfigFile is not included in the help message
        [Fact]
        public void HelpCommand_SpecCommand()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            // Act
            CommandRunnerResult r = CommandRunner.Run(
                nugetexe,
                Directory.GetCurrentDirectory(),
                "help spec",
                waitForExit: true);

            // Assert
            Assert.Equal(0, r.ExitCode);
            Assert.DoesNotContain("-ConfigFile", r.Output, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void HelpCommand_Failure_InvalidArguments()
        {
            Util.TestCommandInvalidArguments("help aCommand otherCommand -ForceEnglishOutput");
        }

        [Fact]
        public void HelpCommand_Help_ContainsLocalizedOption()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            // Act
            CommandRunnerResult r = CommandRunner.Run(
                nugetexe,
                Directory.GetCurrentDirectory(),
                "help help -ForceEnglishOutput",
                waitForExit: true);

            // Assert
            Assert.Equal(0, r.ExitCode);
            Assert.Contains("Show command help and usage information.", r.Output);
        }
    }
}
