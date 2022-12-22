// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.Arcade.Common;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class ArgumentEscaperTests
    {
        [Fact]
        public void EscapesOnlyArgsWithSpecialCharacters()
        {
            var args = new[]
            {
                "subcommand",
                "--not-escaped",
                "1.0.0-prerelease.21165.2",
                "--with-space",
                "/mnt/d/Program Files",
                "--already-escaped",
                "\"some value\"",
                "containing-\"-quote",
            };

            string escaped = ArgumentEscaper.EscapeAndConcatenateArgArrayForProcessStart(args);
            escaped.Should().Be(
                "subcommand " +
                "--not-escaped 1.0.0-prerelease.21165.2 " +
                "--with-space \"/mnt/d/Program Files\" " +
                "--already-escaped \"some value\" " +
                "\"containing-\\\"-quote\"");
        }
    }
}
