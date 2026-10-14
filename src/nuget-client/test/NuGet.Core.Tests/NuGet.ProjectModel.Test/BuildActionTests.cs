// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class BuildActionTests
    {
        [Fact]
        public void Parse_WithNullValue_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => BuildAction.Parse(value: null!));
        }
    }
}
