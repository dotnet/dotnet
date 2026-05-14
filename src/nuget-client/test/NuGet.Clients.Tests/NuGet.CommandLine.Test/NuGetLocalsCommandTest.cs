// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using Moq;
using NuGet.CommandLine.Commands;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetLocalsCommandTest
    {
        private const string LocalsHelpStringFragment =
            "usage: NuGet locals <all | http-cache | global-packages | temp | plugins-cache> [-clear | -list]";

        [Theory]
        [InlineData("locals -?")]
        [InlineData("locals")]
        public void LocalsCommand_Success_HelpMessage(string args)
        {
            // Arrange & Act
            var result = CommandRunner.Run(
                Util.GetNuGetExePath(),
                Directory.GetCurrentDirectory(),
                args);

            // Assert
            Util.VerifyResultSuccess(result, LocalsHelpStringFragment);
        }

        [Theory]
        [InlineData("locals all -list extraArg")]
        [InlineData("locals http-cache temp")]
        public void LocalsCommand_Failure_InvalidArguments_HelpMessage(string cmd)
        {
            Util.TestCommandInvalidArguments(cmd);
        }

        [Theory]
        [InlineData("locals unknownResource -list")]
        [InlineData("locals unknownResource -clear")]
        public void LocalsCommand_Success_InvalidLocalResourceName_HelpMessage(string args)
        {
            // Arrange & Act
            var result = CommandRunner.Run(
                Util.GetNuGetExePath(),
                Directory.GetCurrentDirectory(),
                args);

            // Assert
            Util.VerifyResultFailure(result, "An invalid local resource name was provided. Provide one of the following values: http-cache, temp, global-packages, all.");
        }

        [Theory]
        [InlineData("locals -list")]
        [InlineData("locals -clear")]
        public void LocalsCommand_Success_NoLocalResourceName_HelpMessage(string args)
        {
            // Arrange & Act
            var result = CommandRunner.Run(
                Util.GetNuGetExePath(),
                Directory.GetCurrentDirectory(),
                args);

            // Assert
            Util.VerifyResultSuccess(result, LocalsHelpStringFragment);
        }

        [Theory]
        [InlineData("locals -list -clear")]
        [InlineData("locals all -clear -list")]
        public void LocalsCommand_Success_BothListAndClearOptions_HelpMessage(string args)
        {
            // Arrange & Act
            var result = CommandRunner.Run(
                Util.GetNuGetExePath(),
                Directory.GetCurrentDirectory(),
                args);

            // Assert
            Util.VerifyResultSuccess(result, LocalsHelpStringFragment);
        }

        [Theory]
        [InlineData("http-cache")]
        [InlineData("global-packages")]
        [InlineData("temp")]
        public void LocalsCommand_Success_ValidLocalResource_ListMessage(string args)
        {
            // Arrange & Act
            var result = CommandRunner.Run(
                Util.GetNuGetExePath(),
                Directory.GetCurrentDirectory(),
                $"locals {args} -list");

            // Assert
            Util.VerifyResultSuccess(result, $"{args}: ");
        }

        [Theory]
        [InlineData("all")]
        [InlineData("temp")]
        [InlineData("http-cache")]
        [InlineData("global-packages")]
        public async Task LocalsCommand_ParsingValidation_WithNoConfigParam(string arg)
        {
            // Use a test directory to validate test key-value pairs within ISettings object passed to Runner
            using (var mockCurrentDirectory = TestDirectory.Create())
            {
                // Arrange
                LocalsUtil.CreateDummyConfigFile(mockCurrentDirectory.Path);
                var mockLocalsCommandRunner = new Mock<ILocalsCommandRunner>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.Setup(c => c.Verbosity).Returns(Verbosity.Detailed);

                var localsCommand = new LocalsCommand
                {
                    LocalsCommandRunner = mockLocalsCommandRunner.Object,
                    Clear = false,
                    List = true,
                    Console = mockConsole.Object
                };
                localsCommand.Arguments.Add(arg);
                localsCommand.CurrentDirectory = mockCurrentDirectory.Path;
                var defaultSettings = Configuration.Settings.LoadDefaultSettings(mockCurrentDirectory,
                                                                                 null,
                                                                                 localsCommand.MachineWideSettings);

                // Act
                localsCommand.Execute();
                await localsCommand.ExecuteCommandAsync();

                // Assert
                mockLocalsCommandRunner.Verify(mock => mock.ExecuteCommand(It.Is<LocalsArgs>(l => l.List && !l.Clear &&
                                                                                             l.Arguments.Count == 1 && l.Arguments[0] == arg &&
                                                                                             l.LogError == mockConsole.Object.LogError &&
                                                                                             l.LogInformation == mockConsole.Object.LogInformation &&
                                                                                             SettingsUtility.GetConfigValue(l.Settings, "foo", false, false) == SettingsUtility.GetConfigValue(defaultSettings, "foo", false, false) &&
                                                                                             SettingsUtility.GetConfigValue(l.Settings, "kung foo", false, false) == SettingsUtility.GetConfigValue(defaultSettings, "kung foo", false, false))));
            }
        }

        [Theory]
        [InlineData("all")]
        [InlineData("temp")]
        [InlineData("http-cache")]
        [InlineData("global-packages")]
        public async Task LocalsCommand_ParsingValidation_WithConfigParam(string arg)
        {
            // Use a test directory to validate test key-value pairs within ISettings object passed to Runner
            using (var mockCurrentDirectory = TestDirectory.Create())
            {
                // Arrange
                var dummyConfigPath = LocalsUtil.CreateDummyConfigFile(mockCurrentDirectory.Path);
                var mockLocalsCommandRunner = new Mock<ILocalsCommandRunner>();
                var mockConsole = new Mock<IConsole>();
                mockConsole.Setup(c => c.Verbosity).Returns(Verbosity.Detailed);
                var localsCommand = new LocalsCommand
                {
                    LocalsCommandRunner = mockLocalsCommandRunner.Object,
                    Clear = true,
                    List = false,
                    Console = mockConsole.Object
                };
                localsCommand.Arguments.Add(arg);
                localsCommand.ConfigFile = dummyConfigPath;
                localsCommand.CurrentDirectory = mockCurrentDirectory.Path;
                var directory = Path.GetDirectoryName(dummyConfigPath);
                var configFileName = Path.GetFileName(dummyConfigPath);
                var defaultSettings = Configuration.Settings.LoadDefaultSettings(directory,
                                                                                 configFileName,
                                                                                 localsCommand.MachineWideSettings);

                // Act
                localsCommand.Execute();
                await localsCommand.ExecuteCommandAsync();

                // Assert
                mockLocalsCommandRunner.Verify(mock => mock.ExecuteCommand(It.Is<LocalsArgs>(l => !l.List && l.Clear &&
                                                                                             l.Arguments.Count == 1 && l.Arguments[0] == arg &&
                                                                                             l.LogError == mockConsole.Object.LogError &&
                                                                                             l.LogInformation == mockConsole.Object.LogInformation &&
                                                                                             SettingsUtility.GetConfigValue(l.Settings, "foo", false, false) == SettingsUtility.GetConfigValue(defaultSettings, "foo", false, false) &&
                                                                                             SettingsUtility.GetConfigValue(l.Settings, "kung foo", false, false) == SettingsUtility.GetConfigValue(defaultSettings, "kung foo", false, false))));
            }
        }
    }
}
