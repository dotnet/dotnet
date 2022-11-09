// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Internal.DependencyInjection.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class InstallDotNetToolTests
    {
        private const string InstallPath = @"C:\dotnet_tools";
        private const string ToolName = "Microsoft.DotNet.Arcade.FakeTool";
        private const string ToolVersion = "1.0.0-prerelease.21108.1";
        private static readonly string s_installedPath = Path.Combine(InstallPath, ToolName, ToolVersion);

        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly Mock<IHelpers> _helpersMock;
        private readonly Mock<ICommandFactory> _commandFactoryMock;
        private readonly Mock<ICommand> _commandMock;

        private readonly InstallDotNetTool _task;

        private string _dotnetPath = "dotnet";
        private IEnumerable<string> _expectedArgs = new[]
        {
            "tool",
            "install",
            "--version",
            ToolVersion,
            "--tool-path",
            s_installedPath,
            ToolName,
        };

        public InstallDotNetToolTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _commandMock = new Mock<ICommand>();

            bool succeeded = false;

            _helpersMock = new Mock<IHelpers>();
            _helpersMock
                .Setup(x => x.DirectoryMutexExec(It.IsAny<Func<bool>>(), It.IsAny<string>()))
                .Callback<Func<bool>, string>((function, path) => {
                    succeeded = function();
                })
                .Returns(() => succeeded);

            _commandFactoryMock = new Mock<ICommandFactory>();
            _commandFactoryMock
                .Setup(x => x.Create(
                    It.Is<string>(s => s == _dotnetPath),
                    It.IsAny<IEnumerable<string>>()))
                .Returns(_commandMock.Object)
                .Verifiable();

            _task = new InstallDotNetTool()
            {
                DestinationPath = InstallPath,
                Name = ToolName,
                Version = ToolVersion,
                BuildEngine = new MockBuildEngine(),
            };
        }

        [Fact]
        public void SkipsInstallation()
        {
            // Setup
            _fileSystemMock
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(true);

            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeTrue();

            // Verify
            _commandFactoryMock
                .Verify(
                    x => x.Create(
                        It.Is<string>(dotnet => dotnet == _dotnetPath),
                        It.IsAny<IEnumerable<string>>()),
                    Times.Never);

            _commandMock.Verify(x => x.Execute(), Times.Never);
        }

        [Fact]
        public void InstallsToolSuccessfully()
        {
            // Setup
            _fileSystemMock
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(false);

            _commandMock
                .Setup(x => x.Execute())
                .Returns(new CommandResult(new ProcessStartInfo(), 0, "Tool installed", null));

            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeTrue();

            // Verify
            _commandFactoryMock.VerifyAll();
            _commandMock.Verify(x => x.Execute(), Times.Once);
            _task.ToolPath.Should().Be(s_installedPath);
        }

        [Fact]
        public void TriesToInstallToolUnsuccessfully()
        {
            // Setup
            _fileSystemMock
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(false);

            _commandMock
                .Setup(x => x.Execute())
                .Returns(new CommandResult(new ProcessStartInfo(), 1, null, "Installation failed"));

            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeFalse();

            // Verify
            _commandFactoryMock.VerifyAll();
            _commandMock.Verify(x => x.Execute(), Times.Once);
        }

        [Fact]
        public void InstallsToolWithExtraParamsSuccessfully()
        {
            // Setup
            _fileSystemMock
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(false);

            _commandMock
                .Setup(x => x.Execute())
                .Returns(new CommandResult(new ProcessStartInfo(), 0, "Tool installed", null));

            _expectedArgs = new[]
            {
                "tool",
                "install",
                "--version",
                ToolVersion,
                "--tool-path",
                Path.Combine(InstallPath, ToolName, ToolVersion),
                "--framework",
                "net6.0",
                "--arch",
                "arm64",
                "--add-source",
                "https://dev.azure.com/some/feed",
                ToolName,
            };

            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);
            _task.Source = "https://dev.azure.com/some/feed";
            _task.DotnetPath = _dotnetPath = @"D:\dotnet\dotnet.exe";
            _task.TargetArchitecture = "arm64";
            _task.TargetFramework = "net6.0";

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeTrue();

            // Verify
            _commandFactoryMock.VerifyAll();
            _commandMock.Verify(x => x.Execute(), Times.Once);
            _task.ToolPath.Should().Be(s_installedPath);
        }

        /// <summary>
        /// This test spawns 2 real threads that use real Mutex.
        /// First thread (installation task), calls the `dotnet tool install` command.
        /// Second thread (skip task), waits for the first thread to finish and then verifies the existence of the tool.
        /// </summary>
        [Fact]
        public async Task InstallsInParallelWithRealMutex()
        {
            // Setup
            var fileSystemMock1 = new Mock<IFileSystem>();
            fileSystemMock1
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(false);

            var fileSystemMock2 = new Mock<IFileSystem>();
            fileSystemMock2
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(true);

            var helpers = new Microsoft.Arcade.Common.Helpers();
            var hangingCommandCalled = new TaskCompletionSource<bool>();
            var dotnetToolInstalled = new TaskCompletionSource<bool>();

            var hangingCommand = new Mock<ICommand>();
            hangingCommand
                .Setup(x => x.Execute())
                .Callback(() =>
                {
                    hangingCommandCalled.SetResult(true);
                    dotnetToolInstalled.Task.GetAwaiter().GetResult(); // stop here
                })
                .Returns(new CommandResult(new ProcessStartInfo(), 0, "Tool installed", null));

            var hangingCommandFactoryMock = new Mock<ICommandFactory>();
            hangingCommandFactoryMock
                .Setup(x => x.Create(
                    It.Is<string>(s => s == _dotnetPath),
                    It.Is<IEnumerable<string>>(args => args.All(y => _expectedArgs.Contains(y)))))
                .Returns(hangingCommand.Object)
                .Verifiable();

            var collection1 = new ServiceCollection();
            collection1.AddSingleton(hangingCommandFactoryMock.Object);
            collection1.AddSingleton(fileSystemMock1.Object);
            collection1.AddTransient<IHelpers, Microsoft.Arcade.Common.Helpers>();

            var collection2 = new ServiceCollection();
            collection2.AddSingleton(_commandFactoryMock.Object);
            collection2.AddSingleton(fileSystemMock2.Object);
            collection2.AddTransient<IHelpers, Microsoft.Arcade.Common.Helpers>();

            var task1 = new InstallDotNetTool()
            {
                DestinationPath = InstallPath,
                Name = ToolName,
                Version = ToolVersion,
                BuildEngine = new MockBuildEngine(),
            };

            var task2 = new InstallDotNetTool()
            {
                DestinationPath = InstallPath,
                Name = ToolName,
                Version = ToolVersion,
                BuildEngine = new MockBuildEngine(),
            };

            // Act
            using var provider1 = collection1.BuildServiceProvider();
            using var provider2 = collection2.BuildServiceProvider();

            var installationTask = Task.Run(() => task1.InvokeExecute(provider1).Should().BeTrue());

            // Let's wait for the first `dotnet tool install` to be called (it will stay spinning)
            await hangingCommandCalled.Task;

            var skipTask = Task.Run(() => task2.InvokeExecute(provider2).Should().BeTrue());

            // The first command must have been executed, let's verify the parameters
            hangingCommandFactoryMock
                .Verify(
                    x => x.Create(
                        It.Is<string>(dotnet => dotnet == _dotnetPath),
                        It.IsAny<IEnumerable<string>>()),
                    Times.Once);

            // The other command is waiting on the Mutex now
            skipTask.IsCompleted.Should().BeFalse();

            _commandFactoryMock
                .Verify(
                    x => x.Create(
                        It.Is<string>(dotnet => dotnet == _dotnetPath),
                        It.IsAny<IEnumerable<string>>()),
                    Times.Never);

            // We now let `dotnet tool install` run to completion
            dotnetToolInstalled.SetResult(true);

            // Let's give the installation task time to evaluate the command result
            // Let's give the skip task get its own Mutex and verify existence of the installation
            await Task.WhenAll(installationTask, skipTask);

            // Verify
            _commandFactoryMock
                .Verify(
                    x => x.Create(
                        It.Is<string>(dotnet => dotnet == _dotnetPath),
                        It.Is<IEnumerable<string>>(args => args.Count() == _expectedArgs.Count() && args.All(y => _expectedArgs.Contains(y)))),
                    Times.Never);

            task1.ToolPath.Should().Be(task2.ToolPath);
            task1.ToolPath.Should().Be(s_installedPath);
        }

        [Fact]
        public void AreDependenciesRegistered()
        {
            var task = new InstallDotNetTool();

            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            var provider = collection.BuildServiceProvider();

            DependencyInjectionValidation.IsDependencyResolutionCoherent(
                    s =>
                    {
                        task.ConfigureServices(s);
                    },
                    out string message,
                    additionalSingletonTypes: task.GetExecuteParameterTypes()
                )
                .Should()
                .BeTrue(message);
        }

        private IServiceCollection CreateMockServiceCollection()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton(_commandFactoryMock.Object);
            collection.AddSingleton(_fileSystemMock.Object);
            collection.AddSingleton(_helpersMock.Object);
            return collection;
        }
    }
}
