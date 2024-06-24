// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using FluentAssertions;
using NuGet.CommandLine.Commands;
using Xunit;

namespace NuGet.CommandLine.Test.Commands
{
    public class EulaCommandTests
    {
        [Fact]
        public void Execute_ShowsRepoLicense()
        {
            // Arrange
            using MemoryStream memoryStream = new MemoryStream();

            // Act
            using (TextWriter writer = new StreamWriter(memoryStream, Encoding.UTF8, 4096, leaveOpen: true))
            {
                EulaCommand.Execute(writer);
            }

            // Assert
            memoryStream.Position = 0;
            using MemoryStream repoLicenseStream = new MemoryStream(TestResource.LICENSE);

            using TextReader actualText = new StreamReader(memoryStream);
            using TextReader expectedText = new StreamReader(repoLicenseStream);

            string actualLine;
            do
            {
                actualLine = actualText.ReadLine();
                string expectedLine = expectedText.ReadLine();
                actualLine.Should().Be(expectedLine);
            } while (actualLine != null);
        }
    }
}
