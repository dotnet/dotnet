// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility
{
    public class VsSemanticVersionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SemanticVersion_ParseVersion_ThrowsIfNullOrEmpty(string version)
        {
            var exception = Assert.Throws<ArgumentException>(paramName: "version", () => SemanticVersion.Parse(version));

            Assert.StartsWith("Value cannot be null or an empty string.", exception.Message);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("invalid")]
        [InlineData("1.1.1.1.1")]
        [InlineData("%")]
        public void SemanticVersion_ParseVersion_ThrowsIfInvalid(string invalidVersion)
        {
            var invalidException = Assert.Throws<ArgumentException>(paramName: "version", () => SemanticVersion.Parse(invalidVersion));

            Assert.StartsWith("'" + invalidVersion + "' is not a valid version string.", invalidException.Message);
        }
    }
}
