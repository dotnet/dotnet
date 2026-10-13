// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileItemTests
    {
        [Fact]
        public void Constructor_WithNullPath_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new LockFileItem(path: null!));
        }

        [Fact]
        public void RuntimeTargetConstructor_WithNullRuntime_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new LockFileRuntimeTarget("path", runtime: null!, assetType: "runtime"));
        }

        [Fact]
        public void RuntimeTargetConstructor_WithNullAssetType_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new LockFileRuntimeTarget("path", runtime: "rid", assetType: null!));
        }

        [Fact]
        public void OptionalProperty_SetToNull_RemovesProperty()
        {
            var item = new LockFileContentFile("path")
            {
                OutputPath = "output"
            };

            item.OutputPath = null;

            Assert.False(item.Properties.ContainsKey(LockFileContentFile.OutputPathProperty));
        }
    }
}
