// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Xunit;
using FluentAssertions;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class PackageIndexTests
    {
        [Fact]
        public void IndexCacheConsidersModifiedTime()
        {
            string packageIndexFile = $"{nameof(IndexCacheConsidersModifiedTime)}.json";
            
            PackageIndex packageIndex = new PackageIndex();
            packageIndex.Packages.Add("MyPackage", new PackageInfo());

            packageIndex.Packages.Should().HaveCount(1);
            packageIndex.Packages.Should().ContainKey("MyPackage");

            packageIndex.Save(packageIndexFile);

            DateTime originalModifiedTime = File.GetLastWriteTimeUtc(packageIndexFile);
            string[] packageIndexFiles = new[] { packageIndexFile };

            packageIndex = PackageIndex.Load(packageIndexFiles);
            packageIndex.Packages.Should().HaveCount(1);
            packageIndex.Packages.Should().ContainKey("MyPackage");
            
            packageIndex = new PackageIndex();
            packageIndex.Packages.Add("MyPackage", new PackageInfo());
            packageIndex.Packages.Add("MyPackage2", new PackageInfo());

            packageIndex.Packages.Should().HaveCount(2);
            packageIndex.Packages.Should().ContainKey("MyPackage");
            packageIndex.Packages.Should().ContainKey("MyPackage2");

            packageIndex.Save(packageIndexFile);

            // force the same modified time, but should be different size
            File.SetLastWriteTimeUtc(packageIndexFile, originalModifiedTime);
            packageIndex = PackageIndex.Load(packageIndexFiles);

            packageIndex.Packages.Should().HaveCount(2);
            packageIndex.Packages.Should().ContainKey("MyPackage");
            packageIndex.Packages.Should().ContainKey("MyPackage2");

            // now change the content so that it has the same size, but different modified time
            long previousLength = new FileInfo(packageIndexFile).Length;
            packageIndex.Packages.Remove("MyPackage2");
            packageIndex.Packages.Add("MyPackage3", new PackageInfo());
            packageIndex.Save(packageIndexFile);
            var newFileInfo = new FileInfo(packageIndexFile);

            newFileInfo.Length.Should().Be(previousLength);

            // ensure we have a different modified time
            File.SetLastWriteTimeUtc(packageIndexFile, new DateTime(originalModifiedTime.Ticks + 100));
            packageIndex = PackageIndex.Load(packageIndexFiles);

            packageIndex.Packages.Should().HaveCount(2);
            packageIndex.Packages.Should().ContainKey("MyPackage");
            packageIndex.Packages.Should().ContainKey("MyPackage3");
        }
    }
}
