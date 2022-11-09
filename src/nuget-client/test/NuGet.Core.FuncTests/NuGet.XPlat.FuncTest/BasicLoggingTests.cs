// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.XPlat.FuncTest
{
    public class BasicLoggingTests
    {
        [Fact]
        public void BasicLogging_NoParams_ExitCode()
        {
            // Arrange
            var log = new TestCommandOutputLogger();

            var args = new string[]
            {
                //empty
            };

            // Act
            var exitCode = NuGet.CommandLine.XPlat.Program.MainInternal(args, log);

            // Assert
            Assert.Equal(0, exitCode);
        }
    }
}
