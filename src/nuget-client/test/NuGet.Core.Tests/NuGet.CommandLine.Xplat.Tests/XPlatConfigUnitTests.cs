// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.CommandLine.XPlat;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class XPlatConfigUnitTests
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ConfigPathsCommand_WithNullArguments_ThrowsArgumentNullException(bool argsIsNull, bool loggerIsNull)
        {
            // Arrange
            ConfigPathsArgs args = null;
            CommandOutputLogger getLogger = null;

            if (!argsIsNull)
            {
                args = new ConfigPathsArgs();
            }

            if (!loggerIsNull)
            {
                getLogger = new CommandOutputLogger(Common.LogLevel.Debug);
            }

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ConfigPathsRunner.Run(args, loggerIsNull ? null : () => getLogger));
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ConfigGetCommand_WithNullArguments_ThrowsArgumentNullException(bool argsIsNull, bool loggerIsNull)
        {
            // Arrange
            ConfigGetArgs args = null;
            CommandOutputLogger getLogger = null;

            if (!argsIsNull)
            {
                args = new ConfigGetArgs();
            }

            if (!loggerIsNull)
            {
                getLogger = new CommandOutputLogger(Common.LogLevel.Debug);
            }

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ConfigGetRunner.Run(args, loggerIsNull ? null : () => getLogger));
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ConfigSetCommand_WithNullArguments_ThrowsArgumentNullException(bool argsIsNull, bool loggerIsNull)
        {
            // Arrange
            ConfigSetArgs args = null;
            CommandOutputLogger getLogger = null;

            if (!argsIsNull)
            {
                args = new ConfigSetArgs();
            }

            if (!loggerIsNull)
            {
                getLogger = new CommandOutputLogger(Common.LogLevel.Debug);
            }

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ConfigSetRunner.Run(args, loggerIsNull ? null : () => getLogger));
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ConfigUnsetCommand_WithNullArguments_ThrowsArgumentNullException(bool argsIsNull, bool loggerIsNull)
        {
            // Arrange
            ConfigUnsetArgs args = null;
            CommandOutputLogger getLogger = null;

            if (!argsIsNull)
            {
                args = new ConfigUnsetArgs();
            }

            if (!loggerIsNull)
            {
                getLogger = new CommandOutputLogger(Common.LogLevel.Debug);
            }

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ConfigUnsetRunner.Run(args, loggerIsNull ? null : () => getLogger));
        }
    }
}
