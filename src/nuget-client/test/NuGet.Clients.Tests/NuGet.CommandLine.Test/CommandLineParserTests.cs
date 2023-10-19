// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;
using NuGet.Commands;
using System;
using Moq;
using System.Reflection;

namespace NuGet.CommandLine.Test
{
    public class CommandLineParserTests
    {
        private Mock<ICommandManager> _mockCommandManager = new Mock<ICommandManager>();

        private Dictionary<OptionAttribute, PropertyInfo> _expectedRestoreOptions = new Dictionary<OptionAttribute, PropertyInfo>
            {
                { new OptionAttribute(typeof(NuGetCommand), "CommandNoHttpCache"), typeof(DownloadCommandBase).GetProperty(nameof(DownloadCommandBase.NoHttpCache)) } };
        private Dictionary<OptionAttribute, PropertyInfo> _emptySetOfOptions = new();

        [Fact]
        public void ExtractOptions_WhenPassingOptionNoHttpCache_NoHttpCacheShouldBeTrue()
        {
            // Arrange
            RestoreCommand restoreCommand = new RestoreCommand();
            List<string> args = new() { "-NoHttpCache" };
            _mockCommandManager.Setup(manager => manager.GetCommandOptions(restoreCommand)).Returns(_expectedRestoreOptions);
            CommandLineParser commandLineParser = new CommandLineParser(_mockCommandManager.Object);

            // Act
            commandLineParser.ExtractOptions(restoreCommand, args.GetEnumerator());

            // Assert
            Assert.True(restoreCommand.NoHttpCache);
        }

        [Fact]
        public void ExtractOptions_WhenNotPassingOptionNoHttpCache_NoHttpCacheShouldBeFalse()
        {
            // Arrange
            RestoreCommand restoreCommand = new RestoreCommand();
            List<string> args = new();
            _mockCommandManager.Setup(manager => manager.GetCommandOptions(restoreCommand)).Returns(_expectedRestoreOptions);
            CommandLineParser commandLineParser = new CommandLineParser(_mockCommandManager.Object);

            // Act
            commandLineParser.ExtractOptions(restoreCommand, args.GetEnumerator());

            // Assert
            Assert.False(restoreCommand.NoHttpCache);
        }

        [Fact]
        public void ExtractOptions_WhenPassingOptionNoHttpCacheToDeleteCommand_ExtractOptionsShouldRaseCommandException()
        {
            // Arrange
            DeleteCommand deleteCommand = new DeleteCommand();
            string option = "-NoHttpCache";
            List<string> args = new() { option };
            _mockCommandManager.Setup(manager => manager.GetCommandOptions(deleteCommand)).Returns(_emptySetOfOptions);
            CommandLineParser commandLineParser = new CommandLineParser(_mockCommandManager.Object);
            CommandException expectedException = new CommandException(LocalizedResourceManager.GetString("UnknownOptionError"), option);

            // Act
            Action action = () => commandLineParser.ExtractOptions(deleteCommand, args.GetEnumerator());

            // Assert
            CommandException exception = Assert.Throws<CommandException>(action);
            Assert.Equal(expectedException.Message, exception.Message);
        }
    }
}
