// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public partial class OutputConsoleLoggerTests
    {
        public class ImportingConstructor : OutputConsoleLoggerTests
        {
            public ImportingConstructor(GlobalServiceProvider sp)
                : base(sp)
            { }

            [Fact]
            public void When_null_consoleProvider_is_passed_ArgumentNullException_is_thrown()
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new OutputConsoleLogger(consoleProvider: null, new Lazy<INuGetErrorList>(() => _errorList.Object)));
                exception.ParamName.Should().Be("consoleProvider");
            }

            [Fact]
            public void When_null_errorList_is_passed_ArgumentNullException_is_thrown()
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new OutputConsoleLogger(_outputConsoleProvider.Object, errorList: null));
                exception.ParamName.Should().Be("errorList");
            }

            [Fact]
            public void Does_not_throw()
            {
                new OutputConsoleLogger(_outputConsoleProvider.Object, new Lazy<INuGetErrorList>(() => _errorList.Object));
            }
        }
    }
}
