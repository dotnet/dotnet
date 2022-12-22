// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.ContentModel;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class RelatedFileExtensionPropertyTests
    {
        [Theory]
        [InlineData("lib/net50/system.dll", new string[] { "lib/net50/system.dll" }, null)]
        [InlineData("lib/net50/system.dll", new string[] { "lib/net50/system.dll",
                                                           "lib/net50/system.exe",
                                                           "lib/net50/system.winmd" }, null)]
        [InlineData("lib/net50/system.dll", new string[] { "lib/net50/system.dll",
                                                           "lib/net50/system.Core.dll" }, null)]
        [InlineData("lib/net50/system.dll", new string[] { "lib/net50/system.dll",
                                                           "lib/net50/system.EXE",
                                                           "lib/net50/system.Core.DLL"}, null)]
        [InlineData("lib/net50/system.test.dll", new string[] { "lib/net50/system.test.dll",
                                                                "lib/net50/system.test.pdb",
                                                                "lib/net50/system.test.xml",
                                                                "lib/net50/system.test.dll.config" }, ".dll.config;.pdb;.xml")]
        [InlineData("lib/net50/system.test.dll", new string[] { "lib/net50/system.test2.dll",
                                                                "lib/net50/system.test2.pdb",
                                                                "lib/net50/system.test2.xml",
                                                                "lib/net50/system.test2.dll.config" }, null)]
        [InlineData("lib/net50/system.test.dll", new string[] { "lib/net50/system.test.dll",
                                                                "lib/net50/SYSTEM.TEST.pdb",
                                                                "lib/net50/System.Test.xml" }, null)]
        [InlineData("lib/net50/system.test.dll", new string[] { "lib/net50/system.test.dll",
                                                                "lib/net50/system.test.PDB",
                                                                "lib/net50/system.test.XML", }, ".PDB;.XML")]

        public void GetRelatedFileExtensionProperty_SingleAssemblyAsset_GetNullProperty(string assembly, string[] assetsPaths, string expectedRelatedProperty)
        {
            // Arrange
            var collection = new ContentItemCollection();
            collection.Load(assetsPaths);

            // Act
            string relatedProperty = collection.GetRelatedFileExtensionProperty(assembly, CreateAssetsFromPathList(assetsPaths));

            // Assert
            Assert.Equal(expectedRelatedProperty, relatedProperty);
        }


        [Fact]
        public void GetRelatedFileExtensionProperty_PlaceHolderAssembly_GetNullProperty()
        {
            // Arrange
            var collection = new ContentItemCollection();
            string assembly = "_._";
            string[] paths = new string[]
            {
                "_._"
            };
            collection.Load(paths);

            // Act
            string relatedProperty = collection.GetRelatedFileExtensionProperty(assembly, CreateAssetsFromPathList(paths));

            // Assert
            Assert.Equal(null, relatedProperty);
        }


        private IEnumerable<Asset> CreateAssetsFromPathList(string[] paths)
        {
            List<Asset> assets = new List<Asset>();
            foreach (string path in paths)
            {
                Asset asset = new Asset();
                asset.Path = path;
                assets.Add(asset);
            }
            return assets;
        }
    }
}
