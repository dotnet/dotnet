// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class CommandTests
    {
        [Fact]
        public void Execute_WhenExceptionIsThrown_PropagatesOriginalException()
        {
            var command = new TestCommand()
            {
                Console = Mock.Of<IConsole>()
            };

            Assert.Throws<DivideByZeroException>(() => command.Execute());
        }

        private sealed class TestCommand : Command
        {
            public override async Task ExecuteCommandAsync()
            {
                await Task.CompletedTask;

                throw new DivideByZeroException();
            }
        }
    }
}
