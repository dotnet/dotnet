// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    [CollectionDefinition(Name)]
    public class TestProjectCollection : ICollectionFixture<TestProjectFixture>
    {
        public const string Name = nameof(TestProjectCollection);
    }

    public class TestProjectFixture : IDisposable
    {
        private readonly ConcurrentQueue<IDisposable> _disposables = new ConcurrentQueue<IDisposable>();
        private readonly string _logOutputDir;
        private readonly string _testAssets;
        private readonly string _boilerPlateDir;

        private static readonly string[] _packagesToClear =
        {
            "Microsoft.DotNet.Arcade.Sdk",
        };

        public TestProjectFixture()
        {
            ClearPackages();
            _logOutputDir = GetType().Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "LogOutputDir").Value;
            _testAssets = Path.Combine(AppContext.BaseDirectory, "testassets");
            _boilerPlateDir = Path.Combine(_testAssets, "boilerplate");
        }

        public TestApp CreateTestApp(string name)
        {
            var testAppFiles = Path.Combine(_testAssets, name);
            var instanceName = Path.GetRandomFileName();
            var tempDir = Path.Combine(Path.GetTempPath(), "arcade", instanceName);
            var app = new TestApp(tempDir, _logOutputDir, new[] { testAppFiles, _boilerPlateDir });
            _disposables.Enqueue(app);
            return app;
        }

        private void ClearPackages()
        {
            var nugetRoot = GetType().Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "NuGetPackageRoot").Value;
            var pkgVersion = GetType().Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "PackageVersion").Value;
            foreach (var package in _packagesToClear)
            {
                var pkgRoot = Path.Combine(nugetRoot, package, pkgVersion);
                if (Directory.Exists(pkgRoot))
                {
                    Directory.Delete(pkgRoot, recursive: true);
                }
            }
        }

        public void Dispose()
        {
            while (_disposables.Count > 0)
            {
                if (_disposables.TryDequeue(out var disposable))
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
