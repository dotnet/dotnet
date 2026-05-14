// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class ConsoleTest
    {
        [Fact]
        public void TestConsoleWindowWidthNotZero()
        {
            Assert.NotEqual(0, new Console().WindowWidth);
        }

        [Theory]
        [InlineData(2, "abcd\nwxyz", "  abcdXXX  wxyzXXX", 10, 0)]
        [InlineData(2, "abcd\nwxyz", " abcdXXX wxyzXXX", 9, 1)]
        [InlineData(2, "abcd\rwxyz", "  abcdXXX  wxyzXXX", 10, 0)]
        [InlineData(3, "abcde\n\nwcjb", "   abcdeXXXXXX   wcjbXXX", 10, 0)]
        [InlineData(2, "abcde\r\nwcjb", "  abcdeXXX  wcjbXXX", 10, 0)]
        [InlineData(2, "abcde\rwcjb", "  abcdeXXX  wcjbXXX", 10, 0)]
        [InlineData(2, "abcdefghijk\nAB", "  abcdeXXX  fghijXXX  kXXX  ABXXX", 7, 0)]
        [InlineData(2, "abcdefghijk\nAB", " abcdeXXX fghijXXX kXXX ABXXX", 7, 1)]
        [InlineData(2, "ab  cd", "  abXXX  cdXXX", 6, 0)]
        [InlineData(2, "  abcd    ", "  abcdXXX", 10, 0)]
        [InlineData(2, "  abcd\t", "  abcdXXX", 10, 0)]
        [InlineData(2, "  abcd\n", "  abcdXXX", 10, 0)]
        [InlineData(2, "  abcd\n", "abcdXXX", 8, 2)]
        [InlineData(2, "  abcd\n\n", "  abcdXXXXXX", 10, 0)]
        [InlineData(2, "\t\nabcd\n\n", "XXX  abcdXXXXXX", 10, 0)]
        [InlineData(0, "abcd e", "abcdXXXeXXX", 5, 0)]
        public void TestPrintJustified(int indent, string input, string expected, int width, int cursorLeft)
        {
            var sw = new StringWriter();
            System.Console.SetOut(sw);

            var console = new Console();
            console.MockWindowWidth = width;
            console.MockCursorLeft = cursorLeft;
            console.PrintJustified(indent, input);

            Assert.Equal(expected.Replace("XXX", Environment.NewLine), sw.ToString());
        }
    }
}
