// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test
{
    public class PathLookupTrieTests
    {
        private static readonly PathLookupTrie<int> TestInstance = new PathLookupTrie<int>
        {
            [@"C:\src\MyProject\packages\NuGet.Versioning.3.5.0-rc1-final"] = 1,
            [@"C:\Users\.nuget\packages\Autofac\3.5.2"] = 2,
            [@"\\SharedFolder\packages\NuGet.Core\2.12.0"] = 3
        };

        [Theory]
        [InlineData(@"C:\src\MyProject\packages\NuGet.Versioning.3.5.0-rc1-final\lib\net45\NuGet.Versioning.dll", 1)]
        [InlineData(@"C:\SRC\MYPROJECT\PACKAGES\NUGET.VERSIONING.3.5.0-RC1-FINAL\LIB\NET45\NUGET.VERSIONING.DLL", 1)]
        [InlineData(@"C:\Users\.nuget\packages\Autofac\3.5.2\lib\net40\Autofac.dll", 2)]
        [InlineData(@"\\SharedFolder\packages\NuGet.Core\2.12.0\lib\net40\NuGet.Core.dll", 3)]
        public void Indexer_WithFullFilePath_FindsPackageDirectory(string filePath, int expected)
        {
            var found = TestInstance[filePath];
            Assert.Equal(expected, found);
        }

        [Theory]
        [InlineData(@"C:\src\OtherProject\packages\NuGet.Versioning.3.5.0-rc1-final\lib\net45\NuGet.Versioning.dll")]
        [InlineData(@"C:\src\MyProject\packages\NuGet.Versioning\3.5.0-rc1-final\lib\net45\NuGet.Versioning.dll")]
        [InlineData(@"C:\path\to\non\package\assembly\Newtonsoft.Json.dll")]
        [InlineData(@"C:\Users\.nuget\packages\Autofac")]
        [InlineData(@"C:\Users\.nuget\packages\Autofac\lib\net40\Autofac.dll")]
        [InlineData(@"C:\Users\.nuget\packages\Autofac.3.5.2\lib\net40\Autofac.dll")]
        public void Indexer_WithNotFoundPath_Throws(string filePath)
        {
            Assert.Throws<KeyNotFoundException>(() => TestInstance[filePath]);
        }
    }
}
