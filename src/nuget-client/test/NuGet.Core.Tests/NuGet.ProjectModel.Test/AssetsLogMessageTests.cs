// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class AssetsLogMessageTests
    {
        [Fact]
        public void Constructor_WithNullMessage_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, errorString: null!));
        }

        [Fact]
        public void Create_WithNullLogMessage_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => AssetsLogMessage.Create(logMessage: null!));
        }
    }
}
