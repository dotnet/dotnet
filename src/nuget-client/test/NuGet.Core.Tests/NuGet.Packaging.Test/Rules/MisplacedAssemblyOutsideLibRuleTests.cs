// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Rules;
using Xunit;

namespace NuGet.Commands.Test.Rules
{
    public class MisplacedAssemblyOutsideLibRuleTests
    {
        [Fact]
        public void Validate_DllsInKnownPaths_DoesNotReturnWarnings()
        {
            // Arrange
            var target = new MisplacedAssemblyOutsideLibRule();
            Func<IEnumerable<string>> getFiles = () =>
            [
                "lib/netstandard2.0/a.dll",
                "analyzers/a.dll",
                "ref/a.dll",
                "runtimes/a.dll",
                "native/a.dll",
                "build/a.dll",
                "buildTransitive/a.dll",
                "buildCrossTargeting/a.dll",
                "tools/a.dll",
            ];

            // Act
            var result = target.Validate(getFiles);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Validate_DllsNotInKnownPaths_ReturnWarnings()
        {
            // Arrange
            var target = new MisplacedAssemblyOutsideLibRule();
            string[] files =
            [
                "a.dll",
                Path.Combine("content", "a.dll"),
                Path.Combine("contentFiles", "a.dll"),
                Path.Combine("bin", "a.dll"),
                Path.Combine("contoso", "a.dll"),
            ];
            Func<IEnumerable<string>> getFiles = () => files;

            // Act
            var result = target.Validate(getFiles).ToList();

            // Assert
            result.Count.Should().Be(files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                result[i].Code.Should().Be(NuGetLogCode.NU5100);
                result[i].Message.Should().Contain(files[i]);
            }
        }
    }
}
